using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Project
{
    public class ProjectReprioritized : Event
    {
        public string ProjectId { get; private set; }
        public string OldPriority { get; private set; }
        public string NewPriority { get; private set; }

        public ProjectReprioritized(string projectId, string oldPriority, string newPriority)
        {
            ProjectId = projectId;
            OldPriority = oldPriority;
            NewPriority = newPriority;
        }
    }
}