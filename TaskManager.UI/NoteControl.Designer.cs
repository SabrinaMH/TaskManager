namespace TaskManager
{
    partial class NoteControl
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
            this.noteRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // noteRichTextBox
            // 
            this.noteRichTextBox.Location = new System.Drawing.Point(3, 36);
            this.noteRichTextBox.Name = "noteRichTextBox";
            this.noteRichTextBox.Size = new System.Drawing.Size(575, 149);
            this.noteRichTextBox.TabIndex = 0;
            this.noteRichTextBox.Text = "";
            this.noteRichTextBox.TextChanged += new System.EventHandler(this.noteRichTextBox_TextChanged);
            this.noteRichTextBox.Leave += new System.EventHandler(this.noteRichTextBox_Leave);
            // 
            // NoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.noteRichTextBox);
            this.Name = "NoteControl";
            this.Size = new System.Drawing.Size(581, 217);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox noteRichTextBox;
    }
}
