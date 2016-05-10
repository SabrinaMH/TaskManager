namespace TaskManager
{
    partial class AddTaskForm
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
            this.addTaskButton = new System.Windows.Forms.Button();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.deadlineDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.hasDeadlineCheckBox = new System.Windows.Forms.CheckBox();
            this.taskPriorityLabel = new System.Windows.Forms.Label();
            this.taskPriorityComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // addTaskButton
            // 
            this.addTaskButton.Location = new System.Drawing.Point(228, 98);
            this.addTaskButton.Name = "addTaskButton";
            this.addTaskButton.Size = new System.Drawing.Size(75, 23);
            this.addTaskButton.TabIndex = 5;
            this.addTaskButton.Text = "Add";
            this.addTaskButton.UseVisualStyleBackColor = true;
            this.addTaskButton.Click += new System.EventHandler(this.addTaskButton_Click);
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(68, 11);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(238, 20);
            this.titleTextBox.TabIndex = 4;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(9, 14);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(27, 13);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "Title";
            // 
            // deadlineDateTimePicker
            // 
            this.deadlineDateTimePicker.Location = new System.Drawing.Point(106, 72);
            this.deadlineDateTimePicker.Name = "deadlineDateTimePicker";
            this.deadlineDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.deadlineDateTimePicker.TabIndex = 6;
            // 
            // hasDeadlineCheckBox
            // 
            this.hasDeadlineCheckBox.AutoSize = true;
            this.hasDeadlineCheckBox.Location = new System.Drawing.Point(12, 74);
            this.hasDeadlineCheckBox.Name = "hasDeadlineCheckBox";
            this.hasDeadlineCheckBox.Size = new System.Drawing.Size(88, 17);
            this.hasDeadlineCheckBox.TabIndex = 8;
            this.hasDeadlineCheckBox.Text = "Has deadline";
            this.hasDeadlineCheckBox.UseVisualStyleBackColor = true;
            this.hasDeadlineCheckBox.CheckedChanged += new System.EventHandler(this.hasDeadlineCheckBox_CheckedChanged);
            // 
            // taskPriorityLabel
            // 
            this.taskPriorityLabel.AutoSize = true;
            this.taskPriorityLabel.Location = new System.Drawing.Point(9, 45);
            this.taskPriorityLabel.Name = "taskPriorityLabel";
            this.taskPriorityLabel.Size = new System.Drawing.Size(38, 13);
            this.taskPriorityLabel.TabIndex = 9;
            this.taskPriorityLabel.Text = "Priority";
            // 
            // taskPriorityComboBox
            // 
            this.taskPriorityComboBox.FormattingEnabled = true;
            this.taskPriorityComboBox.Location = new System.Drawing.Point(185, 42);
            this.taskPriorityComboBox.Name = "taskPriorityComboBox";
            this.taskPriorityComboBox.Size = new System.Drawing.Size(121, 21);
            this.taskPriorityComboBox.TabIndex = 10;
            // 
            // AddTaskForm
            // 
            this.AcceptButton = this.addTaskButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 211);
            this.Controls.Add(this.taskPriorityComboBox);
            this.Controls.Add(this.taskPriorityLabel);
            this.Controls.Add(this.hasDeadlineCheckBox);
            this.Controls.Add(this.deadlineDateTimePicker);
            this.Controls.Add(this.addTaskButton);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.titleLabel);
            this.Name = "AddTaskForm";
            this.Text = "New Task";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addTaskButton;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.DateTimePicker deadlineDateTimePicker;
        private System.Windows.Forms.CheckBox hasDeadlineCheckBox;
        private System.Windows.Forms.Label taskPriorityLabel;
        private System.Windows.Forms.ComboBox taskPriorityComboBox;
    }
}