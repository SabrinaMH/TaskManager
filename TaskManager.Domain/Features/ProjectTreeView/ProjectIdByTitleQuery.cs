namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class ProjectIdByTitleQuery
    {
        public ProjectIdByTitleQuery(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }
}