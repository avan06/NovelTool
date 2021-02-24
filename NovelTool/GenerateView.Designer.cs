
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.outputView = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripPage = new System.Windows.Forms.ToolStripLabel();
            this.toolStripPageBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomFactorBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripFilter = new System.Windows.Forms.ToolStripLabel();
            this.toolStripFilterBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.statusStrip1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 425);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(800, 450);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.outputView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 403);
            this.panel1.TabIndex = 2;
            // 
            // outputView
            // 
            this.outputView.BackColor = System.Drawing.Color.Transparent;
            this.outputView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputView.Location = new System.Drawing.Point(0, 0);
            this.outputView.Name = "outputView";
            this.outputView.Size = new System.Drawing.Size(235, 205);
            this.outputView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.outputView.TabIndex = 1;
            this.outputView.TabStop = false;
            this.outputView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OutputView_MouseDown);
            this.outputView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OutputView_MouseMove);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 403);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSave,
            this.toolStripSaveAll,
            this.toolStripSeparator1,
            this.toolStripPrevious,
            this.toolStripNext,
            this.toolStripPage,
            this.toolStripPageBox,
            this.toolStripSeparator2,
            this.toolStripZoomIn,
            this.toolStripZoomOut,
            this.toolStripZoomFactorBox,
            this.toolStripSeparator3,
            this.toolStripFilter,
            this.toolStripFilterBox});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(552, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripSave
            // 
            this.toolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSave.Image = global::NovelTool.Properties.Resources.Save_16x;
            this.toolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSave.Name = "toolStripSave";
            this.toolStripSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripSave.Text = "toolStripSave";
            this.toolStripSave.ToolTipText = "toolStripSave";
            this.toolStripSave.Click += new System.EventHandler(this.ToolStripSave_Click);
            // 
            // toolStripSaveAll
            // 
            this.toolStripSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSaveAll.Image = global::NovelTool.Properties.Resources.SaveAll_16x;
            this.toolStripSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSaveAll.Name = "toolStripSaveAll";
            this.toolStripSaveAll.Size = new System.Drawing.Size(23, 22);
            this.toolStripSaveAll.Text = "toolStripSaveAll";
            this.toolStripSaveAll.ToolTipText = "toolStripSaveAll";
            this.toolStripSaveAll.Click += new System.EventHandler(this.ToolStripSaveAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripPrevious
            // 
            this.toolStripPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPrevious.Image = global::NovelTool.Properties.Resources.Previous_16x;
            this.toolStripPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPrevious.Name = "toolStripPrevious";
            this.toolStripPrevious.Size = new System.Drawing.Size(23, 22);
            this.toolStripPrevious.Text = "toolStripPrevious";
            this.toolStripPrevious.ToolTipText = "toolStripPrevious";
            this.toolStripPrevious.Click += new System.EventHandler(this.ToolStripPrevious_Click);
            // 
            // toolStripNext
            // 
            this.toolStripNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripNext.Image = global::NovelTool.Properties.Resources.Next_16x;
            this.toolStripNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripNext.Name = "toolStripNext";
            this.toolStripNext.Size = new System.Drawing.Size(23, 22);
            this.toolStripNext.Text = "toolStripNext";
            this.toolStripNext.ToolTipText = "toolStripNext";
            this.toolStripNext.Click += new System.EventHandler(this.ToolStripNext_Click);
            // 
            // toolStripPage
            // 
            this.toolStripPage.Name = "toolStripPage";
            this.toolStripPage.Size = new System.Drawing.Size(87, 22);
            this.toolStripPage.Text = "Current page: ";
            // 
            // toolStripPageBox
            // 
            this.toolStripPageBox.AutoSize = false;
            this.toolStripPageBox.Name = "toolStripPageBox";
            this.toolStripPageBox.Size = new System.Drawing.Size(50, 23);
            this.toolStripPageBox.SelectedIndexChanged += new System.EventHandler(this.ToolStripPageBox_SelectedIndexChanged);
            this.toolStripPageBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripPageBox_KeyUp);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripZoomIn
            // 
            this.toolStripZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomIn.Image = global::NovelTool.Properties.Resources.ZoomIn_16x;
            this.toolStripZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZoomIn.Name = "toolStripZoomIn";
            this.toolStripZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripZoomIn.Text = "toolStripZoomIn";
            this.toolStripZoomIn.ToolTipText = "toolStripZoomIn";
            this.toolStripZoomIn.Click += new System.EventHandler(this.ToolStripZoomIn_Click);
            // 
            // toolStripZoomOut
            // 
            this.toolStripZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomOut.Image = global::NovelTool.Properties.Resources.ZoomOut_16x;
            this.toolStripZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZoomOut.Name = "toolStripZoomOut";
            this.toolStripZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripZoomOut.Text = "toolStripZoomOut";
            this.toolStripZoomOut.ToolTipText = "toolStripZoomOut";
            this.toolStripZoomOut.Click += new System.EventHandler(this.ToolStripZoomOut_Click);
            // 
            // toolStripZoomFactorBox
            // 
            this.toolStripZoomFactorBox.AutoSize = false;
            this.toolStripZoomFactorBox.Items.AddRange(new object[] {
            "100",
            "150",
            "200",
            "250",
            "300",
            "350",
            "400",
            "450",
            "500"});
            this.toolStripZoomFactorBox.Name = "toolStripZoomFactorBox";
            this.toolStripZoomFactorBox.Size = new System.Drawing.Size(50, 23);
            this.toolStripZoomFactorBox.SelectedIndexChanged += new System.EventHandler(this.ToolStripZoomFactorBox_SelectedIndexChanged);
            this.toolStripZoomFactorBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripZoomFactorBox_KeyUp);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripFilter
            // 
            this.toolStripFilter.Name = "toolStripFilter";
            this.toolStripFilter.Size = new System.Drawing.Size(37, 22);
            this.toolStripFilter.Text = "Filter:";
            // 
            // toolStripFilterBox
            // 
            this.toolStripFilterBox.Name = "toolStripFilterBox";
            this.toolStripFilterBox.Size = new System.Drawing.Size(121, 25);
            // 
            // GenerateView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "GenerateView";
            this.Text = "GenerateView";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.PictureBox outputView;
        private System.Windows.Forms.ToolStripButton toolStripPrevious;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton toolStripNext;
        private System.Windows.Forms.ToolStripLabel toolStripPage;
        private System.Windows.Forms.ToolStripComboBox toolStripPageBox;
        private System.Windows.Forms.ToolStripButton toolStripSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripFilter;
        private System.Windows.Forms.ToolStripComboBox toolStripFilterBox;
        private System.Windows.Forms.ToolStripButton toolStripZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripZoomOut;
        private System.Windows.Forms.ToolStripComboBox toolStripZoomFactorBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripSaveAll;
    }
}