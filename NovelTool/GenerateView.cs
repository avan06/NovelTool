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
    public partial class GenerateView : Form
    {
        private readonly Main mainForm;
        private Point MouseDownLocation;
        private List<Image> outputImgs;
        public List<Image> OutputImgs { get => outputImgs; set => outputImgs = value; }
        private int outputCount = -1;
        private int outputIdx;
        public GenerateView(Main mainForm)
        {
            if (mainForm.PageDatas.Count == 0) return;

            this.mainForm = mainForm;
            InitializeComponent();
            toolStripZoomFactorBox.SelectedItem = (mainForm.ZoomFactor * 100).ToString();
            #region InitFilterBox
            toolStripFilterBox.Items.Add("None");
            foreach (BitmapFilter.Filter filterEnum in (BitmapFilter.Filter[])Enum.GetValues(typeof(BitmapFilter.Filter))) toolStripFilterBox.Items.Add(filterEnum);
            foreach (BitmapFilter.FilterXY filterEnum in (BitmapFilter.FilterXY[])Enum.GetValues(typeof(BitmapFilter.FilterXY))) toolStripFilterBox.Items.Add(filterEnum);
            if (Enum.TryParse(mainForm.Filter.name, out BitmapFilter.Filter filter)) toolStripFilterBox.SelectedItem = filter;
            else if (Enum.TryParse(mainForm.Filter.name, out BitmapFilter.FilterXY filterXY)) toolStripFilterBox.SelectedItem = filterXY;
            toolStripFilterBox.SelectedIndexChanged += ToolStripFilterBox_SelectedIndexChanged;
            #endregion

            outputIdx = SetOutputPage(10);
        }
        private int SetOutputPage(int outputIdx)
        {
            Color ColorBoxOutputBack = (Color)Properties.Settings.Default["ColorBoxOutputBack"];
            outputCount = ParseOutput(outputIdx);
            if (toolStripPageBox.Items.Count != outputCount)
            {
                toolStripPageBox.BeginUpdate();
                toolStripPageBox.Items.Clear();
                for (int idx = 0; idx < outputCount; idx++) toolStripPageBox.Items.Add(idx);
                toolStripPageBox.EndUpdate();
            }
            if (outputIdx >= outputCount) outputIdx = 0; //顯示第一頁
            else if (outputIdx < 0) outputIdx = outputCount - 1; //顯示最後一頁
            if (OutputImgs.Count == 0) ParseOutput(outputIdx);
            Bitmap result = (Bitmap)OutputImgs[0];
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
            outputView.Image = result;
            outputView.BackColor = ColorBoxOutputBack;
            toolStripPageBox.SelectedItem = outputIdx;

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
            PositionType PositionTypeBoxHead = (PositionType)Properties.Settings.Default["PositionTypeBoxHead"]; //Head輸出位置
            PositionType PositionTypeBoxFooter = (PositionType)Properties.Settings.Default["PositionTypeBoxFooter"]; //Footer輸出位置

            int initWidthLocation = IntUDOutputWidth - IntUDMarginRight;
            int initHeightLocation = IntUDMarginTop;

            int outputCount = 0;

            double zoomFactor =  mainForm.ZoomFactor;

            outputImgs = new List<Image>();
            Bitmap destImage = null;
            Point destPoint = new Point(initWidthLocation, initHeightLocation);

            for (int pIdx = 0; pIdx < mainForm.PageDatas.Count; ++pIdx)
            {
                Bitmap srcImage = null;
                PageData pageData = mainForm.PageDatas[pIdx];
                AddNowCreateNext(pageData.path, pageData.name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation,
                    ref outputCount, ref srcImage, ref destImage, ref destPoint,
                    PositionTypeBoxHead, PositionTypeBoxFooter, zoomFactor, pageData.columnHeadList, pageData.columnFooterList,
                    false, true);
                if (pageData.isIllustration)
                {   //此頁為圖片時，先儲存目前已產生頁面，再儲存圖片檔案
                    AddNowCreateNext(pageData.path, pageData.name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, 
                        ref outputCount, ref srcImage, ref destImage, ref destPoint,
                        PositionTypeBoxHead, PositionTypeBoxFooter, zoomFactor, pageData.columnHeadList, pageData.columnFooterList,
                        true, false, true);
                }
                else
                {
                    float offsetX = -1;
                    var columnRects = pageData.columnBodyList;
                    float bodyTop = pageData.rectBody.Y;
                    //if (destImage == null) (destImage, destPoint) = CreateDestImage(IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, srcImage);
                    (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) columnRuby = (RectType.None, 0, 0, 0, 0, null);
                    for (int cIdx = columnRects.Count - 1; cIdx >= 0; cIdx--)
                    {
                        (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) = columnRects[cIdx];
                        int newWidth = (int)(Width * zoomFactor);
                        int blankWidth = offsetX != -1 ? (int)((offsetX - X - Width) * zoomFactor) : 0;
                        double blankFactor = blankWidth / newWidth;
                        blankFactor = blankFactor > 2.5 ? 2.5 : (blankFactor < 1.5 ? 0 : blankFactor);
                        destPoint.X -= (int)(newWidth * blankFactor);
                        if (RType == RectType.BodyIn) AddNowCreateNext(pageData.path, pageData.name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, 
                            ref outputCount, ref srcImage, ref destImage, ref destPoint,
                            PositionTypeBoxHead, PositionTypeBoxFooter, zoomFactor, pageData.columnHeadList, pageData.columnFooterList);
                        if (Entitys == null || Entitys.Count == 0) continue;
                        if (RType == RectType.Ruby)
                        { //保留Ruby行內容，下一行主句中再處理
                            columnRuby = (RType, X, Y, Width, Height, Entitys);
                            continue;
                        }
                        if (destPoint.X == initWidthLocation) destPoint.X -= newWidth + IntUDLeading; //位移右邊第一行輸出位置
                        if (columnRuby.Entitys != null && columnRuby.X - X - Width > mainForm.Modes.Width * FloatUDEntityAdjacentRate)
                        { //column 與 Ruby 非相鄰時，代表右側非 Ruby
                            var rubyWidth = (int)(columnRuby.Width * zoomFactor);
                            GenerateDrawImage(pageData.path, pageData.name, bodyTop, columnRuby.X, columnRuby.Width, columnRuby.Entitys, rubyWidth,
                                PositionTypeBoxHead, PositionTypeBoxFooter, pageData.columnHeadList, pageData.columnFooterList,
                                IntUDOutputWidth, IntUDOutputHeight, IntUDLeading, IntUDMarginLeft, IntUDMarginBottom, initWidthLocation, initHeightLocation, zoomFactor, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, (RectType.None, 0, 0, 0, 0, null), outputAll);
                            columnRuby = (RectType.None, 0, 0, 0, 0, null);
                        }

                        GenerateDrawImage(pageData.path, pageData.name, bodyTop, X, Width, Entitys, newWidth,
                            PositionTypeBoxHead, PositionTypeBoxFooter, pageData.columnHeadList, pageData.columnFooterList,
                            IntUDOutputWidth, IntUDOutputHeight, IntUDLeading, IntUDMarginLeft, IntUDMarginBottom, initWidthLocation, initHeightLocation, zoomFactor, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, columnRuby, outputAll);

                        if (columnRuby.Entitys != null) columnRuby = (RectType.None, 0, 0, 0, 0, null);
                        if (RType == RectType.BodyOut) AddNowCreateNext(pageData.path, pageData.name, outputAll, outputIdx, IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, 
                            ref outputCount, ref srcImage, ref destImage, ref destPoint,
                            PositionTypeBoxHead, PositionTypeBoxFooter, zoomFactor, pageData.columnHeadList, pageData.columnFooterList);
                        offsetX = X;
                    }
                }
            }
            if (destImage != null && destPoint.X != initWidthLocation)
            {
                if (outputCount == outputIdx || outputAll) outputImgs.Add(destImage);
                outputCount++;
            }
            return outputCount;
        }

        private void AddNowCreateNext(string path, string name, bool outputAll, int outputIdx,
            int IntUDOutputWidth, int IntUDOutputHeight, int initWidthLocation, int initHeightLocation,
            ref int outputCount, ref Bitmap srcImage, ref Bitmap destImage, ref Point destPoint,
            PositionType PositionTypeBoxHead, PositionType PositionTypeBoxFooter, double zoomFactor,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnHeadList,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnFooterList, 
            bool triggerAdd = true, bool triggerCreate = true, bool triggerSrcAdd = false
            )
        {
            if (triggerAdd && destImage != null && destPoint.X != initWidthLocation)
            {
                if (outputCount == outputIdx || outputAll)
                {
                    if (srcImage == null) srcImage = OpenImage(path, name);
                    int initSize = 10;
                    GenerateDrawTitle(initSize, PositionTypeBoxHead, columnHeadList,
                     IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, zoomFactor, srcImage, ref destImage);
                    GenerateDrawTitle(initSize, PositionTypeBoxFooter, columnFooterList,
                     IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, zoomFactor, srcImage, ref destImage);
                    outputImgs.Add(destImage);
                }
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
            if (triggerCreate && destImage == null) (destImage, destPoint) = CreateDestImage(IntUDOutputWidth, IntUDOutputHeight, initWidthLocation, initHeightLocation, srcImage);
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
            PositionType PositionTypeBoxHead, PositionType PositionTypeBoxFooter,
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
                    if (eIdx == 0 && entity.RType == RectType.EntityHead)
                    {
                        destPoint.X -= newWidth + IntUDLeading;
                        destPoint.Y = initHeightLocation;
                    }
                    else if ((destPoint.Y + blankHeight + newHeight > maxOutputHeight)) //若輸出 Y軸位置已於最下方，則換下一行 //initMaxHeight
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
                               PositionTypeBoxHead, PositionTypeBoxFooter, zoomFactor, columnHeadList, columnFooterList);
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
                if ((eIdx == Entitys.Count - 1 && entity.RType == RectType.EntityEnd)) //若輸出 Y軸位置已於最下方，則換下一行 //IntUDOutputHeight - IntUDMarginBottom
                {
                    destPoint.X -= newWidth + IntUDLeading;
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
                outputView.Left += e.X - MouseDownLocation.X;
                outputView.Top += e.Y - MouseDownLocation.Y;
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

            if (mainForm.Filter.name != null) name = string.Format(@"{0}_{1}", toolStripPageBox.SelectedItem, mainForm.Filter.name);
            else name = string.Format(@"{0}", toolStripPageBox.SelectedItem);
            ImageTool.SaveImage(outputView.Image, path, name, extension, imgEncoder, encoderParameters, PixelFormatBoxOutput);
        }
        private void ToolStripSaveAll_Click(object sender, EventArgs e)
        {
            ImageType ImageTypeBoxOutput = (ImageType)Properties.Settings.Default["ImageTypeBoxOutput"]; //輸出檔案格式
            PixelFormat PixelFormatBoxOutput = (PixelFormat)Properties.Settings.Default["PixelFormatBoxOutput"]; //輸出色彩資料格式
            long LongUDOutputQuality = (long)Properties.Settings.Default["LongUDOutputQuality"]; //輸出品質
            string path = string.Format(@"{0}\Output", mainForm.InputDir);

            string extension;
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

            ParseOutput(-1, true);
            for (int idx=0; idx < OutputImgs.Count; idx++)
            {
                string name;
                if (mainForm.Filter.name != null) name = string.Format(@"{0}_{1}", idx, mainForm.Filter.name);
                else name = string.Format(@"{0}", idx);
                Image srcImage = OutputImgs[idx];
                ImageTool.SaveImage(srcImage, path, name, extension, imgEncoder, encoderParameters, PixelFormatBoxOutput);
                srcImage.Dispose();
            }
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
