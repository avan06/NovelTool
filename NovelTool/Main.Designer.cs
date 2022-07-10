
namespace NovelTool
{
    partial class Main
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ToolProgressBar = new System.Windows.Forms.ProgressBar();
            this.FileListView = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Panel1 = new System.Windows.Forms.Panel();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.StatusStrip2 = new System.Windows.Forms.StatusStrip();
            this.ToolMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.newAnalysisWorker = new System.ComponentModel.BackgroundWorker();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ToolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripZoomIn = new System.Windows.Forms.ToolStripButton();
            this.ToolStripZoomOut = new System.Windows.Forms.ToolStripButton();
            this.ToolStripZoomFactorBox = new System.Windows.Forms.ToolStripComboBox();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripFilter = new System.Windows.Forms.ToolStripLabel();
            this.ToolStripFilterBox = new System.Windows.Forms.ToolStripComboBox();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripOption = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripGenView = new System.Windows.Forms.ToolStripButton();
            this.BasePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.StatusStrip2.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.BasePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            resources.ApplyResources(this.SplitContainer1, "SplitContainer1");
            this.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitContainer1.ForeColor = System.Drawing.Color.White;
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            resources.ApplyResources(this.SplitContainer1.Panel1, "SplitContainer1.Panel1");
            this.SplitContainer1.Panel1.Controls.Add(this.ToolProgressBar);
            this.SplitContainer1.Panel1.Controls.Add(this.FileListView);
            // 
            // SplitContainer1.Panel2
            // 
            resources.ApplyResources(this.SplitContainer1.Panel2, "SplitContainer1.Panel2");
            this.SplitContainer1.Panel2.Controls.Add(this.Panel1);
            this.SplitContainer1.Panel2.Controls.Add(this.StatusStrip2);
            // 
            // ToolProgressBar
            // 
            resources.ApplyResources(this.ToolProgressBar, "ToolProgressBar");
            this.ToolProgressBar.Name = "ToolProgressBar";
            // 
            // FileListView
            // 
            this.FileListView.AllowDrop = true;
            this.FileListView.BackColor = System.Drawing.Color.DimGray;
            this.FileListView.CheckBoxes = true;
            this.FileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.name,
            this.path});
            resources.ApplyResources(this.FileListView, "FileListView");
            this.FileListView.ForeColor = System.Drawing.Color.White;
            this.FileListView.FullRowSelect = true;
            this.FileListView.HideSelection = false;
            this.FileListView.Name = "FileListView";
            this.FileListView.OwnerDraw = true;
            this.FileListView.UseCompatibleStateImageBehavior = false;
            this.FileListView.View = System.Windows.Forms.View.Details;
            this.FileListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.FileListView_DrawColumnHeader);
            this.FileListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.FileListView_DrawItem);
            this.FileListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.FileListView_DrawSubItem);
            this.FileListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.FileListView_ItemChecked);
            this.FileListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.FileListView_ItemSelectionChanged);
            // 
            // id
            // 
            resources.ApplyResources(this.id, "id");
            // 
            // name
            // 
            resources.ApplyResources(this.name, "name");
            // 
            // path
            // 
            resources.ApplyResources(this.path, "path");
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.Panel1.Controls.Add(this.PicBox);
            resources.ApplyResources(this.Panel1, "Panel1");
            this.Panel1.Name = "Panel1";
            // 
            // PicBox
            // 
            this.PicBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            resources.ApplyResources(this.PicBox, "PicBox");
            this.PicBox.Name = "PicBox";
            this.PicBox.TabStop = false;
            this.PicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this.PicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.PicBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PictureBox_PreviewKeyDown);
            // 
            // StatusStrip2
            // 
            this.StatusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMsg});
            resources.ApplyResources(this.StatusStrip2, "StatusStrip2");
            this.StatusStrip2.Name = "StatusStrip2";
            // 
            // ToolMsg
            // 
            this.ToolMsg.BackColor = System.Drawing.Color.DarkGray;
            this.ToolMsg.ForeColor = System.Drawing.Color.Black;
            this.ToolMsg.Name = "ToolMsg";
            resources.ApplyResources(this.ToolMsg, "ToolMsg");
            this.ToolMsg.Spring = true;
            // 
            // BottomToolStripPanel
            // 
            resources.ApplyResources(this.BottomToolStripPanel, "BottomToolStripPanel");
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // TopToolStripPanel
            // 
            resources.ApplyResources(this.TopToolStripPanel, "TopToolStripPanel");
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // RightToolStripPanel
            // 
            resources.ApplyResources(this.RightToolStripPanel, "RightToolStripPanel");
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // LeftToolStripPanel
            // 
            resources.ApplyResources(this.LeftToolStripPanel, "LeftToolStripPanel");
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // ContentPanel
            // 
            resources.ApplyResources(this.ContentPanel, "ContentPanel");
            // 
            // newAnalysisWorker
            // 
            this.newAnalysisWorker.WorkerReportsProgress = true;
            this.newAnalysisWorker.WorkerSupportsCancellation = true;
            this.newAnalysisWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.NewAnalysis_DoWork);
            this.newAnalysisWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.NewAnalysis_ProgressChanged);
            this.newAnalysisWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.NewAnalysis_RunWorkerCompleted);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripOpen,
            this.ToolStripSeparator1,
            this.ToolStripZoomIn,
            this.ToolStripZoomOut,
            this.ToolStripZoomFactorBox,
            this.ToolStripSeparator2,
            this.ToolStripFilter,
            this.ToolStripFilterBox,
            this.ToolStripSeparator3,
            this.ToolStripOption,
            this.ToolStripSeparator4,
            this.ToolStripGenView});
            resources.ApplyResources(this.ToolStrip1, "ToolStrip1");
            this.ToolStrip1.Name = "ToolStrip1";
            // 
            // ToolStripOpen
            // 
            this.ToolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripOpen.Image = global::NovelTool.Properties.Resources.open;
            resources.ApplyResources(this.ToolStripOpen, "ToolStripOpen");
            this.ToolStripOpen.Name = "ToolStripOpen";
            this.ToolStripOpen.Click += new System.EventHandler(this.ToolStripOpen_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            resources.ApplyResources(this.ToolStripSeparator1, "ToolStripSeparator1");
            // 
            // ToolStripZoomIn
            // 
            this.ToolStripZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripZoomIn.Image = global::NovelTool.Properties.Resources.ZoomIn_16x;
            resources.ApplyResources(this.ToolStripZoomIn, "ToolStripZoomIn");
            this.ToolStripZoomIn.Name = "ToolStripZoomIn";
            this.ToolStripZoomIn.Click += new System.EventHandler(this.ToolStripZoomIn_Click);
            // 
            // ToolStripZoomOut
            // 
            this.ToolStripZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripZoomOut.Image = global::NovelTool.Properties.Resources.ZoomOut_16x;
            resources.ApplyResources(this.ToolStripZoomOut, "ToolStripZoomOut");
            this.ToolStripZoomOut.Name = "ToolStripZoomOut";
            this.ToolStripZoomOut.Click += new System.EventHandler(this.ToolStripZoomOut_Click);
            // 
            // ToolStripZoomFactorBox
            // 
            resources.ApplyResources(this.ToolStripZoomFactorBox, "ToolStripZoomFactorBox");
            this.ToolStripZoomFactorBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ToolStripZoomFactorBox.Items.AddRange(new object[] {
            resources.GetString("ToolStripZoomFactorBox.Items"),
            resources.GetString("ToolStripZoomFactorBox.Items1"),
            resources.GetString("ToolStripZoomFactorBox.Items2"),
            resources.GetString("ToolStripZoomFactorBox.Items3"),
            resources.GetString("ToolStripZoomFactorBox.Items4"),
            resources.GetString("ToolStripZoomFactorBox.Items5"),
            resources.GetString("ToolStripZoomFactorBox.Items6"),
            resources.GetString("ToolStripZoomFactorBox.Items7"),
            resources.GetString("ToolStripZoomFactorBox.Items8")});
            this.ToolStripZoomFactorBox.Name = "ToolStripZoomFactorBox";
            this.ToolStripZoomFactorBox.SelectedIndexChanged += new System.EventHandler(this.ToolStripZoomFactorBox_SelectedIndexChanged);
            this.ToolStripZoomFactorBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripZoomFactorBox_KeyUp);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            resources.ApplyResources(this.ToolStripSeparator2, "ToolStripSeparator2");
            // 
            // ToolStripFilter
            // 
            this.ToolStripFilter.ForeColor = System.Drawing.Color.Black;
            this.ToolStripFilter.Name = "ToolStripFilter";
            resources.ApplyResources(this.ToolStripFilter, "ToolStripFilter");
            // 
            // ToolStripFilterBox
            // 
            this.ToolStripFilterBox.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.ToolStripFilterBox, "ToolStripFilterBox");
            this.ToolStripFilterBox.Name = "ToolStripFilterBox";
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            resources.ApplyResources(this.ToolStripSeparator3, "ToolStripSeparator3");
            // 
            // ToolStripOption
            // 
            this.ToolStripOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripOption.Image = global::NovelTool.Properties.Resources.Settings_16x;
            resources.ApplyResources(this.ToolStripOption, "ToolStripOption");
            this.ToolStripOption.Name = "ToolStripOption";
            this.ToolStripOption.Click += new System.EventHandler(this.ToolStripOptions_Click);
            // 
            // ToolStripSeparator4
            // 
            this.ToolStripSeparator4.Name = "ToolStripSeparator4";
            resources.ApplyResources(this.ToolStripSeparator4, "ToolStripSeparator4");
            // 
            // ToolStripGenView
            // 
            this.ToolStripGenView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripGenView.Image = global::NovelTool.Properties.Resources.ResourceView_16x;
            resources.ApplyResources(this.ToolStripGenView, "ToolStripGenView");
            this.ToolStripGenView.Name = "ToolStripGenView";
            this.ToolStripGenView.Click += new System.EventHandler(this.ToolStripGenView_Click);
            // 
            // BasePanel
            // 
            this.BasePanel.Controls.Add(this.SplitContainer1);
            this.BasePanel.Controls.Add(this.ToolStrip1);
            resources.ApplyResources(this.BasePanel, "BasePanel");
            this.BasePanel.Name = "BasePanel";
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.BasePanel);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Main";
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.StatusStrip2.ResumeLayout(false);
            this.StatusStrip2.PerformLayout();
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.BasePanel.ResumeLayout(false);
            this.BasePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.ComponentModel.BackgroundWorker newAnalysisWorker;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.ListView FileListView;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader path;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.ToolStrip ToolStrip1;
        private System.Windows.Forms.ToolStripButton ToolStripOpen;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ToolStripOption;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ToolStripZoomIn;
        private System.Windows.Forms.ToolStripButton ToolStripZoomOut;
        private System.Windows.Forms.ToolStripComboBox ToolStripZoomFactorBox;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
        private System.Windows.Forms.ToolStripButton ToolStripGenView;
        private System.Windows.Forms.ToolStripLabel ToolStripFilter;
        private System.Windows.Forms.ToolStripComboBox ToolStripFilterBox;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        private System.Windows.Forms.Panel BasePanel;
        private System.Windows.Forms.StatusStrip StatusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel ToolMsg;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.ProgressBar ToolProgressBar;
    }
}

