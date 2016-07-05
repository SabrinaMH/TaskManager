using Raven.Client;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskReopenedEventHandler
    {
        private readonly IDocumentStore _documentStore;

        public TaskReopenedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(TaskReopened @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(@event.TaskId);
                taskInGridView.IsDone = false;
                session.SaveChanges();
            }
        }
    }
}