using System;
using MediatR;
using Raven.Client;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class ProjectRegisteredEventHandler : INotificationHandler<ProjectRegistered>
    {
        private readonly IDocumentStore _documentStore;

        public ProjectRegisteredEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(ProjectRegistered @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var projectTreeNode = new ProjectTreeNode(@event.ProjectId.ToString(), @event.Title, @event.Deadline,
                    @event.Priority);
                session.Store(projectTreeNode);
                session.SaveChanges();
            }
        }
    }
}