using MediatR;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Features.RegisterProject
{
    public class RegisterProjectCommandHandler : RequestHandler<RegisterProject>
    {
        private readonly EventStoreRepository<Project> _eventStoreRepository;
        private readonly ProjectQueryService _projectQueryHandler;

        public RegisterProjectCommandHandler(EventStoreRepository<Project> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
            _projectQueryHandler = new ProjectQueryService();
        }

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
            bool projectWithSameTitleExists = _projectQueryHandler.Handle(doesProjectWithTitleExistQuery);

            if (projectWithSameTitleExists)
                throw new ProjectWithSameTitleExistsException();

            _eventStoreRepository.Save(project);
        }
    }
}