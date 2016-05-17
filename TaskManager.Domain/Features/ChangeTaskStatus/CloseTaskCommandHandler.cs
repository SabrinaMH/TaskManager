using MediatR;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class CloseTaskCommandHandler : RequestHandler<CloseTask>
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public CloseTaskCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        protected override void HandleCore(CloseTask command)
        {
            Task task = _eventStoreRepository.GetById(command.Id.ToString());
            task.Close();
            _eventStoreRepository.Save(task);
        }
    }
}