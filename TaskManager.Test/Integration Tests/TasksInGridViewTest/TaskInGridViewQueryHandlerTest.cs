using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test.TasksInGridViewTest
{
    [TestFixture]
    public class TaskInGridViewQueryHandlerTest : BaseIntegrationTest
    {
        [Test]
        public void Can_Retrieve_All_Projects()
        {
            var fixture = new Fixture();
            Guid projectId = fixture.Create<Guid>();
            using (var session = DocumentStore.OpenSession())
            {
                Guid id1 = fixture.Create<Guid>();
                var taskInGridView1 = new TaskInGridView(id1, projectId, fixture.Create<string>(),
                    fixture.Create<DateTime>().ToShortDateString(), TaskPriority.Lowest.DisplayName, true);

                Guid id2 = fixture.Create<Guid>();
                var taskInGridView2 = new TaskInGridView(id2, projectId, fixture.Create<string>(),
                    fixture.Create<DateTime>().ToShortDateString(), TaskPriority.Highest.DisplayName, false);

                session.Store(taskInGridView1);
                session.Store(taskInGridView2);
                session.SaveChanges();
            }

            var taskGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            List<TaskInGridView> tasksInGridView = taskGridViewQueryHandler.Handle(allTasksInProjectQuery);
            Assert.That(tasksInGridView.Count, Is.EqualTo(2));
        }
    }
}