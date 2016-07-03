using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Task
{
    public class TaskReprioritized : Event
    {
        public string TaskId { get; private set; }
        public string OldPriority { get; private set; }
        public string NewPriority { get; private set; }

        public TaskReprioritized(string taskId, string oldPriority, string newPriority)
        {
            TaskId = taskId;
            OldPriority = oldPriority;
            NewPriority = newPriority;
        }
    }
}