using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Domain.Features.ProjectTreeView
{
    public class ProjectTreeViewQueryHandler
    {
            private readonly IDocumentStore _documentStore;

            public ProjectTreeViewQueryHandler()
        {
            _documentStore = new RavenDbStore().Instance;
        }

        public List<ProjectTreeNode> Handle(AllProjectTreeNodesQuery query)
        {
            using (var session = _documentStore.OpenSession())
            {
                List<ProjectTreeNode> projectTreeNodes = session.Query<ProjectTreeNode>().ToList();
                return projectTreeNodes;
            }
        }

        public string Handle(ProjectIdByTitleQuery query)
        {
            using (var session = _documentStore.OpenSession())
            {
                ProjectTreeNode projectTreeNode = session.Query<ProjectTreeNode>().FirstOrDefault(x => x.Title == query.Title);

                if (projectTreeNode == null) return null;
                return projectTreeNode.Id;
            }
        }
    }
}