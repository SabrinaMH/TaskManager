using System;
using System.Collections.Generic;
using NUnit.Framework;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Test
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void Can_Reopen_Task()
        {
            var task = new Task(new ProjectId(Guid.NewGuid()), new Title("my task"), TaskPriority.Low);
            task.Done();
            task.Reopen();
            IList<Event> uncommittedEvents = task.GetUncommittedEvents();
            Assert.IsTrue(uncommittedEvents.Contains(new TaskReopened(task.Id)));
        }
    }
}