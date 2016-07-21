using System.Collections.Generic;
using NUnit.Framework;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.SaveNote;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void Reopen_Task_Raises_Event()
        {
            var projectId = new ProjectId("my project");
            var task = new Task(projectId, new Title("my task"), TaskPriority.Low);
            task.Done();
            task.Reopen();
            IList<Event> uncommittedEvents = task.GetUncommittedEvents();
            Assert.IsTrue(uncommittedEvents.Contains(new TaskReopened(task.Id, projectId)));
        }

        [Test]
        public void Save_Note_Raises_Event()
        {
            var task = new Task(new ProjectId("my project"), new Title("my task"), TaskPriority.Low);
            var note = new Note("this is my note");
            task.SaveNote(note);
            IList<Event> uncommittedEvents = task.GetUncommittedEvents();
            Assert.IsTrue(uncommittedEvents.Contains(new NoteSaved(task.Id, note.Content)));
        }
    }
}