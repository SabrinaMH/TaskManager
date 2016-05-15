﻿using System;
using System.Windows.Forms;
using MediatR;
using Serilog;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Infrastructure;

namespace TaskManager
{
    public partial class AddProjectForm : Form
    {
        private readonly IMediator _mediator;
        private ILogger _logger;
        public event EventHandler<ProjectEventArgs> ProjectRegistered;

        public AddProjectForm(IMediator mediator)
        {
            _mediator = mediator;
            _logger = Logging.Logger;
            InitializeComponent();
            deadlineDateTimePicker.Visible = false;
        }

        private void addProjectButton_Click(object sender, EventArgs e)
        {
            DateTime? deadline = null;
            if (hasDeadlineCheckBox.Checked)
            {
                deadline = deadlineDateTimePicker.Value;
            }
            string title = titleTextBox.Text;
            var registerProject = new RegisterProject(title, deadline);
            try
            {
                _mediator.Send(registerProject);

                if (ProjectRegistered != null)
                {
                    var eventArgs = new ProjectEventArgs(title, deadline);
                    ProjectRegistered(this, eventArgs);
                }
                Close();
            }
            catch (ProjectWithSameTitleExistsException ex)
            {
                _logger.Error(ex, "A project with title {title} already exists", registerProject.Title);
                MessageBox.Show("A project with this title already exists", "Error", MessageBoxButtons.OK);
            }
        }

        private void hasDeadlineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            deadlineDateTimePicker.Visible = hasDeadlineCheckBox.Checked;
        }
    }
}
