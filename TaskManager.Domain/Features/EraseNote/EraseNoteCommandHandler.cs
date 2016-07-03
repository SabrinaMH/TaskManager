using TaskManager.Domain.Features.ReprioritizeTask;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.EraseNote
{
    public class EraseNoteCommandHandler
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public EraseNoteCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        /// <exception cref="TaskDoesNotExistException">Condition.</exception>
        public void Handle(EraseNote command)
        {
            Task task = _eventStoreRepository.GetById(command.TaskId);
            if (task == null)
                throw new TaskDoesNotExistException();

            task.EraseNote();
            _eventStoreRepository.Save(task);
        }
    }
}