using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test.ProjectIntegrationTests
{
    [TestFixture]
    public class ProjectQueryHandlerTest : BaseIntegrationTest
    {
        private Title _title;

        [SetUp]
        public void SetUp()
        {
            _title = Fixture.Create<Title>();
        }

        [Test]
        public void Project_Does_Not_Already_Exist()
        {
            var doesProjectWithTitleExistQuery = new DoesProjectWithTitleExistQuery(_title);
            var projectQueryService = new ProjectQueryService();
            bool doesProjectExist = projectQueryService.Handle(doesProjectWithTitleExistQuery);
            Assert.That(doesProjectExist, Is.False);
        }

        [Test]
        public void Project_Does_Already_Exist()
        {
            using (var session = DocumentStore.OpenSession())
            {
                var projectId = Fixture.Create<ProjectId>();
                var projectTreeNode = new ProjectTreeNode(projectId, _title, Fixture.Create<DateTime>().ToShortDateString(),
                    ProjectPriority.Low.DisplayName, 0);
                session.Store(projectTreeNode);
                session.SaveChanges();
            }

            var doesProjectWithTitleExistQuery = new DoesProjectWithTitleExistQuery(_title);
            var projectQueryService = new ProjectQueryService();
            bool doesProjectExist = projectQueryService.Handle(doesProjectWithTitleExistQuery);
            Assert.That(doesProjectExist, Is.True);
        }
    }
}