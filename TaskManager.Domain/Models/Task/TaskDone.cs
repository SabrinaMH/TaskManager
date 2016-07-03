using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Task
{
    public class TaskDone : Event
    {
        public string TaskId { get; private set; }

        public TaskDone(string taskId)
        {
            TaskId = taskId;
        }
    }
}