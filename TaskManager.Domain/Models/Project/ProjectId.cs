using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Models.Project
{
    public class ProjectId : Identity
    {
        public ProjectId(Guid id)
        {
            Value = id;
        }
    }
}