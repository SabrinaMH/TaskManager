using System;
using Newtonsoft.Json;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.RegisterTask
{
    public class TaskRegistered : Event
    {
        public string Deadline { get; private set; }
        public Guid TaskId { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Priority { get; private set; }

        public TaskRegistered() { }

        public TaskRegistered(Guid taskId, Guid projectId, string title, string priority)
        {
            TaskId = taskId;
            ProjectId = projectId;
            Title = title;
            Priority = priority;
        }

        [JsonConstructor]
        public TaskRegistered(Guid taskId, Guid projectId, string title, string priority, string deadline)
            : this(taskId, projectId, title, priority)
        {
            Deadline = deadline;
        }
    }
}