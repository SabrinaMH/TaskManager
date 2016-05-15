using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MediatR;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using ILogger = Serilog.ILogger;

namespace TaskManager
{
    public partial class MainForm : Form
    {
        IMediator _mediator;
        private List<ProjectTreeNode> _projects;
        private BindingList<TaskInGridView> _allTasksInProject;
        private ILogger _logger;

        public MainForm()
        {
            var eventStoreConnectionBuilder = new EventStoreConnectionBuilder();
            var mediate = new Mediate(eventStoreConnectionBuilder);
            _logger = Logging.Logger;
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
                var selectedNode = projectTreeView.Nodes[0];
                projectTreeView.SelectedNode = selectedNode;
                //string projectId = GetProjectIdBasedOnTitle(selectedNode.Text);
                //PopulateTasksInGridView(projectId);
            }
        }
         
        private void RearrangeColumnsInTaskGridView()
        {
            RemoveColumnInTaskGridView("Id");
            RemoveColumnInTaskGridView("ProjectId");
            if (taskGridView.Columns.Contains("IsDone"))
            {
                taskGridView.Columns["IsDone"].HeaderText = "Done";
                taskGridView.Columns["IsDone"].Name = "Done";
            }
        }

        private void RemoveColumnInTaskGridView(string columnName)
        {
            if (taskGridView.Columns.Contains(columnName))
            {
                taskGridView.Columns.Remove(columnName);
            }
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
            try
            {
                var selectedProjectId = GetProjectIdBasedOnTitle(projectTreeView.SelectedNode.Text);
                var prioritizeProject = new ReprioritizeProject(selectedProjectId, e.ClickedItem.Text);
                _mediator.Send(prioritizeProject);
            }
            catch (ProjectDoesNotExistException ex)
            {
                _logger.Error(ex, "Could not find project with title {title}", projectTreeView.SelectedNode.Text);
                MessageBox.Show("Could not find the selected project", "Error", MessageBoxButtons.OK);
            }
        }

        private string GetProjectIdBasedOnTitle(string title)
        {
            try
            {
                var projectTreeNode = _projects.FirstOrDefault(x => x.Title == title);
                var projectIdByTitleQuery = new ProjectIdByTitleQuery(title);
                var queryHandler = new ProjectTreeViewQueryHandler();
                string projectId = queryHandler.Handle(projectIdByTitleQuery);
                if (string.IsNullOrEmpty(projectId))
                {
                    _logger.Error("Could not find project id for project with title {title}", title);
                    throw new ProjectDoesNotExistException(title);
                }
                return projectTreeNode.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "A project with title {title} does not exist", title);
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

        private List<TaskInGridView> RetrieveAllTasksInProject(string projectId)
        {
            var allTasksInProjectQuery = new AllTasksInProjectQuery(projectId);
            var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
            List<TaskInGridView> tasksInProject = taskInGridViewQueryHandler.Handle(allTasksInProjectQuery);
            return tasksInProject;
        }

        private void AddProjectToTreeView(string title, string deadline)
        {
            _projects.Add(new ProjectTreeNode(null, title, deadline, null));
            var node = new TreeNode(title);
            projectTreeView.Nodes.Add(node);
            node.ContextMenuStrip = projectTreeNodeContextMenuStrip;
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

        private void addProjectForm_ProjectRegistered(object sender, ProjectEventArgs e)
        {
            string deadline = e.Deadline.HasValue ? e.Deadline.ToString() : null;
            AddProjectToTreeView(e.Title, deadline);
        }

        private void addTaskForm_TaskRegistered(object sender, TaskEventArgs e)
        {
            AddTaskToGridView(e.ProjectId, e.Title, e.Priority, e.Deadline);
        }

        private void AddTaskToGridView(string projectId, string title, string priority, DateTime? deadline)
        {
            string possibleDeadline = deadline.HasValue ? deadline.ToString() : null;
            _allTasksInProject.Add(new TaskInGridView(null, projectId, title, possibleDeadline, priority, false));
        }

        private void PopulateTasksInGridView(string projectId)
        {
            _allTasksInProject = new BindingList<TaskInGridView>(RetrieveAllTasksInProject(projectId));
            taskGridView.DataSource = _allTasksInProject;
            RearrangeColumnsInTaskGridView();
            for (int i = 0; i < taskGridView.RowCount; i++)
            {
                var task = (TaskInGridView) taskGridView.Rows[i].DataBoundItem;
                if (task != null && task.IsDone)
                {
                    FadeOut(i);
                }
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

        private void FadeOut(int rowIndex)
        {
            for (int i = 0; i < taskGridView.Columns.Count; i++)
            {
                taskGridView.Rows[rowIndex].Cells[i].Style.ForeColor = Color.DarkGray;
            }
        }

        private void FadeIn(int rowIndex)
        {
            for (int i = 0; i < taskGridView.Columns.Count; i++)
            {
                taskGridView.Rows[rowIndex].Cells[i].Style.ForeColor = Color.Black;
            }
        }

        private void taskGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (TouchesDoneColumn(e.ColumnIndex, e.RowIndex))
            {
                bool previousValue;
                if (Boolean.TryParse(taskGridView.CurrentRow.Cells["Done"].Value.ToString(), out previousValue))
                {
                    var task = (TaskInGridView) (taskGridView.SelectedCells[0].OwningRow.DataBoundItem);

                    // Doesn't retrieve task using id, because id might be null if it's a task that has been added after application start up.
                    TaskInGridView taskInGridView = _allTasksInProject.First(x => x.Title == task.Title);

                    var id = task.Id;
                    if (id == null)
                    {
                        var taskInGridViewQueryHandler = new TaskInGridViewQueryHandler();
                        var query = new TaskIdByTitleQuery(task.Title);
                        id = taskInGridViewQueryHandler.Handle(query);
                        taskInGridView.Id = id;
                    }

                    var isTaskDone = !previousValue;
                    if (isTaskDone)
                    {
                        var markTaskAsDone = new MarkTaskAsDone(id);
                        _mediator.Send(markTaskAsDone);
                        taskInGridView.IsDone = true;
                        FadeOut(e.RowIndex);
                    }
                    else
                    {
                        var reopenTask = new ReopenTask(task.Id);
                        _mediator.Send(reopenTask);
                        taskInGridView.IsDone = false;
                        FadeIn(e.RowIndex);
                    }
                }
            }
        }

        private bool TouchesDoneColumn(int columnIndex, int rowIndex)
        {
            return columnIndex == taskGridView.Columns["Done"].Index && rowIndex != -1;
        }

        private void projectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedProjectId = GetProjectIdBasedOnTitle(projectTreeView.SelectedNode.Text);
            PopulateTasksInGridView(selectedProjectId);
        }
    }
}
