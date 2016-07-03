using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.SaveNote
{
    public class SaveNote : Command
    {
        public string TaskId { get; private set; }
        public string Note { get; private set; }

        public SaveNote(string taskId, string note)
        {
            if (string.IsNullOrWhiteSpace(taskId)) throw new ArgumentException("taskId cannot be null or empty", "taskId");
            if (string.IsNullOrWhiteSpace(note)) throw new ArgumentException("note cannot be null or empty", "note");
            TaskId = taskId;
            Note = note;
        }
    }
}