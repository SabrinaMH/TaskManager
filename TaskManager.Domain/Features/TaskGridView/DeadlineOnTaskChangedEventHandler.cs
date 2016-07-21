using Raven.Client;
using TaskManager.Domain.Features.ChangeDeadlineOnTask;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class DeadlineOnTaskChangedEventHandler 
    {
        private readonly IDocumentStore _documentStore;

        public DeadlineOnTaskChangedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(DeadlineOnTaskChanged @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(@event.TaskId);
                taskInGridView.Deadline = @event.NewDeadline;
                session.SaveChanges();
            }
        }
    }
}