using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class ReopenTaskCommandHandler
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public ReopenTaskCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public void Handle(ReopenTask command)
        {
            Task task = _eventStoreRepository.GetById(command.Id.ToString());
            task.Reopen();
            _eventStoreRepository.Save(task);
        }
    }
}