using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test.ProjectIntegrationTests
{
    [TestFixture]
    public class ProjectTreeViewQueryHandlerTest : BaseIntegrationTest
    {
        [Test]
        public void Can_Retrieve_All_Projects()
        {
            var fixture = new Fixture();
            using (var session = DocumentStore.OpenSession())
            {
                string id = fixture.Create<Guid>().ToString();
                var projectTreeNode = new ProjectTreeNode(id, fixture.Create<string>(), fixture.Create<DateTime>().ToShortDateString(),
                    ProjectPriority.Medium.DisplayName);
                session.Store(projectTreeNode);
                session.SaveChanges();
            }

            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            List<ProjectTreeNode> projectTreeNodes = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);
            Assert.That(projectTreeNodes.Count, Is.EqualTo(1));
        }
    }
}