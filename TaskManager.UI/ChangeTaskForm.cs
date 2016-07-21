using System;
using System.Windows.Forms;
using Serilog;
using TaskManager.Domain.Features.ChangeDeadlineOnTask;
using TaskManager.Domain.Features.ChangeTitleOnTask;
using TaskManager.Domain.Infrastructure;
using TaskManager.TasksInGridViewUI;

namespace TaskManager
{
    public partial class ChangeTaskForm : Form
    {
        private readonly string _taskId;
        private readonly string _title;
        private readonly DateTime _deadline;
        private readonly CommandDispatcher _commandDispatcher;
        private ILogger _logger;
        public event EventHandler<TaskChangedEventArgs> TaskChanged;

        public ChangeTaskForm(string taskId, string title, DateTime deadline, CommandDispatcher commandDispatcher)
        {
            if (string.IsNullOrWhiteSpace(taskId)) throw new ArgumentException("taskId cannot be null or empty", "taskId");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("title cannot be null or empty", "title");
            if (commandDispatcher == null) throw new ArgumentNullException("commandDispatcher");

            _taskId = taskId;
            _title = title;
            _deadline = deadline;
            _commandDispatcher = commandDispatcher;
            _logger = Logging.Logger;

            InitializeComponent();
            deadlineDateTimePicker.Format = DateTimePickerFormat.Custom;
            deadlineDateTimePicker.CustomFormat = "dd-MM-yyyy HH:mm";
            deadlineDateTimePicker.Value = deadline;
            titleTextBox.Text = title;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var newTitle = titleTextBox.Text;
            var newDeadline = deadlineDateTimePicker.Value;

            if (newTitle == _title && newDeadline == _deadline)
            {
                Close();
                return;
            }

            try
            {
                if (newTitle != _title)
                {
                    var changeTitleOnTask = new ChangeTitleOnTask(_taskId, newTitle);
                    _commandDispatcher.Send(changeTitleOnTask);
                }

                if (newDeadline != _deadline)
                {
                    var changeDeadlineOnTask = new ChangeDeadlineOnTask(_taskId, newDeadline);
                    _commandDispatcher.Send(changeDeadlineOnTask);
                }
            }
            catch (Exception ex)
            {
                _logger.ForContext("newTitle", newTitle).ForContext("newDeadline", newDeadline).Error(ex, "Something went wrong changing task title and/or deadline");
                MessageBox.Show("Sorry, something went wrong changing task title and/or deadline", "Error",
                    MessageBoxButtons.OK);
            }

            if (TaskChanged != null)
            {
                var eventArgs = new TaskChangedEventArgs(_taskId, newTitle, newDeadline, _title);
                TaskChanged(this, eventArgs);
            }

            Close();
        }
    }
}
