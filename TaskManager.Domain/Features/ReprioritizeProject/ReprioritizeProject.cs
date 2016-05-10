using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ReprioritizeProject
{
    public class ReprioritizeProject : Command
    {
        public Guid ProjectId { get; private set; }
        public string Priority { get; private set; }

        public ReprioritizeProject(Guid projectId, string priority)
        {
            ProjectId = projectId;
            Priority = priority;
        }
    }
}