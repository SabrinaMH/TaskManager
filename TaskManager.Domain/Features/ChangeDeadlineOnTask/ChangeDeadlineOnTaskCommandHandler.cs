using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.ChangeDeadlineOnTask
{
    public class ChangeDeadlineOnTaskCommandHandler 
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public ChangeDeadlineOnTaskCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        /// <exception cref="DeadlineIsInThePastException">Condition.</exception>
        public void Handle(ChangeDeadlineOnTask command)
        {
            Task task = _eventStoreRepository.GetById(command.Id);
            var newDeadline = new TaskDeadline(command.Deadine);
            task.ChangeDeadline(newDeadline);
            _eventStoreRepository.Save(task);
        }
    }
}