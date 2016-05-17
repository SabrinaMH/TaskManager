using System.Collections.Generic;
using System.Linq;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Features.ReprioritizeProject
{
    public class ProjectPriorityQueryHandler
    {
        public List<string> Handle(AllProjectPrioritiesQuery query)
        {
            ProjectPriority[] priorities = ProjectPriority.GetAll();
            return priorities.Select(x => x.DisplayName).ToList();
        }
    }
}