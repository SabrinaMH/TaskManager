using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class TaskDone : Event
    {
        public string TaskId { get; private set; }
        public string ProjectId { get; private set; }

        public TaskDone(string taskId, string projectId)
        {
            TaskId = taskId;
            ProjectId = projectId;
        }
    }
}