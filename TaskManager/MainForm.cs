using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MediatR;
using TaskManager.Domain.Features.PrioritizeProject;
using TaskManager.Domain.Features.ViewProjectTree;
using TaskManager.Domain.Infrastructure;

namespace TaskManager
{
    public partial class MainForm : Form
    {
        IMediator _mediator;
        private Dictionary<string, string> _projects;
 
        public MainForm()
        {
            var mediate = new Mediate();
            _mediator = mediate.Bootstrap();
            _projects = new Dictionary<string, string>();
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PopulateProjectTreeView();
            InitializeContextMenuForEachProjectNodeInTreeView();
        }

        private void InitializeContextMenuForEachProjectNodeInTreeView()
        {
            var allPrioritiesQuery = new AllPrioritiesQuery();
            var priorityQueryHandler = new PriorityQueryHandler();
            List<string> priorities = priorityQueryHandler.Handle(allPrioritiesQuery);

            foreach (var priority in priorities)
            {
                var priorityMenuItem = new ToolStripMenuItem(StringFormatter.CapitalizeFirstLetter(priority));
                projectTreeNodeContextMenuStrip.Items.Add(priorityMenuItem);
                projectTreeNodeContextMenuStrip.ItemClicked += projectTreeNodeContextMenuStrip_ItemClicked;
            }

            foreach (var node in projectTreeView.Nodes)
            {
                ((TreeNode) node).ContextMenuStrip = projectTreeNodeContextMenuStrip;
            }
        }

        private void projectTreeNodeContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TreeViewHitTestInfo projectClicked = projectTreeView.HitTest(
                projectTreeView.PointToClient(new Point(projectTreeNodeContextMenuStrip.Left,
                    projectTreeContextMenuStrip.Top)));

            if (projectClicked.Node == null || !_projects.ContainsKey(projectClicked.Node.Text))
            {
                MessageBox.Show("Could not find the selected project", "Error", MessageBoxButtons.OK);
                return;
            }

            string projectId = _projects[projectClicked.Node.Text];
                var prioritizeProject = new PrioritizeProject(projectId, e.ClickedItem.Text);

            try
            {
                _mediator.Send(prioritizeProject);
            }
            catch (ProjectDoesNotExistException ex)
            {
                MessageBox.Show("Could not find the selected project", "Error", MessageBoxButtons.OK);                
            }
        }

        private void PopulateProjectTreeView()
        {
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            List<ProjectTreeNode> projectTreeNodes = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);
            foreach (var projectTreeNode in projectTreeNodes)
            {
                string title = projectTreeNode.Title;
                var treeNode = new TreeNode(title);
                projectTreeView.Nodes.Add(treeNode);
                _projects.Add(title, projectTreeNode.ProjectId);
            }
        }

        private void addProjectMenuItem_Click(object sender, EventArgs e)
        {
            var addProjectForm = new AddProjectForm();
            addProjectForm.ProjectRegistered += addProjectForm_ProjectRegistered;
            
            addProjectForm.StartPosition = FormStartPosition.CenterParent;
            addProjectForm.ShowDialog(this);
        }

        private void addProjectForm_ProjectRegistered(object sender, EventArgs e)
        {
            PopulateProjectTreeView();
        }
    }
}
