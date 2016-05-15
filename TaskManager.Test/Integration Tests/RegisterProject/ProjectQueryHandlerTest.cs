using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Models.Project;
using Raven.Tests.Helpers;

namespace TaskManager.Test.RegisterProject
{
    [TestFixture]
    public class ProjectQueryHandlerTest : BaseIntegrationTest
    {
        [Test]
        public void Project_Does_Not_Already_Exist()
        {
            var doesProjectWithTitleExistQuery = new DoesProjectWithTitleExistQuery("my title");
            var projectQueryHandler = new ProjectQueryHandler();
            bool doesProjectExist = projectQueryHandler.Handle(doesProjectWithTitleExistQuery);
            Assert.That(doesProjectExist, Is.False);
        }

        [Test]
        public void Project_Does_Already_Exist()
        {
            var fixture = new Fixture();
            var title = fixture.Create<string>();
            using (var session = DocumentStore.OpenSession())
            {
                string id = fixture.Create<Guid>().ToString();
                var projectTreeNode = new ProjectTreeNode(id, title, fixture.Create<DateTime>().ToShortDateString(),
                    ProjectPriority.Low.DisplayName);
                session.Store(projectTreeNode);
                session.SaveChanges();
            }

            var doesProjectWithTitleExistQuery = new DoesProjectWithTitleExistQuery(title);
            var projectQueryHandler = new ProjectQueryHandler();
            bool doesProjectExist = projectQueryHandler.Handle(doesProjectWithTitleExistQuery);
            Assert.That(doesProjectExist, Is.True);
        }
    }
}