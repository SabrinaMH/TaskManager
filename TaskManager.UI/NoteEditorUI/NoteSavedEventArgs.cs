using System;

namespace TaskManager.NoteEditorUI
{
    public class NoteSavedEventArgs : EventArgs
    {
        public string Content { get; private set; }
        public string TaskId { get; private set; }

        public NoteSavedEventArgs(string content, string taskId)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("content cannot be null or empty", "content");
            if (string.IsNullOrWhiteSpace(taskId)) throw new ArgumentException("taskId cannot be null or empty", "taskId");
            
            TaskId = taskId;
            Content = content;
        }
    }
}