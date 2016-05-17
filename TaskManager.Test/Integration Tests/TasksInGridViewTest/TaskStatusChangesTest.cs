using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test.TasksInGridViewTest
{
    [TestFixture]
    public class TaskStatusChangesTest : BaseIntegrationTest
    {
        [Test]
        public void When_Task_Is_Done_The_Grid_View_Is_Updated()
        {
            var projectId = new ProjectId(Fixture.Create<string>());
            var title = new Title(Fixture.Create<string>());
            var deadline = new Deadline(DateTime.UtcNow);
            var task = new Task(projectId, title, TaskPriority.Low, deadline);
            task.Done();
            var eventStoreRepository = new EventStoreRepository<Task>(Mediator, InMemoryEventStoreConnectionBuilder);
            eventStoreRepository.Save(task);

            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var taskInGridView = taskInGridViewQueryHandler.Handle(allTasksInProjectQuery)[0];
            Assert.That(taskInGridView.IsDone, Is.True);
        }

        [Test]
        public void When_Task_Is_Reopened_The_Grid_View_Is_Updated()
        {
            var projectId = new ProjectId(Fixture.Create<string>());
            var title = new Title(Fixture.Create<string>());
            var deadline = new Deadline(DateTime.UtcNow);
            var task = new Task(projectId, title, TaskPriority.Low, deadline);
            task.Done();
            task.Reopen();
            var eventStoreRepository = new EventStoreRepository<Task>(Mediator, InMemoryEventStoreConnectionBuilder);
            eventStoreRepository.Save(task);

            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var taskInGridView = taskInGridViewQueryHandler.Handle(allTasksInProjectQuery)[0];
            Assert.That(taskInGridView.IsDone, Is.False);
        }
    }
}