using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test.TaskIntegrationTests
{
    [TestFixture]
    public class RegisterTaskTest : BaseIntegrationTest
    {
        EventStoreRepository<Task> _eventStoreRepository;
        private ProjectId _projectId;

        [SetUp]
        public void SetUp()
        {
            _eventStoreRepository = new EventStoreRepository<Task>(EventBus, InMemoryEventStoreConnectionBuilder);
            _projectId = new ProjectId(Fixture.Create<string>());
        }

        [Test]
        public void Can_Register_A_Task_With_Same_Title_Under_Several_Projects()
        {
            var firstProjectId = new ProjectId(Fixture.Create<string>());
            var secondProjectId = new ProjectId(Fixture.Create<string>());
            var taskTitle = Fixture.Create<string>();
            var registerTaskUnderFirstProject = new RegisterTask(firstProjectId, taskTitle, TaskPriority.Low.DisplayName, null);
            var registerTaskUnderSecondProject = new RegisterTask(secondProjectId, taskTitle, TaskPriority.Low.DisplayName, null);

            var registerTaskCommandHandler = new RegisterTaskCommandHandler(_eventStoreRepository);
            Assert.DoesNotThrow(() => registerTaskCommandHandler.Handle(registerTaskUnderFirstProject));
            Assert.DoesNotThrow(() => registerTaskCommandHandler.Handle(registerTaskUnderSecondProject));
        }

        [Test]
        public void Register_Task_Can_Be_Saved_In_Event_Store()
        {
            var title = new Title(Fixture.Create<string>());
            var task = new Task(_projectId, title, TaskPriority.Low);
            _eventStoreRepository.Save(task);

            Task taskFromEventStore = _eventStoreRepository.GetById(task.Id);
            Assert.That(taskFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Register_Task_With_Deadline_Can_Be_Saved_In_Event_Store()
        {
            var title = new Title(Fixture.Create<string>());
            var deadline = new TaskDeadline(DateTime.UtcNow);
            var task = new Task(_projectId, title, TaskPriority.Low, deadline);
            _eventStoreRepository.Save(task);

            Task taskFromEventStore = _eventStoreRepository.GetById(task.Id);
            Assert.That(taskFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Registered_Task_Is_Added_To_Tasks_In_Grid_View()
        {
            var eventHandler = new TaskRegisteredEventHandler(DocumentStore);
            var taskId = Fixture.Create<string>();
            var taskRegistered = new TaskRegistered(taskId, _projectId, Fixture.Create<string>(), TaskPriority.Low.DisplayName);
            eventHandler.Handle(taskRegistered);

            var taskGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(_projectId);
            List<TaskInGridView> tasksInGridView = taskGridViewQueryHandler.Handle(allTasksInProjectQuery);

            Assert.That(tasksInGridView.Any(x => x.Id == taskId));
        }

        [Test]
        public void Registered_Task_With_Deadline_Is_Added_To_Tasks_In_Grid_View()
        {
            var eventHandler = new TaskRegisteredEventHandler(DocumentStore);
            var taskId = Fixture.Create<string>();
            var taskRegistered = new TaskRegistered(taskId, _projectId, Fixture.Create<string>(), TaskPriority.Low.DisplayName, Fixture.Create<TaskDeadline>());
            eventHandler.Handle(taskRegistered);

            var taskGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(_projectId);
            List<TaskInGridView> tasksInGridView = taskGridViewQueryHandler.Handle(allTasksInProjectQuery);

            Assert.That(tasksInGridView.Any(x => x.Id == taskId));
        }
    }
}