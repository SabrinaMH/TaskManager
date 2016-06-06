using TaskManager.Domain.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Models.Task
{
    public class TaskId : Identity
    {
        public TaskId(string id)
        {
            Value = id;
        }

        public static TaskId Create(ProjectId projectId, string title)
        {
            return new TaskId(string.Format("{0}/{1}/{2}", projectId, "task", title));
        }
    }
}