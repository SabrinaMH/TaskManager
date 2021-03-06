﻿namespace TaskManager
{
    partial class MainForm
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.rightSideSplitContainer = new System.Windows.Forms.SplitContainer();
            this.projectTreeControl = new TaskManager.ProjectTreeControl();
            this.taskGridControl = new TaskManager.TaskGridControl();
            this.noteControl = new TaskManager.NoteControl();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightSideSplitContainer)).BeginInit();
            this.rightSideSplitContainer.Panel1.SuspendLayout();
            this.rightSideSplitContainer.Panel2.SuspendLayout();
            this.rightSideSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.AccessibleName = "projectTreeViewPanel";
            this.mainSplitContainer.Panel1.Controls.Add(this.projectTreeControl);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.rightSideSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(944, 568);
            this.mainSplitContainer.SplitterDistance = 311;
            this.mainSplitContainer.TabIndex = 5;
            // 
            // rightSideSplitContainer
            // 
            this.rightSideSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightSideSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.rightSideSplitContainer.Name = "rightSideSplitContainer";
            this.rightSideSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // rightSideSplitContainer.Panel1
            // 
            this.rightSideSplitContainer.Panel1.AccessibleName = "taskViewPanel";
            this.rightSideSplitContainer.Panel1.Controls.Add(this.taskGridControl);
            // 
            // rightSideSplitContainer.Panel2
            // 
            this.rightSideSplitContainer.Panel2.AccessibleName = "noteViewPanel";
            this.rightSideSplitContainer.Panel2.Controls.Add(this.noteControl);
            this.rightSideSplitContainer.Size = new System.Drawing.Size(629, 568);
            this.rightSideSplitContainer.SplitterDistance = 179;
            this.rightSideSplitContainer.TabIndex = 0;
            // 
            // projectTreeControl
            // 
            this.projectTreeControl.AutoSize = true;
            this.projectTreeControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.projectTreeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectTreeControl.Location = new System.Drawing.Point(0, 0);
            this.projectTreeControl.Name = "projectTreeControl";
            this.projectTreeControl.Size = new System.Drawing.Size(311, 568);
            this.projectTreeControl.TabIndex = 0;
            // 
            // taskGridControl
            // 
            this.taskGridControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.taskGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taskGridControl.Location = new System.Drawing.Point(0, 0);
            this.taskGridControl.Name = "taskGridControl";
            this.taskGridControl.Size = new System.Drawing.Size(629, 179);
            this.taskGridControl.TabIndex = 0;
            // 
            // noteControl
            // 
            this.noteControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noteControl.Location = new System.Drawing.Point(0, 0);
            this.noteControl.Name = "noteControl";
            this.noteControl.Size = new System.Drawing.Size(629, 385);
            this.noteControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 568);
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "MainForm";
            this.Text = "Task Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel1.PerformLayout();
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.rightSideSplitContainer.Panel1.ResumeLayout(false);
            this.rightSideSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rightSideSplitContainer)).EndInit();
            this.rightSideSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer rightSideSplitContainer;
        private ProjectTreeControl projectTreeControl;
        private TaskGridControl taskGridControl;
        private NoteControl noteControl;
    }
}

