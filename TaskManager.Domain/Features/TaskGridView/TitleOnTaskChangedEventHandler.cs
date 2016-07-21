using Raven.Client;
using TaskManager.Domain.Features.ChangeTitleOnTask;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TitleOnTaskChangedEventHandler 
    {
        private readonly IDocumentStore _documentStore;

        public TitleOnTaskChangedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(TitleOnTaskChanged @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(@event.TaskId);
                taskInGridView.Title = @event.NewTitle;
                session.SaveChanges();
            }
        }
    }
}