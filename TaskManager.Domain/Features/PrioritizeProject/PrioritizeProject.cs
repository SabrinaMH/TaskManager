using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.PrioritizeProject
{
    public class PrioritizeProject : Command
    {
        public string ProjectId { get; private set; }
        public string Priority { get; private set; }

        public PrioritizeProject(string projectId, string priority)
        {
            ProjectId = projectId;
            Priority = priority;
        }
    }
}