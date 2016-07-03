using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.RegisterTask
{
    public class RegisterTaskCommandHandler
    {
        private readonly EventStoreRepository<Task> _eventStoreRepository;
        private readonly TaskQueryService _taskQueryHandler;

        public RegisterTaskCommandHandler(EventStoreRepository<Task> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
            _taskQueryHandler = new TaskQueryService();
        }

        /// <exception cref="UnknownPriorityException">Condition.</exception>
        /// <exception cref="TaskWithSameTitleExistsInProjectException">Condition.</exception>
        public void Handle(RegisterTask command)
        {
            var title = new Title(command.Title);
            var projectId = new ProjectId(command.ProjectId);
            TaskPriority priority;
            if (!TaskPriority.TryParse(command.Priority, out priority))
                throw new UnknownPriorityException(command.Priority);

            Task task;
            if (command.Deadline.HasValue)
            {
                var deadline = new Deadline(command.Deadline.Value);
                task = new Task(projectId, title, priority, deadline);
            }
            else
            {
                task = new Task(projectId, title, priority);
            }

            var query = new DoesTaskWithTitleAlreadyExistUnderSameProjectQuery(title, projectId);
            bool taskWithSameTitleExists = _taskQueryHandler.Handle(query);

            if (taskWithSameTitleExists)
                throw new TaskWithSameTitleExistsInProjectException();

            _eventStoreRepository.Save(task);
        }
    }
}