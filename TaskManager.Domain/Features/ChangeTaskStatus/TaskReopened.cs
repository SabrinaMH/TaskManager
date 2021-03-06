﻿using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class TaskReopened : Event
    {
        public string TaskId { get; private set; }
        public string ProjectId { get; private set; }

        public TaskReopened(string taskId, string projectId)
        {
            TaskId = taskId;
            ProjectId = projectId;
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