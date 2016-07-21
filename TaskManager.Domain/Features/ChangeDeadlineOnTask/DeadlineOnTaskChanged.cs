using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.ChangeDeadlineOnTask
{
    public class DeadlineOnTaskChanged : Event
    {
        public string NewDeadline { get; private set; }
        public string TaskId { get; private set; }

        public DeadlineOnTaskChanged(string taskId, string newDeadline)
        {
            TaskId = taskId;
            NewDeadline = newDeadline;
        }

        protected bool Equals(DeadlineOnTaskChanged other)
        {
            return string.Equals(NewDeadline, other.NewDeadline) && string.Equals(TaskId, other.TaskId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DeadlineOnTaskChanged) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((NewDeadline != null ? NewDeadline.GetHashCode() : 0)*397) ^ (TaskId != null ? TaskId.GetHashCode() : 0);
            }
        }
    }
}