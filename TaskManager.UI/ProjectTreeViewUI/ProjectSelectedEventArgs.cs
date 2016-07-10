using System;

namespace TaskManager.ProjectTreeViewUI
{
    public class ProjectSelectedEventArgs : EventArgs
    {
        public string SelectedProjectId { get; set; }

        public ProjectSelectedEventArgs(string selectedProjectId)
        {
            if (string.IsNullOrWhiteSpace(selectedProjectId)) throw new ArgumentException("selectedProjectId cannot be null or empty", "selectedProjectId");
            SelectedProjectId = selectedProjectId;
        }
    }
}