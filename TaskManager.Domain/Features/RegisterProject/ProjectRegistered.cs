using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.RegisterProject
{
    public class ProjectRegistered : Event
    {
        public string Deadline { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }

        public ProjectRegistered() { }

        public ProjectRegistered(Guid projectId, string title)
        {
            ProjectId = projectId;
            Title = title;
        }

        public ProjectRegistered(Guid projectId, string title, string deadline) : this(projectId, title)
        {
            Deadline = deadline;
        }
    }
}