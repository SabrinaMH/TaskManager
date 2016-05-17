using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.ProjectTreeViewUI
{
    public class ProjectUtils
    {
        private ILogger _logger;

        public ProjectUtils()
        {
            _logger = Logging.Logger;
        }

        public List<ProjectTreeNode> RetrieveAllProjects()
        {
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            List<ProjectTreeNode> allProjects = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);
            return allProjects;
        }

        public string GetProjectIdBasedOnTitle(List<ProjectTreeNode> projects, string title)
        {
            try
            {
                var projectTreeNode = projects.FirstOrDefault(x => x.Title == title);
                var projectIdByTitleQuery = new ProjectIdByTitleQuery(title);
                var queryHandler = new ProjectTreeViewQueryHandler();
                string projectId = queryHandler.Handle(projectIdByTitleQuery);
                if (string.IsNullOrEmpty(projectId))
                {
                    _logger.Error("Could not find project id for project with title {title}", title);
                    throw new ProjectDoesNotExistException(title);
                }
                return projectTreeNode.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "A project with title {title} does not exist", title);
                throw new ProjectDoesNotExistException(title, ex);
            }
        }
    }
}