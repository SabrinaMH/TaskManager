using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeDeadlineOnTask
{
    public class ChangeDeadlineOnTask : Command
    {
        public string Id { get; private set; }
        public DateTime Deadine { get; private set; }

        public ChangeDeadlineOnTask(string id, DateTime deadine)
        {
            Id = id;
            Deadine = deadine;
        }
    }
}