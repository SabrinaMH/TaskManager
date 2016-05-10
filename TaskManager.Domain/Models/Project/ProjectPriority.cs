using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Project
{
    public class ProjectPriority : Enumeration<ProjectPriority, int>
    {
        public static ProjectPriority None = new ProjectPriority(0, "none");
        public static ProjectPriority Low = new ProjectPriority(1, "low");
        public static ProjectPriority Medium = new ProjectPriority(2, "medium");
        public static ProjectPriority High = new ProjectPriority(3, "high");

        public ProjectPriority(int value, string displayName) : base(value, displayName) { }
    }
}