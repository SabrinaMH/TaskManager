﻿using Raven.Client;
using TaskManager.Domain.Features.RegisterProject;

namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class ProjectRegisteredEventHandler
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
                var projectTreeNode = new ProjectTreeNode(@event.ProjectId, @event.Title, @event.Deadline,
                    @event.Priority, 0);
                session.Store(projectTreeNode);
                session.SaveChanges();
            }
        }
    }
}