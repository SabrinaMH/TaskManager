using System.Collections.Generic;
using TaskManager.Domain.Features.TaskGridView;

namespace TaskManager.TasksInGridViewUI
{
    public class TaskUtils
    {
       
        public List<TaskInGridView> RetrieveAllTasksInProject(string projectId)
        {
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            List<TaskInGridView> tasksInProject = taskInGridViewQueryHandler.Handle(allTasksInProjectQuery);
            return tasksInProject;
        }
    }
}