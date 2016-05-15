﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MediatR;
using Serilog;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Infrastructure;
using TaskManager.Domain.Models.Task;

namespace TaskManager
{
    public partial class AddTaskForm : Form
    {
        private readonly string _projectId;
        private readonly IMediator _mediator;
        private ILogger _logger;
        public event EventHandler<TaskEventArgs> TaskRegistered;

        public AddTaskForm(string projectId, IMediator mediator)
        {
            InitializeComponent();
            PopulatePriorityDropDown();
            _logger = Logging.Logger;
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
            string title = titleTextBox.Text;
            var registerTask = new RegisterTask(_projectId, title, priority, deadline);
            try
            {
                _mediator.Send(registerTask);
                if (TaskRegistered != null)
                {
                    var eventArgs = new TaskEventArgs(_projectId, title, priority, deadline);
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
