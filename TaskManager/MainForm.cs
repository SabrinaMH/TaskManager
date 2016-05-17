using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using MediatR;
using Serilog;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.ProjectTreeViewUI;
using TaskManager.TasksInGridViewUI;

namespace TaskManager
{
    public partial class MainForm : Form
    {
        IMediator _mediator;
        private List<ProjectTreeNode> _projects;
        private BindingList<TaskInGridView> _allTasksInProject;
        private ILogger _logger;
        ProjectUtils _projectUtils;
        private TaskUtils _taskUtils;
        private GridUtils _gridUtils;
        private TreeUtils _treeUtils;

        public MainForm()
        {
            InitializeComponent();
            var eventStoreConnectionBuilder = new EventStoreConnectionBuilder();
            var mediate = new Mediate(eventStoreConnectionBuilder);
            _logger = Logging.Logger;
            _mediator = mediate.Mediator;
            _projects = new List<ProjectTreeNode>();
            _projectUtils = new ProjectUtils();
            _treeUtils = new TreeUtils(projectTreeView);
            _taskUtils = new TaskUtils();
            _gridUtils = new GridUtils(taskGridView);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _projects = _projectUtils.RetrieveAllProjects();
            _treeUtils.PopulateTreeByProjectPriority(_projects);
            InitializeContextMenuForEachProjectNode();
            if (projectTreeView.Nodes.Count > 0)
            {
                var selectedNode = projectTreeView.Nodes[0];
                projectTreeView.SelectedNode = selectedNode;
            }
        }

        public void RearrangeColumnsInTasksGridView()
        {
            _gridUtils.RemoveColumn("Id");
            _gridUtils.RemoveColumn("ProjectId");
            if (taskGridView.Columns.Contains("IsDone"))
            {
                taskGridView.Columns["IsDone"].HeaderText = "Done";
                taskGridView.Columns["IsDone"].Name = "Done";
            }
        }

        private void projectTreeNodeContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                var selectedProjectId = _projectUtils.GetProjectIdBasedOnTitle(_projects, projectTreeView.SelectedNode.Text);
                var prioritizeProject = new ReprioritizeProject(selectedProjectId, e.ClickedItem.Text);
                _mediator.Send(prioritizeProject);
                ProjectTreeNode projectTreeNode = _projects.Find(x => x.Id == selectedProjectId);
                projectTreeNode.Priority = e.ClickedItem.Text;
                _treeUtils.PopulateTreeByProjectPriority(_projects);
            }
            catch (ProjectDoesNotExistException ex)
            {
                _logger.Error(ex, "Could not find project with title {title}", projectTreeView.SelectedNode.Text);
                MessageBox.Show("Could not find the selected project", "Error", MessageBoxButtons.OK);
            }
        }

        public void InitializeContextMenuForEachProjectNode()
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
                ((TreeNode)node).ContextMenuStrip = projectTreeNodeContextMenuStrip;
            }
        }

        private void AddProjectToTreeView(string title, string deadline)
        {
            _projects.Add(new ProjectTreeNode(null, title, deadline, null));
            var node = new TreeNode(title);
            projectTreeView.Nodes.Add(node);
            node.ContextMenuStrip = projectTreeNodeContextMenuStrip;
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
            _allTasksInProject = new BindingList<TaskInGridView>(_taskUtils.RetrieveAllTasksInProject(projectId));
            taskGridView.DataSource = _allTasksInProject;
            RearrangeColumnsInTasksGridView();
            for (int i = 0; i < taskGridView.RowCount; i++)
            {
                var task = (TaskInGridView) taskGridView.Rows[i].DataBoundItem;
                if (task != null && task.IsDone)
                {
                    _gridUtils.FadeOut(i);
                }
            }
        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedProjectId = _projectUtils.GetProjectIdBasedOnTitle(_projects, projectTreeView.SelectedNode.Text);
            var addTaskForm = new AddTaskForm(selectedProjectId, _mediator);
            addTaskForm.TaskRegistered += addTaskForm_TaskRegistered;
            addTaskForm.StartPosition = FormStartPosition.CenterParent;
            addTaskForm.ShowDialog(this);
        }


        private void taskGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_gridUtils.TouchesColumn("Done", e.ColumnIndex, e.RowIndex))
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
                        _gridUtils.FadeOut(e.RowIndex);
                    }
                    else
                    {
                        var reopenTask = new ReopenTask(task.Id);
                        _mediator.Send(reopenTask);
                        taskInGridView.IsDone = false;
                        _gridUtils.FadeIn(e.RowIndex);
                    }
                }
            }
        }

        private void projectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedProjectId = _projectUtils.GetProjectIdBasedOnTitle(_projects, projectTreeView.SelectedNode.Text);
            PopulateTasksInGridView(selectedProjectId);
        }
    }
}
