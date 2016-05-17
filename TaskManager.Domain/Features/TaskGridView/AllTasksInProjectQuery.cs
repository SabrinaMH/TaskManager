namespace TaskManager.Domain.Features.TaskGridView
{
    public class AllTasksInProjectQuery
    {
        public string ProjectId { get; private set; }

        public AllTasksInProjectQuery(string projectId)
        {
            ProjectId = projectId;
        }
    }
}