using System.Collections.Generic;
using System.Windows.Forms;
using TaskManager.Domain.Features.ProjectTreeView;

namespace TaskManager.ProjectTreeViewUI
{
    public class TreeUtils
    {
        private readonly TreeView _tree;

        public TreeUtils(TreeView tree)
        {
            _tree = tree;
        }

        public void PopulateTreeByProjectPriority(List<ProjectTreeNode> projects)
        {
            _tree.Nodes.Clear();
            var sorter = new Sorter();
            var sortedByPriority = sorter.ByPriority(projects);
            foreach (var projectTreeNode in sortedByPriority)
            {
                string title = projectTreeNode.Title;
                var treeNode = new TreeNode(title);
                _tree.Nodes.Add(treeNode);
            }
        } 
    }
}