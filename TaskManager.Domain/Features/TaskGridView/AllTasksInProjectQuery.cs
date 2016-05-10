using System;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class AllTasksInProjectQuery
    {
        public Guid ProjectId { get; private set; }

        public AllTasksInProjectQuery(Guid projectId)
        {
            ProjectId = projectId;
        }

    }
}