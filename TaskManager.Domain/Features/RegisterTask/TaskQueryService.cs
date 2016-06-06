using System.Linq;
using Raven.Client;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Domain.Features.RegisterTask
{
    public class TaskQueryService
    {
          private readonly IDocumentStore _documentStore;

          public TaskQueryService()
          {
              _documentStore = new RavenDbStore().Instance;
          }

        public bool Handle(DoesTaskWithTitleAlreadyExistUnderSameProjectQuery query)
        {
            using (var session = _documentStore.OpenSession())
            {
                bool doesTaskExist = session.Query<TaskInGridView>().Any(x => x.Title == query.Title && x.ProjectId == query.ProjectId);
                return doesTaskExist;
            }
        }
    }
}