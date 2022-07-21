using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
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
using System.IO.Compression;
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
        private const string filterAozoraExts = "*.TXT";

        private string inputDir;
        private (ConcurrentDictionary<float, int> TopDict, ConcurrentDictionary<float, int> BottomDict,
                ConcurrentDictionary<float, int> LeftDict, ConcurrentDictionary<float, int> RightDict,
                ConcurrentDictionary<float, int> WidthDict, ConcurrentDictionary<float, int> HeightDict) counts;
        private (float Top, float Bottom, float Left, float Right, float Width, float Heigh,
                float TopMin, float BottomMin, float LeftMin, float RightMin,
                float WidthMin, float WidthMax, float HeighMin, float HeighMax) modes;
        private readonly List<PageData> pageDatas = new List<PageData>();
        private Point MouseDownLocation;
        private Bitmap sourceImg;
        private Option option;
        private GenerateView generateView;
        private GenerateView generateWeb;
        private float zoomFactor = 1;
        private (string name, double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias) filter;

        public List<PageData> PageDatas => pageDatas;
        public float ZoomFactor { 
            get => zoomFactor;
            set
            {
                zoomFactor = value;
                ToolStripZoomFactorBox.SelectedItem = (value * 100).ToString();
            }
        }
        public (float Top, float Bottom, float Left, float Right, float Width, float Heigh,
            float TopMin, float BottomMin, float LeftMin, float RightMin,
            float WidthMin, float WidthMax, float HeighMin, float HeighMax) Modes => modes;
        public (string name, double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias) Filter => filter;
        public string InputDir => inputDir;
        public (ConcurrentDictionary<float, int> TopDict, ConcurrentDictionary<float, int> BottomDict, 
            ConcurrentDictionary<float, int> LeftDict, ConcurrentDictionary<float, int> RightDict, 
            ConcurrentDictionary<float, int> WidthDict, ConcurrentDictionary<float, int> HeightDict) Counts { get => counts; set => counts = value; }

        #region Event

        #region PictureBox
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
                PicBox.Left += e.X - MouseDownLocation.X;
                PicBox.Top += e.Y - MouseDownLocation.Y;
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
        /// Zoom PictureBox's image
        /// </summary>
        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control || !(sender is PictureBox pBox) || pBox.Image == null) return;

            ((HandledMouseEventArgs)e).Handled = true;
            if (e.Delta >= 0) ZoomFactor += 0.1F;
            else ZoomFactor -= 0.1F;

            pBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
        }
        private void PictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!(sender is PictureBox pBox) || pBox.Image == null) return;

            switch (e.KeyCode)
            {
                case Keys.Add:
                case Keys.Oemplus:
                    ZoomFactor += 0.1F;
                    break;
                case Keys.Subtract:
                case Keys.OemMinus:
                    ZoomFactor -= 0.1F;
                    break;
            }
            if (zoomFactor != 1) pBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
        }
        private void ToolStripZoomIn_Click(object sender, EventArgs e)
        {
            ZoomFactor += 0.1F;
            PicBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
        }

        private void ToolStripZoomOut_Click(object sender, EventArgs e)
        {
            ZoomFactor -= 0.1F;
            PicBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
        }
        private void ToolStripZoomFactorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is ToolStripComboBox comboBox)) return;

            ZoomFactor = float.Parse(comboBox.SelectedItem.ToString()) / 100;
            if (sourceImg == null) return;

            PicBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
        }
        private void ToolStripZoomFactorBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!(sender is ToolStripComboBox comboBox)) return;

            if (!Regex.IsMatch(((char)e.KeyValue).ToString(), "[0-9]") && comboBox.Text.Length > 0) comboBox.Text.Remove(comboBox.Text.Length - 1);
            if (e.KeyData != Keys.Enter) return;

            ZoomFactor = float.Parse(comboBox.Text) / 100;
            if (sourceImg == null) return;

            PicBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
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

        #region ToolStrip
        private void ToolStripOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter =
                $"All files (*.*)|*.*|" +
                $"Zip Files({filterZipExts})|{filterZipExts}|" +
                $"Epub Files({filterEpubExts})|{filterEpubExts}|" +
                $"Aozora Files({filterAozoraExts})|{filterAozoraExts}|" +
                $"Image Files({filterImageExts})|{filterImageExts}";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            try
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                string fileDirName = Path.GetDirectoryName(openFileDialog.FileName);
                string fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                string fileExt = Path.GetExtension(openFileDialog.FileName).ToUpper();
                if (PicBox.Image != null)
                {
                    PicBox.Image.Dispose();
                    PicBox.Image = null;
                }
                #region UNZIP
                if (filterEpubExts.IndexOf(fileExt) != -1 || filterZipExts.IndexOf(fileExt) != -1)
                {
                    fileDirName = fileDirName + Path.DirectorySeparatorChar + fileName;
                    Directory.CreateDirectory(fileDirName);
                    using (IArchive archive = ArchiveFactory.Open(openFileDialog.FileName))
                    using (IReader reader = archive.ExtractAllEntries())
                    {
                        while (reader.MoveToNextEntry())
                        {
                            if (!reader.Entry.IsDirectory)
                                reader.WriteEntryToDirectory(fileDirName, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true, PreserveFileTime = true });
                        }
                    }
                    //using (FileStream zipToOpen = new FileStream(openFileDialog.FileName, FileMode.Open))
                    //using (System.IO.Compression.ZipArchive archive = new System.IO.Compression.ZipArchive(zipToOpen))
                    //{
                    //    foreach (System.IO.Compression.ZipArchiveEntry entry in archive.Entries)
                    //    {
                    //        Stream s = entry.Open();
                    //        var sr = new StreamReader(s);
                    //        var myStr = sr.ReadToEnd();
                    //    }
                    //}
                }
                #endregion
                #region Parse Epub
                if (filterEpubExts.IndexOf(fileExt) != -1)
                {
                    string fullPath = "";
                    string rootPath = "";
                    string title = "";
                    string publisher = "";
                    List<string> creators = new List<string>();
                    List<string> itemrefs = new List<string>();
                    Dictionary<string, string> xhtmls = new Dictionary<string, string>();
                    //Dictionary<string, string> images = new Dictionary<string, string>();
                    using (XmlTextReader reader = new XmlTextReader($"{fileDirName}/META-INF/container.xml"))
                    {
                        XmlTextReader readerOpt = null;
                        while (reader.Read())
                        {
                            if (reader.Name != "rootfile") continue;

                            fullPath = reader.GetAttribute("full-path");
                            rootPath = Path.GetDirectoryName($"{fileDirName}/{fullPath}");
                            readerOpt = new XmlTextReader($"{fileDirName}/{fullPath}");
                            break;
                        }
                        while (readerOpt.Read())
                        {
                            if (readerOpt.NodeType != XmlNodeType.Element) continue;

                            if (readerOpt.LocalName == "title") title = readerOpt.ReadString();
                            else if (readerOpt.LocalName == "creator") creators.Add(readerOpt.ReadString());
                            else if (readerOpt.LocalName == "publisher") publisher = readerOpt.ReadString();
                            else if (readerOpt.LocalName == "item")
                            {
                                //if (readerOpt.GetAttribute("media-type").StartsWith("image")) images.Add(readerOpt.GetAttribute("id"), readerOpt.GetAttribute("href"));
                                if (readerOpt.GetAttribute("media-type") == "application/xhtml+xml") xhtmls.Add(readerOpt.GetAttribute("id"), readerOpt.GetAttribute("href"));
                            }
                            else if (readerOpt.LocalName == "itemref") itemrefs.Add(readerOpt.GetAttribute("idref"));
                        }
                    }
                    FileListView.BeginUpdate();
                    FileListView.Items.Clear();
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
                        for (int xmlIdx = xmlTextList.Count -1; xmlIdx >= 0; xmlIdx--)
                        {
                            var text = xmlTextList[xmlIdx].text;
                            if (text != "\n") break;
                            xmlTextList.RemoveAt(xmlIdx);
                        }
                        if (xmlTextList.Count == 0) continue;

                        xmlTextList.Add(("_pagebreak_", ""));

                        int itemIdx = FileListView.Items.Count;
                        FileListView.Items.Add(itemIdx.ToString(), rootPath, 0);
                        FileListView.Items[itemIdx].Checked = true;
                        FileListView.Items[itemIdx].SubItems.Add(xhtml);
                        FileListView.Items[itemIdx].SubItems.Add(rootPath);
                        PageData pageData = new PageData(itemIdx, rootPath, xhtml, xhtmlExt);
                        pageData.textList = xmlTextList;
                        PageDatas.Add(pageData);
                    }
                    FileListView.EndUpdate();
                    //XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    //nsmgr.AddNamespace("ns", xmlDoc.DocumentElement.NamespaceURI);
                    //XmlNodeList tt = xmlDoc.SelectNodes("ns:br", nsmgr);
                    //xmlDoc.SelectNodes("//ns:img", nsmgr);
                    //xmlDoc.SelectNodes("ns:span", nsmgr);
                }
                #endregion
                #region Parse Image or Aozora
                else
                {
                    string[] fileEntries = Directory.GetFiles(fileDirName);
                    if (PicBox.Image != null)
                    {
                        PicBox.Image.Dispose();
                        PicBox.Image = null;
                    }
                    FileListView.BeginUpdate();
                    FileListView.Items.Clear();
                    PageDatas.Clear();
                    sourceImg = null;
                    foreach (string filePath in fileEntries)
                    {
                        DirectoryInfo file = new DirectoryInfo(filePath);
                        if (filterAozoraExts.IndexOf(file.Extension.ToUpper()) != -1)
                        {
                            ParseAozoraText(filePath, file);
                            return;
                        }
                        else if (!Regex.IsMatch(filterImageExts, file.Extension.ToUpper())) continue;

                        int itemIdx = FileListView.Items.Count;
                        if (itemIdx == 0) inputDir = file.Parent.FullName;
                        FileListView.Items.Add(itemIdx.ToString(), filePath, 0);
                        FileListView.Items[itemIdx].Checked = true;
                        FileListView.Items[itemIdx].SubItems.Add(file.Name);
                        FileListView.Items[itemIdx].SubItems.Add(file.Parent.FullName);
                        PageDatas.Add(new PageData(itemIdx, file.Parent.FullName, file.Name, file.Extension.ToUpper()));
                    }

                    newAnalysisWorker.RunWorkerAsync();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show("Open file failed, " + ex.Message + "\n" + ex.StackTrace, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ToolStripOptions_Click(object sender, EventArgs e)
        {
            if (option == null || option.IsDisposed) option = new Option();
            option.StartPosition = FormStartPosition.CenterParent;
            option.TopLevel = true;
            option.Show();
        }
        private void ToolStripGenView_Click(object sender, EventArgs e)
        {
            if (PageDatas.Count == 0) return;
            if (generateView == null || generateView.IsDisposed) generateView = new GenerateView(this);
            generateView.StartPosition = FormStartPosition.CenterParent;
            generateView.TopMost = true;
            generateView.Show();
        }
        #endregion

        #region FileListView
        private void FileListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem item = e.Item;

            var pageData = PageDatas.Count > item.Index ? PageDatas[item.Index] : null;

            if (item.Checked)
            {
                item.BackColor = Color.SteelBlue;

                if (newAnalysisWorker.IsBusy || ToolProgressBar.Value != ToolProgressBar.Maximum || PageDatas.Count <= item.Index) return;

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
                item.BackColor = Color.CadetBlue;
                if (pageData != null) pageData.isIllustration = true;
            }
        }
        private void FileListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) return;

            FileStream fs = null;
            ListViewItem item = e.Item;

            string name = item.SubItems[1].Text;
            var pageData = PageDatas[item.Index];

            if (pageData.textList != null)
            {
                if (generateWeb == null || generateWeb.IsDisposed) generateWeb = new GenerateView(this, true);
                generateWeb.StartPosition = FormStartPosition.CenterParent;
                generateWeb.TopMost = true;
                generateWeb.Show();
                return;
            }

            int RectViewWidth = Properties.Settings.Default.RectViewWidth.Value;
            RectType[] rTypes = new RectType[] { //RectType.Head, RectType.Body, RectType.Footer, 
                RectType.EntityBody, RectType.Ruby,
                RectType.MergeTB, RectType.MergeLR,
                RectType.SplitTop, RectType.SplitMiddle, RectType.SplitBottom};
            Dictionary<RectType, (Color color, Pen pen, List<RectangleF> rects)> drawRectObj = new Dictionary<RectType, (Color color, Pen pen, List<RectangleF> rects)>();
            drawRectObj.Add(RectType.None, (Color.Black, new Pen(Color.Black, RectViewWidth), new List<RectangleF>()));
            for (int idx = 0; idx < rTypes.Length; idx++)
            {
                var rType = rTypes[idx];
                Color color = Color.FromName(Properties.Settings.Default["Rect" + rType.ToString() + "Color"].ToString());
                Pen pen = new Pen(color, RectViewWidth);
                List<RectangleF> rects = new List<RectangleF>();
                drawRectObj.Add(rType, (color, pen, rects));
            }

            Color RectHeadColor = Color.FromKnownColor(Properties.Settings.Default.RectHeadColor.Value);
            Color RectBodyColor = Color.FromKnownColor(Properties.Settings.Default.RectBodyColor.Value);
            Color RectFooterColor = Color.FromKnownColor(Properties.Settings.Default.RectFooterColor.Value);
            Color RectColumnColor = Color.FromKnownColor(Properties.Settings.Default.RectColumnColor.Value);
            Color RectColumnRubyColor = Color.FromKnownColor(Properties.Settings.Default.RectColumnRubyColor.Value);

            Pen penHead = new Pen(RectHeadColor, RectViewWidth);
            Pen penBody = new Pen(RectBodyColor, RectViewWidth);
            Pen penFooter = new Pen(RectFooterColor, RectViewWidth);
            Pen penColumn = new Pen(RectColumnColor, RectViewWidth);
            Pen penColumnRuby = new Pen(RectColumnRubyColor, RectViewWidth);
            try
            {
                if (PicBox.Image != null) PicBox.Image.Dispose();

                fs = File.OpenRead(item.Text);
                Bitmap img = (Bitmap)Image.FromStream(fs);
                //https://www.c-sharpcorner.com/article/solution-for-a-graphics-object-cannot-be-created-from-an-im/
                if (img.PixelFormat == PixelFormat.Undefined || img.PixelFormat == PixelFormat.DontCare || img.PixelFormat == PixelFormat.Format1bppIndexed ||
                    img.PixelFormat == PixelFormat.Format4bppIndexed || img.PixelFormat == PixelFormat.Format8bppIndexed ||
                    img.PixelFormat == PixelFormat.Format16bppGrayScale || img.PixelFormat == PixelFormat.Format16bppArgb1555) img = new Bitmap(img);

                PicBox.Image = img;
                PicBox.Location = Point.Empty;
                sourceImg = (Bitmap)PicBox.Image;
                sourceImg.Tag = item.Index;
                using (Graphics gr = Graphics.FromImage(PicBox.Image))
                {
                    gr.SmoothingMode = SmoothingMode.AntiAlias;
                    gr.DrawRectangles(penHead, new RectangleF[] { ImageTool.EntityToRectangleF(pageData.rectHead) });
                    gr.DrawRectangles(penBody, new RectangleF[] { ImageTool.EntityToRectangleF(pageData.rectBody) });
                    gr.DrawRectangles(penFooter, new RectangleF[] { ImageTool.EntityToRectangleF(pageData.rectFooter) });
                    DrawRectangles(gr, rTypes, pageData.columnBodyList, drawRectObj, penColumn, penColumnRuby);
                }

                if (zoomFactor != 1) PicBox.Image = PictureBox_Zoom(sourceImg, zoomFactor);
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
        private void FileListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            Rectangle rect = e.Bounds;
            using (Brush brush = new SolidBrush(BackColor)) e.Graphics.FillRectangle(brush, rect); //Brushes.DarkGray
            e.Graphics.DrawRectangle(SystemPens.ControlLight, Rectangle.Inflate(rect, 0, -1));

            Rectangle rectangle = Rectangle.Inflate(rect, -4, 0);
            TextRenderer.DrawText(e.Graphics, e.Header.Text, e.Font, rectangle, ForeColor, TextFormatFlags.VerticalCenter);
        }
        private void FileListView_DrawItem(object sender, DrawListViewItemEventArgs e) => e.DrawDefault = true;
        private void FileListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e) => e.DrawDefault = true;
        #endregion

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
                    PicBox.Image = BitmapFilter.ConvolutionFilter(bitMap, filter.xFilterMatrix, filter.factor, filter.bias, true); //, (int)(573 * zoomFactor), (int)(110 * zoomFactor), (int)(17 * zoomFactor), (int)(32 * zoomFactor) //convolutionfilter(filtermatrix, factor, bias, true);
                    if (Regex.IsMatch(filterEnum.ToString(), "Gaussian3x3|Gaussian5x5Type1|Gaussian5x5Type2"))
                    {
                        (double[,] filterMatrix2, double factor2, int bias2) = BitmapFilter.Filters[BitmapFilter.Filter.Laplacian5x5Type1];
                        PicBox.Image = BitmapFilter.ConvolutionFilter((Bitmap)PicBox.Image, filterMatrix2, factor2, bias2, true);
                    }
                }
                else if (comboBox.SelectedItem is BitmapFilter.FilterXY filterXYEnum)
                {
                    filter.name = filterXYEnum.ToString();
                    (filter.xFilterMatrix, filter.yFilterMatrix, filter.factor, filter.bias) = BitmapFilter.FilterXYs[filterXYEnum]; //(double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias) = BitmapFilter.FilterXYs[filterXYEnum];
                    Bitmap bitMap = zoomFactor != 1 ? PictureBox_Zoom(sourceImg, zoomFactor) : sourceImg;
                    PicBox.Image = BitmapFilter.ConvolutionFilter(bitMap, filter.xFilterMatrix, filter.yFilterMatrix, filter.factor, filter.bias, true); //ConvolutionFilter(xFilterMatrix, yFilterMatrix, factor, bias, true);
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

        private void ParseAozoraText(string filePath, DirectoryInfo file)
        {
            bool isIndent = true;
            PageDatas.Clear();
            FileListView.Items.Clear();
            List<(string text, string ruby)> aozoras = new List<(string text, string ruby)>();

            string rawText = File.ReadAllText(filePath);
            if (isIndent) rawText = rawText.Replace("\r\n", "\n").Replace("\n\n", "\n");
            string[] texts = rawText.Split('\n');

            for (int idx = 0; idx < texts.Length; idx++)
            {
                string text = texts[idx];
                if (Regex.Match(text, "［＃[^（]*（([^）]+)）[^］]+］(.*)") is Match m1 && m1.Success)
                { //［＃挿絵（0.jpg）入る］ or ［＃（1.jpg）入る］
                    aozoras.Add(("_img_", file.Parent.FullName + "/" + m1.Groups[1].Value));
                    text = m1.Groups[2].Value;
                }
                else if (Regex.Match(text, "［＃(改頁|改ページ)］(.*)") is Match m2 && m2.Success)
                { //［＃改頁］
                    aozoras.Add(("_pagebreak_", ""));
                    text = m2.Groups[2].Value;
                }
                else if (Regex.Match(text, "［＃地付き］(.*)") is Match m3 && m3.Success)
                { //［＃地付き］
                    aozoras.Add(("", ""));
                    text = m3.Groups[2].Value;
                }
                else if (Regex.Match(text, "［＃太字］([^［]+)［＃太字終わり］") is Match m4 && m4.Success)
                { //［＃太字］ＯＯＯＸＸ［＃太字終わり］
                    aozoras.Add(("_bold_", m4.Groups[1].Value));
                    text = m4.Groups[2].Value;
                }
                else if (text.Trim() == "") aozoras.Add(("\n", ""));
                //［＃改丁］
                //［＃「腹がへっても」に傍点］
                //［＃「傍線」に傍線］
                //［＃「選考」は太字］

                while (text.Trim().Length > 0)
                {
                    if (Regex.Match(text, "([^ -/§-〃〆-ヶ！-＠]+[ぁ-ヶ]*)《([^》]+)》(.*)") is Match m6 && m6.Success) //狩人《かりゆうど》
                    {
                        if (m6.Groups[1].Index > 0) aozoras.Add((text.Substring(0, m6.Groups[1].Index), ""));
                        aozoras.Add((m6.Groups[1].Value, m6.Groups[2].Value));
                        if (m6.Groups[3].Length > 0) text = m6.Groups[3].Value;
                    }
                    else
                    {
                        aozoras.Add((text + "\n", ""));
                        text = "";
                    }
                }
            }

            int itemIdx = FileListView.Items.Count;
            FileListView.Items.Add(itemIdx.ToString(), filePath, 0);
            FileListView.Items[itemIdx].Checked = true;
            FileListView.Items[itemIdx].SubItems.Add(file.Name);
            FileListView.Items[itemIdx].SubItems.Add(file.Parent.FullName);
            PageData pageData = new PageData(itemIdx, file.Parent.FullName, file.Name, file.Extension.ToUpper());
            pageData.textList = aozoras;
            PageDatas.Add(pageData);

            FileListView.EndUpdate();
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

        public void SetToolStripFilterBox(object item) => ToolStripFilterBox.SelectedItem = item;

        #endregion

        public Main()
        {
            InitializeComponent();
            Text += " " + Application.ProductVersion;
            PicBox.MouseWheel += PictureBox_MouseWheel;

            #region InitFilterBox
            ToolStripFilterBox.Items.Add("None");
            foreach (BitmapFilter.Filter filterEnum in (BitmapFilter.Filter[])Enum.GetValues(typeof(BitmapFilter.Filter))) ToolStripFilterBox.Items.Add(filterEnum);
            foreach (BitmapFilter.FilterXY filterEnum in (BitmapFilter.FilterXY[])Enum.GetValues(typeof(BitmapFilter.FilterXY))) ToolStripFilterBox.Items.Add(filterEnum);
            ToolStripFilterBox.SelectedIndexChanged += ToolStripFilterBox_SelectedIndexChanged;
            #endregion
        }

        #region NewAnalysisWorker
        /// <summary>
        /// Invoke(new MethodInvoker(() => {fileListView.BeginUpdate();}));
        /// </summary>
        private void NewAnalysis_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int AnalysisTaskThreadLimit = Properties.Settings.Default.AnalysisTaskThreadLimit.Value;
            Stopwatch tickerMajor = Stopwatch.StartNew();
            Stopwatch tickerMinor = null;
            SemaphoreSlim semaphore = new SemaphoreSlim(AnalysisTaskThreadLimit);
            Invoke(new MethodInvoker(() => { ToolMsg.Text = string.Format("Analysis {0} Image, start...", PageDatas.Count); }));
            
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

                        newAnalysisWorker.ReportProgress((int)((++calIdx / (float)FileListView.Items.Count) * 70),
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
                        newAnalysisWorker.ReportProgress((int)((++calIdx / (float)FileListView.Items.Count) * 30 + 70),
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
            ToolProgressBar.Value = e.ProgressPercentage;
            if (e.UserState is Tuple<string, float, int, TimeSpan, TimeSpan> args)
                ToolMsg.Text = string.Format("Analysis {0} {1}/{2} Image, elapsed: {3}/{4}s.", 
                    args.Item1, args.Item2, args.Item3, args.Item4.TotalSeconds.ToString(), args.Item5.TotalSeconds);
        }

        private void NewAnalysis_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            for (int idx = 0; idx < PageDatas.Count; ++idx) if (PageDatas[idx].isIllustration) FileListView.Items[idx].Checked = false;

            FileListView.EndUpdate();
            ToolProgressBar.Value = ToolProgressBar.Maximum;
        }

        private void RefreshModes()
        {
            float EntityMinRate = Properties.Settings.Default.EntityMinRate.Value;
            float EntityMaxRate = Properties.Settings.Default.EntityMaxRate.Value;
            modes =
                (ImageTool.GetModeMostOftenLen(counts.TopDict), ImageTool.GetModeMostOftenLen(counts.BottomDict),
                ImageTool.GetModeMostOftenLen(counts.LeftDict), ImageTool.GetModeMostOftenLen(counts.RightDict),
                ImageTool.GetModeMostOftenLen(counts.WidthDict), ImageTool.GetModeMostOftenLen(counts.HeightDict), 0, 0, 0, 0, 0, 0, 0, 0);
            modes.WidthMin = modes.Width * EntityMinRate;
            modes.WidthMax = modes.Width * EntityMaxRate;
            modes.HeighMin = modes.Heigh * EntityMinRate;
            modes.HeighMax = modes.Heigh * EntityMaxRate;
            modes.TopMin = (float)(modes.Top + modes.HeighMin * 0.5);
            modes.BottomMin = (float)(modes.Bottom - modes.HeighMin * 0.5);
            modes.LeftMin = (float)(modes.Left + modes.WidthMin * 0.5);
            modes.RightMin = (float)(modes.Right - modes.WidthMin * 0.5);
        }
        #endregion
    }
}
