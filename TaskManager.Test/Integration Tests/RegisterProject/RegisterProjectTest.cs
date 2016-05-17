using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test.RegisterProject
{
    [TestFixture]
    public class RegisterProjectTest : BaseIntegrationTest
    {
        EventStoreRepository<Project> _eventStoreRepository;

        [SetUp]
        public void SetUp()
        {
            _eventStoreRepository = new EventStoreRepository<Project>(Mediator, InMemoryEventStoreConnectionBuilder);
        }

        [Test]
        public void Register_Project_Is_Saved_In_Event_Store()
        {
            var title = Fixture.Create<string>();
            var project = new Project(new Title(title));
            _eventStoreRepository.Save(project);

            Project projectFromEventStore = _eventStoreRepository.GetById(project.Id);
            Assert.That(projectFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Register_Project_With_Deadline_Is_Saved_In_Event_Store()
        {
            var title = Fixture.Create<string>();
            var project = new Project(new Title(title), new Deadline(DateTime.UtcNow));
            _eventStoreRepository.Save(project);

            Project projectFromEventStore = _eventStoreRepository.GetById(project.Id);
            Assert.That(projectFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Project_Is_Added_To_Project_Tree_View()
        {
            var title = Fixture.Create<string>();
            var registerProject = new Domain.Features.RegisterProject.RegisterProject(title, null);

            Mediator.Send(registerProject);
            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            List<ProjectTreeNode> projectTreeNodes = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);

            Assert.That(projectTreeNodes.Any(x => x.Title == title));
        }

        [Test]
        public void Project_With_Deadline_Is_Added_To_Project_Tree_View()
        {
            var title = Fixture.Create<string>();
            DateTime deadline = DateTime.UtcNow;
            var registerProject = new Domain.Features.RegisterProject.RegisterProject(title, deadline);

            Mediator.Send(registerProject);
            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            List<ProjectTreeNode> projectTreeNodes = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);

            Assert.That(projectTreeNodes.Any(x => x.Deadline == deadline.ToShortDateString()));
        }
    }
}