using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.EraseNote
{
    public class EraseNote : Command
    {
        public string TaskId { get; private set; }

        public EraseNote(string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId))
                throw new ArgumentException("taskId cannot be null or empty", "taskId");
            TaskId = taskId;
        }
    }
}