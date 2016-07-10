using System;

namespace TaskManager.ProjectTreeViewUI
{
    public class ProjectRegisteredEventArgs : EventArgs
    {
        public ProjectRegisteredEventArgs(string title, DateTime? deadline)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("title cannot be null or empty", "title");
            Title = title;
            Deadline = deadline;
        }

        public string Title { get; private set; }
        public DateTime? Deadline { get; private set; }
    }
}