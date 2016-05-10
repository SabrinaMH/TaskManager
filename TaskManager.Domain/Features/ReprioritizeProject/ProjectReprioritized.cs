using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ReprioritizeProject
{
    public class ProjectReprioritized : Event
    {
        public Guid ProjectId { get; private set; }
        public string OldPriority { get; private set; }
        public string NewPriority { get; private set; }

        public ProjectReprioritized(Guid projectId, string oldPriority, string newPriority)
        {
            ProjectId = projectId;
            OldPriority = oldPriority;
            NewPriority = newPriority;
        }
    }
}