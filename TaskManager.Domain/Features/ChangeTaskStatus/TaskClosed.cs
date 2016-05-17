using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class TaskClosed : Event
    {
        public string TaskId { get; private set; }

        public TaskClosed(string taskId)
        {
            TaskId = taskId;
        }
    }
}