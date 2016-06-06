using System;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Features.RegisterTask
{
    public class DoesTaskWithTitleAlreadyExistUnderSameProjectQuery
    {
        public Title Title { get; private set; }
        public ProjectId ProjectId { get; set; }

        /// <exception cref="ArgumentNullException"><paramref name="projectId"/> is <see langword="null" />.</exception>
        public DoesTaskWithTitleAlreadyExistUnderSameProjectQuery(Title title, ProjectId projectId)
        {
            if (title == null) throw new ArgumentNullException("title");
            if (projectId == null) throw new ArgumentNullException("projectId");
            Title = title;
            ProjectId = projectId;
        }
    }
}