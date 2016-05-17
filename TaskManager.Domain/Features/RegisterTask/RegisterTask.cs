using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.RegisterTask
{
    public class RegisterTask : Command
    {
        public string ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Priority { get; set; }
        public DateTime? Deadline { get; private set; }

        public RegisterTask(string projectId, string title, string priority, DateTime? deadline)
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId cannot be null or empty", "projectId");
            if (string.IsNullOrWhiteSpace(priority)) throw new ArgumentException("priority cannot be null or empty", "priority");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("title cannot be null or empty", "title");
            ProjectId = projectId;
            Title = title;
            Priority = priority;
            Deadline = deadline;
        }
    }
}