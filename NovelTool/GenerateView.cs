using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private int OutputWidth; //輸出圖片寬度
        private int OutputHeight; //輸出圖片高度
        private int MarginLeft; //輸出圖片左側邊距
        private int MarginRight; //輸出圖片右側邊距
        private int MarginTop; //輸出圖片上側邊距
        private int MarginBottom; //輸出圖片下側邊距
        private int Leading; //輸出圖片行距
        private float EntityAdjacentRate; //判斷兩行之間是否相鄰之比例

        private int InitWidthLocation; //The initial width of the new generated Bitmap
        private int InitHeightLocation; //he initial height of the new generated Bitmap
        private int MaxOutputHeight;

        private bool OutputAdjustColorCheck;
        private Color OutputBackColor;
        private Color OutputForeColor;
        private float ForeColorRate;

        private PositionType HeadPositionType;
        private PositionType FooterPositionType;
        private PositionType PagePositionType;
        private bool HeadSizeAffectByZoom;
        private bool FooterSizeAffectByZoom;
        private bool PageSizeAffectByZoom;

        private float TextFontSize;
        private FontFamily TextFontFamily;
        private FontStyle FontTextStyle;

        private string WebViewLineHeight;
        private string WebViewFontSize;

        #region Event

        #region OutputView
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
        #endregion

        #region ToolStrip
        private void ToolStripPrevious_Click(object sender, EventArgs e) => outputIdx = SetOutputPage(--outputIdx);

        private void ToolStripNext_Click(object sender, EventArgs e) => outputIdx = SetOutputPage(++outputIdx);

        private void ToolStripPageBox_SelectedIndexChanged(object sender, EventArgs e)
        { //Init Call 2
            if (sender is ToolStripComboBox comboBox) outputIdx = SetOutputPage((int)comboBox.SelectedItem);
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
            (string path, string extension, ImageCodecInfo imgEncoder, EncoderParameters encoderParameters, PixelFormat pixelFormat) = GetSaveInfo();

            string name = mainForm.Filter.name != null ?
                string.Format(@"{0}_{1}", ToolStripPageBox.SelectedItem, mainForm.Filter.name) :
                string.Format(@"{0}", ToolStripPageBox.SelectedItem);
            Bitmap result = (Bitmap)OutputImgs[0];
            if (!(result.Tag is bool))
            { //Bitmap Not Illustration
                if (OutputAdjustColorCheck) ImageTool.ChangeForeColor(result, OutputForeColor.ToArgb(), OutputBackColor.ToArgb(), ForeColorRate);
                result = BitmapFilter.ConvolutionXYFilter(result, mainForm.Filter.xFilterMatrix, mainForm.Filter.yFilterMatrix, mainForm.Filter.factor, mainForm.Filter.bias);
            }
            using (Bitmap outputImage = new Bitmap(result.Width, result.Height))
            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                if (!(result.Tag is bool)) //Bitmap Not Illustration
                    using (SolidBrush backBrush = new SolidBrush(OutputAdjustColorCheck ? OutputBackColor : Color.White))
                        graphics.FillRectangle(backBrush, 0, 0, result.Width, result.Height);

                graphics.DrawImage(result, 0, 0, result.Width, result.Height);
                ImageTool.SaveImage(outputImage, path, name, extension, imgEncoder, encoderParameters, pixelFormat);
            }
            GC.Collect();
        }

        private void ToolStripSaveAll_Click(object sender, EventArgs e)
        {
            (string path, string extension, ImageCodecInfo imgEncoder, EncoderParameters encoderParameters, PixelFormat pixelFormat) = GetSaveInfo();

            for (int outputIdx = 0; outputIdx < outputCount; outputIdx++)
            {
                ParseOutput(outputIdx);

                string name = mainForm.Filter.name != null ?
                    string.Format(@"{0}_{1}", outputIdx, mainForm.Filter.name) :
                    string.Format(@"{0}", outputIdx);
                Bitmap result = (Bitmap)OutputImgs[0];
                if (!(result.Tag is bool))
                { //Bitmap Not Illustration
                    if (OutputAdjustColorCheck) ImageTool.ChangeForeColor(result, OutputForeColor.ToArgb(), OutputBackColor.ToArgb(), ForeColorRate);
                    result = BitmapFilter.ConvolutionXYFilter(result, mainForm.Filter.xFilterMatrix, mainForm.Filter.yFilterMatrix, mainForm.Filter.factor, mainForm.Filter.bias);
                }
                using (Bitmap outputImage = new Bitmap(result.Width, result.Height))
                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    if (!(result.Tag is bool)) //Bitmap Not Illustration
                        using (SolidBrush backBrush = new SolidBrush(OutputAdjustColorCheck ? OutputBackColor : Color.White))
                            graphics.FillRectangle(backBrush, 0, 0, result.Width, result.Height);

                    graphics.DrawImage(result, 0, 0, result.Width, result.Height);
                    ImageTool.SaveImage(outputImage, path, name, extension, imgEncoder, encoderParameters, pixelFormat);
                }
                if (outputIdx % 10 == 0) GC.Collect();
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
            mainForm.ZoomFactor += 0.1F;
            SetOutputPage(outputIdx);
        }

        private void ToolStripZoomOut_Click(object sender, EventArgs e)
        {
            mainForm.ZoomFactor -= 0.1F;
            SetOutputPage(outputIdx);
        }

        private void ToolStripZoomFactorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            { //Init Call 3
                mainForm.ZoomFactor = float.Parse(comboBox.SelectedItem.ToString()) / 100;
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
                    mainForm.ZoomFactor = float.Parse(comboBox.Text) / 100;
                    SetOutputPage(outputIdx);
                }
            }
        }
        #endregion

        private (string path, string extension, ImageCodecInfo imgEncoder, EncoderParameters encoderParameters, PixelFormat pixelFormat) GetSaveInfo()
        {
            string path = string.Format(@"{0}\Output", mainForm.InputDir);
            (string extension, ImageCodecInfo imgEncoder, EncoderParameters encoderParameters, PixelFormat pixelFormat) = ImageTool.GetSaveImageInfo();

            return (path, extension, imgEncoder, encoderParameters, pixelFormat);
        }
        #endregion

        private GenerateView()
        {
            OutputWidth = Properties.Settings.Default.OutputWidth.Value; //輸出圖片寬度
            OutputHeight = Properties.Settings.Default.OutputHeight.Value; //輸出圖片高度
            MarginLeft = Properties.Settings.Default.MarginLeft.Value; //輸出圖片左側邊距
            MarginRight = Properties.Settings.Default.MarginRight.Value; //輸出圖片右側邊距
            MarginTop = Properties.Settings.Default.MarginTop.Value; //輸出圖片上側邊距
            MarginBottom = Properties.Settings.Default.MarginBottom.Value; //輸出圖片下側邊距
            Leading = Properties.Settings.Default.Leading.Value; //輸出圖片行距
            EntityAdjacentRate = Properties.Settings.Default.EntityAdjacentRate.Value; //判斷兩行之間是否相鄰之比例

            InitWidthLocation = OutputWidth - MarginRight;
            InitHeightLocation = MarginTop;
            MaxOutputHeight = OutputHeight - MarginBottom;

            OutputAdjustColorCheck = Properties.Settings.Default.OutputAdjustColorCheck.Value;
            OutputBackColor = Color.FromKnownColor(Properties.Settings.Default.OutputBackColor.Value);
            OutputForeColor = Color.FromKnownColor(Properties.Settings.Default.OutputForeColor.Value);
            ForeColorRate = Properties.Settings.Default.ForeColorRate.Value;

            HeadPositionType = Properties.Settings.Default.HeadPositionType.Value; //Head輸出位置
            FooterPositionType = Properties.Settings.Default.FooterPositionType.Value; //Footer輸出位置
            PagePositionType = Properties.Settings.Default.PagePositionType.Value; //Page輸出位置
            HeadSizeAffectByZoom = Properties.Settings.Default.HeadSizeAffectByZoom.Value;
            FooterSizeAffectByZoom = Properties.Settings.Default.FooterSizeAffectByZoom.Value;
            PageSizeAffectByZoom = Properties.Settings.Default.PageSizeAffectByZoom.Value;

            TextFontSize = Properties.Settings.Default.TextFontSize.Value;
            TextFontFamily = Properties.Settings.Default.TextFontFamily.Value;
            FontTextStyle = (Properties.Settings.Default.TextFontBold.Value ? FontStyle.Bold : 0) | (Properties.Settings.Default.TextFontItalic.Value ? FontStyle.Italic : 0);

            WebViewLineHeight = Properties.Settings.Default.WebViewLineHeight.Value;
            WebViewFontSize = Properties.Settings.Default.WebViewFontSize.Value;

            InitializeComponent();
        }

        private void GenerateView_FormClosing(object sender, FormClosingEventArgs e) => GC.Collect();

        public GenerateView(Main mainForm, bool isWeView = false) : this()
        {
            if (mainForm.PageDatas.Count == 0) return;

            this.mainForm = mainForm;

            if (!isWeView)
            {
                #region InitFilterBox
                ToolStripFilterBox.Items.Add("None");
                foreach (BitmapFilter.Filter filterEnum in (BitmapFilter.Filter[])Enum.GetValues(typeof(BitmapFilter.Filter))) ToolStripFilterBox.Items.Add(filterEnum);
                foreach (BitmapFilter.FilterXY filterEnum in (BitmapFilter.FilterXY[])Enum.GetValues(typeof(BitmapFilter.FilterXY))) ToolStripFilterBox.Items.Add(filterEnum);
                if (Enum.TryParse(mainForm.Filter.name, out BitmapFilter.Filter filter)) ToolStripFilterBox.SelectedItem = filter;
                else if (Enum.TryParse(mainForm.Filter.name, out BitmapFilter.FilterXY filterXY)) ToolStripFilterBox.SelectedItem = filterXY;
                ToolStripFilterBox.SelectedIndexChanged += ToolStripFilterBox_SelectedIndexChanged;
                #endregion

                //Init Call 1
                outputIdx = SetOutputPage(10);

                ToolStripZoomFactorBox.SelectedItem = (mainForm.ZoomFactor * 100).ToString();
            }
            #region EpubAozoraWebView
            else
            {
                if (WebBox == null)
                {
                    SetWebBrowserFeatures();
                    WebBox = new WebBrowser();
                    Panel1.Controls.Add(WebBox);
                    WebBox.Location = OutputView.Location;
                    WebBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    WebBox.Dock = DockStyle.Fill;
                    WebBox.DocumentCompleted += (object sender, WebBrowserDocumentCompletedEventArgs e) =>
                    {
                        WebBox.Document.Window.Error += (w, we) =>
                        {
                            we.Handled = true; // Do something with the error...
                            Debug.WriteLine(string.Format("Error: {1}\nline: {0}\nurl: {2}", we.LineNumber, we.Description, we.Url));
                        };
                    };
                    WebBox.ScriptErrorsSuppressed = true;
                    WebBox.BringToFront();
                }

                this.mainForm = mainForm;

                System.Text.StringBuilder webStr = new System.Text.StringBuilder();

                for (int pIdx = 0; pIdx < mainForm.PageDatas.Count; ++pIdx)
                {
                    PageData pageData = mainForm.PageDatas[pIdx];
                    if (pageData.TextList == null || pageData.TextList.Count <= 0) continue;

                    bool isBegin = false;
                    for (int tIdx = 0; tIdx < pageData.TextList.Count; tIdx++)
                    {
                        (string text, string ruby) = pageData.TextList[tIdx];
                        bool newLineFlag = text.EndsWith("\n");
                        text = text.Replace("\n", "");
                        if (text == "_pagebreak_") webStr.AppendLine("<p style='page-break-after: always;'>&nbsp;</p><p style='page-break-before: always;'>&nbsp;</p>");
                        else if (text == "_img_") webStr.AppendLine($"<p><img height='100%' style='page-break-before: always;' src='{ruby}'></p>");
                        else if (text == "_bold_") webStr.Append($"<b>{ruby}</b>");
                        else if (ruby.Length > 0)
                        {
                            if (!isBegin)
                            {
                                isBegin = true;
                                webStr.Append("<p>");
                            }
                            webStr.Append($"<ruby><rb>{text}</rb><rp>（</rp><rt>{ruby}</rt><rp>）</rp></ruby>");
                        }
                        else
                        {
                            if (!isBegin)
                            {
                                isBegin = true;
                                webStr.Append("<p>");
                            }
                            webStr.Append($"{text}"); //webStr.AppendLine($"<ruby><rb>{text}</rb><rp>（</rp><rt>&nbsp;</rt><rp>）</rp></ruby>");
                        }
                        if (isBegin && newLineFlag)
                        {
                            isBegin = false;
                            webStr.Append("</p>\n");
                        }
                        //if (tIdx > 0 && tIdx < pageData.textList.Count - 1 && pageData.textList[tIdx - 1].text == "\n" && pageData.textList[tIdx + 1].text == "\n") webStr.AppendLine("<h1>" + text + "</h1>");
                    }
                    webStr.AppendLine("<hr>");
                }

                if (webStr.Length > 0 && WebBox.DocumentText == "")
                {
                    WebBox.Navigate("about:blank");
                    WebBox.Document.Write(String.Empty);
                    WebBox.DocumentText = "<html>\n<head>\n<meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'/>\n" +
                        "<style type='text/css'>\nhtml { height: 100%; }\n" +
                        "body {\n" +
                        "background-color: " + "#" + OutputBackColor.R.ToString("X2") + OutputBackColor.G.ToString("X2") + OutputBackColor.B.ToString("X2") + ";" +
                        "color: " + "#" + OutputForeColor.R.ToString("X2") + OutputForeColor.G.ToString("X2") + OutputForeColor.B.ToString("X2") + ";" +
                        "line-height: " + WebViewLineHeight + ";" +
                        "font-size: " + WebViewFontSize + ";" +
                        "font-family: '" + TextFontFamily.Name + "', 'Lucida Grande', 'Meiryo', 'Meiryo UI', 'Microsoft JhengHei UI', 'Microsoft JhengHei', sans-serif;" +
                        "-ms-writing-mode: tb-rl;" +
                        "writing-mode: vertical-rl;" +
                        "overflow-x: scroll;" +
                        "padding: 10px;" +
                        "margin: 0px auto;\n" +
                        "}\n" +
                        "</style>\n</head>\n" +
                        "<body>\n" +
                        "<script>\n" +
                        "function scrollLeft(element, type, value) {" +
                        "if (!value || value <= 0) value = element.clientWidth*0.9;" +
                        "if (!type) type = 'down';" +
                        "if (type === 'up') {element.scrollTop-=value;element.scrollLeft+=value;}" +
                        "else if (type === 'down') {element.scrollTop+=value;element.scrollLeft-=value;}}\n" +
                        "const novel = document.querySelector('body');\n" +
                        "window.addEventListener('keydown', function(e) {" +
                        "if (e.keyCode === 33 || e.keyCode === 38) { /*page up、up*/" +
                        "e.preventDefault();scrollLeft(novel, 'up');}" +
                        "else if (e.keyCode === 32 || e.keyCode === 34 || e.keyCode === 40) { /*space、page down、down*/" +
                        "e.preventDefault();scrollLeft(novel, 'down');}" +
                        "else if (e.keyCode === 37) { /*left*/" +
                        "e.preventDefault();scrollLeft(novel, 'down', 100);}" +
                        "else if (e.keyCode === 39) { /*right*/" +
                        "e.preventDefault();scrollLeft(novel, 'up', 100);}});\n" +
                        "novel.addEventListener('mousewheel', function(e){" +
                        "e.preventDefault();" +
                        "if (e.wheelDelta > 0) scrollLeft(novel, 'up');else scrollLeft(novel, 'down');}, { passive: false });\n" +
                        "</script>\n" +
                        $"{webStr}\n" +
                        "</body>\n</html>";
                }
            }
            #endregion
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
            if (!(result.Tag is bool))
            { //Bitmap Not Illustration
                if (OutputAdjustColorCheck) ImageTool.ChangeForeColor(result, OutputForeColor.ToArgb(), OutputBackColor.ToArgb(), ForeColorRate);
                result = BitmapFilter.ConvolutionXYFilter(result, mainForm.Filter.xFilterMatrix, mainForm.Filter.yFilterMatrix, mainForm.Filter.factor, mainForm.Filter.bias);
            }
            OutputView.Image = result;
            if (!(result.Tag is bool)) OutputView.BackColor = OutputAdjustColorCheck ? OutputBackColor : Color.White;

            ToolStripPageBox.SelectedItem = outputIdx;

            if (outputIdx % 10 == 0) GC.Collect();
            return outputIdx;
        }

        private int ParseOutput(int outputIdx, bool outputAll = false)
        {
            int outputCount = 0;

            var modeWidth = mainForm.Modes.Width * mainForm.ZoomFactor;
            var modeWidthMax = mainForm.Modes.WidthMax * mainForm.ZoomFactor;

            outputImgs = new List<Image>();
            Bitmap destImage = null;
            PointF destPoint = new PointF(InitWidthLocation, InitHeightLocation);

            for (int pIdx = 0; pIdx < mainForm.PageDatas.Count; ++pIdx)
            {
                Bitmap srcImage = null;
                PageData pageData = mainForm.PageDatas[pIdx];
                #region Parse Text Image
                if (pageData.TextList == null || pageData.TextList.Count <= 0)
                {
                    AddNowCreateNext(pageData, outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, false, true); //trigger create new dest Image when destImage is null
                    if (pageData.IsIllustration) //此頁為圖片時，先儲存目前已產生頁面，再儲存圖片檔案
                        AddNowCreateNext(pageData, outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, true, false, true); //Trigger to save Illustration Image
                    else if (pageData.ColumnBodyList != null)
                    {
                        float offsetX = -1;
                        var columnRects = pageData.ColumnBodyList;
                        float bodyTop = pageData.RectBody.Y;
                        var columnRuby = ImageTool.NewEntitys();
                        for (int cIdx = columnRects.Count - 1; cIdx >= 0; cIdx--)
                        {
                            (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) = columnRects[cIdx];
                            var columnNext = cIdx > 1 ? columnRects[cIdx - 1] : ImageTool.NewEntitys();
                            if (columnNext.Entitys != null && columnNext.RType != RectType.Ruby && RType != RectType.Ruby && X - columnNext.X - columnNext.Width < mainForm.Modes.Width * EntityAdjacentRate && Width <= mainForm.Modes.WidthMin) //前後行為相鄰時
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

                            if (RType == RectType.BodyIn && Width > mainForm.Modes.WidthMax && destPoint.X != InitWidthLocation) // && Entitys[0].RType == RectType.EntityHead
                            { //該行為本頁最右邊第一段，且寬度大於眾數最大寬度，可能為標題，且輸出座標已非初始位置，換一頁輸出顯示
                                AddNowCreateNext(pageData, outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint); //trigger save and create new dest Image
                            }

                            int newWidth = (int)(Width * mainForm.ZoomFactor);
                            int blankWidth = offsetX != -1 ? (int)((offsetX - X - Width) * mainForm.ZoomFactor) : 0;
                            double blankFactor = (double)blankWidth / newWidth;
                            blankFactor = blankFactor > 2.5 ? 2.5 : (blankFactor < 1.5 ? 0 : blankFactor);
                            destPoint.X -= (int)(newWidth * blankFactor); //原圖兩行之間的空白行較大時，輸出位置加上空白行比例寬度
                            if (newWidth > modeWidthMax) destPoint.X -= newWidth + (int)modeWidth; //當該行寬度大於眾數最大寬度，輸出 X軸位置往左偏移，避免蓋掉前一行內容

                            if (RType == RectType.Ruby)
                            { //保留Ruby行內容，下一行主句中再處理
                                columnRuby = (RType, X, Y, Width, Height, Entitys);
                                continue;
                            }
                            if (destPoint.X == InitWidthLocation) destPoint.X -= newWidth + Leading; //位移右邊第一行輸出位置
                            if (columnRuby.Entitys != null && columnRuby.X - X - Width > mainForm.Modes.Width * EntityAdjacentRate)
                            { //column 與 Ruby 非相鄰時，代表右側非 Ruby
                                var rubyWidth = (int)(columnRuby.Width * mainForm.ZoomFactor);
                                GenerateDrawImage(pageData, columnRuby.X, columnRuby.Width, columnRuby.Entitys, rubyWidth, outputIdx,
                                    ref outputCount, ref srcImage, ref destImage, ref destPoint, ImageTool.NewEntitys(), outputAll);
                                columnRuby = ImageTool.NewEntitys();
                            }

                            GenerateDrawImage(pageData, X, Width, Entitys, newWidth, outputIdx,
                                ref outputCount, ref srcImage, ref destImage, ref destPoint, columnRuby, outputAll);

                            offsetX = X;
                            if (columnRuby.Entitys != null) columnRuby = ImageTool.NewEntitys();

                            if (Entitys[Entitys.Count - 1].Height < mainForm.Modes.Heigh && Entitys[Entitys.Count - 1].RType != RectType.EntityEnd)
                                destPoint.Y += (int)(mainForm.Modes.HeighMin * mainForm.ZoomFactor); //每行非句尾的最後一字(句子連接下一頁)，若高度不到通常高度(例如符號字)，則增加空白輸出
                            else if (RType == RectType.BodyOut && Entitys[Entitys.Count - 1].RType == RectType.EntityEnd)
                                AddNowCreateNext(pageData, outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint); //trigger save and create new dest Image
                        }
                    }
                }
                #endregion
                #region Parse EpubAozora
                else
                {
                    bool isBegin = false;
                    float fontSize = TextFontSize * mainForm.ZoomFactor;
                    float blankSize = fontSize / 6;
                    PointF destRubyPTmp = PointF.Empty;
                    StringFormat format = new StringFormat();/*StringFormat.GenericTypographic*/
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    for (int tIdx = 0; tIdx < pageData.TextList.Count; tIdx++)
                    {
                        AddNowCreateNext(outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, false, true); //trigger create new dest Image when destImage is null
                        (string text, string ruby) = pageData.TextList[tIdx];
                        bool newLineFlag = text.EndsWith("\n");
                        text = text.Replace("\n", "");
                        if (text == "_pagebreak_")
                        {
                            AddNowCreateNext(outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint); //trigger save and create new dest Image
                        }
                        else if (text == "_img_")
                        {
                            AddNowCreateNext(outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, true, false, true, ruby); //Trigger to save Illustration Image
                        }
                        else if (text == "_bold_") { /*webStr.Append($"<b>{ruby}</b>");*/ }
                        else
                        {
                            text = text.Replace("―", "｜").Replace("─", "｜").Replace("…", "︙").Replace("〟", "〞"); //FixVerticalStyle
                            if (!isBegin)
                            {
                                isBegin = true;
                                if (text.Length > 0 && "「『(（〈《〔﹝【　〝".IndexOf(text[0]) == -1) destPoint.Y += fontSize + blankSize;
                            }
                            int idxRuby = 0;
                            int idxText = 0;
                            bool isContinue;
                            do
                            {
                                isContinue = false;
                                using (var foreBrush = new SolidBrush(OutputForeColor))
                                using (Graphics gr = Graphics.FromImage(destImage))
                                {
                                    gr.CompositingQuality = CompositingQuality.HighQuality;
                                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    gr.SmoothingMode = SmoothingMode.AntiAlias;
                                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                    gr.PageUnit = GraphicsUnit.Pixel;
                                    gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;//AntiAlias;

                                    if (ruby.Length > 0)
                                    {
                                        RectangleF destRect = new RectangleF(destPoint.X + fontSize + blankSize, destPoint.Y, fontSize / 2, fontSize / 2);
                                        if (destPoint.X == InitWidthLocation) destRect.X -= fontSize + Leading; //位移右邊第一行輸出位置
                                        if (destRubyPTmp.X == destRect.X && destRubyPTmp.Y > destRect.Y) destRect.Y = destRubyPTmp.Y;
                                        for (var idx = idxRuby; idx < ruby.Length; idx++)
                                        {
                                            if (destRect.X - fontSize - Leading < MarginLeft)
                                            { //若輸出 X軸位置已於最左側，則紀錄Ruby目前索引
                                                //AddNowCreateNext(outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint, mainForm.ZoomFactor); //trigger save and create new dest Image
                                                idxRuby = idx;
                                                isContinue = true;
                                                break;
                                            }
                                            if (outputCount == outputIdx || outputAll)
                                            {
                                                var charR = ruby[idx];
                                                using (GraphicsPath rubyPath = new GraphicsPath())
                                                {
                                                    #region FixVerticalAlign
                                                    if ("ー".IndexOf(charR) != -1)
                                                    { //日本語の長音符
                                                        rubyPath.AddString("丨", TextFontFamily, (int)FontTextStyle, fontSize / 2, new PointF(0, 0), format);
                                                        using (var flipXMatrix = new Matrix(-1, 0, 0, 1, destRect.X + fontSize / 4, destRect.Y + fontSize / 4))
                                                        using (Matrix matrix = new Matrix())
                                                        { //Flip a Graphics object. https://stackoverflow.com/a/53182901
                                                            matrix.Multiply(flipXMatrix, MatrixOrder.Append);
                                                            rubyPath.Transform(matrix);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        rubyPath.AddString(charR.ToString(), TextFontFamily, (int)FontTextStyle, fontSize / 2, destRect, format);
                                                        if (IsVerticalAlign(charR)) //「」『』ー─―…()（）〈〉《》〔〕﹝﹞【】〝〟=＝~～
                                                        {
                                                            using (Matrix mRotate = new Matrix())
                                                            {
                                                                mRotate.RotateAt(90, destPoint, MatrixOrder.Append);
                                                                if ("~".IndexOf(charR) != -1) mRotate.Translate(fontSize * 0.6F, 0, MatrixOrder.Append);
                                                                else if ("…".IndexOf(charR) != -1) mRotate.Translate(fontSize * 1.2F, 0, MatrixOrder.Append);
                                                                else mRotate.Translate(fontSize, 0, MatrixOrder.Append);
                                                                rubyPath.Transform(mRotate);
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    gr.FillPath(foreBrush, rubyPath); //Fill the font with White brush
                                                }

                                            }
                                            destRect.Y += fontSize / 2 + blankSize / 2;
                                            if (destRect.Y + fontSize / 2 + blankSize / 2 > MaxOutputHeight)
                                            {
                                                destRect.X -= fontSize + Leading;
                                                destRect.Y = InitHeightLocation;
                                            }
                                        }
                                        //if (isContinue) continue;
                                        destRubyPTmp = destRect.Location;
                                    }
                                    else destRubyPTmp = PointF.Empty;

                                    for (var idx = idxText; idx < text.Length; idx++)
                                    {
                                        if (destPoint.X - Leading < MarginLeft)
                                        { //若輸出 X軸位置已於最左側，則換一頁
                                            AddNowCreateNext(outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint); //trigger save and create new dest Image
                                            idxText = idx;
                                            isContinue = true;
                                            break;
                                        }
                                        if (destPoint.X == InitWidthLocation) destPoint.X -= fontSize + Leading; //位移右邊第一行輸出位置
                                        if (outputCount == outputIdx || outputAll)
                                        {
                                            var charT = text[idx];
                                            RectangleF destRect = new RectangleF(destPoint.X, destPoint.Y, fontSize, fontSize);
                                            using (GraphicsPath textPath = new GraphicsPath())
                                            {
                                                #region FixVerticalAlign
                                                if ("ー".IndexOf(charT) != -1)
                                                { //日本語の長音符
                                                    textPath.AddString("丨", TextFontFamily, (int)FontTextStyle, fontSize, new PointF(0, 0), format);
                                                    using (var flipXMatrix = new Matrix(-1, 0, 0, 1, destRect.X + fontSize / 2, destRect.Y + fontSize / 2))
                                                    using (Matrix matrix = new Matrix())
                                                    { //Flip a Graphics object. https://stackoverflow.com/a/53182901
                                                        matrix.Multiply(flipXMatrix, MatrixOrder.Append);
                                                        textPath.Transform(matrix);
                                                    }
                                                }
                                                else
                                                {
                                                    textPath.AddString(charT.ToString(), TextFontFamily, (int)FontTextStyle, fontSize, destRect, format);
                                                    if (IsVerticalAlign(charT)) //「」『』ー─―…()（）〈〉《》〔〕﹝﹞【】〝〟=＝~～
                                                    {
                                                        using (Matrix mRotate = new Matrix())
                                                        {
                                                            mRotate.RotateAt(90, destPoint, MatrixOrder.Append);
                                                            if ("~".IndexOf(charT) != -1) mRotate.Translate(fontSize * 0.6F, 0, MatrixOrder.Append);
                                                            else if ("…".IndexOf(charT) != -1) mRotate.Translate(fontSize * 1.2F, 0, MatrixOrder.Append);
                                                            else mRotate.Translate(fontSize, 0, MatrixOrder.Append);
                                                            textPath.Transform(mRotate);
                                                        }
                                                    }
                                                }
                                                #endregion
                                                gr.FillPath(foreBrush, textPath); //Fill the font with White brush
                                            }
                                        }
                                        if (!newLineFlag || idx + 1 < text.Length) destPoint.Y += fontSize + blankSize;
                                        if (destPoint.Y + fontSize + blankSize > MaxOutputHeight)
                                        {
                                            destPoint.X -= fontSize + Leading;
                                            destPoint.Y = InitHeightLocation;
                                        }
                                    }
                                }
                            } while (isContinue);

                            if (isBegin && newLineFlag)
                            {
                                isBegin = false;

                                destPoint.X -= fontSize + Leading;
                                destPoint.Y = InitHeightLocation;
                                if (destPoint.X - Leading < MarginLeft)
                                {
                                    AddNowCreateNext(outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint); //trigger save and create new dest Image
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            if (destImage != null && destPoint.X != InitWidthLocation)
            {
                if (outputCount == outputIdx || outputAll) outputImgs.Add(destImage);
                outputCount++;
            }

            return outputCount;
        }

        /// <summary>
        /// Perform save generated Bitmap or create a new Bitmap
        /// </summary>
        /// <param name="pageData">PageData</param>
        /// <param name="outputAll">is generate all pages</param>
        /// <param name="outputIdx">generate specifies the content of the index (page number)</param>
        /// <param name="outputCount">Count of currently generated pages</param>
        /// <param name="srcImage">The source Bitmap currently read</param>
        /// <param name="destImage">The currently generated Bitmap</param>
        /// <param name="destPoint">The coordinate position of the current text bitmap</param>
        /// <param name="zoomFactor">Zoom to print the content of the header and footer according to the zoom factor</param>
        /// <param name="triggerAdd">Trigger save for the currently generated Bitmap, a new page needs to be changed</param>
        /// <param name="triggerCreate">Trigger to generate new Bitmap page</param>
        /// <param name="triggerSrcAdd">Trigger to save the current source Bitmap when the source is an illustration</param>
        private void AddNowCreateNext(PageData pageData, bool outputAll, int outputIdx,
            ref int outputCount, ref Bitmap srcImage, ref Bitmap destImage, ref PointF destPoint,
            bool triggerAdd = true, bool triggerCreate = true, bool triggerSrcAdd = false) => AddNowCreateNext(outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint,
                triggerAdd, triggerCreate, triggerSrcAdd, string.Format("{0}/{1}", pageData.Path, pageData.Name), false, pageData.ColumnHeadList, pageData.ColumnFooterList);

        private void AddNowCreateNext(bool outputAll, int outputIdx,
            ref int outputCount, ref Bitmap srcImage, ref Bitmap destImage, ref PointF destPoint,
            bool triggerAdd = true, bool triggerCreate = true, bool triggerSrcAdd = false, string fullPath = "", bool isEpubAozora = true,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnHeadList = null,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnFooterList = null)
        {
            if (triggerAdd && destImage != null && destPoint.X != InitWidthLocation)
            {
                if (outputCount == outputIdx || outputAll) outputImgs.Add(destImage);
                outputCount++;
                destImage = null;
            }
            if (triggerSrcAdd)
            {
                if (outputCount == outputIdx || outputAll)
                {
                    if (srcImage == null || isEpubAozora) srcImage = ImageTool.OpenImage(fullPath);
                    srcImage.Tag = true; //Illustration
                    outputImgs.Add(srcImage);
                }
                outputCount++;
            }

            if (!isEpubAozora && (outputCount == outputIdx || outputAll) && srcImage == null) srcImage = ImageTool.OpenImage(fullPath);

            if (triggerCreate && (destImage == null || (outputCount == outputIdx && destImage.Size == new Size(1, 1))))
            {
                if (outputCount == outputIdx || outputAll)
                {
                    (destImage, destPoint) = CreateDestImage(OutputWidth, OutputHeight, srcImage);

                    if (!isEpubAozora)
                    {
                        if (srcImage == null) srcImage = ImageTool.OpenImage(fullPath);

                        GenerateDrawTitle(HeadPositionType, HeadSizeAffectByZoom, columnHeadList, srcImage, ref destImage);
                        GenerateDrawTitle(FooterPositionType, FooterSizeAffectByZoom, columnFooterList, srcImage, ref destImage);
                    }
                    var pageCnt = outputIdx != -1 ? outputIdx : outputImgs.Count;
                    GenerateDrawText(PagePositionType, pageCnt.ToString(), srcImage, ref destImage);
                }
                else (destImage, destPoint) = CreateDestImage(1, 1, null);
            }
        }

        /// <summary>
        /// Print out headers or footers in the generated Bitmap
        /// </summary>
        /// <param name="PositionTypeBoxTitle">The print position of the header or footer</param>
        /// <param name="columnTitleList">Header or footer columns information parsed from the source image</param>
        /// <param name="zoomFactor">Zoom to print the content of the header and footer according to the zoom factor</param>
        /// <param name="srcImage">The source Bitmap currently read</param>
        /// <param name="destImage">The currently generated Bitmap</param>
        /// <param name="initSize">Margin size of print header or footer</param>
        private void GenerateDrawTitle(PositionType PositionTypeBoxTitle, bool SizeAffectByZoom,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnTitleList,
            Bitmap srcImage, ref Bitmap destImage, int initSize = 10)
        {
            if (PositionTypeBoxTitle == PositionType.None || columnTitleList == null || columnTitleList.Count <= 0) return;

            float offsetX = -1;
            float zoomFactor = SizeAffectByZoom ? mainForm.ZoomFactor : 1;
            int modeWidth = (int)(mainForm.Modes.Width * zoomFactor);
            Point destPointTitle = new Point(initSize, initSize);
            if (PositionTypeBoxTitle == PositionType.TopRight) destPointTitle = new Point(OutputWidth - initSize, initSize);
            else if (PositionTypeBoxTitle == PositionType.BottomRight) destPointTitle = new Point(OutputWidth - initSize, OutputHeight - initSize);
            else if (PositionTypeBoxTitle == PositionType.TopLeft) destPointTitle = new Point(initSize, initSize);
            else if (PositionTypeBoxTitle == PositionType.BottomLeft) destPointTitle = new Point(initSize, OutputHeight - initSize);
            else if (PositionTypeBoxTitle == PositionType.Top) destPointTitle = new Point(OutputWidth / 2, initSize);
            else if (PositionTypeBoxTitle == PositionType.Bottom) destPointTitle = new Point(OutputWidth / 2, OutputHeight - initSize);

            if (PositionTypeBoxTitle == PositionType.TopRight || PositionTypeBoxTitle == PositionType.BottomRight)
            {
                for (int cIdx = columnTitleList.Count - 1; cIdx >= 0; cIdx--)
                {
                    var columnTitle = columnTitleList[cIdx];
                    int newWidth = (int)(columnTitle.Width * zoomFactor);
                    int newHeight = (int)(columnTitle.Height * zoomFactor);
                    int blankWidth = offsetX != -1 ? (int)((offsetX - columnTitle.X - columnTitle.Width) * zoomFactor) : 0;
                    blankWidth = blankWidth > modeWidth ? modeWidth : blankWidth;
                    DrawTitle(columnTitle, newWidth, newHeight, -blankWidth, true, destPointTitle.Y == OutputHeight - initSize, srcImage, ref destImage, ref destPointTitle);
                    offsetX = columnTitle.X;
                }
            }
            else if (PositionTypeBoxTitle == PositionType.TopLeft || PositionTypeBoxTitle == PositionType.BottomLeft || PositionTypeBoxTitle == PositionType.Top || PositionTypeBoxTitle == PositionType.Bottom)
            {
                if (PositionTypeBoxTitle == PositionType.Top || PositionTypeBoxTitle == PositionType.Bottom)
                    destPointTitle.X -= (int)(columnTitleList.Count > 1 ? (columnTitleList[columnTitleList.Count - 1].X + 
                        columnTitleList[columnTitleList.Count - 1].Width - columnTitleList[0].X) : columnTitleList[0].Width) / 2;
                
                for (int cIdx = 0; cIdx < columnTitleList.Count; cIdx++)
                {
                    var columnTitle = columnTitleList[cIdx];
                    int newWidth = (int)(columnTitle.Width * zoomFactor);
                    int newHeight = (int)(columnTitle.Height * zoomFactor);
                    int blankWidth = offsetX != -1 ? (int)((columnTitle.X - offsetX) * zoomFactor) : 0;
                    blankWidth = blankWidth > modeWidth ? modeWidth : blankWidth;
                    DrawTitle(columnTitle, newWidth, newHeight, blankWidth, false, destPointTitle.Y == OutputHeight - initSize, srcImage, ref destImage, ref destPointTitle);
                    offsetX = columnTitle.X + columnTitle.Width;
                }
            }
        }

        /// <summary>
        /// Print header or footer content according to columnTitle
        /// </summary>
        /// <param name="columnTitle">Header or footer column information parsed from the source image</param>
        /// <param name="newWidth">The width of the print depends on the zoomFactor</param>
        /// <param name="newHeight">The height of the print depends on the zoomFactor</param>
        /// <param name="blankWidth">The blank width after printing is based on zoomFactor</param>
        /// <param name="shiftLeftX">Determines whether the X axis should be moved to the left before printing</param>
        /// <param name="shiftDownY">Determines whether the Y-axis should be moved up and then printed</param>
        /// <param name="srcImage">The source Bitmap currently read</param>
        /// <param name="destImage">The currently generated Bitmap</param>
        /// <param name="destPointTitle">The coordinate position of the current printout</param>
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

        private void GenerateDrawText(PositionType PositionTypeBoxTitle, string text,
            Bitmap srcImage, ref Bitmap destImage, int initSize = 10)
        {
            if (PositionTypeBoxTitle == PositionType.None || text.Trim() == "") return;

            Point destPoint = new Point(initSize, initSize);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near;

            if (PositionTypeBoxTitle == PositionType.TopRight)
            {
                destPoint = new Point(OutputWidth - initSize, initSize);
                format.Alignment = StringAlignment.Far;
            }
            else if (PositionTypeBoxTitle == PositionType.BottomRight)
            {
                destPoint = new Point(OutputWidth - initSize, OutputHeight - initSize);
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Far;
            }
            else if (PositionTypeBoxTitle == PositionType.TopLeft)
            {
                destPoint = new Point(initSize, initSize);
            }
            else if (PositionTypeBoxTitle == PositionType.BottomLeft)
            {
                destPoint = new Point(initSize, OutputHeight - initSize);
                format.LineAlignment = StringAlignment.Far;
            }
            else if (PositionTypeBoxTitle == PositionType.Top)
            {
                destPoint = new Point(OutputWidth / 2, initSize);
                format.Alignment = StringAlignment.Center;
            }
            else if (PositionTypeBoxTitle == PositionType.Bottom)
            {
                destPoint = new Point(OutputWidth / 2, OutputHeight - initSize);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Far;
            }

            DrawText(text, format, ref destImage, ref destPoint, PageSizeAffectByZoom);
        }

        private void DrawText(string text, StringFormat format, ref Bitmap destImage, ref Point destPoint, bool PageZoomAffect = false)
        {
            using (Graphics gr = Graphics.FromImage(destImage))
            {
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.PageUnit = GraphicsUnit.Pixel;
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                using (var foreBrush = new SolidBrush(OutputForeColor))
                using (GraphicsPath textPath = new GraphicsPath())
                {
                    textPath.AddString(text, TextFontFamily, (int)FontTextStyle, TextFontSize * (PageZoomAffect ? mainForm.ZoomFactor : 1), destPoint, format);
                    gr.FillPath(foreBrush, textPath);
                }
            }
        }

        /// <summary>
        /// Print out text images to generate Bitmap
        /// </summary>
        /// <param name="pageData">PageData</param>
        /// <param name="columnX">The X-axis position of the currently column of the source Bitmap</param>
        /// <param name="columnWidth">The width of the currently column of the source Bitmap</param>
        /// <param name="Entitys">The Entity list of the currently column of the source Bitmap</param>
        /// <param name="newWidth">The width of the print depends on the zoomFactor</param>
        /// <param name="zoomFactor">Zoom to print the content of the header and footer according to the zoom factor</param>
        /// <param name="outputIdx">generate specifies the content of the index (page number)</param>
        /// <param name="outputCount">Count of currently generated pages</param>
        /// <param name="srcImage">The source Bitmap currently read</param>
        /// <param name="destImage">The currently generated Bitmap</param>
        /// <param name="destPoint">The coordinate position of the current text bitmap</param>
        /// <param name="columnRuby">Ruby column information of the current column of the source bitmap</param>
        /// <param name="outputAll">is generate all pages</param>
        private void GenerateDrawImage(PageData pageData, float columnX, float columnWidth,
            List<(RectType RType, float X, float Y, float Width, float Height)> Entitys, int newWidth, int outputIdx,
            ref int outputCount, ref Bitmap srcImage, ref Bitmap destImage, ref PointF destPoint,
            (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) columnRuby, bool outputAll = false)
        {
            float offsetY = pageData.RectBody.Y; //The Y-axis position of the top of the body of the source Bitmap
            int rubyIdx = 0;
            var rubyNewWidth = 0;
            var modeWidth = mainForm.Modes.Width * mainForm.ZoomFactor;
            (RectType RType, float X, float Y, float Width, float Height) rubyEntity = (0, 0, 0, 0, 0);
            if (columnRuby.Entitys != null && columnRuby.Entitys.Count > 0) rubyNewWidth = (int)(columnRuby.Width * mainForm.ZoomFactor);
            if (columnRuby.Entitys != null && rubyIdx < columnRuby.Entitys.Count) rubyEntity = columnRuby.Entitys[rubyIdx++];
            for (int eIdx = 0; eIdx < Entitys.Count; eIdx++)
            {
                var entity = Entitys[eIdx];
                var blankHeight = entity.Y > offsetY ? ((entity.Y - offsetY) * mainForm.ZoomFactor) : 0;
                int newHeight = (int)(entity.Height * mainForm.ZoomFactor);
                if (destPoint.Y != InitHeightLocation)
                {
                    if (eIdx == 0) destPoint.X += newWidth < modeWidth ? (int)(modeWidth - newWidth) : 0; //該行寬度小於通常寬度時，將輸出 X軸位置往右偏移一點
                    if (eIdx == 0 && entity.RType == RectType.EntityHead)
                    {
                        destPoint.X -= newWidth + Leading;
                        destPoint.Y = InitHeightLocation;
                    }
                    else if (destPoint.Y + blankHeight + newHeight > MaxOutputHeight)
                    { //若輸出 Y軸位置已於最下方，則換下一行
                        var sliceHeight = MaxOutputHeight - destPoint.Y;
                        if (destPoint.Y < MaxOutputHeight)
                        { //Console.WriteLine("name:{0}, blankSize:{1}, sliceHeight:{2}, destPoint:{3}, columnRType:{4}", name, blankSize, sliceHeight, destPoint, columnRType);
                            if (blankHeight > sliceHeight) blankHeight -= sliceHeight;
                            else blankHeight = 0;
                        }
                        destPoint.X -= newWidth + Leading;
                        destPoint.Y = InitHeightLocation;
                    }
                }
                if (destPoint.X < MarginLeft)
                { //若輸出 X軸位置已於最左側，則換一頁
                    AddNowCreateNext(pageData, outputAll, outputIdx, ref outputCount, ref srcImage, ref destImage, ref destPoint); //trigger save and create new dest Image
                    destPoint.X -= newWidth + Leading;
                }
                destPoint.Y += blankHeight;
                if (outputCount == outputIdx || outputAll)
                {
                    RectangleF destRect = new RectangleF(destPoint.X, destPoint.Y, newWidth, newHeight);
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
                            gr.DrawImage(srcImage, Rectangle.Round(destRect), columnX, entity.Y, columnWidth, entity.Height, GraphicsUnit.Pixel, imgAttr);
                            if (rubyEntity.RType == RectType.Ruby)
                            {
                                while ((rubyEntity.Y >= entity.Y && rubyEntity.Y <= entity.Y + entity.Height) || (entity.Y >= rubyEntity.Y && entity.Y <= rubyEntity.Y + rubyEntity.Height))
                                {
                                    int rubyNewHeight = (int)(rubyEntity.Height * mainForm.ZoomFactor);
                                    int rubyOffsetX = (int)((rubyEntity.X - entity.X - entity.Width) * mainForm.ZoomFactor);
                                    int rubyOffsetY = (int)((rubyEntity.Y - entity.Y) * mainForm.ZoomFactor);
                                    RectangleF rubyDestRect = new RectangleF(destPoint.X + newWidth + rubyOffsetX, destPoint.Y + rubyOffsetY, rubyNewWidth, rubyNewHeight);
                                    gr.DrawImage(srcImage, Rectangle.Round(rubyDestRect), columnRuby.X, rubyEntity.Y, columnRuby.Width, rubyEntity.Height, GraphicsUnit.Pixel, imgAttr);
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
                    destPoint.X -= newWidth < modeWidth ? (int)modeWidth + Leading : newWidth + Leading;
                    destPoint.Y = InitHeightLocation;
                }
                offsetY = entity.Y + entity.Height;
            }
        }

        private (Bitmap destImage, PointF destPoint) CreateDestImage(int OutputWidth, int OutputHeight, Bitmap srcImage = null)
        {
            PointF destPoint = new PointF(InitWidthLocation, InitHeightLocation);
            Bitmap destImage = new Bitmap(OutputWidth, OutputHeight);
            if (srcImage == null || srcImage.HorizontalResolution < 128) destImage.SetResolution(300, 300);
            else destImage.SetResolution(srcImage.HorizontalResolution, srcImage.VerticalResolution);

            return (destImage, destPoint);
        }

        /// <summary>
        /// Web browser control emulation
        /// https://stackoverflow.com/a/28626667
        /// set WebBrowser features, more info: http://stackoverflow.com/a/18333982/1768303
        /// </summary>
        private void SetWebBrowserFeatures()
        {
            // don't change the registry if running in-proc inside Visual Studio
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime) return;

            var appName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            var featureControlRegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\";

            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, GetBrowserEmulationMode(), RegistryValueKind.DWord);

            // enable the features which are "On" for the full Internet Explorer browser
            //Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
            //Registry.SetValue(featureControlRegKey + "FEATURE_AJAX_CONNECTIONEVENTS", appName, 1, RegistryValueKind.DWord);
            //Registry.SetValue(featureControlRegKey + "FEATURE_GPU_RENDERING", appName, 1, RegistryValueKind.DWord);
            //Registry.SetValue(featureControlRegKey + "FEATURE_WEBOC_DOCUMENT_ZOOM", appName, 1, RegistryValueKind.DWord);
            //Registry.SetValue(featureControlRegKey + "FEATURE_NINPUT_LEGACYMODE", appName, 0, RegistryValueKind.DWord);
        }

        /// <summary>
        /// fake Edge dword:00002ee1(12001)
        /// 
        /// While the proposed "solution" to add dword:00002ee1 to FEATURE_BROWSER_EMULATION
        /// causes the webbrowser to (falsely) report Edge/12.9200_AGENT as USER_AGENT,
        /// in fact it still uses the Trident engine to render the web content.
        /// You can verify this by browsing to http://html5test.com/
        /// where the webbrowser control scores between 342 and 347 points (the same as IE11),
        /// while Edge scores 397 points.
        /// https://stackoverflow.com/a/32034725
        /// </summary>
        private UInt32 GetBrowserEmulationMode()
        {
            int browserVersion = 0;
            using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                RegistryKeyPermissionCheck.ReadSubTree, System.Security.AccessControl.RegistryRights.QueryValues))
            {
                var version = ieKey.GetValue("svcVersion");
                if (null == version)
                {
                    version = ieKey.GetValue("Version");
                    if (null == version) throw new ApplicationException("Microsoft Internet Explorer is required!");
                }
                int.TryParse(version.ToString().Split('.')[0], out browserVersion);
            }

            UInt32 mode = 12001;//fake Edge //11000; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode. 
            if (browserVersion < 7) throw new ApplicationException("Unsupported version of Microsoft Internet Explorer!");
            else if (browserVersion == 7) mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode.
            else if (browserVersion == 8) mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode.
            else if (browserVersion == 9) mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.
            else if (browserVersion == 10) mode = 10000; // Internet Explorer 10.

            return mode;
        }

        private bool IsVerticalAlign(char text) => "「」『』ー─―…()（）〈〉《》〔〕﹝﹞【】〝=＝~～".IndexOf(text) != -1; //〟
    }
}
