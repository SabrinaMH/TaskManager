using System.Collections.Generic;
using NUnit.Framework;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;
using TechTalk.SpecFlow;

namespace TaskManager.Test
{
    public static class ScenarioContextExtensions
    {
        public static void TaskId(this ScenarioContext scenarioContext, TaskId taskId)
        {
            scenarioContext.Set(taskId, "taskId");
        }

        public static TaskId TaskId(this ScenarioContext scenarioContext)
        {
            return scenarioContext.Get<TaskId>("taskId");
        }

        public static void ProjectId(this ScenarioContext scenarioContext, ProjectId projectId)
        {
            scenarioContext.Set(projectId, "projectId");
        }

        public static ProjectId ProjectId(this ScenarioContext scenarioContext)
        {
            return scenarioContext.Get<ProjectId>("projectId");
        }

        public static void TaskInGridViews(this ScenarioContext scenarioContext, List<TaskInGridView> taskInGridViews)
        {
            scenarioContext.Set(taskInGridViews, "taskInGridViews");
        }

        public static List<TaskInGridView> TaskInGridViews(this ScenarioContext scenarioContext)
        {
            return scenarioContext.Get<List<TaskInGridView>>("taskInGridViews");
        }

        public static void NoteContent(this ScenarioContext scenarioContext, string noteContent)
        {
            scenarioContext.Set(noteContent, "noteContent");
        }

        public static string NoteContent(this ScenarioContext scenarioContext)
        {
            return scenarioContext.Get<string>("noteContent");
        }
    }
}