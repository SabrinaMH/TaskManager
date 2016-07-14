
namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskInGridView
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Title { get; set; }
        public string Deadline { get; set; }
        public string Priority { get; set; }
        public bool IsDone { get; set; }
        public string Note { get; set; }

        public TaskInGridView(string taskId, string projectId, string title, string deadline, string priority, bool isDone, string note = "")
        {
            Id = taskId;
            ProjectId = projectId;
            Title = title;
            Deadline = deadline;
            Priority = priority;
            IsDone = isDone;
            Note = note;
        }
    }
}