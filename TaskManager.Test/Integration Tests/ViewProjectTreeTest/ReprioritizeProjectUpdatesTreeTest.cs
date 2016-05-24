using System;
using System.Collections.Generic;
using MediatR;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.ProjectTreeViewUI;

namespace TaskManager.Test.ViewProjectTreeTest
{
    [TestFixture]
    public class RepioritizeProjectUpdatesViewTest : BaseIntegrationTest
    {
        [Test]
        public void Reprioritizing_Project_Updates_Project_Ordering()
        {
            var projectWithMediumPriority = new Project(new Title(Fixture.Create<string>()), new Deadline(DateTime.UtcNow));
            projectWithMediumPriority.Reprioritize(ProjectPriority.Medium);
            var projectWithHighPriority = new Project(new Title(Fixture.Create<string>()), new Deadline(DateTime.UtcNow));
            projectWithHighPriority.Reprioritize(ProjectPriority.High);
            var projectWithNoPriority = new Project(new Title(Fixture.Create<string>()), new Deadline(DateTime.UtcNow));
            projectWithNoPriority.Reprioritize(ProjectPriority.None);

            var eventStoreRepository = new EventStoreRepository<Project>(Mediator, InMemoryEventStoreConnectionBuilder);
            eventStoreRepository.Save(projectWithMediumPriority);
            eventStoreRepository.Save(projectWithHighPriority);
            eventStoreRepository.Save(projectWithNoPriority);

            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();

            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            List<ProjectTreeNode> projectTreeNodes = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);
            var sorter = new Sorter();
            List<ProjectTreeNode> sortedProjects = sorter.ByPriority(projectTreeNodes);

            Assert.That(sortedProjects[0].Id, Is.EqualTo(projectWithHighPriority.Id.ToString()));
            Assert.That(sortedProjects[1].Id, Is.EqualTo(projectWithMediumPriority.Id.ToString()));
            Assert.That(sortedProjects[2].Id, Is.EqualTo(projectWithNoPriority.Id.ToString()));
        }
    }
}