using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class TaskReopened : Event
    {
        public Guid TaskId { get; private set; }

        public TaskReopened(Guid taskId)
        {
            TaskId = taskId;
        }

        protected bool Equals(TaskReopened other)
        {
            return TaskId.Equals(other.TaskId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TaskReopened) obj);
        }

        public override int GetHashCode()
        {
            return TaskId.GetHashCode();
        }
    }
}