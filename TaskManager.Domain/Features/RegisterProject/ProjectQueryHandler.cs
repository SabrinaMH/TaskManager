using System.Configuration;
using Couchbase;
using TaskManager.Domain.Features.ViewProjectTree;

namespace TaskManager.Domain.Features.RegisterProject
{
    public class ProjectQueryHandler
    {
        public bool Handle(DoesProjectWithTitleExistQuery query)
        {
            var cluster = new Cluster();
            string projectTreeNodeBucket = ConfigurationManager.AppSettings.Get("couchbase.bucket.projectTreeNode");
            using (var bucket = cluster.OpenBucket(projectTreeNodeBucket))
            {
                var databaseQuery = string.Format("select count(*) from {0} where title = '{1}'", bucket.Name, query.Title);
                var result = bucket.Query<ProjectTreeNode>(databaseQuery);
                return result.Rows.Count > 0;
            }
        }
    }
}