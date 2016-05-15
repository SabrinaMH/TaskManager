using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ReprioritizeProject
{
    public class ReprioritizeProject : Command
    {
        public string ProjectId { get; private set; }
        public string Priority { get; private set; }

        public ReprioritizeProject(string projectId, string priority)
        {
            ProjectId = projectId;
            Priority = priority;
        }
    }
}