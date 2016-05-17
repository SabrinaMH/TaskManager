using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class ReopenTask : Command
    {
        public string Id { get; private set; }

        public ReopenTask(string id)
        {
            Id = id;
        }
    }
}