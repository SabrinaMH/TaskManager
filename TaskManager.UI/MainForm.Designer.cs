namespace TaskManager
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
            this.components = new System.ComponentModel.Container();
            this.projectTreeView = new System.Windows.Forms.TreeView();
            this.projectTreeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.taskGridView = new System.Windows.Forms.DataGridView();
            this.taskInGridViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.projectTreeNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.rightSideSplitContainer = new System.Windows.Forms.SplitContainer();
            this.noteControl = new TaskManager.NoteControl();
            this.newProjectButton = new System.Windows.Forms.Button();
            this.projectTreeContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskGridView)).BeginInit();
            this.taskInGridViewContextMenuStrip.SuspendLayout();
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
            // projectTreeView
            // 
            this.projectTreeView.ContextMenuStrip = this.projectTreeContextMenuStrip;
            this.projectTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectTreeView.Location = new System.Drawing.Point(0, 0);
            this.projectTreeView.Name = "projectTreeView";
            this.projectTreeView.Size = new System.Drawing.Size(304, 506);
            this.projectTreeView.TabIndex = 0;
            this.projectTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projectTreeView_AfterSelect);
            this.projectTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.projectTreeView_MouseUp);
            // 
            // projectTreeContextMenuStrip
            // 
            this.projectTreeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProjectToolStripMenuItem});
            this.projectTreeContextMenuStrip.Name = "projectTreeContextMenuStrip";
            this.projectTreeContextMenuStrip.Size = new System.Drawing.Size(137, 26);
            // 
            // addProjectToolStripMenuItem
            // 
            this.addProjectToolStripMenuItem.Name = "addProjectToolStripMenuItem";
            this.addProjectToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.addProjectToolStripMenuItem.Text = "Add project";
            this.addProjectToolStripMenuItem.Click += new System.EventHandler(this.addProjectToolStripMenuItem_Click);
            // 
            // taskGridView
            // 
            this.taskGridView.AllowUserToDeleteRows = false;
            this.taskGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.taskGridView.ContextMenuStrip = this.taskInGridViewContextMenuStrip;
            this.taskGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taskGridView.Location = new System.Drawing.Point(0, 0);
            this.taskGridView.MultiSelect = false;
            this.taskGridView.Name = "taskGridView";
            this.taskGridView.Size = new System.Drawing.Size(612, 160);
            this.taskGridView.TabIndex = 3;
            this.taskGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.taskGridView_CellMouseUp);
            this.taskGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.taskGridView_CellValueChanged);
            this.taskGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.taskGridView_CurrentCellDirtyStateChanged);
            this.taskGridView.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.taskGridView_RowStateChanged);
            // 
            // taskInGridViewContextMenuStrip
            // 
            this.taskInGridViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTaskToolStripMenuItem});
            this.taskInGridViewContextMenuStrip.Name = "taskInGridViewContextMenuStrip";
            this.taskInGridViewContextMenuStrip.Size = new System.Drawing.Size(121, 26);
            // 
            // addTaskToolStripMenuItem
            // 
            this.addTaskToolStripMenuItem.Name = "addTaskToolStripMenuItem";
            this.addTaskToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.addTaskToolStripMenuItem.Text = "Add task";
            this.addTaskToolStripMenuItem.Click += new System.EventHandler(this.addTaskToolStripMenuItem_Click);
            // 
            // projectTreeNodeContextMenuStrip
            // 
            this.projectTreeNodeContextMenuStrip.Name = "projectTreeNodeContextMenuStrip";
            this.projectTreeNodeContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Location = new System.Drawing.Point(12, 50);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.AccessibleName = "projectTreeViewPanel";
            this.mainSplitContainer.Panel1.Controls.Add(this.projectTreeView);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.rightSideSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(920, 506);
            this.mainSplitContainer.SplitterDistance = 304;
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
            this.rightSideSplitContainer.Panel1.Controls.Add(this.taskGridView);
            // 
            // rightSideSplitContainer.Panel2
            // 
            this.rightSideSplitContainer.Panel2.AccessibleName = "noteViewPanel";
            this.rightSideSplitContainer.Panel2.Controls.Add(this.noteControl);
            this.rightSideSplitContainer.Size = new System.Drawing.Size(612, 506);
            this.rightSideSplitContainer.SplitterDistance = 160;
            this.rightSideSplitContainer.TabIndex = 0;
            // 
            // noteControl
            // 
            this.noteControl.Location = new System.Drawing.Point(0, 2);
            this.noteControl.Name = "noteControl";
            this.noteControl.Size = new System.Drawing.Size(612, 340);
            this.noteControl.TabIndex = 0;
            // 
            // newProjectButton
            // 
            this.newProjectButton.Location = new System.Drawing.Point(12, 12);
            this.newProjectButton.Name = "newProjectButton";
            this.newProjectButton.Size = new System.Drawing.Size(75, 23);
            this.newProjectButton.TabIndex = 6;
            this.newProjectButton.Text = "New Project";
            this.newProjectButton.UseVisualStyleBackColor = true;
            this.newProjectButton.Click += new System.EventHandler(this.newProjectButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 568);
            this.Controls.Add(this.newProjectButton);
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "MainForm";
            this.Text = "Task Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.projectTreeContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.taskGridView)).EndInit();
            this.taskInGridViewContextMenuStrip.ResumeLayout(false);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
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

        private System.Windows.Forms.TreeView projectTreeView;
        private System.Windows.Forms.DataGridView taskGridView;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ContextMenuStrip projectTreeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addProjectToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip projectTreeNodeContextMenuStrip;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer rightSideSplitContainer;
        private System.Windows.Forms.ContextMenuStrip taskInGridViewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addTaskToolStripMenuItem;
        private System.Windows.Forms.Button newProjectButton;
        private NoteControl noteControl;
    }
}

