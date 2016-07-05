using Raven.Client;
using TaskManager.Domain.Features.SaveNote;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class NoteSavedEventHandler 
    {
        private readonly IDocumentStore _documentStore;

        public NoteSavedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(NoteSaved @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(@event.TaskId);
                taskInGridView.Note = @event.NoteContent;
                session.SaveChanges();
            }
        }
    }
}