using System.Collections.Generic;
using System.Configuration;
using Couchbase;

namespace TaskManager.Domain.Features.ViewProjectTree
{
    public class ProjectTreeViewQueryHandler
    {
        public List<ProjectTreeNode> Handle(AllProjectTreeNodesQuery allProjectTreeNodesQuery)
        {
            var cluster = new Cluster();
            string projectTreeNodeBucket = ConfigurationManager.AppSettings.Get("couchbase.bucket.projectTreeNode");
            using (var bucket = cluster.OpenBucket(projectTreeNodeBucket))
            {
                var query = string.Format("select {0}.* from {0}", bucket.Name);
                var result = bucket.Query<ProjectTreeNode>(query);
                return result.Rows;
            }
        }
    }
}