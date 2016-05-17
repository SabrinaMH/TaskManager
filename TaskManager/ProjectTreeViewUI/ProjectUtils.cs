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

        /// <exception cref="ProjectDoesNotExistException">Condition.</exception>
        public string GetProjectIdBasedOnTitle(string title)
        {
            var projectIdByTitleQuery = new ProjectIdByTitleQuery(title);
            var queryHandler = new ProjectTreeViewQueryHandler();
            string projectId = queryHandler.Handle(projectIdByTitleQuery);
            if (string.IsNullOrEmpty(projectId))
            {
                _logger.Error("Could not find project id for project with title {title}", title);
                throw new ProjectDoesNotExistException(title);
            }
            return projectId;
        }
    }
}