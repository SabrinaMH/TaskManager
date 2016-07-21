using Raven.Client;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.RegisterTask;

namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class TaskReopenedEventHandler 
    {
        private readonly IDocumentStore _documentStore;

        public TaskReopenedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(TaskReopened @event)
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