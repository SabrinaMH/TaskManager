using System.Collections.Generic;
using System.Linq;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.RegisterTask
{
    public class TaskPriorityQueryHandler
    {
        public List<string> Handle(AllTaskPrioritiesQuery query)
        {
            TaskPriority[] priorities = TaskPriority.GetAll();
            return priorities.Select(x => x.DisplayName).ToList();
        }
    }
}