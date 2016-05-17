using System;

namespace TaskManager.ProjectTreeViewUI
{
    public class ProjectEventArgs : EventArgs
    {
        public ProjectEventArgs(string title, DateTime? deadline)
        {
            Title = title;
            Deadline = deadline;
        }

        public string Title { get; private set; }
        public DateTime? Deadline { get; private set; }
    }
}