using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using EventStore.ClientAPI;
using MediatR;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;

namespace TaskManager
{
    public partial class MainForm : Form
    {
        IMediator _mediator;
        private List<ProjectTreeNode> _projects;

        public MainForm()
        {
            ConnectionSettings connectionSettings = ConnectionSettings.Create().KeepReconnecting().Build();
            var eventStoreConnection = EventStoreConnection.Create(connectionSettings, new IPEndPoint(IPAddress.Loopback, 1113));
            var mediate = new Mediate(eventStoreConnection);
            _mediator = mediate.Mediator;
            _projects = new List<ProjectTreeNode>();
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _projects = RetrieveAllProjects();
            PopulateProjectTreeView(_projects);
            InitializeContextMenuForEachProjectNodeInTreeView();
            if (projectTreeView.Nodes.Count > 0)
            {
                projectTreeView.SelectedNode = projectTreeView.Nodes[0];
                projectTreeView_NodeMouseClick(this, new TreeNodeMouseClickEventArgs(projectTreeView.SelectedNode, MouseButtons.Left, 1, 0, 0));
            }
        }

        private void RearrangeColumnsInTaskGridView()
        {
            taskGridView.Columns.Remove("projectId");
            taskGridView.Columns.Remove("taskId");
            taskGridView.Columns["isDone"].DisplayIndex = 0;
            taskGridView.Columns["isDone"].Name = "Done";
            taskGridView.Columns["title"].DisplayIndex = 1;
            taskGridView.Columns["priority"].DisplayIndex = 2;
            taskGridView.Columns["deadline"].DisplayIndex = 3;
        }

        private void InitializeContextMenuForEachProjectNodeInTreeView()
        {
            var allPrioritiesQuery = new AllProjectPrioritiesQuery();
            var priorityQueryHandler = new ProjectPriorityQueryHandler();
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
            //TreeViewHitTestInfo projectClickedInfo = projectTreeView.HitTest(
            //    projectTreeView.PointToClient(new Point(projectTreeNodeContextMenuStrip.Left,
            //        projectTreeContextMenuStrip.Top)));

            try
            {
                //var projectClicked = _projects.FirstOrDefault(x => x.Title == projectClickedInfo.Node.Text);
                var selectedProjectId = GetProjectIdBasedOnTitle(projectTreeView.SelectedNode.Text);
                var prioritizeProject = new ReprioritizeProject(selectedProjectId, e.ClickedItem.Text);
                _mediator.Send(prioritizeProject);
            }
            catch (ProjectDoesNotExistException ex)
            {
                MessageBox.Show("Could not find the selected project", "Error", MessageBoxButtons.OK);
            }
        }

        private Guid GetProjectIdBasedOnTitle(string title)
        {
            try
            {
                var projectTreeNode = _projects.FirstOrDefault(x => x.Title == title);
                return Guid.Parse(projectTreeNode.Id);
            }
            catch (Exception ex)
            {
                throw new ProjectDoesNotExistException(title, ex);
            }
        }

        private List<ProjectTreeNode> RetrieveAllProjects()
        {
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            List<ProjectTreeNode> allProjects = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);
            return allProjects;
        }

        private List<TaskInGridView> RetrieveAllTasksInProject(Guid projectId)
        {
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            List<TaskInGridView> tasksInProject = taskInGridViewQueryHandler.Handle(allTasksInProjectQuery);
            return tasksInProject;
        }

        private void PopulateProjectTreeView(List<ProjectTreeNode> projects)
        {
            projectTreeView.Nodes.Clear();
            var sorter = new Sorter();
            var sortedByPriority = sorter.ByPriority(projects);
            foreach (var projectTreeNode in sortedByPriority)
            {
                string title = projectTreeNode.Title;
                var treeNode = new TreeNode(title);
                projectTreeView.Nodes.Add(treeNode);
            }
        }

        private void addProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addProjectForm = new AddProjectForm(_mediator);
            addProjectForm.ProjectRegistered += addProjectForm_ProjectRegistered;
            
            addProjectForm.StartPosition = FormStartPosition.CenterParent;
            addProjectForm.ShowDialog(this);
        }

        private void addProjectForm_ProjectRegistered(object sender, EventArgs e)
        {
            // Could optimize performance by not retrieving all projects again
            _projects = RetrieveAllProjects();
            PopulateProjectTreeView(_projects);
        }

        private void addTaskForm_TaskRegistered(object sender, EventArgs e)
        {
            UpdateTasksInGridView();
        }

        private void UpdateTasksInGridView()
        {
            // Could optimize performance by not retrieving all tasks again
            var selectedProjectId = GetProjectIdBasedOnTitle(projectTreeView.SelectedNode.Text);
            List<TaskInGridView> allTasksInProject = RetrieveAllTasksInProject(selectedProjectId);
            taskGridView.DataSource = new BindingSource(allTasksInProject, null);
            RearrangeColumnsInTaskGridView();
            for (int i = 0; i < taskGridView.RowCount; i++)
            {
                var task = (TaskInGridView) taskGridView.Rows[i].DataBoundItem;
                if (task.IsDone)
                {
                    FadeOut(i);
                }
            }
        }

        private void projectTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpdateTasksInGridView();
            }
        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedProjectId = GetProjectIdBasedOnTitle(projectTreeView.SelectedNode.Text);
            var addTaskForm = new AddTaskForm(selectedProjectId, _mediator);
            addTaskForm.TaskRegistered += addTaskForm_TaskRegistered;
            addTaskForm.StartPosition = FormStartPosition.CenterParent;
            addTaskForm.ShowDialog(this);
        }

        private void taskGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (TouchesIsTaskDoneColumn(e.ColumnIndex, e.RowIndex))
            {
                bool isTaskDone;
                if (Boolean.TryParse(taskGridView.CurrentRow.Cells["isDone"].Value.ToString(), out isTaskDone))
                {
                    var task = (TaskInGridView) (taskGridView.SelectedRows[0].DataBoundItem);
                    if (isTaskDone)
                    {
                        var markTaskAsDone = new MarkTaskAsDone(task.Id);
                        _mediator.Send(markTaskAsDone);
                        FadeOut(e.RowIndex);
                    }
                    else
                    {
                        var reopenTask = new ReopenTask(task.Id);
                        _mediator.Send(reopenTask);
                    }
                }
            }
        }

        private void FadeOut(int rowIndex)
        {
            for (int i = 0; i < taskGridView.Columns.Count; i++)
            {
                taskGridView.Rows[rowIndex].Cells[0].Style.ForeColor = Color.DarkGray;
            }
        }

        private void taskGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (TouchesIsTaskDoneColumn(e.ColumnIndex, e.RowIndex))
            {
                taskGridView.EndEdit();
            }
        }

        private bool TouchesIsTaskDoneColumn(int columnIndex, int rowIndex)
        {
            return columnIndex == taskGridView.Columns["isDone"].Index && rowIndex != -1;
        }
    }
}
