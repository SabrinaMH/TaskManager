﻿using Raven.Client;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class ProjectReprioritizedEventHandler
    {
        private readonly IDocumentStore _documentStore;

        public ProjectReprioritizedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(ProjectReprioritized @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var projectTreeNode = session.Load<ProjectTreeNode>(@event.ProjectId);
                projectTreeNode.Priority = @event.NewPriority;
                session.SaveChanges();
            }
        }
    }
}