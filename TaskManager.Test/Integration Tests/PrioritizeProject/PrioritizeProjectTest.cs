using System;
using MediatR;
using NUnit.Framework;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Test.PrioritizeProject
{
    [TestFixture]
    public class PrioritizeProjectTest
    {
        [Test]
        public void Can_Prioritize_Project()
        {
            var mediate = new Mediate(InMemoryEventStore.Connection);
            var eventStoreRepository = new EventStoreRepository<Project>(mediate.Mediator, InMemoryEventStore.Connection);
            var project = new Project(new Title("my project"), new Deadline(DateTime.UtcNow));
            project.Reprioritize(ProjectPriority.Medium);
            eventStoreRepository.Save(project);

            Project projectFromEventStore = eventStoreRepository.GetById(project.Id);
            Assert.That(projectFromEventStore.Version, Is.EqualTo(2));
        }
    }
}