using System.Linq;
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
    public class ReprioritizeTaskUpdatesGridTest : BaseIntegrationTest
    {
        [Test]
        public void Reprioritizing_Task_Updates_Grid()
        {
            var projectId = Fixture.Create<ProjectId>();
            var title = Fixture.Create<Title>();
            var task = new Task(projectId, title, TaskPriority.Low);
            var newPriority = TaskPriority.High;
            task.Reprioritize(newPriority);
            var eventStoreRepository = new EventStoreRepository<Task>(Mediator, InMemoryEventStoreConnectionBuilder);
            eventStoreRepository.Save(task);

            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            var tasksInProject = taskInGridViewQueryHandler.Handle(allTasksInProjectQuery);

            var reprioritizedTask = tasksInProject.First(x => x.Id == task.Id);
            Assert.That(reprioritizedTask.Priority, Is.EqualTo(newPriority.DisplayName));
        }
    }
}