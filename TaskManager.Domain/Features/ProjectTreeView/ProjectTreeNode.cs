namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class ProjectTreeNode
    {
        public string Title { get; set; }
        public string Deadline { get; set; }
        public string Id { get; set; }
        public string Priority { get; set; }
        public int NumberOfOpenTasks { get; set; }

        public ProjectTreeNode()
        {
        }

        public ProjectTreeNode(string projectId, string title, string deadline, string priority, int numberOfOpenTasks)
        {
            Id = projectId;
            Title = title;
            Deadline = deadline;
            Priority = priority;
            NumberOfOpenTasks = numberOfOpenTasks;
        }
    }
}