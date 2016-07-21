using Raven.Client;
using TaskManager.Domain.Features.ChangeTaskStatus;

namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class TaskDoneEventHandler 
    {
        private readonly IDocumentStore _documentStore;

        public TaskDoneEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(TaskDone @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var projectTreeNode = session.Load<ProjectTreeNode>(@event.ProjectId);
                projectTreeNode.NumberOfOpenTasks--;
                session.SaveChanges();
            }
        }
    }
}