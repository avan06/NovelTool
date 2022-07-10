
namespace NovelTool
{
    partial class GenerateView
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
            this.Panel1 = new System.Windows.Forms.Panel();
            this.OutputView = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ToolStripSave = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripPrevious = new System.Windows.Forms.ToolStripButton();
            this.ToolStripNext = new System.Windows.Forms.ToolStripButton();
            this.ToolStripPage = new System.Windows.Forms.ToolStripLabel();
            this.ToolStripPageBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripZoomIn = new System.Windows.Forms.ToolStripButton();
            this.ToolStripZoomOut = new System.Windows.Forms.ToolStripButton();
            this.ToolStripZoomFactorBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripFilter = new System.Windows.Forms.ToolStripLabel();
            this.ToolStripFilterBox = new System.Windows.Forms.ToolStripComboBox();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutputView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.AutoScroll = true;
            this.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.Panel1.Controls.Add(this.OutputView);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 25);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(800, 403);
            this.Panel1.TabIndex = 2;
            // 
            // OutputView
            // 
            this.OutputView.BackColor = System.Drawing.Color.Transparent;
            this.OutputView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutputView.Location = new System.Drawing.Point(0, 0);
            this.OutputView.Name = "OutputView";
            this.OutputView.Size = new System.Drawing.Size(235, 205);
            this.OutputView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.OutputView.TabIndex = 1;
            this.OutputView.TabStop = false;
            this.OutputView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OutputView_MouseDown);
            this.OutputView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OutputView_MouseMove);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripSave,
            this.ToolStripSaveAll,
            this.toolStripSeparator1,
            this.ToolStripPrevious,
            this.ToolStripNext,
            this.ToolStripPage,
            this.ToolStripPageBox,
            this.toolStripSeparator2,
            this.ToolStripZoomIn,
            this.ToolStripZoomOut,
            this.ToolStripZoomFactorBox,
            this.toolStripSeparator3,
            this.toolStripFilter,
            this.ToolStripFilterBox});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // ToolStripSave
            // 
            this.ToolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripSave.Image = global::NovelTool.Properties.Resources.Save_16x;
            this.ToolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripSave.Name = "ToolStripSave";
            this.ToolStripSave.Size = new System.Drawing.Size(23, 22);
            this.ToolStripSave.Text = "toolStripSave";
            this.ToolStripSave.ToolTipText = "toolStripSave";
            this.ToolStripSave.Click += new System.EventHandler(this.ToolStripSave_Click);
            // 
            // ToolStripSaveAll
            // 
            this.ToolStripSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripSaveAll.Image = global::NovelTool.Properties.Resources.SaveAll_16x;
            this.ToolStripSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripSaveAll.Name = "ToolStripSaveAll";
            this.ToolStripSaveAll.Size = new System.Drawing.Size(23, 22);
            this.ToolStripSaveAll.Text = "toolStripSaveAll";
            this.ToolStripSaveAll.ToolTipText = "toolStripSaveAll";
            this.ToolStripSaveAll.Click += new System.EventHandler(this.ToolStripSaveAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripPrevious
            // 
            this.ToolStripPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripPrevious.Image = global::NovelTool.Properties.Resources.Previous_16x;
            this.ToolStripPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripPrevious.Name = "ToolStripPrevious";
            this.ToolStripPrevious.Size = new System.Drawing.Size(23, 22);
            this.ToolStripPrevious.Text = "toolStripPrevious";
            this.ToolStripPrevious.ToolTipText = "toolStripPrevious";
            this.ToolStripPrevious.Click += new System.EventHandler(this.ToolStripPrevious_Click);
            // 
            // ToolStripNext
            // 
            this.ToolStripNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripNext.Image = global::NovelTool.Properties.Resources.Next_16x;
            this.ToolStripNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripNext.Name = "ToolStripNext";
            this.ToolStripNext.Size = new System.Drawing.Size(23, 22);
            this.ToolStripNext.Text = "toolStripNext";
            this.ToolStripNext.ToolTipText = "toolStripNext";
            this.ToolStripNext.Click += new System.EventHandler(this.ToolStripNext_Click);
            // 
            // ToolStripPage
            // 
            this.ToolStripPage.ForeColor = System.Drawing.Color.Black;
            this.ToolStripPage.Name = "ToolStripPage";
            this.ToolStripPage.Size = new System.Drawing.Size(87, 22);
            this.ToolStripPage.Text = "Current page: ";
            // 
            // ToolStripPageBox
            // 
            this.ToolStripPageBox.AutoSize = false;
            this.ToolStripPageBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ToolStripPageBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ToolStripPageBox.Name = "ToolStripPageBox";
            this.ToolStripPageBox.Size = new System.Drawing.Size(50, 23);
            this.ToolStripPageBox.SelectedIndexChanged += new System.EventHandler(this.ToolStripPageBox_SelectedIndexChanged);
            this.ToolStripPageBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripPageBox_KeyUp);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripZoomIn
            // 
            this.ToolStripZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripZoomIn.Image = global::NovelTool.Properties.Resources.ZoomIn_16x;
            this.ToolStripZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripZoomIn.Name = "ToolStripZoomIn";
            this.ToolStripZoomIn.Size = new System.Drawing.Size(23, 22);
            this.ToolStripZoomIn.Text = "toolStripZoomIn";
            this.ToolStripZoomIn.ToolTipText = "toolStripZoomIn";
            this.ToolStripZoomIn.Click += new System.EventHandler(this.ToolStripZoomIn_Click);
            // 
            // ToolStripZoomOut
            // 
            this.ToolStripZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripZoomOut.Image = global::NovelTool.Properties.Resources.ZoomOut_16x;
            this.ToolStripZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripZoomOut.Name = "ToolStripZoomOut";
            this.ToolStripZoomOut.Size = new System.Drawing.Size(23, 22);
            this.ToolStripZoomOut.Text = "toolStripZoomOut";
            this.ToolStripZoomOut.ToolTipText = "toolStripZoomOut";
            this.ToolStripZoomOut.Click += new System.EventHandler(this.ToolStripZoomOut_Click);
            // 
            // ToolStripZoomFactorBox
            // 
            this.ToolStripZoomFactorBox.AutoSize = false;
            this.ToolStripZoomFactorBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ToolStripZoomFactorBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ToolStripZoomFactorBox.Items.AddRange(new object[] {
            "100",
            "150",
            "200",
            "250",
            "300",
            "350",
            "400",
            "450",
            "500"});
            this.ToolStripZoomFactorBox.Name = "ToolStripZoomFactorBox";
            this.ToolStripZoomFactorBox.Size = new System.Drawing.Size(50, 23);
            this.ToolStripZoomFactorBox.SelectedIndexChanged += new System.EventHandler(this.ToolStripZoomFactorBox_SelectedIndexChanged);
            this.ToolStripZoomFactorBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripZoomFactorBox_KeyUp);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripFilter
            // 
            this.toolStripFilter.ForeColor = System.Drawing.Color.Black;
            this.toolStripFilter.Name = "toolStripFilter";
            this.toolStripFilter.Size = new System.Drawing.Size(37, 22);
            this.toolStripFilter.Text = "Filter:";
            // 
            // ToolStripFilterBox
            // 
            this.ToolStripFilterBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ToolStripFilterBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ToolStripFilterBox.Name = "ToolStripFilterBox";
            this.ToolStripFilterBox.Size = new System.Drawing.Size(121, 25);
            // 
            // GenerateView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "GenerateView";
            this.Text = "GenerateView";
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutputView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.PictureBox OutputView;
        private System.Windows.Forms.ToolStripButton ToolStripPrevious;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.ToolStripButton ToolStripNext;
        private System.Windows.Forms.ToolStripLabel ToolStripPage;
        private System.Windows.Forms.ToolStripComboBox ToolStripPageBox;
        private System.Windows.Forms.ToolStripButton ToolStripSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripFilter;
        private System.Windows.Forms.ToolStripComboBox ToolStripFilterBox;
        private System.Windows.Forms.ToolStripButton ToolStripZoomIn;
        private System.Windows.Forms.ToolStripButton ToolStripZoomOut;
        private System.Windows.Forms.ToolStripComboBox ToolStripZoomFactorBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ToolStripSaveAll;
    }
}