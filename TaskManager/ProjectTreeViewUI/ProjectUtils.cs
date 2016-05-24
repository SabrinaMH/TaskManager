using System.Collections.Generic;
using TaskManager.Domain.Features.ProjectTreeView;

namespace TaskManager.ProjectTreeViewUI
{
    public class ProjectUtils
    {
        public List<ProjectTreeNode> RetrieveAllProjects()
        {
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            List<ProjectTreeNode> allProjects = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);
            return allProjects;
        }
    }
}