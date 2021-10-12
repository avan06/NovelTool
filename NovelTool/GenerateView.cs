using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NovelTool
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class GenerateView : Form
    {
        private readonly Main mainForm;
        private Point MouseDownLocation;
        private List<Image> outputImgs;
        public List<Image> OutputImgs { get => outputImgs; set => outputImgs = value; }
        private int outputCount = -1;
        private int outputIdx;
        private WebBrowser WebBox;
        public GenerateView(Main mainForm)
        {
            if (mainForm.PageDatas.Count == 0) return;

            this.mainForm = mainForm;
            InitializeComponent();
            ToolStripZoomFactorBox.SelectedItem = (mainForm.ZoomFactor * 100).ToString();
            #region InitFilterBox
            ToolStripFilterBox.Items.Add("None");
            foreach (BitmapFilter.Filter filterEnum in (BitmapFilter.Filter[])Enum.GetValues(typeof(BitmapFilter.Filter))) ToolStripFilterBox.Items.Add(filterEnum);
            foreach (BitmapFilter.FilterXY filterEnum in (BitmapFilter.FilterXY[])Enum.GetValues(typeof(BitmapFilter.FilterXY))) ToolStripFilterBox.Items.Add(filterEnum);
            if (Enum.TryParse(mainForm.Filter.name, out BitmapFilter.Filter filter)) ToolStripFilterBox.SelectedItem = filter;
            else if (Enum.TryParse(mainForm.Filter.name, out BitmapFilter.FilterXY filterXY)) ToolStripFilterBox.SelectedItem = filterXY;
            ToolStripFilterBox.SelectedIndexChanged += ToolStripFilterBox_SelectedIndexChanged;
            #endregion

            outputIdx = SetOutputPage(10);
        }
        private int SetOutputPage(int outputIdx)
        {
            outputCount = ParseOutput(outputIdx);
            if (ToolStripPageBox.Items.Count != outputCount)
            {
                ToolStripPageBox.BeginUpdate();
                ToolStripPageBox.Items.Clear();
                for (int idx = 0; idx < outputCount; idx++) ToolStripPageBox.Items.Add(idx);
                ToolStripPageBox.EndUpdate();
            }
            if (outputIdx >= outputCount) outputIdx = 0; //顯示第一頁
            else if (outputIdx < 0) outputIdx = outputCount - 1; //顯示最後一頁
            if (OutputImgs.Count == 0) ParseOutput(outputIdx);
            if (OutputImgs.Count == 0) return 0;

            Bitmap result = (Bitmap)OutputImgs[0];
            bool CheckBoxOutputAdjustColor = (bool)Properties.Settings.Default["CheckBoxOutputAdjustColor"];
            Color ColorBoxOutputBack = (Color)Properties.Settings.Default["ColorBoxOutputBack"];
            Color ColorBoxOutputFore = (Color)Properties.Settings.Default["ColorBoxOutputFore"];
            float FloatUDForeColorRate = (float)Properties.Settings.Default["FloatUDForeColorRate"];
            if (CheckBoxOutputAdjustColor && (result.Tag == null || (bool)result.Tag != true)) ImageTool.ChangeForeColor(result, ColorBoxOutputFore.ToArgb(), ColorBoxOutputBack.ToArgb(), FloatUDForeColorRate);
            if (!(result.Tag is bool))
            {
                if (mainForm.Filter.xFilterMatrix != null)
                {
                    if (mainForm.Filter.yFilterMatrix == null) result = BitmapFilter.ConvolutionFilter(result,
                        mainForm.Filter.xFilterMatrix, mainForm.Filter.factor, mainForm.Filter.bias);
                    else result = BitmapFilter.ConvolutionFilter(result,
                        mainForm.Filter.xFilterMatrix, mainForm.Filter.yFilterMatrix, mainForm.Filter.factor, mainForm.Filter.bias);
                }
                result.MakeTransparent(Color.Transparent);
            }

            OutputView.Image = result;
            if (result.Tag == null || (bool)result.Tag != true)
            {
                if (CheckBoxOutputAdjustColor) OutputView.BackColor = ColorBoxOutputBack;
                else OutputView.BackColor = Color.White;
            }
            ToolStripPageBox.SelectedItem = outputIdx;

            return outputIdx;
        }
        private int ParseOutput(int outputIdx, bool outputAll=false)
        {
            int IntUDOutputWidth = (int)Properties.Settings.Default["IntUDOutputWidth"]; //輸出圖片寬度
            int IntUDOutputHeight = (int)Properties.Settings.Default["IntUDOutputHeight"]; //輸出圖片高度
            int IntUDMarginLeft = (int)Properties.Settings.Default["IntUDMarginLeft"]; //輸出圖片左側邊距
            int IntUDMarginRight = (int)Properties.Settings.Default["IntUDMarginRight"]; //輸出圖片右側邊距
            int IntUDMarginTop = (int)Properties.Settings.Default["IntUDMarginTop"]; //輸出圖片上側邊距
            int IntUDMarginBottom = (int)Properties.Settings.Default["IntUDMarginBottom"]; //輸出圖片下側邊距
            int IntUDLeading = (int)Properties.Settings.Default["IntUDLeading"]; //輸出圖片行距
            float FloatUDEntityAdjacentRate = (float)Properties.Settings.Default["FloatUDEntityAdjacentRate"]; //判斷兩行之間是否相鄰之比例

            int initWidthLocation = IntUDOutputWidth - IntUDMarginRight;
            int initHeightLocation = IntUDMarginTop;

            int outputCount = 0;

            double zoomFactor =  mainForm.ZoomFactor;

            outputImgs = new List<Image>();
            Bitmap destImage = null;
            Point destPoint = new Point(initWidthLocation, initHeightLocation);
            System.Text.StringBuilder webStr = new System.Text.StringBuilder();

            for (int pIdx = 0; pIdx < mainForm.PageDatas.Count; ++pIdx)
            {
                PageData pageData = mainForm.PageDatas[pIdx];
                if (pageData.textList != null && pageData.textList.Count > 0)
                {
                    if (WebBox == null)
                    {
                        WebBox = new WebBrowser();
                        Panel1.Controls.Add(WebBox);
                        WebBox.Location = OutputView.Location;
                        WebBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        WebBox.Dock = DockStyle.Fill;
                        //WebBox.DocumentCompleted += WebBox_DocumentCompleted;
                        WebBox.DocumentCompleted += (o, e) =>
                        {
                            WebBox.Document.Window.Error += (w, we) =>
                            {
                                we.Handled = true;
                                // Do something with the error...
                                Debug.WriteLine(
                                    string.Format(
                                       "Error: {1}\nline: {0}\nurl: {2}",
                                       we.LineNumber, //#0
                                       we.Description, //#1
                                       we.Url));  //#2
                            };
                        };
                        WebBox.ScriptErrorsSuppressed = true;
                        WebBox.BringToFront();
                    }
                    for (int tIdx = 0; tIdx < pageData.textList.Count; tIdx++)
                    {
                        (string text, string ruby) = pageData.textList[tIdx];
                        if (text == "\n") webStr.Append("<br/>");
                        if (text == "_img_") webStr.AppendLine($"<img height='100%' src='{ruby}'>");
                        else if (ruby.Length > 0) webStr.AppendLine($"<ruby><rb>{text}</rb><rp>（</rp><rt>{ruby}</rt><rp>）</rp></ruby>");
                        else webStr.AppendLine(text);
                        //if (tIdx > 0 && tIdx < pageData.textList.Count - 1 && pageData.textList[tIdx - 1].text == "\n" && pageData.textList[tIdx + 1].text == "\n") webStr.AppendLine("<h1>" + text + "</h1>");
                    }
                    webStr.AppendLine("<hr>");
                }
                else
                {
                    Bitmap srcImage = null;
                    AddNowCreateNext(pageData.path, pageData.name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation,
                        ref outputCount, ref srcImage, ref destImage, ref destPoint, zoomFactor, pageData.columnHeadList, pageData.columnFooterList,
                        false, true);
                    if (pageData.isIllustration)
                    {   //此頁為圖片時，先儲存目前已產生頁面，再儲存圖片檔案
                        AddNowCreateNext(pageData.path, pageData.name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation,
                            ref outputCount, ref srcImage, ref destImage, ref destPoint, zoomFactor, pageData.columnHeadList, pageData.columnFooterList,
                            true, false, true);
                    }
                    else if (pageData.columnBodyList != null)
                    {
                        float offsetX = -1;
                        var columnRects = pageData.columnBodyList;
                        float bodyTop = pageData.rectBody.Y;
                        (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) columnRuby = (RectType.None, 0, 0, 0, 0, null);
                        for (int cIdx = columnRects.Count - 1; cIdx >= 0; cIdx--)
                        {
                            (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) = columnRects[cIdx];
                            (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) columnNext = cIdx > 1 ? columnRects[cIdx - 1] : ImageTool.NewEntitys();
                            int newWidth = (int)(Width * zoomFactor);
                            int blankWidth = offsetX != -1 ? (int)((offsetX - X - Width) * zoomFactor) : 0;
                            double blankFactor = (double)blankWidth / newWidth;
                            blankFactor = blankFactor > 2.5 ? 2.5 : (blankFactor < 1.5 ? 0 : blankFactor);
                            destPoint.X -= (int)(newWidth * blankFactor); //原圖兩行之間的空白行較大時，輸出位置加上空白行比例寬度
                            if (columnNext.Entitys != null && columnNext.RType != RectType.Ruby && RType != RectType.Ruby && X - columnNext.X - columnNext.Width < mainForm.Modes.Width * FloatUDEntityAdjacentRate && Width <= mainForm.Modes.WidthMin) //前後行為相鄰時
                            {
                                RType = RectType.Ruby;
                                for (int eIdx = 0; eIdx < Entitys.Count; eIdx++)
                                {
                                    var entity = Entitys[eIdx];
                                    entity.RType = RectType.Ruby;
                                    Entitys[eIdx] = entity;
                                }
                            }
                            if (Entitys == null || Entitys.Count == 0) continue;
                            if (RType == RectType.BodyIn && Entitys[0].RType == RectType.EntityHead) AddNowCreateNext(pageData.path, pageData.name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation,
                                ref outputCount, ref srcImage, ref destImage, ref destPoint, zoomFactor, pageData.columnHeadList, pageData.columnFooterList);
                            if (RType == RectType.Ruby)
                            { //保留Ruby行內容，下一行主句中再處理
                                columnRuby = (RType, X, Y, Width, Height, Entitys);
                                continue;
                            }
                            if (destPoint.X == initWidthLocation) destPoint.X -= newWidth + IntUDLeading; //位移右邊第一行輸出位置
                            if (columnRuby.Entitys != null && columnRuby.X - X - Width > mainForm.Modes.Width * FloatUDEntityAdjacentRate)
                            { //column 與 Ruby 非相鄰時，代表右側非 Ruby
                                var rubyWidth = (int)(columnRuby.Width * zoomFactor);
                                GenerateDrawImage(pageData.path, pageData.name, bodyTop, columnRuby.X, columnRuby.Width, columnRuby.Entitys, rubyWidth, pageData.columnHeadList, pageData.columnFooterList,
                                    IntUDOutputWidth, IntUDOutputHeight, IntUDLeading, IntUDMarginLeft, IntUDMarginBottom, initWidthLocation, initHeightLocation, zoomFactor, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, (RectType.None, 0, 0, 0, 0, null), outputAll);
                                columnRuby = (RectType.None, 0, 0, 0, 0, null);
                            }
                            
                            GenerateDrawImage(pageData.path, pageData.name, bodyTop, X, Width, Entitys, newWidth, pageData.columnHeadList, pageData.columnFooterList,
                                IntUDOutputWidth, IntUDOutputHeight, IntUDLeading, IntUDMarginLeft, IntUDMarginBottom, initWidthLocation, initHeightLocation, zoomFactor, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, columnRuby, outputAll);

                            if (columnRuby.Entitys != null) columnRuby = (RectType.None, 0, 0, 0, 0, null);
                            if (RType == RectType.BodyOut && RType == RectType.EntityEnd) AddNowCreateNext(pageData.path, pageData.name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation,
                                ref outputCount, ref srcImage, ref destImage, ref destPoint, zoomFactor, pageData.columnHeadList, pageData.columnFooterList);
                            offsetX = X;
                            if (Entitys[Entitys.Count -1].Height < mainForm.Modes.HeighMin && Entitys[Entitys.Count - 1].RType != RectType.EntityEnd)
                            { //每行非句尾的最後一字，若高度不到最小高度(例如符號字)，則增加空白輸出
                                destPoint.Y += (int)(mainForm.Modes.HeighMin * zoomFactor);
                            }
                        }
                    }
                }
            }
            if (webStr.Length > 0 && WebBox.DocumentText == "")
            {
                WebBox.Navigate("about:blank");
                WebBox.Document.Write(String.Empty);
                WebBox.DocumentText = "<html><head><meta http-equiv='X-UA-Compatible' content='IE=edge'/>" +
                    "<style type='text/css'>body {background-color: #000;color: #ccc;line-height: 150%; font-size: 140%; font-family: 'Lucida Grande', 'Meiryo', 'Meiryo UI', 'Microsoft JhengHei UI', 'Microsoft JhengHei', sans-serif; -ms-writing-mode: tb-rl; writing-mode: vertical-rl; overflow-x: scroll; padding: 10px; margin: 0px auto;}</style></head>" +
                    $"<body>{webStr}" +
                    "<script>" +
                    "function scrollLeft(element, type, value) { if (!value || value <= 0) value = element.clientWidth*0.9; if (!type) type = 'down'; if (type === 'up') element.scrollTop-=value;else if (type === 'down') element.scrollTop+=value;}" +
                    "const novel = document.querySelector('body');" +
                    "novel.addEventListener('mousewheel', function(e){e.preventDefault(); if(e.wheelDelta > 0) scrollLeft(novel, 'up'); else scrollLeft(novel, 'down');});" +
                    "</script>" +
                    "</body></html>";
            }
            if (destImage != null && destPoint.X != initWidthLocation)
            {
                if (outputCount == outputIdx || outputAll) outputImgs.Add(destImage);
                outputCount++;
            }
            return outputCount;
        }
        //private void WebBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    WebBox.Document.MouseMove += WebBoxDocument_MouseMove;
        //}

        //private void WebBoxDocument_MouseMove(object sender, HtmlElementEventArgs e)
        //{
        //    WebBox.Document.Focus();
        //}

        private void AddNowCreateNext(string path, string name, bool outputAll, int outputIdx,
            int IntUDOutputWidth, int IntUDOutputHeight, int initWidthLocation, int initHeightLocation,
            ref int outputCount, ref Bitmap srcImage, ref Bitmap destImage, ref Point destPoint, double zoomFactor,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnHeadList,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnFooterList, 
            bool triggerAdd = true, bool triggerCreate = true, bool triggerSrcAdd = false
            )
        {
            PositionType PositionTypeBoxHead = (PositionType)Properties.Settings.Default["PositionTypeBoxHead"]; //Head輸出位置
            PositionType PositionTypeBoxFooter = (PositionType)Properties.Settings.Default["PositionTypeBoxFooter"]; //Footer輸出位置
            PositionType PositionTypeBoxPage = (PositionType)Properties.Settings.Default["PositionTypeBoxPage"]; //Page輸出位置
            if (triggerAdd && destImage != null && destPoint.X != initWidthLocation)
            {
                if (outputCount == outputIdx || outputAll) outputImgs.Add(destImage);
                outputCount++;
                destImage = null;
            }
            if (triggerSrcAdd)
            {
                if (outputCount == outputIdx || outputAll)
                {
                    if (srcImage == null) srcImage = OpenImage(path, name);
                    srcImage.Tag = true;
                    outputImgs.Add(srcImage);
                }
                outputCount++;
            }
            if ((outputCount == outputIdx || outputAll) && srcImage == null) srcImage = OpenImage(path, name);
            if (triggerCreate && destImage == null)
            {
                (destImage, destPoint) = CreateDestImage(IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, srcImage);
                if (srcImage == null)
                {
                    srcImage = OpenImage(path, name);
                }
                int initSize = 10;
                GenerateDrawTitle(initSize, PositionTypeBoxHead, columnHeadList,
                 IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, zoomFactor, srcImage, ref destImage);
                GenerateDrawTitle(initSize, PositionTypeBoxFooter, columnFooterList,
                 IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, zoomFactor, srcImage, ref destImage);
            }
        }

        private void GenerateDrawTitle(int initSize, PositionType PositionTypeBoxTitle,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnTitleList,
            int IntUDOutputWidth, int IntUDOutputHeight, int initWidthLocation, int initHeightLocation, double zoomFactor, Bitmap srcImage, ref Bitmap destImage)
        {
            if (PositionTypeBoxTitle != PositionType.None && columnTitleList != null && columnTitleList.Count > 0)
            {
                float offsetX = -1;
                Point destPointTitle = new Point(10, 10);
                if (PositionTypeBoxTitle == PositionType.TopRight || PositionTypeBoxTitle == PositionType.BottomRight)
                {
                    if (PositionTypeBoxTitle == PositionType.TopRight) destPointTitle = new Point(IntUDOutputWidth - initSize, initSize);
                    else if (PositionTypeBoxTitle == PositionType.BottomRight) destPointTitle = new Point(IntUDOutputWidth - initSize, IntUDOutputHeight - initSize);
                    for (int cIdx = columnTitleList.Count - 1; cIdx >= 0; cIdx--)
                    {
                        var columnTitle = columnTitleList[cIdx];
                        int newWidth = (int)(columnTitle.Width * zoomFactor);
                        int newHeight = (int)(columnTitle.Height * zoomFactor);
                        int modeWidth = (int)(mainForm.Modes.Width * zoomFactor);
                        int blankWidth = offsetX != -1 ? (int)((offsetX - columnTitle.X - columnTitle.Width) * zoomFactor) : 0;
                        blankWidth = blankWidth > modeWidth ? modeWidth : blankWidth;
                        DrawTitle(columnTitle, newWidth, newHeight, -blankWidth, true, destPointTitle.Y == IntUDOutputHeight - initSize, srcImage, ref destImage, ref destPointTitle);
                        offsetX = columnTitle.X;
                    }
                }
                else if (PositionTypeBoxTitle == PositionType.TopLeft || PositionTypeBoxTitle == PositionType.BottomLeft)
                {
                    if (PositionTypeBoxTitle == PositionType.TopLeft) destPointTitle = new Point(initSize, initSize);
                    else if (PositionTypeBoxTitle == PositionType.BottomLeft) destPointTitle = new Point(10, IntUDOutputHeight - initSize);
                    for (int cIdx = 0; cIdx < columnTitleList.Count; cIdx++)
                    {
                        var columnTitle = columnTitleList[cIdx];
                        int newWidth = (int)(columnTitle.Width * zoomFactor);
                        int newHeight = (int)(columnTitle.Height * zoomFactor);
                        int modeWidth = (int)(mainForm.Modes.Width * zoomFactor);
                        int blankWidth = offsetX != -1 ? (int)((columnTitle.X - offsetX) * zoomFactor) : 0;
                        blankWidth = blankWidth > modeWidth ? modeWidth : blankWidth;
                        DrawTitle(columnTitle, newWidth, newHeight, blankWidth, false, destPointTitle.Y == IntUDOutputHeight - initSize, srcImage, ref destImage, ref destPointTitle);
                        offsetX = columnTitle.X + columnTitle.Width;
                    }
                }
            }
        }

        private void DrawTitle((RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) columnTitle,
             int newWidth, int newHeight, int blankWidth, bool shiftLeftX, bool shiftDownY, Bitmap srcImage, ref Bitmap destImage, ref Point destPointTitle)
        {
            destPointTitle.X += blankWidth;
            if (shiftLeftX) destPointTitle.X -= newWidth;
            if (shiftDownY) destPointTitle.Y -= newHeight;
            Rectangle destRect = new Rectangle(destPointTitle.X, destPointTitle.Y, newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(destImage))
            {
                gr.CompositingMode = CompositingMode.SourceCopy;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (ImageAttributes imgAttr = new ImageAttributes())
                {
                    imgAttr.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(srcImage, destRect, columnTitle.X, columnTitle.Y, columnTitle.Width, columnTitle.Height, GraphicsUnit.Pixel, imgAttr);
                }
            }
            if (!shiftLeftX) destPointTitle.X += newWidth;
        }

        private Bitmap OpenImage(string path, string name)
        {
            FileStream fs = null;
            Bitmap srcImage = null;
            try
            {
                if (name.EndsWith("xhtml")) return srcImage;

                fs = File.OpenRead(string.Format("{0}/{1}", path, name));
                srcImage = (Bitmap)Image.FromStream(fs);
                //https://www.c-sharpcorner.com/article/solution-for-a-graphics-object-cannot-be-created-from-an-im/
                if (srcImage.PixelFormat == PixelFormat.Undefined || srcImage.PixelFormat == PixelFormat.DontCare || srcImage.PixelFormat == PixelFormat.Format1bppIndexed ||
                    srcImage.PixelFormat == PixelFormat.Format4bppIndexed || srcImage.PixelFormat == PixelFormat.Format8bppIndexed ||
                    srcImage.PixelFormat == PixelFormat.Format16bppGrayScale || srcImage.PixelFormat == PixelFormat.Format16bppArgb1555) srcImage = new Bitmap(srcImage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show("fileList Item Selection failed, " + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (fs != null) fs.Dispose();
            }
            return srcImage;
        }

        private void GenerateDrawImage(string path, string name, float bodyTop,
            float columnX, float columnWidth, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys, int newWidth,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnHeadList,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnFooterList,
            int IntUDOutputWidth, int IntUDOutputHeight, int IntUDLeading, 
            int IntUDMarginLeft, int IntUDMarginBottom, 
            int initWidthLocation, int initHeightLocation,
            double zoomFactor, int outputIdx,
            ref int outputCount, ref Bitmap srcImage, ref Bitmap destImage, ref Point destPoint,
            (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) columnRuby, bool outputAll=false)
        {
            var maxOutputHeight = IntUDOutputHeight - IntUDMarginBottom;
            float offsetY = bodyTop;
            int rubyIdx = 0;
            var rubyNewWidth = 0;
            (RectType RType, float X, float Y, float Width, float Height) rubyEntity = (0,0,0,0,0);
            if (columnRuby.Entitys != null && columnRuby.Entitys.Count > 0) rubyNewWidth = (int)(columnRuby.Width * zoomFactor);
            if (columnRuby.Entitys != null && rubyIdx < columnRuby.Entitys.Count) rubyEntity = columnRuby.Entitys[rubyIdx++];
            for (int eIdx = 0; eIdx < Entitys.Count; eIdx++)
            {
                var entity = Entitys[eIdx];
                int blankHeight = entity.Y > offsetY ? (int)((entity.Y - offsetY) * zoomFactor) : 0;
                int newHeight = (int)(entity.Height * zoomFactor);
                if (destPoint.Y != initHeightLocation)
                {
                    if (eIdx == 0) destPoint.X += newWidth < mainForm.Modes.Width * zoomFactor ? (int)(mainForm.Modes.Width * zoomFactor - newWidth) : 0; //該行寬度小於通常寬度時，將輸出 X軸位置往右偏移一點
                    if (eIdx == 0 && entity.RType == RectType.EntityHead)
                    {
                        destPoint.X -= newWidth + IntUDLeading;
                        destPoint.Y = initHeightLocation;
                    }
                    else if (destPoint.Y + blankHeight + newHeight > maxOutputHeight) //若輸出 Y軸位置已於最下方，則換下一行
                    {
                        var sliceHeight = maxOutputHeight - destPoint.Y;
                        if (destPoint.Y < maxOutputHeight)
                        {
                            //Console.WriteLine("name:{0}, blankSize:{1}, sliceHeight:{2}, destPoint:{3}, columnRType:{4}", name, blankSize, sliceHeight, destPoint, columnRType);
                            if (blankHeight > sliceHeight) blankHeight -= sliceHeight;
                            else blankHeight = 0;
                        }
                        destPoint.X -= newWidth + IntUDLeading;
                        destPoint.Y = initHeightLocation;
                    }
                }
                if (destPoint.X < IntUDMarginLeft) //若輸出 X軸位置已於最左側，則換一頁
                {
                    AddNowCreateNext(path, name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, ref outputCount, ref srcImage, ref destImage, ref destPoint,
                               zoomFactor, columnHeadList, columnFooterList);
                    destPoint.X -= newWidth + IntUDLeading;
                }
                destPoint.Y += blankHeight;
                if (outputCount == outputIdx || outputAll)
                {
                    Rectangle destRect = new Rectangle(destPoint.X, destPoint.Y, newWidth, newHeight);
                    using (Graphics gr = Graphics.FromImage(destImage))
                    {
                        gr.CompositingMode = CompositingMode.SourceCopy;
                        gr.CompositingQuality = CompositingQuality.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.SmoothingMode = SmoothingMode.AntiAlias;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        using (ImageAttributes imgAttr = new ImageAttributes())
                        {
                            imgAttr.SetWrapMode(WrapMode.TileFlipXY);
                            gr.DrawImage(srcImage, destRect, columnX, entity.Y, columnWidth, entity.Height, GraphicsUnit.Pixel, imgAttr);
                            if (rubyEntity.RType == RectType.Ruby)
                            {
                                while ((rubyEntity.Y >= entity.Y && rubyEntity.Y <= entity.Y + entity.Height) || (entity.Y >= rubyEntity.Y && entity.Y <= rubyEntity.Y + rubyEntity.Height))
                                {
                                    int rubyNewHeight = (int)(rubyEntity.Height * zoomFactor);
                                    int rubyOffsetX = (int)((rubyEntity.X - entity.X - entity.Width) * zoomFactor);
                                    int rubyOffsetY = (int)((rubyEntity.Y - entity.Y) * zoomFactor);
                                    Rectangle rubyDestRect = new Rectangle(destPoint.X + newWidth + rubyOffsetX, destPoint.Y + rubyOffsetY, rubyNewWidth, rubyNewHeight);
                                    gr.DrawImage(srcImage, rubyDestRect, columnRuby.X, rubyEntity.Y, columnRuby.Width, rubyEntity.Height, GraphicsUnit.Pixel, imgAttr);
                                    if (columnRuby.Entitys != null && rubyIdx < columnRuby.Entitys.Count) rubyEntity = columnRuby.Entitys[rubyIdx++];
                                    else break;
                                }
                            }
                        }
                    }
                }
                destPoint.Y += newHeight;
                if (eIdx == Entitys.Count - 1 && entity.RType == RectType.EntityEnd) //若該行最後一字為句尾，則換下一行
                {
                    if (newWidth < mainForm.Modes.Width * zoomFactor) destPoint.X -= (int)(mainForm.Modes.Width * zoomFactor) + IntUDLeading;
                    else destPoint.X -= newWidth + IntUDLeading;
                    destPoint.Y = initHeightLocation;
                }
                offsetY = entity.Y + entity.Height;
            }
        }

        private (Bitmap destImage, Point destPoint) CreateDestImage(int IntUDOutputWidth, int IntUDOutputHeight, int initWidthLocation, int initHeightLocation, Bitmap srcImage=null)
        {
            Point destPoint = new Point(initWidthLocation, initHeightLocation);
            Bitmap destImage = new Bitmap(IntUDOutputWidth, IntUDOutputHeight);
            if (srcImage != null) destImage.SetResolution(srcImage.HorizontalResolution, srcImage.VerticalResolution);

            return (destImage, destPoint);
        }

        #region event
        private void ToolStripPrevious_Click(object sender, EventArgs e)
        {
            outputIdx = SetOutputPage(--outputIdx);
        }
        private void ToolStripNext_Click(object sender, EventArgs e)
        {
            outputIdx = SetOutputPage(++outputIdx);
        }
        private void OutputView_MouseDown(object sender, MouseEventArgs e)
        {
            if (((PictureBox)sender).Image == null) return;
            if (e.Button == MouseButtons.Left) MouseDownLocation = e.Location;
        }
        private void OutputView_MouseMove(object sender, MouseEventArgs e)
        {
            if (((PictureBox)sender).Image == null) return;
            if (e.Button == MouseButtons.Left)
            {
                OutputView.Left += e.X - MouseDownLocation.X;
                OutputView.Top += e.Y - MouseDownLocation.Y;
            }
        }
        private void ToolStripPageBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            {
                outputIdx = SetOutputPage((int)comboBox.SelectedItem);
            }
        }
        private void ToolStripPageBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            {
                if (!Regex.IsMatch(((char)e.KeyValue).ToString(), "[0-9]") && comboBox.Text.Length > 0) comboBox.Text.Remove(comboBox.Text.Length - 1);
                if (e.KeyData == Keys.Enter)
                {
                    outputIdx = SetOutputPage(int.Parse(comboBox.Text));
                    comboBox.SelectedItem = outputIdx;
                }
            }
        }
        private void ToolStripSave_Click(object sender, EventArgs e)
        {
            (string path, string extension, ImageCodecInfo imgEncoder, EncoderParameters encoderParameters, PixelFormat pixelFormat) = getSaveInfo();
            string name;
            if (mainForm.Filter.name != null) name = string.Format(@"{0}_{1}", ToolStripPageBox.SelectedItem, mainForm.Filter.name);
            else name = string.Format(@"{0}", ToolStripPageBox.SelectedItem);
            ImageTool.SaveImage(OutputView.Image, path, name, extension, imgEncoder, encoderParameters, pixelFormat);
        }
        private void ToolStripSaveAll_Click(object sender, EventArgs e)
        {
            (string path, string extension, ImageCodecInfo imgEncoder, EncoderParameters encoderParameters, PixelFormat pixelFormat) = getSaveInfo();
            ParseOutput(-1, true);
            bool CheckBoxOutputAdjustColor = (bool)Properties.Settings.Default["CheckBoxOutputAdjustColor"];
            Color ColorBoxOutputBack = (Color)Properties.Settings.Default["ColorBoxOutputBack"];
            Color ColorBoxOutputFore = (Color)Properties.Settings.Default["ColorBoxOutputFore"];
            float FloatUDForeColorRate = (float)Properties.Settings.Default["FloatUDForeColorRate"];
            for (int idx=0; idx < OutputImgs.Count; idx++)
            {
                string name;
                if (mainForm.Filter.name != null) name = string.Format(@"{0}_{1}", idx, mainForm.Filter.name);
                else name = string.Format(@"{0}", idx);
                Image srcImage = OutputImgs[idx];
                if (CheckBoxOutputAdjustColor && (srcImage.Tag == null || (bool)srcImage.Tag != true)) ImageTool.ChangeForeColor((Bitmap)srcImage, ColorBoxOutputFore.ToArgb(), ColorBoxOutputBack.ToArgb(), FloatUDForeColorRate);
                using (Bitmap outputImage = new Bitmap(srcImage.Width, srcImage.Height))
                using (Graphics graphics = Graphics.FromImage(outputImage))
                using (SolidBrush brush = new SolidBrush(ColorBoxOutputBack))
                {
                    graphics.FillRectangle(brush, 0, 0, srcImage.Width, srcImage.Height);
                    graphics.DrawImage(srcImage, 0, 0, srcImage.Width, srcImage.Height);
                    ImageTool.SaveImage(outputImage, path, name, extension, imgEncoder, encoderParameters, pixelFormat);
                }
                srcImage.Dispose();
            }
        }

        private (string path, string extension, ImageCodecInfo imgEncoder, EncoderParameters encoderParameters, PixelFormat pixelFormat) getSaveInfo()
        {
            ImageType ImageTypeBoxOutput = (ImageType)Properties.Settings.Default["ImageTypeBoxOutput"]; //輸出檔案格式
            PixelFormat PixelFormatBoxOutput = (PixelFormat)Properties.Settings.Default["PixelFormatBoxOutput"]; //輸出色彩資料格式
            long LongUDOutputQuality = (long)Properties.Settings.Default["LongUDOutputQuality"]; //輸出品質
            string path = string.Format(@"{0}\Output", mainForm.InputDir), name, extension;
            ImageCodecInfo imgEncoder;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            Encoder encoder = Encoder.Quality;
            encoderParameters.Param[0] = new EncoderParameter(encoder, LongUDOutputQuality);

            switch (ImageTypeBoxOutput)
            {
                case ImageType.Jpeg:
                    imgEncoder = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == ImageFormat.Jpeg.Guid).FirstOrDefault();
                    extension = ".jpg";
                    break;
                case ImageType.Png:
                    imgEncoder = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == ImageFormat.Png.Guid).FirstOrDefault();
                    extension = ".png";
                    break;
                case ImageType.Tiff:
                    imgEncoder = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == ImageFormat.Tiff.Guid).FirstOrDefault();
                    extension = ".tiff";
                    break;
                case ImageType.Bmp:
                    imgEncoder = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == ImageFormat.Bmp.Guid).FirstOrDefault();
                    extension = ".bmp";
                    break;
                case ImageType.Gif:
                    imgEncoder = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == ImageFormat.Gif.Guid).FirstOrDefault();
                    extension = ".gif";
                    break;
                default:
                    imgEncoder = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == ImageFormat.Jpeg.Guid).FirstOrDefault();
                    extension = ".jpg";
                    break;
            }
            return (path, extension, imgEncoder, encoderParameters, PixelFormatBoxOutput);
        }
        private void ToolStripFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            {
                mainForm.SetToolStripFilterBox(comboBox.SelectedItem);
                SetOutputPage(outputIdx);
            }
        }
        private void ToolStripZoomIn_Click(object sender, EventArgs e)
        {
            mainForm.ZoomFactor += 0.1;
            SetOutputPage(outputIdx);
        }
        private void ToolStripZoomOut_Click(object sender, EventArgs e)
        {
            mainForm.ZoomFactor -= 0.1;
            SetOutputPage(outputIdx);
        }
        private void ToolStripZoomFactorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            {
                mainForm.ZoomFactor = double.Parse(comboBox.SelectedItem.ToString()) / 100;
                SetOutputPage(outputIdx);
            }
        }
        private void ToolStripZoomFactorBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            {
                if (!Regex.IsMatch(((char)e.KeyValue).ToString(), "[0-9]") && comboBox.Text.Length > 0) comboBox.Text.Remove(comboBox.Text.Length - 1);
                if (e.KeyData == Keys.Enter)
                {
                    mainForm.ZoomFactor = double.Parse(comboBox.Text) / 100;
                    SetOutputPage(outputIdx);
                }
            }
        }
        #endregion
    }
}
