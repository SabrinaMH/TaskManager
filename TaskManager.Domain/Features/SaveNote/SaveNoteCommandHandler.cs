using TaskManager.Domain.Features.ReprioritizeTask;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.SaveNote
{
    public class SaveNoteCommandHandler 
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public SaveNoteCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        /// <exception cref="TaskDoesNotExistException">Condition.</exception>
        public void Handle(SaveNote command)
        {
            Task task = _eventStoreRepository.GetById(command.TaskId);
            if (task == null)
                throw new TaskDoesNotExistException();

            var note = new Note(command.Note);
            task.SaveNote(note);
            _eventStoreRepository.Save(task);
        }
    }
}