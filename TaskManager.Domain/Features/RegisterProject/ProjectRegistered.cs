using System;
using Newtonsoft.Json;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.RegisterProject
{
    public class ProjectRegistered : Event
    {
        public string Deadline { get; private set; }
        public string ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Priority { get; private set; }

        public ProjectRegistered() { }

        public ProjectRegistered(string projectId, string title, string priority)
        {
            ProjectId = projectId;
            Title = title;
            Priority = priority;
        }

        [JsonConstructor]
        public ProjectRegistered(string projectId, string title, string priority, string deadline)
            : this(projectId, title, priority)
        {
            Deadline = deadline;
        }
    }
}