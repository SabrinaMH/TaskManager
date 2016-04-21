using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Project
{
    public class Priority : Enumeration<Priority, int>
    {
        public static Priority Low = new Priority(0, "low");
        public static Priority Medium = new Priority(1, "medium");
        public static Priority High = new Priority(2, "high");

        public Priority(int value, string displayName) : base(value, displayName) { }
    }
}