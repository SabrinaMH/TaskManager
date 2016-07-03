using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test.TaskIntegrationTests
{
    [TestFixture]
    public class ReopenTaskTest : BaseIntegrationTest
    {
        [Test]
        public void When_Task_Is_Reopened_The_Grid_Read_Model_Is_Updated()
        {
            var taskId = Fixture.Create<string>();
            var projectId = Fixture.Create<string>();

            using (var session = DocumentStore.OpenSession())
            {
                var taskInGridView = new TaskInGridView(taskId, projectId,
                    Fixture.Create<string>(), Fixture.Create<Deadline>(), TaskPriority.Low.DisplayName, true);
                session.Store(taskInGridView);
                session.SaveChanges();
            }

            var eventHandler = new TaskReopenedEventHandler(DocumentStore);
            var taskReopened = new TaskReopened(taskId);
            eventHandler.Handle(taskReopened);

            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var task = taskInGridViewQueryHandler.Handle(allTasksInProjectQuery)[0];
            Assert.That(task.IsDone, Is.False);
        }
    }
}