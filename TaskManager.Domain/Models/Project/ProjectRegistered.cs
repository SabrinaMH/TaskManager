using Newtonsoft.Json;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Project
{
    public class ProjectRegistered : Event
    {
        public string Deadline { get; private set; }
        public string ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Priority { get; private set; }

        public ProjectRegistered() { }

        public ProjectRegistered(string projectId, string title, string priority)
        {
            ProjectId = projectId;
            Title = title;
            Priority = priority;
        }

        [JsonConstructor]
        public ProjectRegistered(string projectId, string title, string priority, string deadline)
            : this(projectId, title, priority)
        {
            Deadline = deadline;
        }

        protected bool Equals(ProjectRegistered other)
        {
            return string.Equals(Deadline, other.Deadline) && string.Equals(ProjectId, other.ProjectId) && string.Equals(Title, other.Title) && string.Equals(Priority, other.Priority);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectRegistered) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Deadline != null ? Deadline.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ProjectId != null ? ProjectId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Priority != null ? Priority.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}