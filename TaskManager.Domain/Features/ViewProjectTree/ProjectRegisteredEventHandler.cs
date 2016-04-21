using System.Configuration;
using Couchbase;
using MediatR;
using TaskManager.Domain.Features.RegisterProject;

namespace TaskManager.Domain.Features.ViewProjectTree
{
    public class ProjectRegisteredEventHandler : INotificationHandler<ProjectRegistered>
    {
        public void Handle(ProjectRegistered @event)
        {
            var cluster = new Cluster();
            string projectTreeViewBucket = ConfigurationManager.AppSettings["couchbase.bucket.projectTreeNode"];
            using (var bucket = cluster.OpenBucket(projectTreeViewBucket))
            {
                var document = new Document<ProjectTreeNode>
                {
                    Id = @event.ProjectId.ToString(),
                    Content = new ProjectTreeNode(@event.ProjectId.ToString(), @event.Title, @event.Deadline)
                };

                bucket.Insert(document);
            }
        }
    }
}