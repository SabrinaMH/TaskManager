using System.Collections.Generic;
using TaskManager.Domain.Features.ProjectTreeView;

namespace TaskManager.ProjectTreeViewUI
{
    public class Sorter
    {
        public List<ProjectTreeNode> ByPriority(List<ProjectTreeNode> nodes)
        {
            nodes.Sort(Compare);
            return nodes;
        }

        private int Compare(ProjectTreeNode nodeA, ProjectTreeNode nodeB)
        {
            if (nodeA.Priority.Equals(nodeB.Priority)) 
                return string.Compare(nodeA.Title, nodeB.Title);

            if (nodeA.Priority.Equals("none"))
            {
                return 1;
            }

            if (nodeA.Priority.Equals("low"))
            {
                if (nodeB.Priority.Equals("none")) return -1;
                return 1;
            }

            if (nodeA.Priority.Equals("medium"))
            {
                if (nodeB.Priority.Equals("high")) return 1;
                return -1;
            }

            if (nodeA.Priority.Equals("high"))
            {
                return -1;
            }

            throw new SortFailedException(string.Format("Priorities {0} and {1} could not be sorted", nodeA.Priority,
                nodeB.Priority));
        }
    }
}