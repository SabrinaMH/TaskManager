module ProjectTreeViewSteps

type ProjectTreeViewFixture () = inherit TickSpec.NUnit.FeatureFixture("ProjectTreeView.feature")

open System
open TickSpec
open FsUnit
open FsCheck
open TaskManager.Test
open TaskManager.Domain.Features.ProjectTreeView
open TaskManager.Domain.Features.RegisterTask
open Generators

let mutable projectId = ""
let mutable projectNodes = new Collections.Generic.List<ProjectTreeNode>()

let [<BeforeScenarioAttribute>] SetUp() =
    () |> Base.BaseSetUp

let [<Given(@"I have a project")>] IHaveAProject() =
    let projectRegistered = projectRegisteredGen.Sample(1,1).Item(0)
    projectId <- projectRegistered.ProjectId |> string
    projectRegistered |> Base.EventBus.Publish

let [<Given(@"the project has (.*) open tasks")>] GivenTheProjectHasOpenTasks(numberOfOpenTasks : Int32) = 
    [1..numberOfOpenTasks] |> Seq.iter (fun _ -> 
        let taskRegistered = taskRegisteredGen.Sample(1,1).Item(0)
        let taskRegisteredWithSpecificProjectId = new TaskRegistered(taskRegistered.TaskId, projectId, taskRegistered.Title, taskRegistered.Priority)
        taskRegisteredWithSpecificProjectId |> Base.EventBus.Publish
    )

let [<When(@"I view the project tree view")>] WhenIViewTheProjectTreeView() =
    projectNodes <- new AllProjectTreeNodesQuery() |> (new ProjectTreeViewQueryHandler()).Handle 


let [<Then(@"the project node shows (.*) open tasks")>] ThenTheProjectNodeShowsOpenTasks(numberOfOpenTasks : Int32) = 
    let project = projectNodes.Find(fun x -> x.Id = projectId)
    project.NumberOfOpenTasks |> should equal numberOfOpenTasks

let [<AfterScenarioAttribute>] TearDown() =
    () |> Base.BaseTearDown
