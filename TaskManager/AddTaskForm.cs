using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MediatR;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager
{
    public partial class AddTaskForm : Form
    {
        private readonly Guid _projectId;
        private readonly IMediator _mediator;
        public event EventHandler TaskRegistered;

        public AddTaskForm(Guid projectId, IMediator mediator)
        {
            InitializeComponent();
            PopulatePriorityDropDown();

            _projectId = projectId;
            _mediator = mediator;
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
            var registerTask = new RegisterTask(_projectId, titleTextBox.Text, priority, deadline);
            try
            {
                _mediator.Send(registerTask);
                if (TaskRegistered != null)
                {
                    TaskRegistered(this, EventArgs.Empty);
                }
                Close();
            }
            catch (TaskWithSameTitleExistsInProjectException ex)
            {
                MessageBox.Show("A task with this title already exists in the project", "Error", MessageBoxButtons.OK);
            }
        }

        private void hasDeadlineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            deadlineDateTimePicker.Visible = hasDeadlineCheckBox.Checked;
        }
    }
}
