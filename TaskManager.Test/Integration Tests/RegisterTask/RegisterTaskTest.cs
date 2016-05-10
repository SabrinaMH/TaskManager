using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test.RegisterTask
{
    [TestFixture]
    public class RegisterTaskTest
    {
        IMediator _mediator;
        EventStoreRepository<Task> _eventStoreRepository;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            var mediate = new Mediate(InMemoryEventStore.Connection);
            _mediator = mediate.Mediator;
            _eventStoreRepository = new EventStoreRepository<Task>(_mediator, InMemoryEventStore.Connection);
        }

        [Test]
        public void Register_Task_Is_Saved_In_Event_Store()
        {
            var title = new Title(_fixture.Create<string>());
            var projectId = new ProjectId(Guid.NewGuid());
            var task = new Task(projectId, title, TaskPriority.Low);
            _eventStoreRepository.Save(task);

            Task taskFromEventStore = _eventStoreRepository.GetById(task.Id);
            Assert.That(taskFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Register_Task_With_Deadline_Is_Saved_In_Event_Store()
        {
            var title = new Title(_fixture.Create<string>());
            var projectId = new ProjectId(Guid.NewGuid());
            var deadline = new Deadline(DateTime.UtcNow);
            var task = new Task(projectId, title, TaskPriority.Low, deadline);
            _eventStoreRepository.Save(task);

            Task taskFromEventStore = _eventStoreRepository.GetById(task.Id);
            Assert.That(taskFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Task_Is_Added_To_Tasks_In_Grid_View()
        {
            var fixture = new Fixture();
            var title = fixture.Create<string>();
            var projectId = Guid.NewGuid();
            var registerTask = new Domain.Features.RegisterTask.RegisterTask(projectId, title, TaskPriority.Low.DisplayName, null);

            _mediator.Send(registerTask);
            var taskGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            List<TaskInGridView> tasksInGridView = taskGridViewQueryHandler.Handle(allTasksInProjectQuery);

            Assert.That(tasksInGridView.Any(x => x.Title == title));
        }
    }
}