using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.EraseNote
{
    public class NoteErased : Event
    {
        public string TaskId { get; private set; }

        public NoteErased(string taskId)
        {
            TaskId = taskId;
        }

        protected bool Equals(NoteErased other)
        {
            return string.Equals(TaskId, other.TaskId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NoteErased) obj);
        }

        public override int GetHashCode()
        {
            return (TaskId != null ? TaskId.GetHashCode() : 0);
        }
    }
}