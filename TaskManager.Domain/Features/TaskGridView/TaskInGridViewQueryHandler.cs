using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskInGridViewQueryHandler
    {
           private readonly IDocumentStore _documentStore;

           public TaskInGridViewQueryHandler()
           {
               _documentStore = new RavenDbStore().Instance;
           }

        public List<TaskInGridView> Handle(AllTasksInProjectQuery query)
        {
            using (var session = _documentStore.OpenSession())
            {
                List<TaskInGridView> tasksInGridView = session.Query<TaskInGridView>().Where(x => x.ProjectId == query.ProjectId).ToList();
                return tasksInGridView;
            }
        }

        public string Handle(TaskIdByTitleQuery query)
        {
            using (var session = _documentStore.OpenSession())
            {
                TaskInGridView taskInGridView = session.Query<TaskInGridView>().FirstOrDefault(x => x.Title == query.Title);

                if (taskInGridView == null) return null;
                return taskInGridView.Id;
            }
        }
    }
}