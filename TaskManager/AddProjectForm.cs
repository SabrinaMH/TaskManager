using System;
using System.Windows.Forms;
using MediatR;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Infrastructure;

namespace TaskManager
{
    public partial class AddProjectForm : Form
    {
        private readonly IMediator _mediator;
        public event EventHandler ProjectRegistered;

        public AddProjectForm()
        {
            InitializeComponent();
            deadlineDateTimePicker.Visible = false;
            var mediate = new Mediate();
            _mediator = mediate.Bootstrap();
        }

        private void addProjectButton_Click(object sender, EventArgs e)
        {
            DateTime? deadline = null;
            if (hasDeadlineCheckBox.Checked)
            {
                deadline = deadlineDateTimePicker.Value;
            }
            var registerProject = new RegisterProject(titleTextBox.Text, deadline);
            try
            {
                _mediator.Send(registerProject);

                if (ProjectRegistered != null)
                {
                    ProjectRegistered(this, EventArgs.Empty);
                }
            }
            catch (ProjectWithSameTitleExistsException ex)
            {
                MessageBox.Show("A project with this title already exists", "Error", MessageBoxButtons.OK);
            }
        }

        private void hasDeadlineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            deadlineDateTimePicker.Visible = hasDeadlineCheckBox.Checked;
        }
    }
}
