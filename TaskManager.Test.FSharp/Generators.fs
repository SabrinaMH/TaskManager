module Generators

open FsCheck
open System
open TaskManager.Domain.Models.Task
open TaskManager.Domain.Models.Project
open TaskManager.Domain.Features.RegisterProject
open TaskManager.Domain.Features.RegisterTask
open TaskManager.Domain.Features.SaveNote
open TaskManager.Domain.Features.TaskGridView


let shuffleG xs = xs |> Seq.sortBy (fun _ -> Guid.NewGuid())

let projectPriority =
    [|ProjectPriority.None; ProjectPriority.Low; ProjectPriority.Medium; ProjectPriority.High|]
    |> shuffleG 
    |> Seq.head
    |> string
    
let projectRegisteredGen =
    gen {
        let! projectId = Arb.generate<NonEmptyString> 
        let! title = Arb.generate<NonEmptyString>
        let! priority = Gen.constant projectPriority
        let! deadline = Arb.generate<DateTime> 
        let formattedDeadline = deadline.ToString("dd-MM-yyyy")

        return new ProjectRegistered(projectId |> string, title |> string, priority, formattedDeadline)
    }

let registerProjectGen =
    gen {
        let! title = Arb.generate<NonEmptyString>
        let! deadline = Arb.generate<DateTime> 

        return new RegisterProject(title |> string, System.Nullable deadline)
    }

let taskPriority =
    [|TaskPriority.None; TaskPriority.Low; TaskPriority.Lowest; TaskPriority.Medium; TaskPriority.High; TaskPriority.Highest|]
    |> shuffleG 
    |> Seq.head
    |> string

let registerTaskGen =
    gen {
        let! projectId = Arb.generate<NonEmptyString>
        let! title = Arb.generate<NonEmptyString>
        let! deadline = Arb.generate<DateTime> 
        let! priority = Gen.constant taskPriority

        return new RegisterTask(projectId |> string, title |> string, priority, System.Nullable deadline)
    }

let taskRegisteredGen =
    gen {
        let! taskId = Arb.generate<NonEmptyString> 
        let! projectId = Arb.generate<NonEmptyString>
        let! title = Arb.generate<NonEmptyString>
        let! deadline = Arb.generate<DateTime> 
        let! priority = Gen.constant taskPriority
        let formattedDeadline = deadline.ToString("dd-MM-yyyy")

        return new TaskRegistered(taskId |> string, projectId |> string, title |> string, priority, formattedDeadline)
    }

let noteSavedGen =
    gen {
        let! taskId = Arb.generate<NonEmptyString>
        let! noteContent = Arb.generate<NonEmptyString>

        return new NoteSaved(taskId |> string, noteContent |> string)
    }

let taskInGridViewGen = 
    gen {
        let! taskId = Arb.generate<NonEmptyString> 
        let! projectId = Arb.generate<NonEmptyString>
        let! title = Arb.generate<NonEmptyString>
        let! deadline = Arb.generate<DateTime>
        let! priority = Gen.constant taskPriority
        let! isDone = Arb.generate<Boolean>
        let! note = Arb.generate<NonEmptyString>

        return new TaskInGridView(taskId |> string, projectId |> string, title |> string, deadline |> string, priority, isDone, note |> string)
    }

