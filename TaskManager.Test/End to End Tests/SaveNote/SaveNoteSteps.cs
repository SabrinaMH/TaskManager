using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;
using TechTalk.SpecFlow;

namespace TaskManager.Test.SaveNote
{
    [Binding]
    public class SaveNoteSteps
    {
        [Given(@"I have a task")]
        public void GivenIHaveATask()
        {
            var title = Base.Fixture.Create<string>();
            var projectId = Base.Fixture.Create<ProjectId>();
            var registerTask = new Domain.Features.RegisterTask.RegisterTask(projectId, title,
                TaskPriority.Low.DisplayName, Base.Fixture.Create<DateTime>());
            Base.CommandDispatcher.Send(registerTask);
            ScenarioContext.Current.ProjectId(new ProjectId(projectId));
            ScenarioContext.Current.TaskId(TaskId.Create(projectId, title));
        }

        [Given(@"the task has a note")]
        public void GivenTheTaskHasANote()
        {
            var taskId = ScenarioContext.Current.TaskId();
            var content = Base.Fixture.Create<string>();
            var saveNote = new Domain.Features.SaveNote.SaveNote(taskId, content);
            Base.CommandDispatcher.Send(saveNote);
            ScenarioContext.Current.NoteContent(content);
        }

        [When(@"I select the task")]
        public void WhenISelectTheTask()
        {
            var projectId = ScenarioContext.Current.ProjectId();
            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var taskInGridViews = taskInGridViewQueryHandler.Handle(new AllTasksInProjectQuery(projectId));
            ScenarioContext.Current.TaskInGridViews(taskInGridViews);
        }

        [Then(@"I can see the note")]
        public void ThenICanSeeTheNote()
        {
            var taskInGridViews = ScenarioContext.Current.TaskInGridViews();
            var idOfSelectedTask = ScenarioContext.Current.TaskId();
            var selectedTask = taskInGridViews.First(x => x.Id == idOfSelectedTask);
            var noteContent = ScenarioContext.Current.NoteContent();
            Assert.That(selectedTask.Note, Is.EqualTo(noteContent));
        }

    }
}