using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class MarkTaskAsDoneCommandHandler 
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public MarkTaskAsDoneCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public void Handle(MarkTaskAsDone command)
        {
            Task task = _eventStoreRepository.GetById(command.Id);
            task.Done();
            _eventStoreRepository.Save(task);
        }
    }
}