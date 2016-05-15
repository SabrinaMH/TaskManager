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
        public void Can_Retrieve_All_Tasks_In_Project()
        {
            string projectId = Fixture.Create<string>();
            string otherProjectId = Fixture.Create<string>();
            using (var session = DocumentStore.OpenSession())
            {
                string id1 = Fixture.Create<string>();
                var taskInGridView1 = new TaskInGridView(id1, projectId, Fixture.Create<string>(),
                    Fixture.Create<DateTime>().ToShortDateString(), TaskPriority.Lowest.DisplayName, true);

                string id2 = Fixture.Create<string>();
                var taskInGridView2 = new TaskInGridView(id2, projectId, Fixture.Create<string>(),
                    Fixture.Create<DateTime>().ToShortDateString(), TaskPriority.Highest.DisplayName, false);

                string id3 = Fixture.Create<string>();
                var taskInOtherProject = new TaskInGridView(id3, otherProjectId, Fixture.Create<string>(),
                    Fixture.Create<DateTime>().ToShortDateString(), TaskPriority.Highest.DisplayName, false);

                session.Store(taskInGridView1);
                session.Store(taskInGridView2);
                session.Store(taskInOtherProject);
                session.SaveChanges();
            }

            var taskGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            List<TaskInGridView> tasksInGridView = taskGridViewQueryHandler.Handle(allTasksInProjectQuery);

            Assert.That(tasksInGridView.Count, Is.EqualTo(2));
            Assert.That(tasksInGridView.TrueForAll(x => x.ProjectId == projectId));
        }
    }
}