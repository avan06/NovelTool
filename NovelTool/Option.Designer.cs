namespace NovelTool
{
    partial class Option
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
            this.OptionTreeView1 = new OptionTreeView.OptionTreeView();
            this.SuspendLayout();
            // 
            // OptionTreeView1
            // 
            this.OptionTreeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.OptionTreeView1.BackColorLeftView = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.OptionTreeView1.BorderStyleLeftView = System.Windows.Forms.BorderStyle.None;
            this.OptionTreeView1.ContextMenuStripLeftView = null;
            this.OptionTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionTreeView1.FloatingPointDecimalPlaces = 2;
            this.OptionTreeView1.FontLeftView = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.OptionTreeView1.ForeColor = System.Drawing.Color.White;
            this.OptionTreeView1.ForeColorLeftView = System.Drawing.Color.White;
            this.OptionTreeView1.FullRowSelectLeftView = true;
            this.OptionTreeView1.ItemHeightLeftView = 14;
            this.OptionTreeView1.Location = new System.Drawing.Point(0, 0);
            this.OptionTreeView1.Name = "OptionTreeView1";
            this.OptionTreeView1.OptionLeftCollapsed = false;
            this.OptionTreeView1.OptionLeftMinSize = 25;
            this.OptionTreeView1.OptionRightCollapsed = false;
            this.OptionTreeView1.OptionRightMinSize = 25;
            this.OptionTreeView1.Size = new System.Drawing.Size(584, 511);
            this.OptionTreeView1.SortGroupBeforeUnderline = true;
            this.OptionTreeView1.SortTreeBeforeUnderline = true;
            this.OptionTreeView1.SplitterDistance = 160;
            this.OptionTreeView1.SplitterIncrement = 1;
            this.OptionTreeView1.SplitterWidth = 4;
            this.OptionTreeView1.TabIndex = 0;
            // 
            // Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 511);
            this.Controls.Add(this.OptionTreeView1);
            this.Name = "Option";
            this.Text = "Option";
            this.ResumeLayout(false);

        }

        #endregion

        private OptionTreeView.OptionTreeView OptionTreeView1;
    }
}