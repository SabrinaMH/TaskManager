using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Features.ReprioritizeTask;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;
using TaskManager.NoteEditorUI;
using TaskManager.ProjectTreeViewUI;
using TaskManager.TasksInGridViewUI;

namespace TaskManager
{
    public partial class MainForm : Form
    {
        private CommandDispatcher _commandDispatcher;
        private List<ProjectTreeNode> _projects;
        private BindingList<TaskInGridView> _allTasksInProject;
        private string _selectedProjectId;
        private ILogger _logger;
        ProjectUtils _projectUtils;
        private TaskUtils _taskUtils;
        private GridUtils _gridUtils;
        private TreeUtils _treeUtils;

        public MainForm()
        {
            InitializeComponent();
            _logger = Logging.Logger;
            var eventStoreConnectionBuilder = new EventStoreConnectionBuilder();
            var eventBus = new EventBus((@event, next) =>
                new ExceptionDecorator<Event>(next).Handle(@event));
            _commandDispatcher = new CommandDispatcher(eventStoreConnectionBuilder, eventBus);
            _projects = new List<ProjectTreeNode>();
            _projectUtils = new ProjectUtils();
            _treeUtils = new TreeUtils(projectTreeView);
            _taskUtils = new TaskUtils();
            _gridUtils = new GridUtils(taskGridView);
            noteControl.Initialize(_commandDispatcher);
            noteControl.NoteSaved += noteControl_NoteSaved;
            noteControl.NoteErased += noteControl_NoteErased;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _projects = _projectUtils.RetrieveAllProjects();
            RefreshProjectTree();
            if (projectTreeView.Nodes.Count > 0)
            {
                var selectedNode = projectTreeView.Nodes[0];
                projectTreeView.SelectedNode = selectedNode;
            }
        }

        void noteControl_NoteErased(object sender, NoteErasedEventArgs e)
        {
            var task = _allTasksInProject.First(x => x.Id == e.TaskId);
            task.Note = null;
            _gridUtils.SetHasNoteCheckBox(null, task.Title);
        }

        void noteControl_NoteSaved(object sender, NoteSavedEventArgs e)
        {
            var task = _allTasksInProject.First(x => x.Id == e.TaskId);
            task.Note = e.Content;
            _gridUtils.SetHasNoteCheckBox(e.Content, task.Title);
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

            foreach (var node in projectTreeView.Nodes)
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
            projectTreeView.Nodes.Add(node);
            node.ContextMenuStrip = projectTreeNodeContextMenuStrip;
        }
        
        private void addProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAddProjectForm();
        }

        private void addProjectForm_ProjectRegistered(object sender, ProjectEventArgs e)
        {
            AddProjectToTreeView(e.Title);

            string deadline = e.Deadline.HasValue ? e.Deadline.ToString() : null;
             var projectId = ProjectId.Create(e.Title);
            _projects.Add(new ProjectTreeNode(projectId, e.Title, deadline, "none"));

            if (projectTreeView.SelectedNode == null)
            {
                SelectProjectInTreeView(projectId);
            }
        }

        private void SelectProjectInTreeView(string projectId)
        {
            var project = _projects.Find(x => x.Id == projectId);
            var selectedNode = projectTreeView.Nodes[project.Title];
            _selectedProjectId = project.Id;
            projectTreeView.SelectedNode = selectedNode;
            PopulateTasksInGridView(_selectedProjectId);
        }

        private void addTaskForm_TaskRegistered(object sender, TaskEventArgs e)
        {
            AddTaskToGridView(e.ProjectId, e.Title, e.Priority, e.Deadline);
        }

        private void AddTaskToGridView(string projectId, string title, string priority, DateTime? deadline)
        {
            var taskId = TaskId.Create(new ProjectId(projectId), title);
            string possibleDeadline = deadline.HasValue ? deadline.Value.ToShortDateString() : null;
            _allTasksInProject.Add(new TaskInGridView(taskId, projectId, title, possibleDeadline, priority, false));

            int priorityColumn = taskGridView.Columns["Priority"].Index;
            var indexOfNewRow = taskGridView.Rows.GetLastRow(DataGridViewElementStates.None);
            var newRow = taskGridView.Rows[indexOfNewRow];
            newRow.Cells[priorityColumn].Value = priority;
            newRow.Selected = true;
        }

        private void PopulateTasksInGridView(string projectId)
        {
            // Second time in this method, we need to remove the priority combobox column, because it's added again later
            _gridUtils.RemoveColumn("Priority");

            try
            {
                _allTasksInProject = new BindingList<TaskInGridView>(_taskUtils.RetrieveAllTasksInProject(projectId));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something went wrong fetching tasks for project {projectId}", projectId);
                MessageBox.Show("Sorry, something went wrong reprioritizing project", "Error", MessageBoxButtons.OK);
                return;
            }

            taskGridView.DataSource = _allTasksInProject;

            _gridUtils.RemoveColumn("Id");
            _gridUtils.RemoveColumn("ProjectId");
            _gridUtils.RemoveColumn("Priority");
            _gridUtils.RemoveColumn("Note");

            if (taskGridView.Columns.Contains("IsDone"))
            {
                taskGridView.Columns["IsDone"].HeaderText = "Done";
                taskGridView.Columns["IsDone"].Name = "Done";
            }

            int indexOfHasNoteColumn;
            if (!taskGridView.Columns.Contains("HasNote"))
            {
                DataGridViewCheckBoxColumn hasNoteColumn = new DataGridViewCheckBoxColumn();
                hasNoteColumn.ReadOnly = true;
                hasNoteColumn.HeaderText = "Has Note";
                hasNoteColumn.Name = "HasNote";
                indexOfHasNoteColumn = taskGridView.Columns.Add(hasNoteColumn);
            }
            else
            {
                var hasNoteColumn = taskGridView.Columns["HasNote"];
                indexOfHasNoteColumn = hasNoteColumn.Index;
            }

            DataGridViewComboBoxColumn priorityColumn = new DataGridViewComboBoxColumn();
            priorityColumn.HeaderText = "Priority";
            priorityColumn.Name = "Priority";
            var indexOfPriorityColumn = taskGridView.Columns.Add(priorityColumn);
            priorityColumn.DataSource = TaskPriority.GetAll().Select(x => x.DisplayName).ToList();


            if (!_allTasksInProject.Any())
            {
                noteControl.Clear();
                return;
            }

            RenderNote((TaskInGridView)taskGridView.Rows[0].DataBoundItem);
            for (int i = 0; i < taskGridView.RowCount; i++)
            {
                var row = taskGridView.Rows[i];
                row.Cells[indexOfHasNoteColumn].Value = !string.IsNullOrWhiteSpace(_allTasksInProject[i].Note);
                row.Cells[indexOfPriorityColumn].Value = _allTasksInProject[i].Priority;
                var task = (TaskInGridView) row.DataBoundItem;
                if (task != null && task.IsDone)
                {
                    _gridUtils.FadeOut(i);
                }
            }
        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addTaskForm = new AddTaskForm(_selectedProjectId, _commandDispatcher);
            addTaskForm.TaskRegistered += addTaskForm_TaskRegistered;
            addTaskForm.StartPosition = FormStartPosition.CenterParent;
            addTaskForm.ShowDialog(this);
        }


        private void taskGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var selectedRow = taskGridView.CurrentRow;
            if (selectedRow == null) return;

            RenderNote((TaskInGridView) selectedRow.DataBoundItem);

            if (_gridUtils.TouchesColumn("Done", e.ColumnIndex, e.RowIndex))
            {
                bool isTaskDone;
                if (Boolean.TryParse(selectedRow.Cells["Done"].Value.ToString(), out isTaskDone))
                {
                    var task = (TaskInGridView) (taskGridView.SelectedCells[0].OwningRow.DataBoundItem);

                    TaskInGridView taskInGridView = _allTasksInProject.First(x => x.Id == task.Id);
                    if (isTaskDone)
                    {
                        var markTaskAsDone = new MarkTaskAsDone(task.Id);
                        _commandDispatcher.Send(markTaskAsDone);
                        // Fake in UI to increase user experience
                        taskInGridView.IsDone = true;
                        _gridUtils.FadeOut(e.RowIndex);
                    }
                    else
                    {
                        var reopenTask = new ReopenTask(task.Id);
                        _commandDispatcher.Send(reopenTask);
                        // Fake in UI to increase user experience
                        taskInGridView.IsDone = false;
                        _gridUtils.FadeIn(e.RowIndex);
                    }
                }
            }
        }

        private void projectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedProjectId = _projects.First(x => x.Title == projectTreeView.SelectedNode.Text).Id;
            if (selectedProjectId == _selectedProjectId) return;

            PopulateTasksInGridView(selectedProjectId);
            _selectedProjectId = selectedProjectId;
        }

        private void OpenAddProjectForm()
        {
            var addProjectForm = new AddProjectForm(_commandDispatcher);
            addProjectForm.ProjectRegistered += addProjectForm_ProjectRegistered;
            addProjectForm.StartPosition = FormStartPosition.CenterParent;
            addProjectForm.ShowDialog(this);
        }

        private void newProjectButton_Click(object sender, EventArgs e)
        {
            OpenAddProjectForm();
        }

        private void taskGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // We do not care about header modifications
            if (e.RowIndex == -1) return;

            DataGridViewColumn priorityColumn = taskGridView.Columns["Priority"];
            DataGridViewComboBoxCell priorityCell = (DataGridViewComboBoxCell)taskGridView.Rows[e.RowIndex].Cells[priorityColumn.Index];
            
            if (priorityCell.Value != null)
            {
                var selectedTask = _allTasksInProject[e.RowIndex];
                string newPriority = priorityCell.Value.ToString();
                if (selectedTask.Priority == newPriority) return;

                var reprioritizeTask = new ReprioritizeTask(selectedTask.Id, newPriority);
                _commandDispatcher.Send(reprioritizeTask);
                selectedTask.Priority = newPriority;
            }
        }

        private void taskGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (taskGridView.IsCurrentCellDirty)
            {
                taskGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void projectTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var selectedNode = projectTreeView.GetNodeAt(e.Location);
                if (selectedNode == null) return;

                _selectedProjectId = _projects.First(x => x.Title == selectedNode.Text).Id;
                var project = _projects.Find(x => x.Id == _selectedProjectId);
                HighlightCurrentPriority(project.Priority);
                projectTreeNodeContextMenuStrip.Show(projectTreeView, e.Location);
            }
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            RavenDbStore.CleanUp();
        }

        private void taskGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) 
                return;

            if (!_allTasksInProject.Any())
            {
                noteControl.Clear();
                return;
            }

            var rowIndex = 0;
            if (_allTasksInProject.First().ProjectId == _selectedProjectId)
            {
                rowIndex = e.Row.Index;
            }

            var selectedTask = _allTasksInProject[rowIndex];
            RenderNote(selectedTask);
        }

        private void RenderNote(TaskInGridView selectedTask)
        {
            noteControl.RenderNote(selectedTask.Id, selectedTask.Note);
        }

    }
}
