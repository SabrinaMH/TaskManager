using Raven.Client;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class NoteErasedEventHandler 
    {
        private readonly IDocumentStore _documentStore;

        public NoteErasedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(NoteErased @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(@event.TaskId);
                taskInGridView.Note = null;
                session.SaveChanges();
            }
        }
    }
}