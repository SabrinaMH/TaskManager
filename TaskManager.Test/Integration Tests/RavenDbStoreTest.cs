using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Raven.Client;
using Raven.Client.Linq;
using Raven.Tests.Helpers;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test
{
    [TestFixture]
    public class RavenDbStoreTest : RavenTestBase
    {
        [Test]
        public void Can_Insert_Into_RavenDb()
        {
            var fixture = new Fixture();
            var title = fixture.Create<string>();
            IDocumentStore documentStore = new RavenDbStore().Instance;
            string id = fixture.Create<string>();
            using (var session = documentStore.OpenSession())
            {
                string projectId = fixture.Create<string>();
                var taskInGridView = new TaskInGridView(id, projectId, title,
                    fixture.Create<DateTime>().ToShortDateString(), ProjectPriority.Low.DisplayName, true);
                session.Store(taskInGridView);
                session.SaveChanges();
            }

            using (var session = documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(id);
                // WaitForUserToContinueTheTest(documentStore, true, 8079);
                Assert.That(taskInGridView, Is.Not.Null);
            }
            documentStore.Dispose();
        }
    }
}