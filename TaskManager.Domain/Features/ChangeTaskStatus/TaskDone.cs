using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class TaskDone : Event
    {
        public Guid TaskId { get; private set; }

        public TaskDone(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}