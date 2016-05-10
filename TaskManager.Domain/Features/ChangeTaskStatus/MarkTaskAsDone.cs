using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class MarkTaskAsDone : Command
    {
        public Guid Id { get; private set; }

        public MarkTaskAsDone(Guid id)
        {
            Id = id;
        }
    }
}