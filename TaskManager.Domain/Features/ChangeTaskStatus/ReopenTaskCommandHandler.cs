using MediatR;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class ReopenTaskCommandHandler : RequestHandler<ReopenTask>
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public ReopenTaskCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        protected override void HandleCore(ReopenTask command)
        {
            Task task = _eventStoreRepository.GetById(command.Id.ToString());
            task.Reopen();
            _eventStoreRepository.Save(task);
        }
    }
}