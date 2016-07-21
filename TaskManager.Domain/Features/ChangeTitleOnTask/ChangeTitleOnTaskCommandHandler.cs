using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.ChangeTitleOnTask
{
    public class ChangeTitleOnTaskCommandHandler 
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;

        public ChangeTitleOnTaskCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public void Handle(ChangeTitleOnTask command)
        {
            Task task = _eventStoreRepository.GetById(command.Id);
            var newTitle = new Title(command.NewTitle);
            task.ChangeTitle(newTitle);
            _eventStoreRepository.Save(task);
        }
    }
}