﻿module TaskManager.Test.FSharp

open FsCheck
open FsUnit
open Raven.Client;
open NUnit.Framework
open TaskManager.Domain.Infrastructure
open TaskManager.Domain.Models.Task
open TaskManager.Domain.Models.Project
open TaskManager.Domain.Models.Common
open TaskManager.Domain.Features.EraseNote
open TaskManager.Domain.Features.RegisterTask
open TaskManager.Domain.Features.SaveNote
open TaskManager.Domain.Features.TaskGridView
open Generators


[<Test>] 
let ``Erase note on task raises event`` () =
    let taskRegistered = taskRegisteredGen.Sample(1,1).Item(0)
    let noteSaved = noteSavedGen.Sample(1,1).Item(0)
    let taskRegisteredAsEvent = taskRegistered :> TaskManager.Domain.Common.Event
    let noteSavedAsEvent = noteSaved :> TaskManager.Domain.Common.Event
    let history = [|taskRegisteredAsEvent;noteSavedAsEvent|] :> System.Collections.Generic.IList<TaskManager.Domain.Common.Event>
    let task = new Task(history)
    
    task.EraseNote()
    let noteErased = new NoteErased(task.Id.ToString())
    let uncommittedEvents = task.GetUncommittedEvents()
    Assert.That(uncommittedEvents.Contains(noteErased))

[<Test>]
let ``Can retrieve all tasks`` () =
    let taskInGridView1 = taskInGridViewGen.Sample(1,1).Item(0)
    let taskInGridView2 = taskInGridViewGen.Sample(1,1).Item(0)
    let ravenDbStore = (new RavenDbStore(true, false)).Instance
    use session = ravenDbStore.OpenSession()
    session.Store(taskInGridView1)
    session.Store(taskInGridView2)
    session.SaveChanges()

    let queryHandler = new TaskInGridViewQueryHandler()
    let query = new AllTasksQuery()
    
    query |> queryHandler.Handle |> should haveCount 2

