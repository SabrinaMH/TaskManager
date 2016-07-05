using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.SaveNote
{
    public class NoteSaved : Event
    {
        public string TaskId { get; private set; }
        public string NoteContent { get; private set; }

        public NoteSaved(string taskId, string noteContent)
        {
            NoteContent = noteContent;
            TaskId = taskId;
        }

        protected bool Equals(NoteSaved other)
        {
            return string.Equals(TaskId, other.TaskId) && string.Equals(NoteContent, other.NoteContent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NoteSaved) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TaskId != null ? TaskId.GetHashCode() : 0)*397) ^ (NoteContent != null ? NoteContent.GetHashCode() : 0);
            }
        }
    }
}