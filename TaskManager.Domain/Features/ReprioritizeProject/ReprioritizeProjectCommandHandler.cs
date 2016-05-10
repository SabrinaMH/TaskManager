using MediatR;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Features.ReprioritizeProject
{
    public class ReprioritizeProjectCommandHandler : RequestHandler<ReprioritizeProject>
    {
        private readonly EventStoreRepository<Project> _eventStoreRepository;

        public ReprioritizeProjectCommandHandler(EventStoreRepository<Project> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        /// <exception cref="UnknownPriorityException"></exception>
        /// <exception cref="ProjectDoesNotExistException">Condition.</exception>
        protected override void HandleCore(ReprioritizeProject command)
        {
            Project project = _eventStoreRepository.GetById(command.ProjectId.ToString());
            if (project == null)
                throw new ProjectDoesNotExistException();

            ProjectPriority priority;
            if (ProjectPriority.TryParse(command.Priority.ToLower(), out priority))
            {
                project.Reprioritize(priority);
            }
            else
            {
                throw new UnknownPriorityException(command.Priority);
            }
        }
    }
}