using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using TaskManager.Domain.Features.ChangeTaskStatus;
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
    public partial class TaskGridControl : UserControl
    {
        private GridUtils _gridUtils;
        private CommandDispatcher _commandDispatcher;
        private TaskUtils _taskUtils;
        private List<TaskInGridView> _allTasks;
        private ILogger _logger;
        private string _selectedProjectId;
        public event EventHandler<TaskSelectedEventArgs> TaskSelected;
        public event EventHandler<NoTaskSelectedEventArgs> NoTaskSelected;
        private static bool firstTime = true;
        private BindingList<TaskInGridView> _allTasksInProject;

        public TaskGridControl()
        {
            InitializeComponent();
        }

        /// <exception cref="ArgumentNullException"><paramref name="commandDispatcher"/> is <see langword="null" />.</exception>
        public void Initialize(CommandDispatcher commandDispatcher, ProjectTreeControl projectTreeControl, NoteControl noteControl)
        {
            if (commandDispatcher == null) throw new ArgumentNullException("commandDispatcher");
            if (projectTreeControl == null) throw new ArgumentNullException("projectTreeControl");
            if (noteControl == null) throw new ArgumentNullException("noteControl");
            _commandDispatcher = commandDispatcher;
            _gridUtils = new GridUtils(taskGrid);
            _taskUtils = new TaskUtils();
            _allTasks = new List<TaskInGridView>();
            _logger = Logging.Logger;

            projectTreeControl.ProjectSelected += projectTreeControl_ProjectSelected;
            noteControl.NoteErased += noteControl_NoteErased;
            noteControl.NoteSaved += noteControl_NoteSaved;

            try
            {
                _allTasks = _taskUtils.RetrieveAllTasks();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something went wrong fetching all tasks");
                MessageBox.Show("Sorry, something went wrong getting tasks", "Error", MessageBoxButtons.OK);
            }
        }

        void noteControl_NoteSaved(object sender, NoteSavedEventArgs e)
        {
            var task = _allTasks.First(x => x.Id == e.TaskId);
            task.Note = e.Content;
            _gridUtils.SetHasNoteCheckBox(e.Content, task.Title);
        }

        void noteControl_NoteErased(object sender, NoteErasedEventArgs e)
        {
            var task = _allTasks.First(x => x.Id == e.TaskId);
            task.Note = null;
            _gridUtils.SetHasNoteCheckBox(null, task.Title);
        }

        private void projectTreeControl_ProjectSelected(object sender, ProjectSelectedEventArgs e)
        {
            _selectedProjectId = e.SelectedProjectId;
            PopulateTasksInGridView();
        }

        private void PopulateTasksInGridView()
        {
            var allTasksInProject = _allTasks.Where(x => x.ProjectId == _selectedProjectId).ToList();
            _allTasksInProject = new BindingList<TaskInGridView>(allTasksInProject);
            taskGrid.DataSource = _allTasksInProject;
            taskGrid.AutoGenerateColumns = false;

            // Second time in this method, we need to remove the priority combobox column, because it's added again later
            _gridUtils.RemoveColumn("Priority");
            _gridUtils.RemoveColumn("Id");
            _gridUtils.RemoveColumn("ProjectId");
            _gridUtils.RemoveColumn("Priority");
            _gridUtils.RemoveColumn("Note");

            if (taskGrid.Columns.Contains("IsDone"))
            {
                taskGrid.Columns["IsDone"].HeaderText = "Done";
                taskGrid.Columns["IsDone"].Name = "Done";
            }

            int indexOfHasNoteColumn;
            if (!taskGrid.Columns.Contains("HasNote"))
            {
                DataGridViewCheckBoxColumn hasNoteColumn = new DataGridViewCheckBoxColumn();
                hasNoteColumn.ReadOnly = true;
                hasNoteColumn.HeaderText = "Has Note";
                hasNoteColumn.Name = "HasNote";
                indexOfHasNoteColumn = taskGrid.Columns.Add(hasNoteColumn);
            }
            else
            {
                var hasNoteColumn = taskGrid.Columns["HasNote"];
                indexOfHasNoteColumn = hasNoteColumn.Index;
            }

            DataGridViewComboBoxColumn priorityColumn = new DataGridViewComboBoxColumn();
            priorityColumn.HeaderText = "Priority";
            priorityColumn.Name = "Priority";
            var indexOfPriorityColumn = taskGrid.Columns.Add(priorityColumn);
            priorityColumn.DataSource = TaskPriority.GetAll().Select(x => x.DisplayName).ToList();


            if (!_allTasksInProject.Any())
            {
                if (NoTaskSelected != null)
                {
                    var eventArgs = new NoTaskSelectedEventArgs();
                    NoTaskSelected(this, eventArgs);
                }
                return;
            }

            for (int i = 0; i < taskGrid.RowCount; i++)
            {
                var row = taskGrid.Rows[i];
                row.Cells[indexOfHasNoteColumn].Value = !string.IsNullOrWhiteSpace(_allTasksInProject[i].Note);
                row.Cells[indexOfPriorityColumn].Value = _allTasksInProject[i].Priority;
                var task = (TaskInGridView)row.DataBoundItem;
                if (task != null && task.IsDone)
                {
                    _gridUtils.FadeOut(i);
                }
            }

            taskGrid.Rows[0].Selected = true;
        }


        private void addTaskForm_TaskRegistered(object sender, TaskRegisteredEventArgs e)
        {
            AddTaskToGridView(e.ProjectId, e.Title, e.Priority, e.Deadline);
        }

        private void AddTaskToGridView(string projectId, string title, string priority, DateTime? deadline)
        {
            var taskId = TaskId.Create(new ProjectId(projectId), title);
            string possibleDeadline = deadline.HasValue ? deadline.Value.ToShortDateString() : null;
            var newTask = new TaskInGridView(taskId, projectId, title, possibleDeadline, priority, false);
            _allTasks.Add(newTask);
            _allTasksInProject.Add(newTask);

            int priorityColumn = taskGrid.Columns["Priority"].Index;
            var indexOfNewRow = taskGrid.Rows.GetLastRow(DataGridViewElementStates.None);
            var newRow = taskGrid.Rows[indexOfNewRow];
            newRow.Cells[priorityColumn].Value = priority;
            newRow.Selected = true; 
        }

        private void taskGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var selectedRow = taskGrid.CurrentRow;
            if (selectedRow == null) return;

            if (_gridUtils.TouchesColumn("Done", e.ColumnIndex, e.RowIndex))
            {
                bool isTaskDone;
                if (Boolean.TryParse(selectedRow.Cells["Done"].Value.ToString(), out isTaskDone))
                {
                    var task = (TaskInGridView)(taskGrid.SelectedCells[0].OwningRow.DataBoundItem);

                    TaskInGridView taskInGridView = _allTasks.First(x => x.Id == task.Id);
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

        private void taskGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // We do not care about header modifications
            if (e.RowIndex == -1) return;

            DataGridViewColumn priorityColumn = taskGrid.Columns["Priority"];
            DataGridViewComboBoxCell priorityCell = (DataGridViewComboBoxCell)taskGrid.Rows[e.RowIndex].Cells[priorityColumn.Index];
            var tasksInProject = _allTasks.Where(x => x.ProjectId == _selectedProjectId).ToList();
            if (priorityCell.Value != null)
            {
                var selectedTask = tasksInProject[e.RowIndex];
                string newPriority = priorityCell.Value.ToString();
                if (selectedTask.Priority == newPriority) return;

                var reprioritizeTask = new ReprioritizeTask(selectedTask.Id, newPriority);
                _commandDispatcher.Send(reprioritizeTask);
                selectedTask.Priority = newPriority;
            }
        }

        private void taskGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (taskGrid.IsCurrentCellDirty)
            {
                taskGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }


        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addTaskForm = new AddTaskForm(_selectedProjectId, _commandDispatcher);
            addTaskForm.TaskRegistered += addTaskForm_TaskRegistered;
            addTaskForm.StartPosition = FormStartPosition.CenterParent;
            addTaskForm.ShowDialog(this);
        }

        private void taskGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected)
                return;

            var allTasksInProject = _allTasks.Where(x => x.ProjectId == _selectedProjectId).ToList();
            if (!allTasksInProject.Any())
            {
                if (NoTaskSelected != null)
                {
                    var eventArgs = new NoTaskSelectedEventArgs();
                    NoTaskSelected(this, eventArgs);
                }
                return;
            }

            var rowIndex = 0;
            if (allTasksInProject.First().ProjectId == _selectedProjectId)
            {
                rowIndex = e.Row.Index;
            }

            var selectedTask = allTasksInProject[rowIndex];
            if (TaskSelected != null)
            {
                var eventArgs = new TaskSelectedEventArgs(selectedTask.Id, selectedTask.Note);
                TaskSelected(this, eventArgs);
            }
        }
    }
}
