using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace NovelTool
{
    public partial class Main : Form
    {
        private const string filterImageExts = "*.BMP;*.JPG;*.GIF;*.PNG";
        private const string filterZipExts = "*.ZIP;*.RAR;*.7Z";
        private const string filterEpubExts = "*.EPUB";
        
        private string inputDir;
        private (ConcurrentDictionary<float, int> TopDict, ConcurrentDictionary<float, int> BottomDict,
                ConcurrentDictionary<float, int> LeftDict, ConcurrentDictionary<float, int> RightDict,
                ConcurrentDictionary<float, int> WidthDict, ConcurrentDictionary<float, int> HeightDict) counts;
        private (float Top, float Bottom, float Left, float Right, float Width, float Heigh,
                float TopMin, float BottomMin, float LeftMin, float RightMin,
                float WidthMin, float WidthMax, float HeighMin, float HeighMax) modes;
        private List<PageData> pageDatas = new List<PageData>();
        private Point MouseDownLocation;
        private bool isDefaultTheme = false;
        private Bitmap sourceImg;
        private Options options;
        private GenerateView generateView;
        private double zoomFactor = 1;
        private (string name, double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias) filter;

        public List<PageData> PageDatas { get => pageDatas; }
        public double ZoomFactor { 
            get => zoomFactor;
            set
            {
                zoomFactor = value;
                toolStripZoomFactorBox.SelectedItem = (value * 100).ToString();
            }
        }
        public (float Top, float Bottom, float Left, float Right, float Width, float Heigh, 
            float TopMin, float BottomMin, float LeftMin, float RightMin, 
            float WidthMin, float WidthMax, float HeighMin, float HeighMax) Modes { get => modes; }
        public (string name, double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias) Filter { get => filter; }
        public string InputDir { get => inputDir; }
        public (ConcurrentDictionary<float, int> TopDict, ConcurrentDictionary<float, int> BottomDict, 
            ConcurrentDictionary<float, int> LeftDict, ConcurrentDictionary<float, int> RightDict, 
            ConcurrentDictionary<float, int> WidthDict, ConcurrentDictionary<float, int> HeightDict) Counts { get => counts; set => counts = value; }

        #region event
        #region MinMaxCloseButtonEvent

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void MaximizeBtn_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
            else WindowState = FormWindowState.Maximized;
        }
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
        private void MenuStrip_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownEvent(sender, e);
        }
        #region PictureBoxEvent
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (((PictureBox)sender).Image == null) return;
            if (e.Button == MouseButtons.Left) MouseDownLocation = e.Location;
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (((PictureBox)sender).Image == null) return;
            if (e.Button == MouseButtons.Left)
            {
                pictureBox.Left += e.X - MouseDownLocation.X;
                pictureBox.Top += e.Y - MouseDownLocation.Y;
            }
            else
            {
                if (sourceImg.Tag == null) return;
                var pageData = PageDatas[(int)sourceImg.Tag];
                var columnBodyList = pageData.columnBodyList;
                var X = e.X / zoomFactor;
                var Y = e.Y / zoomFactor;
                if (columnBodyList == null)
                {
                    ToolMsg.Text = string.Format("X={0},Y={1}", (int)X, (int)Y);
                    return;
                }
                for (int colIdx = 0; colIdx < columnBodyList.Count; colIdx++)
                {
                    var columnBody = columnBodyList[colIdx];
                    if (X < columnBody.X || X > columnBody.X + columnBody.Width) continue;
                    var entitys = columnBody.Entitys;
                    if (Y > columnBody.Y && Y < columnBody.Y + columnBody.Height) ToolMsg.Text = string.Format("X={0},Y={1}, Column{2}: {{{3}, {4}, {5}, {6}, {7}, Entitys:{8}}}", (int)X, (int)Y, colIdx, columnBody.RType, columnBody.X, columnBody.Y, columnBody.Width, columnBody.Height, entitys.Count);
                    for (int idx = 0; idx < entitys.Count; idx++)
                    {
                        var entity = entitys[idx];
                        if (Y < entity.Y || Y > entity.Y + entity.Height) continue;
                        ToolMsg.Text = string.Format("X={0},Y={1}, Entity{2}: {3}",  (int)X, (int)Y, idx, entity);
                    }
                }
            }

        }
        /// <summary>
        /// 縮放 PictureBox的圖片
        /// </summary>
        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {   if (Control.ModifierKeys == Keys.Control && sender is PictureBox pBox)
            {
                if (pBox.Image == null) return;
                ((HandledMouseEventArgs)e).Handled = true;
                if (e.Delta >= 0) ZoomFactor += 0.1;
                else ZoomFactor -= 0.1;

                pBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
            }
        }
        private void PictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (sender is PictureBox pBox)
            {
                if (pBox.Image == null) return;
                switch (e.KeyCode)
                {
                    case Keys.Add:
                    case Keys.Oemplus:
                        ZoomFactor += 0.1;
                        break;
                    case Keys.Subtract:
                    case Keys.OemMinus:
                        ZoomFactor -= 0.1;
                        break;
                }
                if (zoomFactor != 1) pBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
            }
        }
        private void ToolStripZoomIn_Click(object sender, EventArgs e)
        {
            ZoomFactor += 0.1;
            pictureBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
        }

        private void ToolStripZoomOut_Click(object sender, EventArgs e)
        {
            ZoomFactor -= 0.1;
            pictureBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
        }
        private void ToolStripZoomFactorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            {
                ZoomFactor = double.Parse(comboBox.SelectedItem.ToString()) / 100;
                if (sourceImg == null) return;
                pictureBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
            }
        }
        private void ToolStripZoomFactorBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            {
                if (!Regex.IsMatch(((char)e.KeyValue).ToString(), "[0-9]") && comboBox.Text.Length > 0) comboBox.Text.Remove(comboBox.Text.Length - 1);
                if (e.KeyData == Keys.Enter)
                {
                    ZoomFactor = double.Parse(comboBox.Text) / 100;
                    if (sourceImg == null) return;
                    pictureBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
                }
            }
        }
        private Bitmap PictureBox_Zoom(Bitmap bitMap, double zoomFactor)
        {
            if (bitMap == null) return null;
            int newWidth = (int)(bitMap.Width * zoomFactor);
            int newHeight = (int)(bitMap.Height * zoomFactor);
            Size newSize = new Size(newWidth, newHeight);

            Rectangle destRect = new Rectangle(Point.Empty, newSize);
            Bitmap destImage = new Bitmap(newWidth, newHeight);
            destImage.SetResolution(sourceImg.HorizontalResolution, sourceImg.VerticalResolution);
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
                    gr.DrawImage(sourceImg, destRect, 0, 0, sourceImg.Width, sourceImg.Height, GraphicsUnit.Pixel, imgAttr);
                }
            }
            return destImage;
        }
        #endregion
        private void ToolStripOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter =
                $"All files (*.*)|*.*|" +
                $"Zip Files({filterZipExts})|{filterZipExts}|" +
                $"Epub Files({filterEpubExts})|{filterEpubExts}|" +
                $"Image Files({filterImageExts})|{filterImageExts}"
                ;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            string fileDirName = Path.GetDirectoryName(openFileDialog.FileName);
            string fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            string fileExt = Path.GetExtension(openFileDialog.FileName).ToUpper();
            try
            {
                if (filterEpubExts.IndexOf(fileExt) != -1 || filterZipExts.IndexOf(fileExt) != -1)
                {
                    fileDirName = fileDirName + @"\" + fileName;
                    IArchive archive = ArchiveFactory.Open(openFileDialog.FileName);
                    IReader reader = archive.ExtractAllEntries();
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                            reader.WriteEntryToDirectory(fileDirName, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                    }
                }
                if (filterEpubExts.IndexOf(fileExt) != -1)
                {
                    string fullPath = "";
                    string rootPath = "";
                    XmlTextReader reader = new XmlTextReader($"{fileDirName}/META-INF/container.xml");
                    XmlTextReader readerOpt = null;
                    while (reader.Read())
                    {
                        if (reader.Name != "rootfile") continue;

                        fullPath = reader.GetAttribute("full-path");
                        rootPath = Path.GetDirectoryName($"{fileDirName}/{fullPath}");
                        readerOpt = new XmlTextReader($"{fileDirName}/{fullPath}");
                        break;
                    }
                    string title = "";
                    string publisher = "";
                    List<string> creators = new List<string>();
                    List<string> itemrefs = new List<string>();
                    Dictionary<string, string> images = new Dictionary<string, string>();
                    Dictionary<string, string> xhtmls = new Dictionary<string, string>();
                    while (readerOpt.Read())
                    {
                        if (readerOpt.NodeType != XmlNodeType.Element) continue;

                        if (readerOpt.LocalName == "title") title = readerOpt.ReadString();
                        else if (readerOpt.LocalName == "creator") creators.Add(readerOpt.ReadString());
                        else if (readerOpt.LocalName == "publisher") publisher = readerOpt.ReadString();
                        else if (readerOpt.LocalName == "item")
                        {
                            if (readerOpt.GetAttribute("media-type").StartsWith("image")) images.Add(readerOpt.GetAttribute("id"), readerOpt.GetAttribute("href"));
                            if (readerOpt.GetAttribute("media-type") == "application/xhtml+xml") xhtmls.Add(readerOpt.GetAttribute("id"), readerOpt.GetAttribute("href"));
                        }
                        else if (readerOpt.LocalName == "itemref") itemrefs.Add(readerOpt.GetAttribute("idref"));
                    }
                    fileListView.BeginUpdate();
                    fileListView.Items.Clear();
                    PageDatas.Clear();
                    for (int idx = 0; idx < itemrefs.Count; idx++)
                    {
                        string itemref = itemrefs[idx];
                        string xhtml = xhtmls[itemref];
                        string xhtmlPath = $"{rootPath}\\{xhtml}";
                        string xhtmlDirName = Path.GetDirectoryName(xhtmlPath);
                        string xhtmlExt = Path.GetExtension(xhtmlPath).ToUpper();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(xhtmlPath);
                        XmlNodeList xmlNodes = xmlDoc.GetElementsByTagName("p");
                        List<(string text, string ruby)> xmlTextList = GetXmlText(xmlNodes, xhtmlDirName);

                        int itemIdx = fileListView.Items.Count;
                        fileListView.Items.Add(itemIdx.ToString(), rootPath, 0);
                        fileListView.Items[itemIdx].Checked = true;
                        fileListView.Items[itemIdx].SubItems.Add(xhtml);
                        fileListView.Items[itemIdx].SubItems.Add(rootPath);
                        PageData pageData = new PageData(itemIdx, rootPath, xhtml, xhtmlExt);
                        pageData.textList = xmlTextList;
                        PageDatas.Add(pageData);
                    }
                    fileListView.EndUpdate();
                    //XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    //nsmgr.AddNamespace("ns", xmlDoc.DocumentElement.NamespaceURI);
                    //XmlNodeList tt = xmlDoc.SelectNodes("ns:br", nsmgr);
                    //xmlDoc.SelectNodes("//ns:img", nsmgr);
                    //xmlDoc.SelectNodes("ns:span", nsmgr);
                }
                else
                {
                    string[] fileEntries = Directory.GetFiles(fileDirName);
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                        pictureBox.Image = null;
                    }
                    fileListView.BeginUpdate();
                    fileListView.Items.Clear();
                    PageDatas.Clear();
                    sourceImg = null;
                    foreach (string filePath in fileEntries)
                    {
                        DirectoryInfo file = new DirectoryInfo(filePath);
                        if (!Regex.IsMatch(filterImageExts, file.Extension.ToUpper())) continue;

                        int itemIdx = fileListView.Items.Count;
                        if (itemIdx == 0) inputDir = file.Parent.FullName;
                        fileListView.Items.Add(itemIdx.ToString(), filePath, 0);
                        fileListView.Items[itemIdx].Checked = true;
                        fileListView.Items[itemIdx].SubItems.Add(file.Name);
                        fileListView.Items[itemIdx].SubItems.Add(file.Parent.FullName);
                        PageDatas.Add(new PageData(itemIdx, file.Parent.FullName, file.Name, file.Extension.ToUpper()));
                    }
                    newAnalysisWorker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show("Open file failed, " + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private List<(string text, string ruby)> GetXmlText(XmlNodeList childNodes, string xhtmlDir)
        {
            List<(string, string)> result = new List<(string, string)>();
            for (int cIdx = 0; cIdx < childNodes.Count; cIdx++)
            {
                XmlNode childNode = childNodes[cIdx];
                string text = childNode.InnerText;
                int textLen = text.Length;
                if (childNode.LocalName == "br" && textLen == 0) result.Add(("\n", ""));
                else if (childNode.LocalName == "img" && textLen == 0)
                {
                    XmlAttributeCollection attrs = childNode.Attributes;
                    for (int aIdx = 0; aIdx < attrs.Count; aIdx++)
                    {
                        XmlAttribute attr = attrs[aIdx];
                        if (attr.Name == "src")
                        {
                            result.Add(("_img_", xhtmlDir + "/" + attr.Value));
                            break;
                        }
                    }
                }
                else if (childNode.LocalName == "ruby" && childNode.ChildNodes.Count > 0)
                {
                    XmlNodeList rubyChildNodes = childNode.ChildNodes;
                    for (int rIdx = 0; rIdx < rubyChildNodes.Count; rIdx++)
                    {
                        XmlNode r1 = rubyChildNodes[rIdx];
                        XmlNode r2 = rubyChildNodes[++rIdx];
                        result.Add((r1.InnerText, r2.InnerText));
                    }
                }
                else if (childNode.LocalName == "p" || (textLen == 0 && childNode.ChildNodes.Count > 0))
                {
                    List<(string, string)> xmlText = GetXmlText(childNode.ChildNodes, xhtmlDir);
                    result.AddRange(xmlText);
                    result.Add(("\n", ""));
                }
                else if (childNode.LocalName == "span" && textLen > 0) result.Add((text, ""));
                else if (childNode.NodeType == XmlNodeType.Text && textLen > 0) result.Add((text, ""));
                else if (textLen > 0) result.Add((text, ""));
            }
            return result;
        }
        private void ToolStripOptions_Click(object sender, EventArgs e)
        {
            if (options == null || options.IsDisposed) options = new Options(this);
            options.StartPosition = FormStartPosition.CenterParent;
            options.TopLevel = true;
            options.Show();
        }
        private void ToolStripDarkTheme_Click(object sender, EventArgs e)
        {
            ChangeTheme(Controls, isDefaultTheme);
            isDefaultTheme = !isDefaultTheme;
        }
        private void ToolStripGenView_Click(object sender, EventArgs e)
        {
            if (PageDatas.Count == 0) return;
            if (generateView == null || generateView.IsDisposed) generateView = new GenerateView(this);
            generateView.StartPosition = FormStartPosition.CenterParent;
            generateView.TopMost = true;
            generateView.Show();
        }
        private void FileListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem item = e.Item;

            var pageData = PageDatas.Count > item.Index ? PageDatas[item.Index] : null;
            if (item.Checked)
            {
                item.BackColor = Color.LightSkyBlue;

                if (newAnalysisWorker.IsBusy || toolProgressBar.Value != toolProgressBar.Maximum || PageDatas.Count <= item.Index) return;

                FileStream fs = null;
                Image pageImg = null;

                try
                {
                    RefreshModes();
                    fs = File.OpenRead(pageData.path + @"\" + pageData.name);
                    pageImg = Image.FromStream(fs);
                    if (ImageTool.AnalysisPointStates(new Bitmap(pageImg), pageData, true))
                    {
                        pageData.isIllustration = false;
                        ImageTool.AnalysisPageY(pageData);
                        if (pageData.xStatesHead != null) ImageTool.AnalysisPageX(pageData.rectHead, pageData.xStatesHead, out pageData.columnHeadList);
                        if (pageData.xStatesBody != null) ImageTool.AnalysisPageX(pageData.rectBody, pageData.xStatesBody, out pageData.columnBodyList);
                        if (pageData.xStatesFooter != null) ImageTool.AnalysisPageX(pageData.rectFooter, pageData.xStatesFooter, out pageData.columnFooterList);
                        ImageTool.AnalysisColumnRects(pageData.pStates, pageData.rectBody, pageData.columnBodyList, counts);

                        ImageTool.AnalysisEntityHeighWidth(pageData.pStates, pageData.columnBodyList, modes);
                        ImageTool.AnalysisEntityHeadBodyEnd(pageData.rectBody, pageData.columnBodyList, modes);
                        item.Selected = false;
                        item.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
                finally
                {
                    if (pageImg != null) pageImg.Dispose();
                    if (fs != null) fs.Dispose();
                }
            }
            else
            {
                item.BackColor = Color.Azure;
                if (pageData != null) pageData.isIllustration = true;
            }
        }
        private void FileListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) return;
            FileStream fs = null;
            ListViewItem item = e.Item;

            int IntUDRectViewWidth = (int)Properties.Settings.Default["IntUDRectViewWidth"];
            RectType[] rTypes = new RectType[] { //RectType.Head, RectType.Body, RectType.Footer, 
                RectType.EntityBody, RectType.Ruby, 
                RectType.MergeTB, RectType.MergeLR, 
                RectType.SplitTop, RectType.SplitMiddle, RectType.SplitBottom};
            Dictionary<RectType, (Color color, Pen pen, List<RectangleF> rects)> drawRectObj = new Dictionary<RectType, (Color color, Pen pen, List<RectangleF> rects)>();
            drawRectObj.Add(RectType.None, (Color.Black, new Pen(Color.Black, IntUDRectViewWidth), new List<RectangleF>()));
            for (int idx = 0; idx < rTypes.Length; idx++)
            {
                var rType = rTypes[idx];
                Color color = (Color)Properties.Settings.Default["ColorBoxRect" + rType.ToString()];
                Pen pen = new Pen(color, IntUDRectViewWidth);
                List<RectangleF> rects = new List<RectangleF>();
                drawRectObj.Add(rType, (color, pen, rects));
            }

            Color ColorBoxRectHead = (Color)Properties.Settings.Default["ColorBoxRectHead"];
            Color ColorBoxRectBody = (Color)Properties.Settings.Default["ColorBoxRectBody"];
            Color ColorBoxRectFooter = (Color)Properties.Settings.Default["ColorBoxRectFooter"];
            Color ColorBoxRectColumn = (Color)Properties.Settings.Default["ColorBoxRectColumn"];
            Color ColorBoxRectColumnRuby = (Color)Properties.Settings.Default["ColorBoxRectColumnRuby"];

            Pen penHead = new Pen(ColorBoxRectHead, IntUDRectViewWidth); 
            Pen penBody = new Pen(ColorBoxRectBody, IntUDRectViewWidth);
            Pen penFooter = new Pen(ColorBoxRectFooter, IntUDRectViewWidth);
            Pen penColumn = new Pen(ColorBoxRectColumn, IntUDRectViewWidth);
            Pen penColumnRuby = new Pen(ColorBoxRectColumnRuby, IntUDRectViewWidth);

            string name = item.SubItems[1].Text;
            var pageData = PageDatas[item.Index];
            try
            {
                if (pictureBox.Image != null) pictureBox.Image.Dispose();

                fs = File.OpenRead(item.Text);
                Bitmap img = (Bitmap)Image.FromStream(fs);
                //https://www.c-sharpcorner.com/article/solution-for-a-graphics-object-cannot-be-created-from-an-im/
                if (img.PixelFormat == PixelFormat.Undefined || img.PixelFormat == PixelFormat.DontCare || img.PixelFormat == PixelFormat.Format1bppIndexed ||
                    img.PixelFormat == PixelFormat.Format4bppIndexed || img.PixelFormat == PixelFormat.Format8bppIndexed || 
                    img.PixelFormat == PixelFormat.Format16bppGrayScale || img.PixelFormat == PixelFormat.Format16bppArgb1555 ) img = new Bitmap(img);

                pictureBox.Image = img;
                pictureBox.Location = Point.Empty;
                sourceImg = (Bitmap)pictureBox.Image;
                sourceImg.Tag = item.Index;
                using (Graphics gr = Graphics.FromImage(pictureBox.Image))
                {
                    gr.SmoothingMode = SmoothingMode.AntiAlias;
                    gr.DrawRectangles(penHead, new RectangleF[] { ImageTool.EntityToRectangleF(pageData.rectHead) });
                    gr.DrawRectangles(penBody, new RectangleF[] { ImageTool.EntityToRectangleF(pageData.rectBody) });
                    gr.DrawRectangles(penFooter, new RectangleF[] { ImageTool.EntityToRectangleF(pageData.rectFooter) });
                    DrawRectangles(gr, rTypes, pageData.columnBodyList, drawRectObj, penColumn, penColumnRuby);
                }

                if (zoomFactor != 1) pictureBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
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
        }
        public void DrawRectangles(Graphics gr, RectType[] rTypes, 
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnRects, 
            Dictionary<RectType, (Color color, Pen pen, List<RectangleF> rects)> drawRectObj, Pen penColumn, Pen penColumnRuby)
        {
            if (columnRects == null || columnRects.Count == 0) return;

            (Color color, Pen pen, List<RectangleF> rects) drawRect;

            List<RectangleF> colRects = new List<RectangleF>();
            List<RectangleF> colRectsRuby = new List<RectangleF>();
            for (int idx = 0; idx < columnRects.Count; idx++)
            {
                (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) = columnRects[idx];

                if (Entitys == null || Entitys.Count == 0) continue;

                if (RType == RectType.Ruby) colRectsRuby.Add(new RectangleF(X, Y, Width, Height));
                else colRects.Add(new RectangleF(X, Y, Width, Height));

                for (int eIdx = 0; eIdx < Entitys.Count; eIdx++)
                {
                    var entity = Entitys[eIdx];
                    if (!drawRectObj.TryGetValue(entity.RType, out drawRect)) drawRect = drawRectObj[RectType.None];
                    drawRect.rects.Add(new RectangleF(entity.X, entity.Y, entity.Width, entity.Height));
                }
            }
            if (colRects != null && colRects.Count > 0) gr.DrawRectangles(penColumn, colRects.ToArray());
            if (colRectsRuby != null && colRectsRuby.Count > 0) gr.DrawRectangles(penColumnRuby, colRectsRuby.ToArray());
            if (drawRectObj.TryGetValue(RectType.None, out drawRect) && drawRect.rects.Count > 0) gr.DrawRectangles(drawRect.pen, drawRect.rects.ToArray());
            for (int idx = 0; idx < rTypes.Length; idx++)
            {
                var rType = rTypes[idx];
                if (drawRectObj.TryGetValue(rType, out drawRect) && drawRect.rects.Count > 0) gr.DrawRectangles(drawRect.pen, drawRect.rects.ToArray());
            }
        }

        public void SetToolStripFilterBox(object item)
        {
            toolStripFilterBox.SelectedItem = item;
        }

        #region ConvolutionFilter
        private void ToolStripFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripComboBox comboBox)
            {
                Stopwatch ticker = Stopwatch.StartNew();
                if (comboBox.SelectedIndex == 0)
                {
                    filter = (null, null, null, 0, 0);
                    return;
                }
                else if (comboBox.SelectedItem is BitmapFilter.Filter filterEnum)
                {
                    filter.name = filterEnum.ToString();
                    (filter.xFilterMatrix, filter.factor, filter.bias) = BitmapFilter.Filters[filterEnum]; //(double[,] filterMatrix, double factor, int bias) = BitmapFilter.Filters[filterEnum];
                    Bitmap bitMap = zoomFactor != 1 ? PictureBox_Zoom(sourceImg, zoomFactor) : sourceImg;
                    pictureBox.Image = BitmapFilter.ConvolutionFilter(bitMap, filter.xFilterMatrix, filter.factor, filter.bias, true); //, (int)(573 * zoomFactor), (int)(110 * zoomFactor), (int)(17 * zoomFactor), (int)(32 * zoomFactor) //convolutionfilter(filtermatrix, factor, bias, true);
                    if (Regex.IsMatch(filterEnum.ToString(), "Gaussian3x3|Gaussian5x5Type1|Gaussian5x5Type2"))
                    {
                        (double[,] filterMatrix2, double factor2, int bias2) = BitmapFilter.Filters[BitmapFilter.Filter.Laplacian5x5Type1];
                        pictureBox.Image = BitmapFilter.ConvolutionFilter((Bitmap)pictureBox.Image, filterMatrix2, factor2, bias2, true);
                    }
                }
                else if (comboBox.SelectedItem is BitmapFilter.FilterXY filterXYEnum)
                {
                    filter.name = filterXYEnum.ToString();
                    (filter.xFilterMatrix, filter.yFilterMatrix, filter.factor, filter.bias) = BitmapFilter.FilterXYs[filterXYEnum]; //(double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias) = BitmapFilter.FilterXYs[filterXYEnum];
                    Bitmap bitMap = zoomFactor != 1 ? PictureBox_Zoom(sourceImg, zoomFactor) : sourceImg;
                    pictureBox.Image = BitmapFilter.ConvolutionFilter(bitMap, filter.xFilterMatrix, filter.yFilterMatrix, filter.factor, filter.bias, true); //ConvolutionFilter(xFilterMatrix, yFilterMatrix, factor, bias, true);
                }
                ToolMsg.Text = string.Format("{0}, elapsed: {1}", comboBox.SelectedItem, ticker.Elapsed.TotalSeconds);
            }
        }
        //private void ToolStripSharp_Click(object sender, EventArgs e)
        //{
        //    BitmapTool bmpTool = new BitmapTool((Bitmap)pictureBox.Image, true, true);
        //    Bitmap newBitmap = new Bitmap(bmpTool.Width, bmpTool.Height);
        //    //拉普拉斯模板
        //    int[] Laplacian = {
        //        -1, -1, -1,
        //        -1, 9, -1,
        //        -1, -1, -1 };
        //    for (int x = 1; x < bmpTool.Width - 1; x++)
        //        for (int y = 1; y < bmpTool.Height - 1; y++)
        //        {
        //            int r = 0, g = 0, b = 0;
        //            int Index = 0;
        //            for (int col = -1; col <= 1; col++)
        //                for (int row = -1; row <= 1; row++)
        //                {
        //                    int argb = bmpTool.GetPixel(x + row, y + col);
        //                    r += (byte)(argb >> 16) * Laplacian[Index];
        //                    g += (byte)(argb >> 8) * Laplacian[Index];
        //                    b += (byte)(argb) * Laplacian[Index];
        //                    Index++;
        //                }
        //            //處理顏色值溢出
        //            r = r > 255 ? 255 : r;
        //            r = r < 0 ? 0 : r;
        //            g = g > 255 ? 255 : g;
        //            g = g < 0 ? 0 : g;
        //            b = b > 255 ? 255 : b;
        //            b = b < 0 ? 0 : b;
        //            newBitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
        //        }
        //    pictureBox.Image = newBitmap;
        //}
        #endregion
        #endregion

        public Main()
        {
            isHideControlBox = true;
            InitializeComponent();
            pictureBox.MouseWheel += PictureBox_MouseWheel;
            #region InitFilterBox
            toolStripFilterBox.Items.Add("None");
            foreach (BitmapFilter.Filter filterEnum in (BitmapFilter.Filter[])Enum.GetValues(typeof(BitmapFilter.Filter)))
            {
                toolStripFilterBox.Items.Add(filterEnum);
            }
            foreach (BitmapFilter.FilterXY filterEnum in (BitmapFilter.FilterXY[])Enum.GetValues(typeof(BitmapFilter.FilterXY)))
            {
                toolStripFilterBox.Items.Add(filterEnum);
            }
            toolStripFilterBox.SelectedIndexChanged += ToolStripFilterBox_SelectedIndexChanged;
            #endregion

        }

        #region NewAnalysisWorker
        /// <summary>
        /// Invoke(new MethodInvoker(() => {fileListView.BeginUpdate();}));
        /// </summary>
        private void NewAnalysis_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int IntUDAnalysisTaskThreadLimit = (int)Properties.Settings.Default["IntUDAnalysisTaskThreadLimit"];
            Stopwatch tickerMajor = Stopwatch.StartNew();
            Stopwatch tickerMinor = null;
            SemaphoreSlim semaphore = new SemaphoreSlim(IntUDAnalysisTaskThreadLimit);

            ToolMsg.Text = string.Format("Analysis {0} Image, start...", PageDatas.Count);
            newAnalysisWorker.ReportProgress(0);
            #region AnalysisImage
            Task<(string, TimeSpan)>[] tasks = new Task<(string, TimeSpan)>[PageDatas.Count];
            float calIdx = 0;
            counts = (new ConcurrentDictionary<float, int>(), new ConcurrentDictionary<float, int>(), 
                new ConcurrentDictionary<float, int>(), new ConcurrentDictionary<float, int>(), 
                new ConcurrentDictionary<float, int>(), new ConcurrentDictionary<float, int>());
            for (int idx = 0; idx < PageDatas.Count; ++idx)
            {
                if (newAnalysisWorker.CancellationPending) break;

                PageData pageData = PageDatas[idx];
                FileStream fs = null;
                Image pageImg = null;
                tasks[idx] = Task.Run<(string, TimeSpan)>(() =>
                {
                    try
                    {
                        semaphore.Wait();
                        tickerMinor = Stopwatch.StartNew();
                        fs = File.OpenRead(pageData.path + @"\" + pageData.name);
                        pageImg = Image.FromStream(fs);

                        if (ImageTool.AnalysisPointStates(new Bitmap(pageImg), pageData))
                        {
                            ImageTool.AnalysisPageY(pageData);
                            if (pageData.xStatesHead != null) ImageTool.AnalysisPageX(pageData.rectHead, pageData.xStatesHead, out pageData.columnHeadList);
                            if (pageData.xStatesBody != null) ImageTool.AnalysisPageX(pageData.rectBody, pageData.xStatesBody, out pageData.columnBodyList);
                            if (pageData.xStatesFooter != null) ImageTool.AnalysisPageX(pageData.rectFooter, pageData.xStatesFooter, out pageData.columnFooterList);
                            if (pageData.columnBodyList != null) ImageTool.AnalysisColumnRects(pageData.pStates, pageData.rectBody, pageData.columnBodyList, counts);
                        }

                        newAnalysisWorker.ReportProgress((int)((++calIdx / (float)fileListView.Items.Count) * 70),
                            new Tuple<string, float, int, TimeSpan, TimeSpan>(pageData.name, calIdx, PageDatas.Count, tickerMinor.Elapsed, tickerMajor.Elapsed));
                        return (pageData.name, tickerMinor.Elapsed);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        throw;
                    }
                    finally
                    {
                        if (pageImg != null) pageImg.Dispose();
                        if (fs != null) fs.Dispose();
                        semaphore.Release();
                    }
                });
            }
            Task<(string, TimeSpan)[]> whenTasks = Task.WhenAll(tasks);
            whenTasks.Wait();
            #endregion

            #region CalculateModes
            RefreshModes();
            #endregion

            #region AnalysisEntity
            semaphore = new SemaphoreSlim(1); //<= For TEST
            tasks = new Task<(string, TimeSpan)>[PageDatas.Count];
            calIdx = 0;
            for (int idx = 0; idx < PageDatas.Count; ++idx)
            {
                if (newAnalysisWorker.CancellationPending) break;

                PageData pageData = PageDatas[idx];
                tasks[idx] = Task.Run<(string, TimeSpan)>(() =>
                {
                    try
                    {
                        semaphore.Wait();
                        tickerMinor = Stopwatch.StartNew();
                        if (!pageData.isIllustration && pageData.columnBodyList != null)
                        {
                            ImageTool.AnalysisEntityHeighWidth(pageData.pStates, pageData.columnBodyList, modes);
                            ImageTool.AnalysisEntityHeadBodyEnd(pageData.rectBody, pageData.columnBodyList, modes);
                        }
                        newAnalysisWorker.ReportProgress((int)((++calIdx / (float)fileListView.Items.Count) * 30 + 70),
                            new Tuple<string, float, int, TimeSpan, TimeSpan>(pageData.name, calIdx, PageDatas.Count, tickerMinor.Elapsed, tickerMajor.Elapsed));
                        return (pageData.name, tickerMinor.Elapsed);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        throw;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
            }
            whenTasks = Task.WhenAll(tasks);
            whenTasks.Wait();
            #endregion
        }

        private void NewAnalysis_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            toolProgressBar.Value = e.ProgressPercentage;
            if (e.UserState is Tuple<string, float, int, TimeSpan, TimeSpan> args)
                ToolMsg.Text = string.Format("Analysis {0} {1}/{2} Image, elapsed: {3}/{4}s.", 
                    args.Item1, args.Item2, args.Item3, args.Item4.TotalSeconds.ToString(), args.Item5.TotalSeconds);
        }

        private void NewAnalysis_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            for (int idx = 0; idx < PageDatas.Count; ++idx) if (PageDatas[idx].isIllustration) fileListView.Items[idx].Checked = false;
            fileListView.EndUpdate();
            toolProgressBar.Value = toolProgressBar.Maximum;
        }

        private void RefreshModes()
        {
            float FloatUDEntityMinRate = (float)Properties.Settings.Default["FloatUDEntityMinRate"];
            float FloatUDEntityMaxRate = (float)Properties.Settings.Default["FloatUDEntityMaxRate"];
            modes =
                (ImageTool.GetModeMostOftenLen(counts.TopDict), ImageTool.GetModeMostOftenLen(counts.BottomDict),
                ImageTool.GetModeMostOftenLen(counts.LeftDict), ImageTool.GetModeMostOftenLen(counts.RightDict),
                ImageTool.GetModeMostOftenLen(counts.WidthDict), ImageTool.GetModeMostOftenLen(counts.HeightDict), 0, 0, 0, 0, 0, 0, 0, 0);
            modes.WidthMin = modes.Width * FloatUDEntityMinRate;
            modes.WidthMax = modes.Width * FloatUDEntityMaxRate;
            modes.HeighMin = modes.Heigh * FloatUDEntityMinRate;
            modes.HeighMax = modes.Heigh * FloatUDEntityMaxRate;
            modes.TopMin = (float)(modes.Top + modes.HeighMin * 0.5);
            modes.BottomMin = (float)(modes.Bottom - modes.HeighMin * 0.5);
            modes.LeftMin = (float)(modes.Left + modes.WidthMin * 0.5);
            modes.RightMin = (float)(modes.Right - modes.WidthMin * 0.5);
        }
        #endregion
    }
}
