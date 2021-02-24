using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NovelTool
{
    /// <summary>
    /// No border Form and Resizable
    /// </summary>
    public partial class Main
    {
        protected bool isHideControlBox;
        protected NCCALCSIZE_PARAMS nccsp;
        protected WINDOWPOS wPos;
        protected MINMAXINFO mmInfo;
        protected DateTime mouseDownTime;
        protected bool debugLog;

        /// <summary>
        /// Simulate the move form and zoom function of the titlebar 
        /// </summary>
        protected void MouseDownEvent(object sender, MouseEventArgs e)
        {
            double elapsed = 0;
            if (e.Button == MouseButtons.Left)
            {
                if (mouseDownTime == DateTime.MinValue) mouseDownTime = DateTime.Now;
                else
                {
                    elapsed = ((TimeSpan)(DateTime.Now - mouseDownTime)).TotalSeconds;
                    if (elapsed > 0.5)
                    {
                        elapsed = 0;
                        mouseDownTime = DateTime.Now;
                    }
                    else mouseDownTime = DateTime.MinValue;
                }
                if (elapsed == 0 || elapsed >= 0.5)
                {
                    ReleaseCapture();
                    SendMessage(Handle, (int)WinMessages.WM_NCLBUTTONDOWN, (int)WM_NCHITTEST_return.HT_CAPTION, 0);
                }
                else
                {
                    if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
                    else WindowState = FormWindowState.Maximized;
                }
            }
        }

        public void ChangeTheme(Control.ControlCollection container, bool isDefaultTheme)
        {
            foreach (Control component in container)
            {
                if (component.GetType().GetMember("Controls") != null)
                {
                    ChangeTheme(component.Controls, isDefaultTheme);
                }
                if (component is Panel)
                {
                    if (isDefaultTheme) //Color.DimGray)
                    {
                        component.BackColor = SystemColors.Control;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackPanel"]; //Color.DimGray;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForePanel"]; //SystemColors.Control;
                    }
                }
                else if (component is SplitContainer)
                {
                    if (isDefaultTheme) //component.BackColor == Color.Silver)
                    {
                        component.BackColor = SystemColors.Control;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackSplitContainer"]; //Color.Silver;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxBackSplitContainer"]; //SystemColors.Control;
                    }
                }
                else if (component is MenuStrip strip)
                {
                    foreach (ToolStripMenuItem stripItem in strip.Items)
                    {
                        ChangeTheme(stripItem.DropDownItems, isDefaultTheme);
                    }
                    if (isDefaultTheme) //component.BackColor == Color.SteelBlue)
                    {
                        component.BackColor = SystemColors.ActiveCaption;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackMenuStrip"]; //Color.SteelBlue;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeMenuStrip"]; //SystemColors.ControlText;
                    }
                }
                else if (component is ToolStripContainer)
                {
                }
                else if (component is ToolStripPanel)
                {
                    if (isDefaultTheme) //component.BackColor == Color.Silver)
                    {
                        component.BackColor = SystemColors.Control;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackToolStripPanel"]; //Color.Silver;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeToolStripPanel"]; //SystemColors.Control;
                    }
                }
                else if (component is ToolStrip)
                {
                    if (isDefaultTheme) //component.BackColor == Color.DarkGray)
                    {
                        component.BackColor = SystemColors.Control;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackToolStrip"]; //Color.DarkGray;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeToolStrip"]; //SystemColors.ControlText;
                    }
                }
                else if (component is ListView)
                {
                    if (isDefaultTheme) //component.BackColor == Color.LightGray)
                    {
                        component.BackColor = SystemColors.Window;
                        component.ForeColor = SystemColors.WindowText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackListView"]; //Color.LightGray;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeListView"]; //SystemColors.WindowText;
                    }
                }
                else if (component is PictureBox)
                {
                    if (isDefaultTheme)
                    {
                        component.BackColor = SystemColors.Control;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackPictureBox"];
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForePictureBox"];
                    }
                }
                else if (component is Button)
                {
                    if (isDefaultTheme)
                    {
                        component.BackColor = SystemColors.Control;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackButton"];
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeButton"];
                    }
                }
                else if (component is TextBox)
                {
                    if (isDefaultTheme) //component.BackColor == SystemColors.GrayText)
                    {
                        component.BackColor = SystemColors.Window;
                        component.ForeColor = SystemColors.WindowText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackTextBox"]; //SystemColors.GrayText;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeTextBox"]; //SystemColors.Window;
                    }
                }
                else if (component is ProgressBar)
                {
                    if (isDefaultTheme) //component.BackColor == Color.DarkGray)
                    {
                        component.BackColor = SystemColors.Control;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackProgressBar"]; //Color.DarkGray;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeProgressBar"]; //SystemColors.ControlText;
                    }
                }
                else if (component is TreeView)
                {
                    //if (isDefaultTheme) //component.BackColor == Color.DarkGray)
                    //{
                    //    component.BackColor = SystemColors.Control;
                    //    component.ForeColor = SystemColors.ControlText;
                    //}
                    //else
                    //{
                    //    component.BackColor = (Color)Properties.Settings.Default["TreeViewBackProgressBar"]; //Color.DarkGray;
                    //    component.ForeColor = (Color)Properties.Settings.Default["TreeViewForeProgressBar"]; //SystemColors.ControlText;
                    //}
                }
                else if (component is TabControl)
                {
                    //if (isDefaultTheme) //component.BackColor == Color.DarkGray)
                    //{
                    //    component.BackColor = SystemColors.Control;
                    //    component.ForeColor = SystemColors.ControlText;
                    //}
                    //else
                    //{
                    //    component.BackColor = (Color)Properties.Settings.Default["TabControlBackProgressBar"]; //Color.DarkGray;
                    //    component.ForeColor = (Color)Properties.Settings.Default["TabControlForeProgressBar"]; //SystemColors.ControlText;
                    //}
                }
                else if (component is Label)
                {
                    //if (isDefaultTheme) //component.BackColor == Color.DarkGray)
                    //{
                    //    component.BackColor = SystemColors.Control;
                    //    component.ForeColor = SystemColors.ControlText;
                    //}
                    //else
                    //{
                    //    component.BackColor = (Color)Properties.Settings.Default["LabelBackProgressBar"]; //Color.DarkGray;
                    //    component.ForeColor = (Color)Properties.Settings.Default["LabelForeProgressBar"]; //SystemColors.ControlText;
                    //}
                }
                else if (component is GroupBox)
                {
                    //if (isDefaultTheme) //component.BackColor == Color.DarkGray)
                    //{
                    //    component.BackColor = SystemColors.Control;
                    //    component.ForeColor = SystemColors.ControlText;
                    //}
                    //else
                    //{
                    //    component.BackColor = (Color)Properties.Settings.Default["GroupBoxBackProgressBar"]; //Color.DarkGray;
                    //    component.ForeColor = (Color)Properties.Settings.Default["GroupBoxForeProgressBar"]; //SystemColors.ControlText;
                    //}
                }
                else if (component is ComboBox)
                {
                    //if (isDefaultTheme) //component.BackColor == Color.DarkGray)
                    //{
                    //    component.BackColor = SystemColors.Control;
                    //    component.ForeColor = SystemColors.ControlText;
                    //}
                    //else
                    //{
                    //    component.BackColor = (Color)Properties.Settings.Default["ComboBoxBackProgressBar"]; //Color.DarkGray;
                    //    component.ForeColor = (Color)Properties.Settings.Default["ComboBoxForeProgressBar"]; //SystemColors.ControlText;
                    //}
                }
                else if (component is CheckBox)
                {
                    //if (isDefaultTheme) //component.BackColor == Color.DarkGray)
                    //{
                    //    component.BackColor = SystemColors.Control;
                    //    component.ForeColor = SystemColors.ControlText;
                    //}
                    //else
                    //{
                    //    component.BackColor = (Color)Properties.Settings.Default["ComboBoxBackProgressBar"]; //Color.DarkGray;
                    //    component.ForeColor = (Color)Properties.Settings.Default["ComboBoxForeProgressBar"]; //SystemColors.ControlText;
                    //}
                }
                else
                {
                    if (isDefaultTheme) //component.BackColor == Color.Azure)
                    {
                        component.BackColor = SystemColors.Control;
                        component.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackOther"]; //Color.Azure;
                        component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeOther"]; //SystemColors.ControlText;
                    }
                }
            }
        }
        private void ChangeTheme(ToolStripItemCollection container, bool isDefaultTheme)
        {
            foreach (ToolStripMenuItem component in container)
            {
                if (isDefaultTheme) //component.BackColor == Color.LightSlateGray)
                {
                    component.BackColor = SystemColors.Control;
                    component.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    component.BackColor = (Color)Properties.Settings.Default["ColorBoxBackToolStripMenuItem"]; //Color.LightSlateGray;
                    component.ForeColor = (Color)Properties.Settings.Default["ColorBoxForeToolStripMenuItem"]; //SystemColors.Control;
                }
            }
        }

        #region override Form methods
        protected override void OnLoad(EventArgs e)
        {
            if (isHideControlBox)
            {
                DoubleBuffered = true;
                //MinimizeBox = false;
                //MaximizeBox = false;
                //ShowIcon = false;
                //Text = "";
                //ControlBox = false;
                FormBorderStyle = FormBorderStyle.None;
            }
            base.OnLoad(e);
        }
        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);

        //    var g = e.Graphics;
        //    ControlPaint.DrawBorder(g,
        //        new Rectangle(ClientRectangle.Location, new Size(ClientRectangle.Width - 1, ClientRectangle.Height - 1)),
        //        Color.Crimson,
        //        Width,
        //        ButtonBorderStyle.Solid,
        //        Color.Crimson,
        //        Width,
        //        ButtonBorderStyle.Solid,
        //        Color.Crimson,
        //        Width,
        //        ButtonBorderStyle.Solid,
        //        Color.Crimson,
        //        Width,
        //        ButtonBorderStyle.Solid);

        //    using (var p = new Pen(Color.FromArgb(51, 51, 51)))
        //    {
        //        var modRect = new Rectangle(ClientRectangle.Location, new Size(ClientRectangle.Width - 1, ClientRectangle.Height - 1));
        //        g.DrawRectangle(p, modRect);
        //    }
        //}
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            Color left = (Color)Properties.Settings.Default["ColorBoxFormBorderLeft"];
            Color top = (Color)Properties.Settings.Default["ColorBoxFormBorderTop"];
            Color right = (Color)Properties.Settings.Default["ColorBoxFormBorderRight"];
            Color bottom = (Color)Properties.Settings.Default["ColorBoxFormBorderBottom"];
            bool paddingLeft = (bool)Properties.Settings.Default["CheckBoxFormBorderLeft"];
            bool paddingTop = (bool)Properties.Settings.Default["CheckBoxFormBorderTop"];
            bool paddingRight = (bool)Properties.Settings.Default["CheckBoxFormBorderRight"];
            bool paddingBottom = (bool)Properties.Settings.Default["CheckBoxFormBorderBottom"];
            int.TryParse(Properties.Settings.Default["IntUDFormBorderWidth"].ToString(), out int width);

            ControlPaint.DrawBorder(g,
                new Rectangle(ClientRectangle.Location, new Size(ClientRectangle.Width, ClientRectangle.Height)),
                left, width, ButtonBorderStyle.Solid,
                top, width, ButtonBorderStyle.Solid,
                right, width, ButtonBorderStyle.Solid,
                bottom, width, ButtonBorderStyle.Solid);

            Padding = new Padding(
                paddingLeft ? width : 0,
                paddingTop ? width : 0,
                paddingRight ? width : 0,
                paddingBottom ? width : 0);
        }
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            RedrawWindow(this.Handle, IntPtr.Zero, IntPtr.Zero, RedrawWindowFlags.Frame | RedrawWindowFlags.UpdateNow | RedrawWindowFlags.Invalidate);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (isHideControlBox)
                {
                    cp.Style |= (int)Styles.WS_MINIMIZEBOX | (int)Styles.WS_SYSMENU | (int)Styles.WS_BORDER | (int)Styles.WS_THICKFRAME;
                    cp.ClassStyle |= (int)ClassStyles.CS_DBLCLKS;
                    //cp.ExStyle |= (int)ExStyles.WS_EX_COMPOSITED | (int)ExStyles.WS_EX_WINDOWEDGE | (int)ExStyles.WS_EX_TOPMOST;
                }
                return cp;
            }
        }
        //protected bool chkNCACTIVATE;
        protected override void WndProc(ref Message m)
        {
            object lpObj = null;
            if (m.Msg == (int)WinMessages.WM_NCACTIVATE) // && (m.LParam.ToInt64() <= 0 || (chkNCACTIVATE && m.WParam.ToInt64() == 0 && m.LParam.ToInt64() > 0))) // && (this.Focus() == true || m.WParam.ToInt64() != 0 || m.LParam.ToInt64() == 0 )
            {
                //Console.WriteLine("　　　　WM_NCACTIVATE, Focus:" + this.Focus() + string.Format(", W:{0}, L:{1}", m.WParam.ToString("X"), m.LParam.ToString("X")));
                m.Result = (IntPtr)1;
                return; //&& m.WParam.ToInt32() == 1
            }

            if (m.Msg == (int)WinMessages.WM_NCCALCSIZE && isHideControlBox)
            {
                if (m.WParam != IntPtr.Zero)
                {
                    nccsp = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
                    lpObj = nccsp;
                    if (nccsp.rcNewWindow.Right - nccsp.rcNewWindow.Left != mmInfo.ptMaxSize.x || nccsp.rcNewWindow.Bottom - nccsp.rcNewWindow.Top != mmInfo.ptMaxSize.y) //Full screen does not adjust
                    {
                        int gap = 7;
                        nccsp.rcNewWindow.Top -= gap;
                        //nccsp.rcNewWindow.Bottom += gap;
                        //nccsp.rcNewWindow.Left -= gap;
                        //nccsp.rcNewWindow.Right += gap;
                        Marshal.StructureToPtr(nccsp, m.LParam, false);
                    }
                }
                else
                {
                    lpObj = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                }
            }
            else if (m.Msg == (int)WinMessages.WM_NCACTIVATE)
            {
                lpObj = string.Format("W:{0}, L:{1}", m.WParam.ToString("X"), m.LParam.ToString("X"));
            }
            else if (m.Msg == (int)WinMessages.WM_NCHITTEST)
            {
                lpObj = new Point(m.LParam.ToInt32());
                //if (m.Result == (IntPtr)WM_NCHITTEST_return.HT_CLIENT)
                //{
                //    Point p = this.PointToClient(new Point(m.LParam.ToInt32()));
                //    m.Result =
                //        (IntPtr)
                //        (p.X <= 6
                //             ? p.Y <= 6 ? 13 : p.Y >= this.Height - 7 ? 16 : 10
                //             : p.X >= this.Width - 7
                //                   ? p.Y <= 6 ? 14 : p.Y >= this.Height - 7 ? 17 : 11
                //                   : p.Y <= 6 ? 12 : p.Y >= this.Height - 7 ? 15 : p.Y <= 24 ? 2 : 1);
                //}
            }
            else if (m.Msg == (int)WinMessages.WM_NCMOUSEMOVE)
            {
                lpObj = new Point(m.LParam.ToInt32());
            }
            else if (m.Msg == (int)WinMessages.WM_NCLBUTTONDOWN)
            {
                lpObj = string.Format("hitTestResult:{0}, {1}", Enum.GetName(typeof(WM_NCHITTEST_return), m.WParam.ToInt32()), new Point(m.LParam.ToInt32()));
            }
            else if (m.Msg == (int)WinMessages.WM_LBUTTONDOWN)
            {
                lpObj = string.Format("key:{0}, {1}", Enum.GetName(typeof(WM_LBUTTONDOWN_wParam), m.WParam.ToInt32()), new Point(m.LParam.ToInt32()));
            }
            else if (m.Msg == (int)WinMessages.WM_PARENTNOTIFY)
            {
                int lWParam = (short)m.WParam;
                int hWParam = (short)((uint)m.WParam >> 16);
                if (lWParam == (int)WinMessages.WM_CREATE || lWParam == (int)WinMessages.WM_DESTROY)
                {
                    lpObj = string.Format("lWParam:{0}, hWParam:{1}, handle:{2}", Enum.GetName(typeof(WinMessages), lWParam), hWParam, m.LParam);
                }
                else
                {
                    lpObj = string.Format("lWParam:{0}, hWParam:{1}, P:{2}", Enum.GetName(typeof(WinMessages), lWParam), hWParam, new Point(m.LParam.ToInt32()));
                }
            }
            else if (m.Msg == (int)WinMessages.WM_MOUSEACTIVATE)
            {
                int lLParam = (short)m.LParam;
                int hLParam = (short)((uint)m.WParam >> 16);
                lpObj = string.Format("hitTestResult:{0}, message:{1}", Enum.GetName(typeof(WM_NCHITTEST_return), lLParam), Enum.GetName(typeof(WinMessages), hLParam));
            }
            else if (m.Msg == (int)WinMessages.WM_WINDOWPOSCHANGING)
            {
                wPos = (WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));
                lpObj = wPos;
            }
            else if (m.Msg == (int)WinMessages.WM_WINDOWPOSCHANGED)
            {
                wPos = (WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));
                lpObj = wPos;
            }
            else if (m.Msg == (int)WinMessages.WM_GETMINMAXINFO)
            {
                mmInfo = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
                lpObj = mmInfo;
            }
            else if (m.Msg == (int)WinMessages.WM_MOVE)
            {
                lpObj = new Point((int)m.LParam.ToInt64());
            }
            else if (m.Msg == (int)WinMessages.WM_SIZE)
            {
                lpObj = string.Format("{0}, {1}", Enum.GetName(typeof(WM_SIZE_wParam), m.WParam.ToInt32()), new Point(m.LParam.ToInt32()));
            }
            else if (m.Msg == (int)WinMessages.WM_SIZING)
            {
                lpObj = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
            }
            else if (m.Msg == (int)WinMessages.WM_SETCURSOR)
            {
                int low16 = (short)m.LParam;
                int high16 = (short)((uint)m.LParam >> 16);
                lpObj = string.Format("hitTestResult:{0}, message:{1}", Enum.GetName(typeof(WM_NCHITTEST_return), low16), Enum.GetName(typeof(WinMessages), high16));
            }
            else if (m.Msg == (int)WinMessages.WM_MOVING)
            {
                lpObj = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
            }
            else if (m.Msg == (int)WinMessages.WM_MOUSEMOVE)
            {
                lpObj = string.Format("{0}, P:{1}", Enum.GetName(typeof(WM_MOUSEMOVE_wParam), m.WParam.ToInt32()), new Point(m.LParam.ToInt32()));
            }
            else if (m.Msg == (int)WinMessages.WM_CTLCOLORSTATIC)
            {
                // set your other colors for disabled controls
                //using (var g = Graphics.FromHwnd(Handle))
                //{
                //    using (var p = new Pen(Color.DarkViolet, 10))
                //    {
                //        // its a fat border so we need to draw 3 rectangles to cover it
                //        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                //        g.DrawRectangle(p, 1, 1, Width - 3, Height - 3);
                //        g.DrawRectangle(p, 2, 2, Width - 5, Height - 5);
                //    }
                //}
            }
            else if (m.Msg == (int)WinMessages.WM_PAINT)
            {
                // set the enabled border color here, and it works
                //using (var g = Graphics.FromHwnd(Handle))
                //{
                //    using (var p = new Pen(Color.Magenta, 10))
                //    {
                //        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                //    }
                //}
                //using (var g = Graphics.FromHwnd(Handle))
                //{
                //    var innerBorderWisth = 3;
                //    var innerBorderColor = BackColor;
                //    using (var p = new Pen(innerBorderColor, innerBorderWisth))
                //    {
                //        p.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                //        g.DrawRectangle(p, 1, 1, Width - 1, Height - 1);
                //    }
                //    using (var p = new Pen(Color.Crimson, 20))
                //    {
                //        g.DrawRectangle(p, 0, 0, Width - 10, Height - 10);
                //        g.DrawLine(p, Width -5 , 0, Width -5, Height);
                //    }
                //}
            }
            else if (m.Msg == (int)WinMessages.WM_NCPAINT)
            {
                //IntPtr hDC = GetWindowDC(m.HWnd);
                //Pen ThePen = new Pen(Color.Crimson, 10);
                //using (Graphics g = Graphics.FromHdc(hDC))
                //{
                //    g.DrawRectangle(ThePen, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                //    g.DrawRectangle(SystemPens.Window, new Rectangle(1, 1, this.Width - 3, this.Height - 3));
                //}
                //ReleaseDC(m.HWnd, hDC);
                //ThePen.Dispose();
                // try to set the disabled border color here, but its not working
                //using (var g = Graphics.FromHwnd(Handle))
                //{
                //    using (var p = new Pen(Color.Crimson, 10))
                //    {
                //        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                //    }
                //}
                //var dc = GetWindowDC(Handle);
                //using (Graphics g = Graphics.FromHdc(dc))
                //{
                //    ControlPaint.DrawBorder(g, new Rectangle(0, 0, Width, Height), Color.MediumOrchid, ButtonBorderStyle.Solid);
                //    //g.DrawRectangle(new Pen(Color.MediumOrchid, 10), 0, 0, this.Width , this.Height);
                //}
                //m.Result = (IntPtr)1;
                //return;
                //base.WndProc(ref m);
                //IntPtr hdc = GetDCEx(m.HWnd, m.WParam, (int)GetDCEx_Flags.DCX_PARENTCLIP | (int)GetDCEx_Flags.DCX_WINDOW); // 0x21 //(int)GetDCEx_Flags.DCX_CLIPSIBLINGS); // 
                //if (hdc != IntPtr.Zero)
                //{
                //    Graphics graphics = Graphics.FromHdc(hdc);
                //    //Rectangle bounds = new Rectangle(0, 0, Width, Height);
                //    //ControlPaint.DrawBorder(graphics, bounds, Color.MediumOrchid, ButtonBorderStyle.Solid);
                //    graphics.DrawRectangle(new Pen(Color.MediumOrchid, 10), 0, 0, Width, Height);
                //    //m.Result = (IntPtr)1;
                //    ReleaseDC(Handle, hdc);
                //}
                //m.Result = (IntPtr)1;
                //return;

                //IntPtr hdc = GetWindowDC(Handle);

                //using (var g = Graphics.FromHdcInternal(hdc))
                //using (var p = new Pen(Color.MediumOrchid, 10))
                //{
                //    g.DrawRectangle(p, new Rectangle(10, -1, Width, Height));
                //}

                //ReleaseDC(Handle, hdc);
            }

            if (debugLog)
            {
                string msgName = Enum.GetName(typeof(WinMessages), m.Msg);
                Console.WriteLine("{0}, {1:X4}, {2}\n\t{3}", WindowState, m.Msg, msgName, lpObj);
            }

            base.WndProc(ref m);
        }
        #endregion

        #region import win32 function
        /// <summary>
        /// ReleaseCapture function (winuser.h)
        /// Releases the mouse capture from a window in the current thread and restores normal mouse input processing.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-releasecapture
        /// </summary>
        [DllImport("User32.dll")]
        protected static extern bool ReleaseCapture();
        /// <summary>
        /// SendMessage function (winuser.h)
        /// Sends the specified message to a window or windows.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
        /// </summary>
        [DllImport("User32.dll")]
        protected static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        /// <summary>
        /// DefWindowProcA function (winuser.h)
        /// Calls the default window procedure to provide default processing for any window messages that an application does not process.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-defwindowproca
        /// </summary>
        [DllImport("User32.dll")]
        protected static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, int wParam, int lParam);
        /// <summary>
        /// GetWindowDC function (winuser.h)
        /// The GetWindowDC function retrieves the device context (DC) for the entire window, including title bar, menus, and scroll bars.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowdc
        /// </summary>
        [DllImport("User32.dll")]
        protected static extern IntPtr GetWindowDC(IntPtr hwnd);
        /// <summary>
        /// GetDCEx function (winuser.h)
        /// The GetDCEx function retrieves a handle to a device context (DC) for the client area of a specified window or for the entire screen.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdcex
        /// </summary>
        [DllImport("User32.dll", SetLastError = true)]
        protected static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, int flags);
        /// <summary>
        /// ReleaseDC function (winuser.h)
        /// The ReleaseDC function releases a device context (DC), freeing it for use by other applications.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-releasedc
        /// </summary>
        [DllImport("User32.dll")]
        protected static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);
        /// <summary>
        /// GetWindowRect function (winuser.h)
        /// Retrieves the dimensions of the bounding rectangle of the specified window.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrect
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);
        /// <summary>
        /// GetDoubleClickTime function (winuser.h)
        /// Retrieves the current double-click time for the mouse. 
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdoubleclicktime
        /// </summary>
        [DllImport("User32.dll")]
        public static extern uint GetDoubleClickTime();
        /// <summary>
        /// RedrawWindow function (winuser.h)
        /// The RedrawWindow function updates the specified rectangle or region in a window's client area.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-redrawwindow
        /// </summary>
        [DllImport("User32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);
        /// <summary>
        /// DwmExtendFrameIntoClientArea function (dwmapi.h)
        /// Extends the window frame into the client area.
        /// https://docs.microsoft.com/zh-tw/windows/win32/api/dwmapi/nf-dwmapi-dwmextendframeintoclientarea
        /// </summary>
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        /// <summary>
        /// DwmSetWindowAttribute function (dwmapi.h)
        /// Sets the value of Desktop Window Manager (DWM) non-client rendering attributes for a window.
        /// https://docs.microsoft.com/en-us/windows/win32/api/dwmapi/nf-dwmapi-dwmsetwindowattribute
        /// </summary>
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        #endregion
    }
    #region win32 message and parameters
    public enum WinMessages : int
    {
        WM_NULL = 0x0000,
        WM_CREATE = 0x0001,
        WM_DESTROY = 0x0002,
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,
        WM_SETFOCUS = 0x0007,
        WM_KILLFOCUS = 0x0008,
        WM_ENABLE = 0x000A,
        WM_SETREDRAW = 0x000B,
        WM_SETTEXT = 0x000C,
        WM_GETTEXT = 0x000D,
        WM_GETTEXTLENGTH = 0x000E,
        WM_PAINT = 0x000F,
        WM_CLOSE = 0x0010,
        WM_QUERYENDSESSION = 0x0011,
        WM_QUERYOPEN = 0x0013,
        WM_ENDSESSION = 0x0016,
        WM_QUIT = 0x0012,
        WM_ERASEBKGND = 0x0014,
        WM_SYSCOLORCHANGE = 0x0015,
        WM_SHOWWINDOW = 0x0018,
        WM_WININICHANGE = 0x001A,
        WM_SETTINGCHANGE = WM_WININICHANGE,
        WM_DEVMODECHANGE = 0x001B,
        WM_ACTIVATEAPP = 0x001C,
        WM_FONTCHANGE = 0x001D,
        WM_TIMECHANGE = 0x001E,
        WM_CANCELMODE = 0x001F,
        WM_SETCURSOR = 0x0020,
        WM_MOUSEACTIVATE = 0x0021,
        WM_CHILDACTIVATE = 0x0022,
        WM_QUEUESYNC = 0x0023,
        WM_GETMINMAXINFO = 0x0024,
        WM_PAINTICON = 0x0026,
        WM_ICONERASEBKGND = 0x0027,
        WM_NEXTDLGCTL = 0x0028,
        WM_SPOOLERSTATUS = 0x002A,
        WM_DRAWITEM = 0x002B,
        WM_MEASUREITEM = 0x002C,
        WM_DELETEITEM = 0x002D,
        WM_VKEYTOITEM = 0x002E,
        WM_CHARTOITEM = 0x002F,
        WM_SETFONT = 0x0030,
        WM_GETFONT = 0x0031,
        WM_SETHOTKEY = 0x0032,
        WM_GETHOTKEY = 0x0033,
        WM_QUERYDRAGICON = 0x0037,
        WM_COMPAREITEM = 0x0039,
        WM_GETOBJECT = 0x003D,
        WM_COMPACTING = 0x0041,
        WM_COMMNOTIFY = 0x0044,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WINDOWPOSCHANGED = 0x0047, //Sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
        WM_POWER = 0x0048,
        WM_COPYDATA = 0x004A,
        WM_CANCELJOURNAL = 0x004B,
        WM_NOTIFY = 0x004E,
        WM_INPUTLANGCHANGEREQUEST = 0x0050,
        WM_INPUTLANGCHANGE = 0x0051,
        WM_TCARD = 0x0052,
        WM_HELP = 0x0053,
        WM_USERCHANGED = 0x0054,
        WM_NOTIFYFORMAT = 0x0055,
        WM_CONTEXTMENU = 0x007B,
        WM_STYLECHANGING = 0x007C,
        WM_STYLECHANGED = 0x007D,
        WM_DISPLAYCHANGE = 0x007E,
        WM_GETICON = 0x007F,
        WM_SETICON = 0x0080,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCCALCSIZE = 0x0083,    //Sent when the size and position of a window's client area must be calculated.
        WM_NCHITTEST = 0x0084,     //Sent to a window in order to determine what part of the window corresponds to a particular screen coordinate.
        WM_NCPAINT = 0x0085,       //The WM_NCPAINT message is sent to a window when its frame must be painted.
        WM_NCACTIVATE = 0x0086,    //Sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
        WM_GETDLGCODE = 0x0087,
        WM_SYNCPAINT = 0x0088,
        WM_NCMOUSEMOVE = 0x00A0,
        WM_NCLBUTTONDOWN = 0x00A1, //Posted when the user presses the left mouse button while the cursor is within the nonclient area of a window.
        WM_NCLBUTTONUP = 0x00A2,
        WM_NCLBUTTONDBLCLK = 0x00A3,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCMBUTTONDOWN = 0x00A7,
        WM_NCMBUTTONUP = 0x00A8,
        WM_NCMBUTTONDBLCLK = 0x00A9,
        WM_NCXBUTTONDOWN = 0x00AB,
        WM_NCXBUTTONUP = 0x00AC,
        WM_NCXBUTTONDBLCLK = 0x00AD,
        WM_INPUT = 0x00FF,
        WM_KEYFIRST = 0x0100,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_CHAR = 0x0102,
        WM_DEADCHAR = 0x0103,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_SYSCHAR = 0x0106,
        WM_SYSDEADCHAR = 0x0107,
        WM_UNICHAR = 0x0109,
        WM_KEYLAST = 0x0108,
        WM_IME_STARTCOMPOSITION = 0x010D,
        WM_IME_ENDCOMPOSITION = 0x010E,
        WM_IME_COMPOSITION = 0x010F,
        WM_IME_KEYLAST = 0x010F,
        WM_INITDIALOG = 0x0110,
        WM_COMMAND = 0x0111,
        WM_SYSCOMMAND = 0x0112,
        WM_TIMER = 0x0113,
        WM_HSCROLL = 0x0114,
        WM_VSCROLL = 0x0115,
        WM_INITMENU = 0x0116,
        WM_INITMENUPOPUP = 0x0117,
        WM_MENUSELECT = 0x011F,
        WM_MENUCHAR = 0x0120,
        WM_ENTERIDLE = 0x0121,
        WM_MENURBUTTONUP = 0x0122,
        WM_MENUDRAG = 0x0123,
        WM_MENUGETOBJECT = 0x0124,
        WM_UNINITMENUPOPUP = 0x0125,
        WM_MENUCOMMAND = 0x0126,
        WM_CHANGEUISTATE = 0x0127,
        WM_UPDATEUISTATE = 0x0128,
        WM_QUERYUISTATE = 0x0129,
        WM_CTLCOLOR = 0x0019,
        WM_CTLCOLORMSGBOX = 0x0132,
        WM_CTLCOLOREDIT = 0x0133,
        WM_CTLCOLORLISTBOX = 0x0134,
        WM_CTLCOLORBTN = 0x0135,
        WM_CTLCOLORDLG = 0x0136,
        WM_CTLCOLORSCROLLBAR = 0x0137,
        WM_CTLCOLORSTATIC = 0x0138,
        WM_MOUSEFIRST = 0x0200,
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_MOUSEWHEEL = 0x020A,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C,
        WM_XBUTTONDBLCLK = 0x020D,
        WM_MOUSELAST = 0x020D,
        WM_PARENTNOTIFY = 0x0210,
        WM_ENTERMENULOOP = 0x0211,
        WM_EXITMENULOOP = 0x0212,
        WM_NEXTMENU = 0x0213,
        WM_SIZING = 0x0214,
        WM_CAPTURECHANGED = 0x0215,
        WM_MOVING = 0x0216,
        WM_POWERBROADCAST = 0x0218,
        WM_DEVICECHANGE = 0x0219,
        WM_MDICREATE = 0x0220,
        WM_MDIDESTROY = 0x0221,
        WM_MDIACTIVATE = 0x0222,
        WM_MDIRESTORE = 0x0223,
        WM_MDINEXT = 0x0224,
        WM_MDIMAXIMIZE = 0x0225,
        WM_MDITILE = 0x0226,
        WM_MDICASCADE = 0x0227,
        WM_MDIICONARRANGE = 0x0228,
        WM_MDIGETACTIVE = 0x0229,
        WM_MDISETMENU = 0x0230,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_DROPFILES = 0x0233,
        WM_MDIREFRESHMENU = 0x0234,
        WM_IME_SETCONTEXT = 0x0281,
        WM_IME_NOTIFY = 0x0282,
        WM_IME_CONTROL = 0x0283,
        WM_IME_COMPOSITIONFULL = 0x0284,
        WM_IME_SELECT = 0x0285,
        WM_IME_CHAR = 0x0286,
        WM_IME_REQUEST = 0x0288,
        WM_IME_KEYDOWN = 0x0290,
        WM_IME_KEYUP = 0x0291,
        WM_MOUSEHOVER = 0x02A1,
        WM_MOUSELEAVE = 0x02A3,
        WM_NCMOUSELEAVE = 0x02A2,
        WM_WTSSESSION_CHANGE = 0x02B1,
        WM_TABLET_FIRST = 0x02c0,
        WM_TABLET_LAST = 0x02df,
        WM_CUT = 0x0300,
        WM_COPY = 0x0301,
        WM_PASTE = 0x0302,
        WM_CLEAR = 0x0303,
        WM_UNDO = 0x0304,
        WM_RENDERFORMAT = 0x0305,
        WM_RENDERALLFORMATS = 0x0306,
        WM_DESTROYCLIPBOARD = 0x0307,
        WM_DRAWCLIPBOARD = 0x0308,
        WM_PAINTCLIPBOARD = 0x0309,
        WM_VSCROLLCLIPBOARD = 0x030A,
        WM_SIZECLIPBOARD = 0x030B,
        WM_ASKCBFORMATNAME = 0x030C,
        WM_CHANGECBCHAIN = 0x030D,
        WM_HSCROLLCLIPBOARD = 0x030E,
        WM_QUERYNEWPALETTE = 0x030F,
        WM_PALETTEISCHANGING = 0x0310,
        WM_PALETTECHANGED = 0x0311,
        WM_HOTKEY = 0x0312,
        WM_PRINT = 0x0317,
        WM_PRINTCLIENT = 0x0318,
        WM_APPCOMMAND = 0x0319,
        WM_THEMECHANGED = 0x031A,
        WM_HANDHELDFIRST = 0x0358,
        WM_HANDHELDLAST = 0x035F,
        WM_AFXFIRST = 0x0360,
        WM_AFXLAST = 0x037F,
        WM_PENWINFIRST = 0x0380,
        WM_PENWINLAST = 0x038F,
        WM_USER = 0x0400,
        WM_REFLECT = 0x2000,
        WM_APP = 0x8000
    }
    /// <summary>
    /// For WM_NCHITTEST message Return value
    /// https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest
    /// </summary>
    public enum WM_NCHITTEST_return
    {
        HT_ERROR = -2,         //On the screen background or on a dividing line between windows (same as HTNOWHERE, except that the DefWindowProc function produces a system beep to indicate an error).
        HT_TRANSPARENT = -1,   //In a window currently covered by another window in the same thread (the message will be sent to underlying windows in the same thread until one of them returns a code that is not HTTRANSPARENT).
        HT_NOWHERE = 0,        //On the screen background or on a dividing line between windows.
        HT_CLIENT = 0x1,       //In a client area.
        HT_CAPTION = 0x2,      //In a title bar.
        HT_SYSMENU = 0x3,       //In a window menu or in a Close button in a child window.
        HT_GROWBOX = 0x4,      //In a size box (same as HTSIZE).
        HT_SIZE = 0x4,         //In a size box (same as HTGROWBOX).
        HT_MENU = 0x5,         //In a menu.
        HT_HSCROLL = 0x6,      //In a horizontal scroll bar.
        HT_VSCROLL = 0x7,      //In the vertical scroll bar.
        HT_MINBUTTON = 0x8,    //In a Minimize button.
        HT_REDUCE = 0x8,       //In a Minimize button.
        HT_MAXBUTTON = 0x9,    //In a Maximize button.
        HT_ZOOM = 0x9,         //In a Maximize button.
        HT_LEFT = 0x0A,        //In the left border of a resizable window (the user can click the mouse to resize the window horizontally).
        HT_RIGHT = 0x0B,       //In the right border of a resizable window (the user can click the mouse to resize the window horizontally).
        HT_TOP = 0x0C,         //In the upper-horizontal border of a window.
        HT_TOPLEFT = 0x0D,     //In the upper-left corner of a window border.
        HT_TOPRIGHT = 0x0E,    //In the upper-right corner of a window border.
        HT_BOTTOM = 0xF,       //In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).
        HT_BOTTOMLEFT = 0x10,  //In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        HT_BOTTOMRIGHT = 0x11, //In the lower-right corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        HT_BORDER = 0x12,      //In the border of a window that does not have a sizing border.
        HT_CLOSE = 0x14,       //In a Close button.
        HT_HELP = 0x21,        //In a Help button.
    }
    /// <summary>
    /// For WM_SIZE message wParam value
    /// https://docs.microsoft.com/en-us/windows/win32/winmsg/wm-size
    /// </summary>
    public enum WM_SIZE_wParam
    {
        SIZE_MAXHIDE = 4,    //Message is sent to all pop-up windows when some other window is maximized.
        SIZE_MAXIMIZED = 2,    //The window has been maximized.
        SIZE_MAXSHOW = 3,    //Message is sent to all pop-up windows when some other window has been restored to its former size.
        SIZE_MINIMIZED = 1,    //The window has been minimized.
        SIZE_RESTORED = 0,    //The window has been resized, but neither the SIZE_MINIMIZED nor SIZE_MAXIMIZED value applies.
    }
    /// <summary>
    /// For WM_SIZING message wParam value
    /// https://docs.microsoft.com/en-us/windows/win32/winmsg/wm-sizing
    /// </summary>
    public enum WM_SIZING_wParam
    {
        WMSZ_BOTTOM = 6,      //Bottom edge
        WMSZ_BOTTOMLEFT = 7,  //Bottom-left corner
        WMSZ_BOTTOMRIGHT = 8, //Bottom-right corner
        WMSZ_LEFT = 1,        //Left edge
        WMSZ_RIGHT = 2,       //Right edge
        WMSZ_TOP = 3,         //Top edge
        WMSZ_TOPLEFT = 4,     //Top-left corner
        WMSZ_TOPRIGHT = 5,    //Top-right corner
    }
    /// <summary>
    /// For WM_MOUSEMOVE message wParam value
    /// https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mousemove
    /// </summary>
    public enum WM_MOUSEMOVE_wParam
    {
        MK_CONTROL = 0x0008,  //The CTRL key is down.
        MK_LBUTTON = 0x0001,  //The left mouse button is down.
        MK_MBUTTON = 0x0010,  //The middle mouse button is down.
        MK_RBUTTON = 0x0002,  //The right mouse button is down.
        MK_SHIFT = 0x0004,    //The SHIFT key is down.
        MK_XBUTTON1 = 0x0020, //The first X button is down.
        MK_XBUTTON2 = 0x0040, //The second X button is down.
    }
    /// <summary>
    /// WM_LBUTTONDOWN message wParam value
    /// https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondown
    /// </summary>
    public enum WM_LBUTTONDOWN_wParam
    {
        MK_CONTROL = 0x0008,  //The CTRL key is down.
        MK_LBUTTON = 0x0001,  //The left mouse button is down.
        MK_MBUTTON = 0x0010,  //The middle mouse button is down.
        MK_RBUTTON = 0x0002,  //The right mouse button is down.
        MK_SHIFT = 0x0004,    //The SHIFT key is down.
        MK_XBUTTON1 = 0x0020, //The first X button is down.
        MK_XBUTTON2 = 0x0040, //The second X button is down.
    }
    /// <summary>
    /// For Window Class Styles
    /// </summary>
    public enum ClassStyles
    {
        CS_DBLCLKS = 0x8,        //Sends a double-click message to the window procedure when the user double-clicks the mouse while the cursor is within a window belonging to the class.
        CS_DROPSHADOW = 0x20000, //Enables the drop shadow effect on a window. 
    }
    /// <summary>
    /// For Window Styles
    /// https://docs.microsoft.com/en-us/windows/win32/winmsg/window-styles
    /// </summary>
    public enum Styles
    {
        WS_MINIMIZEBOX = 0x20000,   //The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        WS_CLIPCHILDREN = 0x02000000, //Excludes the area occupied by child windows when drawing occurs within the parent window.
        WS_SYSMENU = 0x80000,       //The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
        WS_SIZEBOX = 0x40000,       //The window has a sizing border. Same as the WS_THICKFRAME style.
        WS_THICKFRAME = 0x00040000, //The window has a sizing border. Same as the WS_SIZEBOX style.
        WS_DLGFRAME = 0x00400000,   //The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
        WS_BORDER = 0x00800000,     //The window has a thin-line border.
        WS_CAPTION = 0x00C00000,    //The window has a title bar (includes the WS_BORDER style).
        WS_CHILD = 0x40000000,      //The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
        WS_MAXIMIZEBOX = 0x00010000, //The window has a maximize button.
        WS_OVERLAPPED = 0x00000000, //The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
    }
    /// <summary>
    /// For Extended Window Styles
    /// https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles
    /// </summary>
    public enum ExStyles
    {
        WS_EX_DLGMODALFRAME = 0x00000001, //The window has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
        WS_EX_CLIENTEDGE = 0x00000200,    //The window has a border with a sunken edge.
        WS_EX_LAYERED = 0x00080000,       //The window is a layered window. This style cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        WS_EX_TRANSPARENT = 0x00000020,   //The window should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
        WS_EX_COMPOSITED = 0x02000000,    //Paints all descendants of a window in bottom-to-top painting order using double-buffering.
        WS_EX_TOPMOST = 0x00000008,       //The window should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
        WS_EX_WINDOWEDGE = 0x00000100,    //The window has a border with a raised edge.
    }
    /// <summary>
    /// The window position. This member can be one or more of the following values.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-windowpos
    /// </summary>
    public enum WINDOWPOS_Flags
    {
        SWP_DRAWFRAME = 0x0020,      //Draws a frame (defined in the window's class description) around the window. Same as the SWP_FRAMECHANGED flag.
        SWP_FRAMECHANGED = 0x0020,   //Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        SWP_HIDEWINDOW = 0x0080,     //Hides the window.
        SWP_NOACTIVATE = 0x0010,     //Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hwndInsertAfter member).
        SWP_NOCOPYBITS = 0x0100,     //Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
        SWP_NOMOVE = 0x0002,         //Retains the current position (ignores the x and y members).
        SWP_NOOWNERZORDER = 0x0200,  //Does not change the owner window's position in the Z order.
        SWP_NOREDRAW = 0x0008,       //Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
        SWP_NOREPOSITION = 0x0200,   //Does not change the owner window's position in the Z order. Same as the SWP_NOOWNERZORDER flag.
        SWP_NOSENDCHANGING = 0x0400, //Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        SWP_NOSIZE = 0x0001,         //Retains the current size (ignores the cx and cy members).
        SWP_NOZORDER = 0x0004,       //Retains the current Z order (ignores the hwndInsertAfter member).
        SWP_SHOWWINDOW = 0x0040,     //Displays the window.
    }
    /// <summary>
    /// GetDCEx function flags
    /// Specifies how the DC is created. This parameter can be one or more of the following values.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdcex
    /// </summary>
    public enum GetDCEx_Flags
    {
        DCX_WINDOW = 0x00000001,           //Returns a DC that corresponds to the window rectangle rather than the client rectangle.
        DCX_CACHE = 0x00000002,            //Returns a DC from the cache, rather than the OWNDC or CLASSDC window. Essentially overrides CS_OWNDC and CS_CLASSDC.
        DCX_NORESETATTRS = 0x00000004,     //This flag is ignored.
        DCX_CLIPCHILDREN = 0x00000008,     //Excludes the visible regions of all child windows below the window identified by hWnd.
        DCX_CLIPSIBLINGS = 0x00000010,     //Excludes the visible regions of all sibling windows above the window identified by hWnd.
        DCX_PARENTCLIP = 0x00000020,       //Uses the visible region of the parent window. The parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The origin is set to the upper-left corner of the window identified by hWnd.
        DCX_EXCLUDERGN = 0x00000040,       //The clipping region identified by hrgnClip is excluded from the visible region of the returned DC.
        DCX_INTERSECTRGN = 0x00000080,     //The clipping region identified by hrgnClip is intersected with the visible region of the returned DC.
        DCX_EXCLUDEUPDATE = 0x00000100,    //
        DCX_INTERSECTUPDATE = 0x00000200,  //Reserved; do not use.
        DCX_LOCKWINDOWUPDATE = 0x00000400, //Allows drawing even if there is a LockWindowUpdate call in effect that would otherwise exclude this window. Used for drawing during tracking.
        DCX_VALIDATE = 0x00200000,         //Reserved; do not use.
    }
    /// <summary>
    /// RedrawWindow function flags
    /// The RedrawWindow function updates the specified rectangle or region in a window's client area.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-redrawwindow
    /// </summary>
    public enum RedrawWindowFlags : uint
    {
        /// <summary>
        /// Invalidates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
        /// You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_INVALIDATE invalidates the entire window.
        /// </summary>
        Invalidate = 0x1,
        /// <summary>Causes the OS to post a WM_PAINT message to the window regardless of whether a portion of the window is invalid.</summary>
        InternalPaint = 0x2,
        /// <summary>
        /// Causes the window to receive a WM_ERASEBKGND message when the window is repainted.
        /// Specify this value in combination with the RDW_INVALIDATE value; otherwise, RDW_ERASE has no effect.
        /// </summary>
        Erase = 0x4,
        /// <summary>
        /// Validates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
        /// You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_VALIDATE validates the entire window.
        /// This value does not affect internal WM_PAINT messages.
        /// </summary>
        Validate = 0x8,
        NoInternalPaint = 0x10,
        /// <summary>Suppresses any pending WM_ERASEBKGND messages.</summary>
        NoErase = 0x20,
        /// <summary>Excludes child windows, if any, from the repainting operation.</summary>
        NoChildren = 0x40,
        /// <summary>Includes child windows, if any, in the repainting operation.</summary>
        AllChildren = 0x80,
        /// <summary>Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND and WM_PAINT messages before the RedrawWindow returns, if necessary.</summary>
        UpdateNow = 0x100,
        /// <summary>
        /// Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND messages before RedrawWindow returns, if necessary.
        /// The affected windows receive WM_PAINT messages at the ordinary time.
        /// </summary>
        EraseNow = 0x200,
        Frame = 0x400,
        NoFrame = 0x800
    }

    /// <summary>
    /// RECT structure (windef.h)
    /// The RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
    /// </summary>
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public override string ToString()
        {
            return string.Format("L:{0,5:N0}, T;{1,5:N0}, R:{2,5:N0}, B:{3,5:N0}, H:{4,5:N0}, W:{5,5:N0}", Left, Top, Right, Bottom, Bottom - Top, Right - Left);
        }
    }
    /// <summary>
    /// NCCALCSIZE_PARAMS structure (winuser.h)
    /// Contains information that an application can use while processing the WM_NCCALCSIZE message to calculate the size, position, and valid contents of the client area of a window.
    /// An array of rectangles. The meaning of the array of rectangles changes during the processing of the WM_NCCALCSIZE message.
    /// The first rectangle contains the new coordinates of a window that has been moved or resized, that is, it is the proposed new window coordinates. 
    /// The second contains the coordinates of the window before it was moved or resized. 
    /// The third contains the coordinates of the window's client area before the window was moved or resized.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-nccalcsize_params
    /// </summary>
    public struct NCCALCSIZE_PARAMS
    {
        public RECT rcNewWindow;
        public RECT rcOldWindow;
        public RECT rcClient;
        public WINDOWPOS lppos;
        public override string ToString()
        {
            return string.Format("rcNewWindow:{0}, \n\trcOldWindow;{1}, \n\trcClient:{2}, \n\twPos:{3,5:N0}", rcNewWindow, rcOldWindow, rcClient, lppos);
        }
    }
    /// <summary>
    /// WINDOWPOS structure (winuser.h)
    /// Contains information about the size and position of a window.
    /// https://docs.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms633573(v=vs.85)
    /// </summary>
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public WINDOWPOS_Flags flags;
        public override string ToString()
        {
            return string.Format("hwnd:{0,4:N0}, hwndInsertAfter:{1,4:N0}, x:{2,4:N0}, y:{3,4:N0}, cx:{4,4:N0}, cy:{5,4:N0}, flags:{4,4:N0}", hwnd, hwndInsertAfter, x, y, cx, cy, flags);
        }
    }
    /// <summary>
    /// POINT structure
    /// The POINT structure defines the x- and y- coordinates of a point.
    /// https://docs.microsoft.com/en-us/previous-versions/dd162805(v=vs.85)
    /// </summary>
    public struct POINT
    {
        public int x;
        public int y;
        public override string ToString()
        {
            return string.Format("X:{0,4:N0}, Y;{1,4:N0}", x, y);
        }
    }
    /// <summary>
    /// MINMAXINFO structure (winuser.h)
    /// Contains information about a window's maximized size and position and its minimum and maximum tracking size.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-minmaxinfo
    /// </summary>
    public struct MINMAXINFO
    {
        public POINT ptReserved;    //Reserved; do not use.
        public POINT ptMaxSize;     //The maximized width (x member) and the maximized height (y member) of the window. For top-level windows, this value is based on the width of the primary monitor.
        public POINT ptMaxPosition; //The position of the left side of the maximized window (x member) and the position of the top of the maximized window (y member).
        public POINT ptMinTrackSize;//The minimum tracking width (x member) and the minimum tracking height (y member) of the window.
        public POINT ptMaxTrackSize;//The maximum tracking width (x member) and the maximum tracking height (y member) of the window.
        public override string ToString()
        {
            return string.Format("ptReserved:{0,4:N0}, \n\tptMaxSize;{1,4:N0}, \n\tptMaxPosition;{2,4:N0}, \n\tptMinTrackSize;{3,4:N0}, \n\tptMaxTrackSize;{4,4:N0}", ptReserved, ptMaxSize, ptMaxPosition, ptMinTrackSize, ptMaxTrackSize);
        }
    }
    /// <summary>
    /// MARGINS structure (uxtheme.h)
    /// define the margins of windows that have visual styles applied.
    /// https://docs.microsoft.com/en-us/windows/win32/api/uxtheme/ns-uxtheme-margins
    /// </summary>
    public struct MARGINS
    {
        public int leftWidth;
        public int rightWidth;
        public int topHeight;
        public int bottomHeight;
    }
    #endregion
}
