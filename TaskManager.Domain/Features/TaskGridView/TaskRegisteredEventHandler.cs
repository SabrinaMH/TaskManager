using Raven.Client;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskRegisteredEventHandler
    {
          private readonly IDocumentStore _documentStore;

          public TaskRegisteredEventHandler(IDocumentStore documentStore)
          {
              _documentStore = documentStore;
          }

        public void Handle(TaskRegistered @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = new TaskInGridView(@event.TaskId, @event.ProjectId, @event.Title, @event.Deadline, @event.Priority,
                    false);
                session.Store(taskInGridView);
                session.SaveChanges();
            }
        }
    }
}