using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Project;
using TaskManager.ProjectTreeViewUI;

namespace TaskManager
{
    public partial class ProjectTreeControl : UserControl
    {
        private List<ProjectTreeNode> _projects;
        private ProjectUtils _projectUtils;
        private TreeUtils _treeUtils;
        private CommandDispatcher _commandDispatcher;
        private string _selectedProjectId;
        private ILogger _logger;
        public event EventHandler<ProjectSelectedEventArgs> ProjectSelected;

        public ProjectTreeControl()
        {
            InitializeComponent();
        }

        /// <exception cref="ArgumentNullException"><paramref name="commandDispatcher"/> is <see langword="null" />.</exception>
        public void Initialize(CommandDispatcher commandDispatcher)
        {
            if (commandDispatcher == null) throw new ArgumentNullException("commandDispatcher");
            _commandDispatcher = commandDispatcher;
            _projectUtils = new ProjectUtils();
            _treeUtils = new TreeUtils(projectTree);
            _projects = _projectUtils.RetrieveAllProjects();
            _logger = Logging.Logger;

            RefreshProjectTree();
        }

        private void projectTreeNodeContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_selectedProjectId == null) return;

            try
            {
                var reprioritizeProject = new ReprioritizeProject(_selectedProjectId, e.ClickedItem.Text);
                _commandDispatcher.Send(reprioritizeProject);
                ProjectTreeNode projectTreeNode = _projects.First(x => x.Id == _selectedProjectId);
                projectTreeNode.Priority = e.ClickedItem.Text;
                RefreshProjectTree();
            }
            catch (ProjectDoesNotExistException ex)
            {
                _logger.ForContext("priority", e.ClickedItem.Text).Error(ex, "Something went wrong reprioritizing project {projectId}", _selectedProjectId);
                MessageBox.Show("Sorry, something went wrong reprioritizing project", "Error", MessageBoxButtons.OK);
            }
        }

        private void RefreshProjectTree()
        {
            _treeUtils.PopulateTreeByProjectPriority(_projects);
            InitializeContextMenuForEachProjectNode();
        }

        private void InitializeContextMenuForEachProjectNode()
        {
            if (projectTreeNodeContextMenuStrip.Items.Count == 0)
            {
                var allPrioritiesQuery = new AllProjectPrioritiesQuery();
                var priorityQueryHandler = new ProjectPriorityQueryHandler();
                List<string> priorities = priorityQueryHandler.Handle(allPrioritiesQuery);

                if (priorities.Any())
                {
                    var priorityLabel = new ToolStripLabel("Priority");
                    priorityLabel.Enabled = false;

                    projectTreeNodeContextMenuStrip.Items.Insert(0, priorityLabel);
                    projectTreeNodeContextMenuStrip.Items.Insert(1, new ToolStripSeparator());
                    foreach (var priority in priorities)
                    {
                        var priorityMenuItem = new ToolStripMenuItem(priority);
                        priorityMenuItem.Name = priority;
                        projectTreeNodeContextMenuStrip.Items.Add(priorityMenuItem);
                    }
                    projectTreeNodeContextMenuStrip.ItemClicked += projectTreeNodeContextMenuStrip_ItemClicked;
                }
            }

            foreach (var node in projectTree.Nodes)
            {
                var treeNode = ((TreeNode)node);
                if (treeNode.ContextMenuStrip == null)
                {
                    treeNode.ContextMenuStrip = projectTreeNodeContextMenuStrip;
                }
            }
        }

        private void AddProjectToTreeView(string title)
        {
            var node = new TreeNode(title);
            projectTree.Nodes.Add(node);
            node.ContextMenuStrip = projectTreeNodeContextMenuStrip;
        }

        private void addProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAddProjectForm(); 
        }

        private void addProjectForm_ProjectRegistered(object sender, ProjectRegisteredEventArgs e)
        {
            AddProjectToTreeView(e.Title);

            string deadline = e.Deadline.HasValue ? e.Deadline.ToString() : null;
            var projectId = ProjectId.Create(e.Title);
            _projects.Add(new ProjectTreeNode(projectId, e.Title, deadline, "none", 0));

            if (projectTree.SelectedNode == null)
            {
                var project = _projects.Find(x => x.Id == projectId);
                var selectedNode = projectTree.Nodes[project.Title];
                _selectedProjectId = project.Id;
                projectTree.SelectedNode = selectedNode;
                if (ProjectSelected != null)
                {
                    var eventArgs = new ProjectSelectedEventArgs(_selectedProjectId);
                    ProjectSelected(this, eventArgs);
                }
            }
        }

        private void projectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedProjectId = _projects.First(x => x.Title == projectTree.SelectedNode.Text).Id;
            if (selectedProjectId == _selectedProjectId) return;
            
            _selectedProjectId = selectedProjectId;
            if (ProjectSelected != null)
            {
                var eventArgs = new ProjectSelectedEventArgs(_selectedProjectId);
                ProjectSelected(this, eventArgs);
            }
        }

        private void projectTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var selectedNode = projectTree.GetNodeAt(e.Location);
                if (selectedNode == null) return;

                _selectedProjectId = _projects.First(x => x.Title == selectedNode.Text).Id;
                var project = _projects.Find(x => x.Id == _selectedProjectId);
                HighlightCurrentPriority(project.Priority);
                projectTreeNodeContextMenuStrip.Show(projectTree, e.Location);
            }
        }

        private void OpenAddProjectForm()
        {
            var addProjectForm = new AddProjectForm(_commandDispatcher);
            addProjectForm.ProjectRegistered += addProjectForm_ProjectRegistered;
            addProjectForm.StartPosition = FormStartPosition.CenterParent;
            addProjectForm.ShowDialog(this);
        }


        private void HighlightCurrentPriority(string currentPriority)
        {
            var menuItems = projectTreeNodeContextMenuStrip.Items;

            if (string.IsNullOrWhiteSpace(currentPriority))
            {
                var noPriority = menuItems["none"];
                noPriority.Font = new Font(noPriority.Font, FontStyle.Bold);
                return;
            }

            for (int i = 0; i < menuItems.Count; i++)
            {
                var menuItem = menuItems[i];
                FontStyle fontStyle = FontStyle.Regular;
                if (menuItem.Name == currentPriority)
                {
                    fontStyle = FontStyle.Bold;
                }
                menuItem.Font = new Font(menuItem.Font, fontStyle);
            }
        }

        private void newProjectButton_Click(object sender, EventArgs e)
        {
            OpenAddProjectForm();
        }
    }
}
