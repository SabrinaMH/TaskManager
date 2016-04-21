using System.Collections.Generic;
using System.Linq;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Features.PrioritizeProject
{
    public class PriorityQueryHandler
    {
        public List<string> Handle(AllPrioritiesQuery query)
        {
            Priority[] priorities = Priority.GetAll();
            return priorities.Select(x => x.DisplayName).ToList();
        }
    }
}