using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test.NoteIntegrationTests
{
    [TestFixture]
    public class SaveNoteTest : BaseIntegrationTest
    {
        [Test]
        public void Task_With_Note_Can_Be_Saved_In_Event_Store()
        {
            var projectId = new ProjectId(Fixture.Create<string>());
            var title = new Title(Fixture.Create<string>());
            var deadline = new Deadline(DateTime.UtcNow);
            var task = new Task(projectId, title, TaskPriority.Low, deadline);
            task.SaveNote(new Note(Fixture.Create<string>()));
            var eventStoreRepository = new EventStoreRepository<Task>(Mediator, InMemoryEventStoreConnectionBuilder);            
            eventStoreRepository.Save(task);

            Task taskFromEventStore = eventStoreRepository.GetById(task.Id);
            Assert.That(taskFromEventStore, Is.Not.Null);
        }

        [Test]
        public void Note_Is_Retrieved_Together_With_Task()
        {
            string projectId = Fixture.Create<string>();
            string note = Fixture.Create<string>();
            using (var session = DocumentStore.OpenSession())
            {
                string id = Fixture.Create<string>();
                var taskInGridView = new TaskInGridView(id, projectId, Fixture.Create<string>(),
                    Fixture.Create<DateTime>().ToShortDateString(), TaskPriority.Lowest.DisplayName, true, note);

                session.Store(taskInGridView);
                session.SaveChanges();
            }

            var taskGridViewQueryHandler = new TaskInGridViewQueryHandler();
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            List<TaskInGridView> tasksInGridView = taskGridViewQueryHandler.Handle(allTasksInProjectQuery);

            Assert.That(tasksInGridView[0].Note, Is.EqualTo(note));
        }
    }
}