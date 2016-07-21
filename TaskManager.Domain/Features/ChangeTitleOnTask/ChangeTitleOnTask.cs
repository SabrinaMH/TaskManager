using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTitleOnTask
{
    public class ChangeTitleOnTask : Command
    {
        public string Id { get; private set; }
        public string NewTitle { get; private set; }

        public ChangeTitleOnTask(string id, string newTitle)
        {
            Id = id;
            NewTitle = newTitle;
        }
    }
}