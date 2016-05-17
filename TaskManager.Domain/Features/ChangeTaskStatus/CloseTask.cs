using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class CloseTask : Command
    {
        public string Id { get; private set; }

        public CloseTask(string id)
        {
            Id = id;
        }
    }
}