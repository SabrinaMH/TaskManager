using MediatR;
using Raven.Client;
using TaskManager.Domain.Features.ChangeTaskStatus;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskClosedEventHandler : INotificationHandler<TaskClosed>
    {
        private readonly IDocumentStore _documentStore;

        public TaskClosedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(TaskClosed @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(@event.TaskId);
                taskInGridView.IsDone = true;
                session.SaveChanges();
            }
        }
    }
}