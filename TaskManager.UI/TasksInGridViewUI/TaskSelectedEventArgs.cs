using System;

namespace TaskManager.TasksInGridViewUI
{
    public class TaskSelectedEventArgs : EventArgs
    {
        public string TaskId { get; set; }
        public string NoteContent { get; set; }

        public TaskSelectedEventArgs(string taskId, string noteContent)
        {
            if (string.IsNullOrWhiteSpace(taskId)) throw new ArgumentException("taskId cannot be null or empty", "taskId");
            
            TaskId = taskId;
            NoteContent = noteContent;
        }
    }
}