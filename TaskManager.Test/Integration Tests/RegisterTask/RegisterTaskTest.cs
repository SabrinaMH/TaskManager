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
    public class RegisterTaskTest : BaseIntegrationTest
    {
        EventStoreRepository<Task> _eventStoreRepository;
        private ProjectId _projectId;

        [SetUp]
        public void SetUp()
        {
            _eventStoreRepository = new EventStoreRepository<Task>(Mediator, InMemoryEventStoreConnectionBuilder);
            _projectId = new ProjectId(Fixture.Create<string>());
        }

        [Test]
        public void Register_Task_Is_Saved_In_Event_Store()
        {
            var title = new Title(Fixture.Create<string>());
            var task = new Task(_projectId, title, TaskPriority.Low);
            _eventStoreRepository.Save(task);

            Task taskFromEventStore = _eventStoreRepository.GetById(task.Id);
            Assert.That(taskFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Register_Task_With_Deadline_Is_Saved_In_Event_Store()
        {
            var title = new Title(Fixture.Create<string>());
            var deadline = new Deadline(DateTime.UtcNow);
            var task = new Task(_projectId, title, TaskPriority.Low, deadline);
            _eventStoreRepository.Save(task);

            Task taskFromEventStore = _eventStoreRepository.GetById(task.Id);
            Assert.That(taskFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Task_Is_Added_To_Tasks_In_Grid_View()
        {
            var title = Fixture.Create<string>();
            var registerTask = new Domain.Features.RegisterTask.RegisterTask(_projectId, title, TaskPriority.Low.DisplayName, null);

            Mediator.Send(registerTask);
            var taskGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(_projectId);
            List<TaskInGridView> tasksInGridView = taskGridViewQueryHandler.Handle(allTasksInProjectQuery);

            Assert.That(tasksInGridView.Any(x => x.Title == title));
        }
    }
}