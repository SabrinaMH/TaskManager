using System.Linq;
using Raven.Client;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Domain.Features.RegisterProject
{
    public class ProjectQueryService
    {
        private readonly IDocumentStore _documentStore;

        public ProjectQueryService()
        {
            _documentStore = new RavenDbStore().Instance;
        }

        public bool Handle(DoesProjectWithTitleExistQuery query)
        {
            using (var session = _documentStore.OpenSession())
            {
                bool doesProjectExist = session.Query<ProjectTreeNode>().Any(x => x.Title == query.Title);
                return doesProjectExist;
            }
        }
    }
}