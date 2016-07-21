using Raven.Client;
using TaskManager.Domain.Features.RegisterTask;

namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class TaskRegisteredEventHandler 
    {
        private readonly IDocumentStore _documentStore;

        public TaskRegisteredEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(TaskRegistered @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var projectTreeNode = session.Load<ProjectTreeNode>(@event.ProjectId);
                projectTreeNode.NumberOfOpenTasks++;
                session.SaveChanges();
            }
        }
    }
}