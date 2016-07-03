using System;

namespace TaskManager.NoteEditorUI
{
    public class NoteErasedEventArgs : EventArgs
    {
        public string TaskId { get; private set; }

        public NoteErasedEventArgs(string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId)) throw new ArgumentException("taskId cannot be null or empty", "taskId");
            
            TaskId = taskId;
        }
    }
}