namespace TaskManager
{
    partial class ProjectTreeControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.projectTree = new System.Windows.Forms.TreeView();
            this.projectTreeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectTreeNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newProjectButton = new System.Windows.Forms.Button();
            this.projectTreeContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // projectTree
            // 
            this.projectTree.ContextMenuStrip = this.projectTreeContextMenuStrip;
            this.projectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectTree.HideSelection = false;
            this.projectTree.Location = new System.Drawing.Point(0, 0);
            this.projectTree.Name = "projectTree";
            this.projectTree.Size = new System.Drawing.Size(367, 656);
            this.projectTree.TabIndex = 1;
            this.projectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projectTree_AfterSelect);
            this.projectTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.projectTree_MouseUp);
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
            // projectTreeNodeContextMenuStrip
            // 
            this.projectTreeNodeContextMenuStrip.Name = "projectTreeNodeContextMenuStrip";
            this.projectTreeNodeContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // newProjectButton
            // 
            this.newProjectButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.newProjectButton.Location = new System.Drawing.Point(0, 0);
            this.newProjectButton.Name = "newProjectButton";
            this.newProjectButton.Size = new System.Drawing.Size(367, 23);
            this.newProjectButton.TabIndex = 7;
            this.newProjectButton.Text = "New Project";
            this.newProjectButton.UseVisualStyleBackColor = true;
            this.newProjectButton.Click += new System.EventHandler(this.newProjectButton_Click);
            // 
            // ProjectTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.newProjectButton);
            this.Controls.Add(this.projectTree);
            this.Name = "ProjectTreeControl";
            this.Size = new System.Drawing.Size(367, 656);
            this.projectTreeContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView projectTree;
        private System.Windows.Forms.ContextMenuStrip projectTreeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addProjectToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip projectTreeNodeContextMenuStrip;
        private System.Windows.Forms.Button newProjectButton;
    }
}
