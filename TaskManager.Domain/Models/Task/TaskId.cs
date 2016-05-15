using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Task
{
    public class TaskId : Identity
    {
        public TaskId(string id)
        {
            Value = id;
        }

        public static TaskId Create(string title)
        {
            return new TaskId(string.Format("{0}/{1}", "task", title));
        }
    }
}