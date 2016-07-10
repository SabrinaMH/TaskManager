module Generators

open FsCheck
open NUnit.Framework
open System
open TaskManager.Domain.Models.Task
open TaskManager.Domain.Features.RegisterTask
open TaskManager.Domain.Features.SaveNote
open TaskManager.Domain.Features.TaskGridView


let shuffleG xs = xs |> Seq.sortBy (fun _ -> Guid.NewGuid())

let taskPriority =
    [|TaskPriority.None; TaskPriority.Low; TaskPriority.Lowest; TaskPriority.Medium; TaskPriority.High; TaskPriority.Highest|]
    |> shuffleG 
    |> Seq.head
    |> string


let taskRegisteredGen =
    gen {
        let! taskId = Arb.generate<NonEmptyString> 
        let! projectId = Arb.generate<NonEmptyString>
        let! title = Arb.generate<NonEmptyString>
        let! deadline = Arb.generate<DateTime>
        let! priority = Gen.constant taskPriority

        return new TaskRegistered(taskId |> string, projectId |> string, title |> string, priority, deadline |> string)
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

