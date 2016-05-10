using System;
using MediatR;
using Raven.Client;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class ProjectPrioritizedEventHandler : INotificationHandler<ProjectReprioritized>
    {
        private readonly IDocumentStore _documentStore;

        public ProjectPrioritizedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(ProjectReprioritized @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var projectTreeNode = session.Load<ProjectTreeNode>(@event.ProjectId.ToString());
                projectTreeNode.Priority = @event.NewPriority;
                session.SaveChanges();
            }
        }
    }
}