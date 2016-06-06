using MediatR;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class MarkTaskAsDoneCommandHandler : RequestHandler<MarkTaskAsDone>
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public MarkTaskAsDoneCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        protected override void HandleCore(MarkTaskAsDone command)
        {
            Task task = _eventStoreRepository.GetById(command.Id);
            task.Done();
            _eventStoreRepository.Save(task);
        }
    }
}