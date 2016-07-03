using System;
using NUnit.Framework;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test.ProjectIntegrationTests
{
    [TestFixture]
    public class PrioritizeProjectTest : BaseIntegrationTest
    {
        [Test]
        public void Reprioritized_Project_Can_Be_Saved_In_Event_Store()
        {
            var project = new Project(new Title("my project"), new Deadline(DateTime.UtcNow));
            project.Reprioritize(ProjectPriority.Medium);
            var eventStoreRepository = new EventStoreRepository<Project>(EventBus, InMemoryEventStoreConnectionBuilder);
            eventStoreRepository.Save(project);

            Project projectFromEventStore = eventStoreRepository.GetById(project.Id);
            Assert.That(projectFromEventStore.Version, Is.EqualTo(2));
        }
    }
}