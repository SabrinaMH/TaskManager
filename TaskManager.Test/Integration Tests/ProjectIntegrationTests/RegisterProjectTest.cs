﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test.ProjectIntegrationTests
{
    [TestFixture]
    public class RegisterProjectTest : BaseIntegrationTest
    {
        EventStoreRepository<Project> _eventStoreRepository;

        [SetUp]
        public void SetUp()
        {
            _eventStoreRepository = new EventStoreRepository<Project>(EventBus, InMemoryEventStoreConnectionBuilder);
        }

        [Test]
        public void Register_Project_Can_Be_Saved_In_Event_Store()
        {
            var title = Fixture.Create<string>();
            var project = new Project(new Title(title));
            _eventStoreRepository.Save(project);

            Project projectFromEventStore = _eventStoreRepository.GetById(project.Id);
            Assert.That(projectFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Register_Project_With_Deadline_Can_Be_Saved_In_Event_Store()
        {
            var title = Fixture.Create<string>();
            var project = new Project(new Title(title), new ProjectDeadline(DateTime.UtcNow));
            _eventStoreRepository.Save(project);

            Project projectFromEventStore = _eventStoreRepository.GetById(project.Id);
            Assert.That(projectFromEventStore, Is.Not.Null);
        }

        
        [Test]
        public void Registered_Project_Is_Added_To_Project_Tree_View()
        {
            var eventHandler = new ProjectRegisteredEventHandler(DocumentStore);
            var projectId = Fixture.Create<string>();
            var projectRegistered = new ProjectRegistered(projectId, Fixture.Create<string>(), ProjectPriority.Low.DisplayName);
            eventHandler.Handle(projectRegistered);

            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            List<ProjectTreeNode> projectTreeNodes = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);

            Assert.That(projectTreeNodes.Any(x => x.Id == projectId));
        }

        [Test]
        public void Registered_Project_With_Deadline_Is_Added_To_Project_Tree_View()
        {
            var eventHandler = new ProjectRegisteredEventHandler(DocumentStore);
            var projectId = Fixture.Create<string>();
            var projectRegistered = new ProjectRegistered(projectId, Fixture.Create<string>(), ProjectPriority.Low.DisplayName, Fixture.Create<ProjectDeadline>());
            eventHandler.Handle(projectRegistered);

            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            List<ProjectTreeNode> projectTreeNodes = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);

            Assert.That(projectTreeNodes.Any(x => x.Id == projectId));
        }
    }
}