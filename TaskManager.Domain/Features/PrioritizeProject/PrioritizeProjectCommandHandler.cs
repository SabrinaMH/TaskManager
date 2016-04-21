using MediatR;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Features.PrioritizeProject
{
    public class PrioritizeProjectCommandHandler : RequestHandler<PrioritizeProject>
    {
        /// <exception cref="ProjectDoesNotExistException">Condition.</exception>
        protected override void HandleCore(PrioritizeProject command)
        {
            var eventStoreRepository = new EventStoreRepository<Project>();
            Project project = eventStoreRepository.GetById(command.ProjectId);
            if (project == null)
                throw new ProjectDoesNotExistException();

            Priority priority;
            if (Priority.TryParse(command.Priority.ToLower(), out priority))
            {
                project.Prioritize(priority);

            }
            else
            {
                throw new UnknownPriorityException(command.Priority);
            }
        }
    }
}