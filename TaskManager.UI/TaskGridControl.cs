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
            var allTasksInProject = FindTasksInProject(_selectedProjectId).OrderBy(x => x.IsDone).ToList();
            _allTasksInProject = new BindingList<TaskInGridView>(allTasksInProject);
            taskGrid.DataSource = _allTasksInProject;
            taskGrid.AutoGenerateColumns = false;

            // Second time in this method, we need to remove the priority combobox column, because it's added again later
            _gridUtils.RemoveColumn("Priority");
            _gridUtils.RemoveColumn("Id");
            _gridUtils.RemoveColumn("ProjectId");
            _gridUtils.RemoveColumn("Priority");
            _gridUtils.RemoveColumn("Note");

            if (taskGrid.Columns.Contains("Title"))
            {
                taskGrid.Columns["Title"].ReadOnly = true;
            }

            if (taskGrid.Columns.Contains("Deadline"))
            {
                taskGrid.Columns["Deadline"].ReadOnly = true;
            }

            if (taskGrid.Columns.Contains("IsDone"))
            {
                DataGridViewColumn isDoneColumn = taskGrid.Columns["IsDone"];
                isDoneColumn.HeaderText = "Done";
                isDoneColumn.Name = "Done";
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

            var rowToBeSelected = taskGrid.Rows[0];
            rowToBeSelected.Selected = true;
            taskGrid.CurrentCell = rowToBeSelected.Cells[0];
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
            AddTaskToGridView(newTask);
        }

        private void AddTaskToGridView(TaskInGridView newTask)
        {
            if (!_allTasks.Contains(newTask))
            {
                _allTasks.Add(newTask);
            }

            InsertTaskRightAboveTasksThatAreDone(newTask);

            int priorityColumn = taskGrid.Columns["Priority"].Index;
            int indexOfNewRow = _allTasksInProject.IndexOf(newTask);
            var newRow = taskGrid.Rows[indexOfNewRow];
            newRow.Cells[priorityColumn].Value = newTask.Priority;
            newRow.Selected = true;
            taskGrid.CurrentCell = newRow.Cells[0];
        }

        private void InsertTaskRightAboveTasksThatAreDone(TaskInGridView task)
        {
            var firstDoneTask = _allTasksInProject.FirstOrDefault(x => x.Id != task.Id && x.IsDone);
            if (firstDoneTask == null)
            {
                _allTasksInProject.Add(task);
            }
            else
            {
                int indexOfFirstDoneTask = _allTasksInProject.IndexOf(firstDoneTask);
                _allTasksInProject.Insert(indexOfFirstDoneTask, task);
            }
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

                        _allTasksInProject.Remove(taskInGridView);
                        AddTaskToGridView(taskInGridView);
                        int indexOfAddedTask = _allTasksInProject.IndexOf(taskInGridView);
                        _gridUtils.FadeOut(indexOfAddedTask);
                    }
                    else
                    {
                        var reopenTask = new ReopenTask(task.Id);
                        _commandDispatcher.Send(reopenTask);
                        // Fake in UI to increase user experience
                        taskInGridView.IsDone = false;

                         _allTasksInProject.Remove(taskInGridView);
                        AddTaskToGridView(taskInGridView);
                        int indexOfAddedTask = _allTasksInProject.IndexOf(taskInGridView);
                        _gridUtils.FadeIn(indexOfAddedTask);
                    }
                }
            }
        }

        private void taskGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // We do not care about header modifications
            if (e.RowIndex == -1) return;

            DataGridViewColumn priorityColumn = taskGrid.Columns["Priority"];
            var priorityCell = (DataGridViewComboBoxCell)taskGrid.Rows[e.RowIndex].Cells[priorityColumn.Index];
            if (priorityCell.Value != null)
            {
                var selectedTask = _allTasksInProject[e.RowIndex];
                string newPriority = priorityCell.Value.ToString();
                if (selectedTask.Priority != newPriority)
                {
                    var reprioritizeTask = new ReprioritizeTask(selectedTask.Id, newPriority);
                    _commandDispatcher.Send(reprioritizeTask);
                    selectedTask.Priority = newPriority;
                }
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

            if (!_allTasksInProject.Any()) 
            {
                if (NoTaskSelected != null)
                {
                    var eventArgs = new NoTaskSelectedEventArgs();
                    NoTaskSelected(this, eventArgs);
                }
                return;
            }

            var selectedTask = FindTaskByRowIndex(_allTasksInProject.ToList(), e.Row.Index);
            if (TaskSelected != null)
            {
                var eventArgs = new TaskSelectedEventArgs(selectedTask.Id, selectedTask.Note);
                TaskSelected(this, eventArgs);
            }
        }

        private List<TaskInGridView> FindTasksInProject(string projectId)
        {
            var tasksInProject = _allTasks.Where(x => x.ProjectId == projectId).ToList();
            return tasksInProject;
        }

        private TaskInGridView FindTaskByRowIndex(List<TaskInGridView> tasksInProject, int selectedRowIndex)
        {
            var rowIndex = 0;
            if (tasksInProject.First().ProjectId == _selectedProjectId)
            {
                rowIndex = selectedRowIndex;
            }

            var task = tasksInProject[rowIndex];
            return task;
        }

        private void taskGrid_DoubleClick(object sender, EventArgs e)
        {
            var selectedRow = taskGrid.CurrentRow;
            if (selectedRow == null) return;

            var selectedTask = FindTaskByRowIndex(_allTasksInProject.ToList(), selectedRow.Index);

            DateTime deadline = DateTime.MinValue;
            var noDeadline = string.IsNullOrWhiteSpace(selectedTask.Deadline);
            if (noDeadline || DateTime.TryParse(selectedTask.Deadline, out deadline))
            {
                var changeTaskForm = new ChangeTaskForm(selectedTask.Id, selectedTask.Title, noDeadline ? null : (DateTime?)deadline, _commandDispatcher);
                changeTaskForm.TaskChanged += changeTaskForm_TaskChanged;
                changeTaskForm.StartPosition = FormStartPosition.CenterParent;
                changeTaskForm.ShowDialog(this);
            }
            else
            {
                _logger.ForContext("deadline", selectedTask.Deadline).Error("Task {taskId} has deadline which isn't a valid datetime", selectedTask.Id);
                MessageBox.Show("The tasks deadline isn't in a valid date format", "Error", MessageBoxButtons.OK);
            }
        }

        private void changeTaskForm_TaskChanged(object sender, TaskChangedEventArgs e)
        {
            int indexOfSelectedTask = -1;
            for (int rowIndex = 0; rowIndex < taskGrid.Rows.Count; rowIndex++)
            {
                var titleColumn = taskGrid.Columns["Title"];    
                var titleCell = taskGrid.Rows[rowIndex].Cells[titleColumn.Index];
                var existingTitle = titleCell.Value;
                if (existingTitle.ToString() == e.OldTitle)
                {
                    indexOfSelectedTask = rowIndex;
                    if (existingTitle.ToString() != e.NewTitle)
                    {
                        titleCell.Value = e.NewTitle;
                    }

                    var deadlineColumn = taskGrid.Columns["Deadline"];
                    var deadlineCell = taskGrid.Rows[rowIndex].Cells[deadlineColumn.Index];
                    var currentDeadline = deadlineCell.Value;
                    if (currentDeadline == null || e.NewDeadline != DateTime.Parse(currentDeadline.ToString()))
                    {
                        deadlineCell.Value = e.NewDeadline;
                    }
                    break;
                }
            }

            if (indexOfSelectedTask == -1)
            {
                _logger.ForContext("newTitle", e.NewTitle)
                    .ForContext("newDeadline", e.NewDeadline)
                    .Error("Something went wrong updating the UI with new title and/or deadline of task");
                MessageBox.Show("Sorry, something went wrong showing the updated task title and/or deadline", "Error",
                    MessageBoxButtons.OK);
            }
        }

        private void taskGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var selectedTask = FindTaskByRowIndex(_allTasksInProject.ToList(), e.RowIndex);
            if (TaskSelected != null)
            {
                var eventArgs = new TaskSelectedEventArgs(selectedTask.Id, selectedTask.Note);
                TaskSelected(this, eventArgs);
            }
        }
    }
}
