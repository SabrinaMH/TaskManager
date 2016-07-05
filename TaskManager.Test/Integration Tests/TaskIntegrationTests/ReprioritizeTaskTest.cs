using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ReprioritizeTask;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test.TaskIntegrationTests
{
    [TestFixture]
    public class ReprioritizeTaskTest : BaseIntegrationTest
    {
        [Test]
        public void Reprioritize_Task_Updates_Grid_Read_Model()
        {
            string taskId = Fixture.Create<string>();
            string oldPriority = TaskPriority.Low.DisplayName;
            var projectId = Fixture.Create<string>();

            using (var session = DocumentStore.OpenSession())
            {
                var taskInGridView = new TaskInGridView(taskId, projectId, Fixture.Create<string>(), Fixture.Create<Deadline>(), oldPriority, false);
                session.Store(taskInGridView);
                session.SaveChanges();
            }

            var eventHandler = new TaskReprioritizedEventHandler(DocumentStore);
            var newPriority = TaskPriority.High.DisplayName;
            var taskReprioritized = new TaskReprioritized(taskId, oldPriority, newPriority);
            eventHandler.Handle(taskReprioritized);
            
            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            var tasksInProject = taskInGridViewQueryHandler.Handle(allTasksInProjectQuery);

            var reprioritizedTask = tasksInProject.First(x => x.Id == taskId);
            Assert.That(reprioritizedTask.Priority, Is.EqualTo(newPriority));
        } 
    }
}