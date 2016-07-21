using TaskManager.Domain.Common;
using TaskManager.Domain.Features.ChangeDeadlineOnTask;

namespace TaskManager.Domain.Features.ChangeTitleOnTask
{
    public class TitleOnTaskChanged : Event
    {
        public string NewTitle { get; private set; }
        public string TaskId { get; private set; }

        public TitleOnTaskChanged(string taskId, string newTitle)
        {
            TaskId = taskId;
            NewTitle = newTitle;
        }

        protected bool Equals(DeadlineOnTaskChanged other)
        {
            return string.Equals(NewTitle, other.NewDeadline) && string.Equals(TaskId, other.TaskId);
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
                return ((NewTitle != null ? NewTitle.GetHashCode() : 0)*397) ^ (TaskId != null ? TaskId.GetHashCode() : 0);
            }
        }
    }
}