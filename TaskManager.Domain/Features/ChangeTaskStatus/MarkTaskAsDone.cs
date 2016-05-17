using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeTaskStatus
{
    public class MarkTaskAsDone : Command
    {
        public string Id { get; private set; }

        public MarkTaskAsDone(string id)
        {
            Id = id;
        }
    }
}