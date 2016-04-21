using System;

namespace TaskManager.Domain.Features.ViewProjectTree
{
    public class ProjectTreeNode
    {
        public string Title { get; set; }
        public string Deadline { get; set; }
        public string ProjectId { get; set; }

        public ProjectTreeNode(string projectId, string title, string deadline)
        {
            ProjectId = projectId;
            Title = title;
            Deadline = deadline;
        }
    }
}