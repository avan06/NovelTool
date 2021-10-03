
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
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.fileListView = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripOptions = new System.Windows.Forms.ToolStripButton();
            this.toolStripDarkTheme = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomFactorBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripGenView = new System.Windows.Forms.ToolStripButton();
            this.toolStripFilter = new System.Windows.Forms.ToolStripLabel();
            this.toolStripFilterBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDarkTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGenView = new System.Windows.Forms.ToolStripMenuItem();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.ToolMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.basePanel = new System.Windows.Forms.Panel();
            this.closeBtn = new System.Windows.Forms.Button();
            this.minimizeBtn = new System.Windows.Forms.Button();
            this.maximizeBtn = new System.Windows.Forms.Button();
            this.newAnalysisWorker = new System.ComponentModel.BackgroundWorker();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.basePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            resources.ApplyResources(this.toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
            this.toolStripContainer.ContentPanel.Controls.Add(this.splitContainer);
            resources.ApplyResources(this.toolStripContainer, "toolStripContainer");
            this.toolStripContainer.Name = "toolStripContainer";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            this.toolStripContainer.TopToolStripPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MenuStrip_MouseDown);
            // 
            // splitContainer
            // 
            resources.ApplyResources(this.splitContainer, "splitContainer");
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            resources.ApplyResources(this.splitContainer.Panel1, "splitContainer.Panel1");
            this.splitContainer.Panel1.Controls.Add(this.fileListView);
            // 
            // splitContainer.Panel2
            // 
            resources.ApplyResources(this.splitContainer.Panel2, "splitContainer.Panel2");
            this.splitContainer.Panel2.Controls.Add(this.pictureBox);
            // 
            // fileListView
            // 
            this.fileListView.AllowDrop = true;
            this.fileListView.CheckBoxes = true;
            this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.name,
            this.path});
            resources.ApplyResources(this.fileListView, "fileListView");
            this.fileListView.FullRowSelect = true;
            this.fileListView.GridLines = true;
            this.fileListView.HideSelection = false;
            this.fileListView.Name = "fileListView";
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.View = System.Windows.Forms.View.Details;
            this.fileListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.FileListView_ItemChecked);
            this.fileListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.FileListView_ItemSelectionChanged);
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
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.pictureBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PictureBox_PreviewKeyDown);
            // 
            // toolStrip
            // 
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpen,
            this.toolStripSeparator1,
            this.toolStripOptions,
            this.toolStripDarkTheme,
            this.toolStripSeparator2,
            this.toolStripZoomIn,
            this.toolStripZoomOut,
            this.toolStripZoomFactorBox,
            this.toolStripSeparator4,
            this.toolStripGenView,
            this.toolStripFilter,
            this.toolStripFilterBox,
            this.toolStripSeparator3});
            this.toolStrip.Name = "toolStrip";
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpen.Image = global::NovelTool.Properties.Resources.open;
            resources.ApplyResources(this.toolStripOpen, "toolStripOpen");
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Click += new System.EventHandler(this.ToolStripOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripOptions
            // 
            this.toolStripOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOptions.Image = global::NovelTool.Properties.Resources.Settings_16x;
            resources.ApplyResources(this.toolStripOptions, "toolStripOptions");
            this.toolStripOptions.Name = "toolStripOptions";
            this.toolStripOptions.Click += new System.EventHandler(this.ToolStripOptions_Click);
            // 
            // toolStripDarkTheme
            // 
            this.toolStripDarkTheme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDarkTheme.Image = global::NovelTool.Properties.Resources.DarkTheme_16x;
            resources.ApplyResources(this.toolStripDarkTheme, "toolStripDarkTheme");
            this.toolStripDarkTheme.Name = "toolStripDarkTheme";
            this.toolStripDarkTheme.Click += new System.EventHandler(this.ToolStripDarkTheme_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripZoomIn
            // 
            this.toolStripZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomIn.Image = global::NovelTool.Properties.Resources.ZoomIn_16x;
            resources.ApplyResources(this.toolStripZoomIn, "toolStripZoomIn");
            this.toolStripZoomIn.Name = "toolStripZoomIn";
            this.toolStripZoomIn.Click += new System.EventHandler(this.ToolStripZoomIn_Click);
            // 
            // toolStripZoomOut
            // 
            this.toolStripZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomOut.Image = global::NovelTool.Properties.Resources.ZoomOut_16x;
            resources.ApplyResources(this.toolStripZoomOut, "toolStripZoomOut");
            this.toolStripZoomOut.Name = "toolStripZoomOut";
            this.toolStripZoomOut.Click += new System.EventHandler(this.ToolStripZoomOut_Click);
            // 
            // toolStripZoomFactorBox
            // 
            resources.ApplyResources(this.toolStripZoomFactorBox, "toolStripZoomFactorBox");
            this.toolStripZoomFactorBox.Items.AddRange(new object[] {
            resources.GetString("toolStripZoomFactorBox.Items"),
            resources.GetString("toolStripZoomFactorBox.Items1"),
            resources.GetString("toolStripZoomFactorBox.Items2"),
            resources.GetString("toolStripZoomFactorBox.Items3"),
            resources.GetString("toolStripZoomFactorBox.Items4"),
            resources.GetString("toolStripZoomFactorBox.Items5"),
            resources.GetString("toolStripZoomFactorBox.Items6"),
            resources.GetString("toolStripZoomFactorBox.Items7"),
            resources.GetString("toolStripZoomFactorBox.Items8")});
            this.toolStripZoomFactorBox.Name = "toolStripZoomFactorBox";
            this.toolStripZoomFactorBox.SelectedIndexChanged += new System.EventHandler(this.ToolStripZoomFactorBox_SelectedIndexChanged);
            this.toolStripZoomFactorBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripZoomFactorBox_KeyUp);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // toolStripGenView
            // 
            this.toolStripGenView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripGenView.Image = global::NovelTool.Properties.Resources.ResourceView_16x;
            resources.ApplyResources(this.toolStripGenView, "toolStripGenView");
            this.toolStripGenView.Name = "toolStripGenView";
            this.toolStripGenView.Click += new System.EventHandler(this.ToolStripGenView_Click);
            // 
            // toolStripFilter
            // 
            this.toolStripFilter.Name = "toolStripFilter";
            resources.ApplyResources(this.toolStripFilter, "toolStripFilter");
            // 
            // toolStripFilterBox
            // 
            this.toolStripFilterBox.Name = "toolStripFilterBox";
            resources.ApplyResources(this.toolStripFilterBox, "toolStripFilterBox");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.menuTools,
            this.imageToolStripMenuItem});
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MenuStrip_MouseDown);
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpen});
            this.MenuFile.Name = "MenuFile";
            resources.ApplyResources(this.MenuFile, "MenuFile");
            // 
            // menuOpen
            // 
            this.menuOpen.Image = global::NovelTool.Properties.Resources.open;
            this.menuOpen.Name = "menuOpen";
            resources.ApplyResources(this.menuOpen, "menuOpen");
            this.menuOpen.Click += new System.EventHandler(this.ToolStripOpen_Click);
            // 
            // menuTools
            // 
            this.menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOptions,
            this.menuDarkTheme});
            this.menuTools.Name = "menuTools";
            resources.ApplyResources(this.menuTools, "menuTools");
            // 
            // menuOptions
            // 
            this.menuOptions.Image = global::NovelTool.Properties.Resources.Settings_16x;
            this.menuOptions.Name = "menuOptions";
            resources.ApplyResources(this.menuOptions, "menuOptions");
            this.menuOptions.Click += new System.EventHandler(this.ToolStripOptions_Click);
            // 
            // menuDarkTheme
            // 
            this.menuDarkTheme.Image = global::NovelTool.Properties.Resources.DarkTheme_16x;
            this.menuDarkTheme.Name = "menuDarkTheme";
            resources.ApplyResources(this.menuDarkTheme, "menuDarkTheme");
            this.menuDarkTheme.Click += new System.EventHandler(this.ToolStripDarkTheme_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuZoomIn,
            this.menuZoomOut,
            this.menuGenView});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            resources.ApplyResources(this.imageToolStripMenuItem, "imageToolStripMenuItem");
            // 
            // menuZoomIn
            // 
            this.menuZoomIn.Image = global::NovelTool.Properties.Resources.ZoomIn_16x;
            this.menuZoomIn.Name = "menuZoomIn";
            resources.ApplyResources(this.menuZoomIn, "menuZoomIn");
            this.menuZoomIn.Click += new System.EventHandler(this.ToolStripZoomIn_Click);
            // 
            // menuZoomOut
            // 
            this.menuZoomOut.Image = global::NovelTool.Properties.Resources.ZoomOut_16x;
            this.menuZoomOut.Name = "menuZoomOut";
            resources.ApplyResources(this.menuZoomOut, "menuZoomOut");
            this.menuZoomOut.Click += new System.EventHandler(this.ToolStripZoomOut_Click);
            // 
            // menuGenView
            // 
            this.menuGenView.Image = global::NovelTool.Properties.Resources.ResourceView_16x;
            this.menuGenView.Name = "menuGenView";
            resources.ApplyResources(this.menuGenView, "menuGenView");
            this.menuGenView.Click += new System.EventHandler(this.ToolStripGenView_Click);
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
            // statusStrip
            // 
            this.statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolProgressBar,
            this.ToolMsg});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.SizingGrip = false;
            // 
            // toolProgressBar
            // 
            this.toolProgressBar.Name = "toolProgressBar";
            resources.ApplyResources(this.toolProgressBar, "toolProgressBar");
            // 
            // ToolMsg
            // 
            this.ToolMsg.Name = "ToolMsg";
            resources.ApplyResources(this.ToolMsg, "ToolMsg");
            this.ToolMsg.Spring = true;
            // 
            // basePanel
            // 
            this.basePanel.Controls.Add(this.toolStripContainer);
            this.basePanel.Controls.Add(this.statusStrip);
            this.basePanel.Controls.Add(this.closeBtn);
            this.basePanel.Controls.Add(this.minimizeBtn);
            this.basePanel.Controls.Add(this.maximizeBtn);
            this.basePanel.Controls.Add(this.menuStrip);
            resources.ApplyResources(this.basePanel, "basePanel");
            this.basePanel.Name = "basePanel";
            // 
            // closeBtn
            // 
            resources.ApplyResources(this.closeBtn, "closeBtn");
            this.closeBtn.BackColor = System.Drawing.SystemColors.Control;
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.TabStop = false;
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // minimizeBtn
            // 
            resources.ApplyResources(this.minimizeBtn, "minimizeBtn");
            this.minimizeBtn.BackColor = System.Drawing.SystemColors.Control;
            this.minimizeBtn.FlatAppearance.BorderSize = 0;
            this.minimizeBtn.Name = "minimizeBtn";
            this.minimizeBtn.TabStop = false;
            this.minimizeBtn.UseVisualStyleBackColor = false;
            this.minimizeBtn.Click += new System.EventHandler(this.MinimizeBtn_Click);
            // 
            // maximizeBtn
            // 
            resources.ApplyResources(this.maximizeBtn, "maximizeBtn");
            this.maximizeBtn.BackColor = System.Drawing.SystemColors.Control;
            this.maximizeBtn.FlatAppearance.BorderSize = 0;
            this.maximizeBtn.Name = "maximizeBtn";
            this.maximizeBtn.TabStop = false;
            this.maximizeBtn.UseVisualStyleBackColor = false;
            this.maximizeBtn.Click += new System.EventHandler(this.MaximizeBtn_Click);
            // 
            // newAnalysisWorker
            // 
            this.newAnalysisWorker.WorkerReportsProgress = true;
            this.newAnalysisWorker.WorkerSupportsCancellation = true;
            this.newAnalysisWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.NewAnalysis_DoWork);
            this.newAnalysisWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.NewAnalysis_ProgressChanged);
            this.newAnalysisWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.NewAnalysis_RunWorkerCompleted);
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.basePanel);
            this.DoubleBuffered = true;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.basePanel.ResumeLayout(false);
            this.basePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button maximizeBtn;
        private System.Windows.Forms.Button minimizeBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Panel basePanel;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader path;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripOpen;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripButton toolStripOptions;
        private System.Windows.Forms.ToolStripProgressBar toolProgressBar;
        private System.ComponentModel.BackgroundWorker newAnalysisWorker;
        private System.Windows.Forms.ToolStripStatusLabel ToolMsg;
        private System.Windows.Forms.ToolStripButton toolStripDarkTheme;
        private System.Windows.Forms.ToolStripMenuItem menuDarkTheme;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox toolStripFilterBox;
        private System.Windows.Forms.ToolStripButton toolStripZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripZoomOut;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuZoomIn;
        private System.Windows.Forms.ToolStripMenuItem menuZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripGenView;
        private System.Windows.Forms.ToolStripComboBox toolStripZoomFactorBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripFilter;
        private System.Windows.Forms.ToolStripMenuItem menuGenView;
    }
}

