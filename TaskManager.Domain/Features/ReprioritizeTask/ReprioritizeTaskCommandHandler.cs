using MediatR;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.ReprioritizeTask
{
    public class ReprioritizeTaskCommandHandler : RequestHandler<ReprioritizeTask>
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public ReprioritizeTaskCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        /// <exception cref="UnknownPriorityException"></exception>
        /// <exception cref="TaskDoesNotExistException">Condition.</exception>
        protected override void HandleCore(ReprioritizeTask command)
        {
            Task task = _eventStoreRepository.GetById(command.TaskId.ToString());
            if (task == null)
                throw new TaskDoesNotExistException();

            TaskPriority priority;
            if (TaskPriority.TryParse(command.Priority.ToLower(), out priority))
            {
                task.Reprioritize(priority);
                _eventStoreRepository.Save(task);
            }
            else
            {
                throw new UnknownPriorityException(command.Priority);
            }
        }
    }
}