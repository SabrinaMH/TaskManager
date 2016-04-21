using MediatR;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Features.RegisterProject
{
    public class RegisterProjectCommandHandler : RequestHandler<RegisterProject>
    {
        /// <exception cref="ProjectWithSameTitleExistsException">Condition.</exception>
        protected override void HandleCore(RegisterProject command)
        {
            var title = new Title(command.Title);
            Project project;
            if (command.Deadline.HasValue)
            {
                var deadline = new Deadline(command.Deadline.Value);
                project = new Project(title, deadline);
            }
            else
            {
                project = new Project(title);
            }

            var doesProjectWithTitleExistQuery = new DoesProjectWithTitleExistQuery(title);
            var projectQueryHandler = new ProjectQueryHandler();
            bool projectWithSameTitleExists = projectQueryHandler.Handle(doesProjectWithTitleExistQuery);

            if (projectWithSameTitleExists)
                throw new ProjectWithSameTitleExistsException();

            var eventStoreRepository = new EventStoreRepository<Project>();
            eventStoreRepository.Save(project);
        }
    }
}