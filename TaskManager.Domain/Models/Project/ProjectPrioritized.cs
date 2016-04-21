using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Project
{
    public class ProjectPrioritized : Event
    {
        public Guid ProjectId { get; private set; }
        public string Priority { get; private set; }

        public ProjectPrioritized(Guid projectId, string priority)
        {
            ProjectId = projectId;
            Priority = priority;
        }
    }
}