using System;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskInGridView
    {
        public string Id { get; set; }
        public string ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Deadline { get; private set; }
        public string Priority { get; private set; }
        public bool IsDone { get; set; }

        public TaskInGridView()
        {

        }

        public TaskInGridView(string taskId, string projectId, string title, string deadline, string priority, bool isDone)
        {
            Id = taskId;
            ProjectId = projectId;
            Title = title;
            Deadline = deadline;
            Priority = priority;
            IsDone = isDone;
        }
    }
}