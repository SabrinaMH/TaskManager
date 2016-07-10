using System;

namespace TaskManager.TasksInGridViewUI
{
    public class TaskRegisteredEventArgs : EventArgs
    {
        public string Title { get; private set; }
        public string Priority { get; private set; }
        public DateTime? Deadline { get; private set; }
        public string ProjectId { get; private set; }

        public TaskRegisteredEventArgs(string projectId, string title, string priority, DateTime? deadline)
        {
            ProjectId = projectId;
            Title = title;
            Priority = priority;
            Deadline = deadline;
        }
    }
}