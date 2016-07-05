module TaskManager.Test.FSharp

open FsUnit
open FsCheck
open NUnit.Framework
open NUnitRunner
open TaskManager.Domain.Models.Task
open TaskManager.Domain.Models.Project
open TaskManager.Domain.Models.Common

[<Test>]
let shouldSayHello2
    = fun () -> Assert.AreEqual(2, 2)

[<Test>]
let ``Erase note on task raises event`` () =
    let task = new Task(new ProjectId("projectId"), new Title("task title"), TaskPriority.Low)
    let uncommittedEvents = task.GetUncommittedEvents()
    let containsEvent event = (event = new NoteErased(task.Id.ToString()))
    Assert.True(Seq.exists containsEvent uncommittedEvents)