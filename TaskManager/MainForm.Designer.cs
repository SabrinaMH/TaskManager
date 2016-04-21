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
            this.addProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.taskGridView = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.projectTreeNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.priorityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectTreeContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskGridView)).BeginInit();
            this.projectTreeNodeContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // projectTreeView
            // 
            this.projectTreeView.ContextMenuStrip = this.projectTreeContextMenuStrip;
            this.projectTreeView.Location = new System.Drawing.Point(13, 44);
            this.projectTreeView.Name = "projectTreeView";
            this.projectTreeView.Size = new System.Drawing.Size(149, 486);
            this.projectTreeView.TabIndex = 0;
            // 
            // projectTreeContextMenuStrip
            // 
            this.projectTreeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProjectMenuItem});
            this.projectTreeContextMenuStrip.Name = "projectTreeContextMenuStrip";
            this.projectTreeContextMenuStrip.Size = new System.Drawing.Size(137, 26);
            // 
            // addProjectMenuItem
            // 
            this.addProjectMenuItem.Name = "addProjectMenuItem";
            this.addProjectMenuItem.Size = new System.Drawing.Size(136, 22);
            this.addProjectMenuItem.Text = "Add project";
            this.addProjectMenuItem.Click += new System.EventHandler(this.addProjectMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(944, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // taskGridView
            // 
            this.taskGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.taskGridView.Location = new System.Drawing.Point(168, 44);
            this.taskGridView.Name = "taskGridView";
            this.taskGridView.Size = new System.Drawing.Size(764, 316);
            this.taskGridView.TabIndex = 3;
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(168, 366);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(764, 164);
            this.richTextBox.TabIndex = 4;
            this.richTextBox.Text = "";
            // 
            // projectTreeNodeContextMenuStrip
            // 
            this.projectTreeNodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.priorityMenuItem});
            this.projectTreeNodeContextMenuStrip.Name = "projectTreeNodeContextMenuStrip";
            this.projectTreeNodeContextMenuStrip.Size = new System.Drawing.Size(153, 48);
            // 
            // priorityMenuItem
            // 
            this.priorityMenuItem.Name = "priorityMenuItem";
            this.priorityMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityMenuItem.Text = "Priority";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 568);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.taskGridView);
            this.Controls.Add(this.projectTreeView);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Task Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.projectTreeContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.taskGridView)).EndInit();
            this.projectTreeNodeContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView projectTreeView;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.DataGridView taskGridView;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.ContextMenuStrip projectTreeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addProjectMenuItem;
        private System.Windows.Forms.ContextMenuStrip projectTreeNodeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem priorityMenuItem;
    }
}

