using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ReprioritizeTask
{
    public class ReprioritizeTask : Command
    {
        public string TaskId { get; private set; }
        public string Priority { get; private set; }

        public ReprioritizeTask(string taskId, string priority)
        {
            TaskId = taskId;
            Priority = priority;
        }
    }
}