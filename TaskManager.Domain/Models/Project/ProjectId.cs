
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Project
{
    public class ProjectId : Identity
    {
        public ProjectId(string id)
        {
            Value = id;
        }

        public static ProjectId Create(string title)
        {
            return new ProjectId(string.Format("{0}/{1}", "project", title));
        }
    }
}