namespace TaskManager.API

open System
open System.Web.Http
open System.Linq
open TaskManager.Domain.Infrastructure
open TaskManager.Domain.Common
open TaskManager.Domain.Models.Project
open TaskManager.Domain.Models.Task
open TaskManager.Domain.Features.ProjectTreeView
open TaskManager.Domain.Features.RegisterProject
open TaskManager.Domain.Features.RegisterTask
open TaskManager.Domain.Features.ChangeDeadlineOnTask
open TaskManager.Domain.Features.ChangeTitleOnTask
open TaskManager.Domain.Features.ChangeTaskStatus
open TaskManager.Domain.Features.EraseNote
open TaskManager.Domain.Features.SaveNote
open TaskManager.Domain.Features.ReprioritizeProject
open TaskManager.Domain.Features.ReprioritizeTask

type Aggregate = { id : string; aggregateType : Type;  eventStream : Event list option }

type ProjectionController() = 
    inherit ApiController()

    [<HttpGet>]
    [<Route("replaceSlashInEvents")>]
    member this.ReplaceSlashInEvents() =
        let projects = new AllProjectTreeNodesQuery() |> (new ProjectTreeViewQueryHandler()).Handle 
        
        let rec eventsWithReplacedIds (acc: Event list) (eventStream: Event list) = 
            match eventStream with 
            | [] -> acc
            | head::tail -> 
                match head with
                | x when x.GetType() = typeof<ProjectRegistered> ->
                    let pr = (box x :?> ProjectRegistered)
                    acc = acc @ [new ProjectRegistered(this.IdWithSlashReplaced pr.ProjectId, pr.Title, pr.Priority, pr.Deadline)]
                | x when x.GetType() = typeof<ProjectReprioritized> ->
                    let pr = (box x :?> ProjectReprioritized)
                    acc = acc @ [new ProjectReprioritized(this.IdWithSlashReplaced pr.ProjectId, pr.OldPriority, pr.NewPriority)]
                | x when x.GetType() = typeof<TaskRegistered> ->
                    let tr = (box x :?> TaskRegistered)
                    acc = acc @ [new TaskRegistered(this.IdWithSlashReplaced tr.TaskId, this.IdWithSlashReplaced tr.ProjectId, tr.Title, tr.Priority, tr.Deadline)]
                | x when x.GetType() = typeof<TaskDone> ->
                    let td = (box x :?> TaskDone)
                    acc = acc @ [new TaskDone(this.IdWithSlashReplaced td.TaskId, this.IdWithSlashReplaced td.ProjectId)]
                | x when x.GetType() = typeof<TaskReopened> ->
                    let tr = (box x :?> TaskReopened)
                    acc = acc @ [new TaskReopened(this.IdWithSlashReplaced tr.TaskId, this.IdWithSlashReplaced tr.ProjectId)]
                | x when x.GetType() = typeof<TaskReprioritized> ->
                    let tr = (box x :?> TaskReprioritized)
                    acc = acc @ [new TaskReprioritized(this.IdWithSlashReplaced tr.TaskId, tr.OldPriority, tr.NewPriority)]
                | x when x.GetType() = typeof<TitleOnTaskChanged> ->
                    let ttc = (box x :?> TitleOnTaskChanged)
                    acc = acc @ [new TitleOnTaskChanged(this.IdWithSlashReplaced ttc.TaskId, ttc.NewTitle)]
                | x when x.GetType() = typeof<DeadlineOnTaskChanged> ->
                    let dtc = (box x :?> DeadlineOnTaskChanged)
                    acc = acc @ [new DeadlineOnTaskChanged(this.IdWithSlashReplaced dtc.TaskId, dtc.NewDeadline)]
                | x when x.GetType() = typeof<NoteSaved> ->
                    let ns = (box x :?> NoteSaved)
                    acc = acc @ [new NoteSaved(this.IdWithSlashReplaced ns.TaskId, ns.NoteContent)]
                | x when x.GetType() = typeof<NoteErased> ->
                    let ne = (box x :?> NoteErased)
                    acc = acc @ [new NoteErased(this.IdWithSlashReplaced ne.TaskId)]
                | _ ->
                    Logging.Logger.Error("Event type {eventType} wasn't matched", head.GetType().ToString())
                    raise (System.ArgumentException("ProjectId contains more or less than a single slash"))
                eventsWithReplacedIds acc tail

        for project in projects do
            let eventStream = project.Id |> this.RetrieveStream
            let firstEvent = List.head eventStream
            let aggregate = 
              match firstEvent.GetType() with 
              | x when x = typeof<ProjectRegistered> -> { id = ((box x :?> ProjectRegistered).ProjectId).ToString(); aggregateType = typeof<Project>; eventStream = None }
              | x when x = typeof<TaskRegistered> -> { id = ((box x :?> TaskRegistered).TaskId).ToString(); aggregateType = typeof<Task>; eventStream = None }
            aggregate.eventStream = Some (eventsWithReplacedIds [] eventStream) |> ignore
            this.SaveStream aggregate
                
    member private this.NumberOfSlashes str =
        let slashes = str |> Seq.countBy(fun c -> c = '/') |> Seq.tryFind(fun(key, value) -> key = true)
        match slashes with 
        | Some (key, numberOfSlashes) -> numberOfSlashes
        | None -> 0

    member private this.IdWithSlashReplaced (currentProjectId: string) = 
        let numberOfSlashes = this.NumberOfSlashes currentProjectId 
        match numberOfSlashes with
        | 1 -> currentProjectId.Split '/' |> Seq.last |> ProjectId.Create |> string
        | _ -> 
            Logging.Logger.Error("{projectId} contains more or less than a single slash", currentProjectId)
            raise (System.ArgumentException("ProjectId contains more or less than a single slash"))

    member private this.SaveStream aggregate =
        let eventStoreConnectionBuilder = new EventStoreConnectionBuilder()
        let eventBus = new EventBus(fun event next -> (new ExceptionDecorator<Event>(next)).Handle(event))
        let projectEventStoreRepository = new EventStoreRepository<Project>(eventBus, eventStoreConnectionBuilder) 
        
        match aggregate.eventStream with
        | Some stream -> 
            let streamAsList = new System.Collections.Generic.List<Event>(stream)
            projectEventStoreRepository.SaveEventStream(Seq.toArray stream, aggregate.aggregateType, aggregate.id)
        | None -> ()

    member private this.RetrieveStream aggregateId =
        let eventStoreConnectionBuilder = new EventStoreConnectionBuilder()
        let eventBus = new EventBus(fun event next -> (new ExceptionDecorator<Event>(next)).Handle(event))
        let projectEventStoreRepository = new EventStoreRepository<Project>(eventBus, eventStoreConnectionBuilder)
        aggregateId |> projectEventStoreRepository.GetEventStream |> Seq.toList
