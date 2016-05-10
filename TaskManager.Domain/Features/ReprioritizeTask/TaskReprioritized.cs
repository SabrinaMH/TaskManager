﻿using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ReprioritizeTask
{
    public class TaskReprioritized : Event
    {
        public Guid TaskId { get; private set; }
        public string OldPriority { get; private set; }
        public string NewPriority { get; private set; }

        public TaskReprioritized(Guid taskId, string oldPriority, string newPriority)
        {
            TaskId = taskId;
            OldPriority = oldPriority;
            NewPriority = newPriority;
        }
    }
}