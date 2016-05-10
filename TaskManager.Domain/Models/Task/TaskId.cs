using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Task
{
    public class TaskId : Identity
    {
        public TaskId(Guid id)
        {
            Value = id;
        }
    }
}