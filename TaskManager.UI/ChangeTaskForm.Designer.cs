namespace TaskManager
{
    partial class ChangeTaskForm
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
            this.deadlineDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.deadlineLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deadlineDateTimePicker
            // 
            this.deadlineDateTimePicker.Location = new System.Drawing.Point(82, 49);
            this.deadlineDateTimePicker.Name = "deadlineDateTimePicker";
            this.deadlineDateTimePicker.Size = new System.Drawing.Size(232, 20);
            this.deadlineDateTimePicker.TabIndex = 1;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(13, 13);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(27, 13);
            this.titleLabel.TabIndex = 4;
            this.titleLabel.Text = "Title";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(82, 10);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(232, 20);
            this.titleTextBox.TabIndex = 0;
            // 
            // deadlineLabel
            // 
            this.deadlineLabel.AutoSize = true;
            this.deadlineLabel.Location = new System.Drawing.Point(13, 55);
            this.deadlineLabel.Name = "deadlineLabel";
            this.deadlineLabel.Size = new System.Drawing.Size(49, 13);
            this.deadlineLabel.TabIndex = 3;
            this.deadlineLabel.Text = "Deadline";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(238, 92);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // ChangeTaskForm
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 130);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.deadlineLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.deadlineDateTimePicker);
            this.Name = "ChangeTaskForm";
            this.Text = "Change Task";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker deadlineDateTimePicker;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label deadlineLabel;
        private System.Windows.Forms.Button saveButton;
    }
}