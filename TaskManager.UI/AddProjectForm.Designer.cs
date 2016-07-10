namespace TaskManager
{
    partial class AddProjectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addProjectButton = new System.Windows.Forms.Button();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.deadlineDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.hasDeadlineCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // addProjectButton
            // 
            this.addProjectButton.Location = new System.Drawing.Point(228, 92);
            this.addProjectButton.Name = "addProjectButton";
            this.addProjectButton.Size = new System.Drawing.Size(75, 23);
            this.addProjectButton.TabIndex = 3;
            this.addProjectButton.Text = "Add";
            this.addProjectButton.UseVisualStyleBackColor = true;
            this.addProjectButton.Click += new System.EventHandler(this.addProjectButton_Click);
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(68, 11);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(238, 20);
            this.titleTextBox.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(9, 14);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(27, 13);
            this.titleLabel.TabIndex = 4;
            this.titleLabel.Text = "Title";
            // 
            // deadlineDateTimePicker
            // 
            this.deadlineDateTimePicker.Location = new System.Drawing.Point(106, 43);
            this.deadlineDateTimePicker.Name = "deadlineDateTimePicker";
            this.deadlineDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.deadlineDateTimePicker.TabIndex = 2;
            // 
            // hasDeadlineCheckBox
            // 
            this.hasDeadlineCheckBox.AutoSize = true;
            this.hasDeadlineCheckBox.Location = new System.Drawing.Point(12, 45);
            this.hasDeadlineCheckBox.Name = "hasDeadlineCheckBox";
            this.hasDeadlineCheckBox.Size = new System.Drawing.Size(88, 17);
            this.hasDeadlineCheckBox.TabIndex = 1;
            this.hasDeadlineCheckBox.Text = "Has deadline";
            this.hasDeadlineCheckBox.UseVisualStyleBackColor = true;
            this.hasDeadlineCheckBox.CheckedChanged += new System.EventHandler(this.hasDeadlineCheckBox_CheckedChanged);
            // 
            // AddProjectForm
            // 
            this.AcceptButton = this.addProjectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 127);
            this.Controls.Add(this.hasDeadlineCheckBox);
            this.Controls.Add(this.deadlineDateTimePicker);
            this.Controls.Add(this.addProjectButton);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.titleLabel);
            this.Name = "AddProjectForm";
            this.Text = "New Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addProjectButton;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.DateTimePicker deadlineDateTimePicker;
        private System.Windows.Forms.CheckBox hasDeadlineCheckBox;
    }
}