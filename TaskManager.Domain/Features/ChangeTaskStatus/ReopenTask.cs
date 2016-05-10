using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class ReopenTask : Command
    {
        public Guid Id { get; private set; }

        public ReopenTask(Guid id)
        {
            Id = id;
        }
    }
}