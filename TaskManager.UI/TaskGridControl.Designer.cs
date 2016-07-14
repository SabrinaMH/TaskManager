namespace TaskManager
{
    partial class TaskGridControl
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
            this.taskGrid = new System.Windows.Forms.DataGridView();
            this.taskGridContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.taskGrid)).BeginInit();
            this.taskGridContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // taskGrid
            // 
            this.taskGrid.AllowUserToDeleteRows = false;
            this.taskGrid.AllowUserToResizeRows = false;
            this.taskGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.taskGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.taskGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.taskGrid.ContextMenuStrip = this.taskGridContextMenuStrip;
            this.taskGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taskGrid.Location = new System.Drawing.Point(0, 0);
            this.taskGrid.MultiSelect = false;
            this.taskGrid.Name = "taskGrid";
            this.taskGrid.Size = new System.Drawing.Size(791, 276);
            this.taskGrid.TabIndex = 4;
            this.taskGrid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.taskGrid_CellMouseUp);
            this.taskGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.taskGrid_CellValueChanged);
            this.taskGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.taskGrid_CurrentCellDirtyStateChanged);
            this.taskGrid.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.taskGrid_RowStateChanged);
            // 
            // taskGridContextMenuStrip
            // 
            this.taskGridContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTaskToolStripMenuItem});
            this.taskGridContextMenuStrip.Name = "taskInGridViewContextMenuStrip";
            this.taskGridContextMenuStrip.Size = new System.Drawing.Size(121, 26);
            // 
            // addTaskToolStripMenuItem
            // 
            this.addTaskToolStripMenuItem.Name = "addTaskToolStripMenuItem";
            this.addTaskToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.addTaskToolStripMenuItem.Text = "Add task";
            this.addTaskToolStripMenuItem.Click += new System.EventHandler(this.addTaskToolStripMenuItem_Click);
            // 
            // TaskGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.taskGrid);
            this.Name = "TaskGridControl";
            this.Size = new System.Drawing.Size(791, 276);
            ((System.ComponentModel.ISupportInitialize)(this.taskGrid)).EndInit();
            this.taskGridContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView taskGrid;
        private System.Windows.Forms.ContextMenuStrip taskGridContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addTaskToolStripMenuItem;
    }
}
