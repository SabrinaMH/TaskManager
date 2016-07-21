using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Serilog;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Infrastructure;
using TaskManager.TasksInGridViewUI;

namespace TaskManager
{
    public partial class AddTaskForm : Form
    {
        private readonly string _projectId;
        private readonly CommandDispatcher _commandDispatcher;
        private ILogger _logger;
        public event EventHandler<TaskRegisteredEventArgs> TaskRegistered;

        public AddTaskForm(string projectId, CommandDispatcher commandDispatcher)
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId cannot be null or empty", "projectId");
            if (commandDispatcher == null) throw new ArgumentNullException("commandDispatcher");
            InitializeComponent();
            PopulatePriorityDropDown();
            deadlineDateTimePicker.Format = DateTimePickerFormat.Custom;
            deadlineDateTimePicker.CustomFormat = "dd-MM-yyyy HH:mm";  
            _logger = Logging.Logger;
            _projectId = projectId;
            _commandDispatcher = commandDispatcher;
            deadlineDateTimePicker.Visible = false;
        }

        private void PopulatePriorityDropDown()
        {
            var allTaskPrioritiesQuery = new AllTaskPrioritiesQuery();
            var taskPriorityQueryHandler = new TaskPriorityQueryHandler();
            List<string> taskPriorities = taskPriorityQueryHandler.Handle(allTaskPrioritiesQuery);
            taskPriorityComboBox.Items.AddRange(taskPriorities.ToArray());
            taskPriorityComboBox.SelectedItem = taskPriorities[0];
        }

        private void addTaskButton_Click(object sender, EventArgs e)
        {
            DateTime? deadline = null;
            if (hasDeadlineCheckBox.Checked)
            {
                deadline = deadlineDateTimePicker.Value;
            }

            string priority = taskPriorityComboBox.SelectedItem.ToString();
            string title = titleTextBox.Text;
            var registerTask = new RegisterTask(_projectId, title, priority, deadline);
            try
            {
                _commandDispatcher.Send(registerTask);
                if (TaskRegistered != null)
                {
                    var eventArgs = new TaskRegisteredEventArgs(_projectId, title, priority, deadline);
                    TaskRegistered(this, eventArgs);
                }
                Close();
            }
            catch (TaskWithSameTitleExistsInProjectException ex)
            {
                _logger.Error(ex, "A task with title {title} already exists in project {projectId}", registerTask.Title, _projectId);
                MessageBox.Show("A task with this title already exists in the project", "Error", MessageBoxButtons.OK);
            }
        }

        private void hasDeadlineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            deadlineDateTimePicker.Visible = hasDeadlineCheckBox.Checked;
        }
    }
}
