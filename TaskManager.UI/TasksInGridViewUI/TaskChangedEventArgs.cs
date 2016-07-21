using System;

namespace TaskManager.TasksInGridViewUI
{
    public class TaskChangedEventArgs : EventArgs
    {
        public string TaskId { get; private set; }
        public string NewTitle { get; private set; }
        public DateTime NewDeadline { get; private set; }
        public string OldTitle { get; private set; }

        public TaskChangedEventArgs(string taskId, string title, DateTime newDeadline, string oldTitle)
        {
            OldTitle = oldTitle;
            TaskId = taskId;
            NewDeadline = newDeadline;
            NewTitle = title;
        }
    }
}