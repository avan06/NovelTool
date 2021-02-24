using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace NovelTool
{
    public partial class Options : Form
    {
        private readonly Main mainForm;
        List<Panel> Panels = new List<Panel>();
        Panel VisiblePanel = null;
        private readonly String[] Colors = { "AliceBlue", "LightSalmon", "AntiqueWhite", "LightSeaGreen", "Aqua", "LightSkyBlue", "Aquamarine", "LightSlateGray", "Azure", "LightSteelBlue", "Beige", "LightYellow", "Bisque", "Lime", "Black", "LimeGreen", "BlanchedAlmond", "Linen", "Blue", "Magenta", "BlueViolet", "Maroon", "Brown", "MediumAquamarine", "BurlyWood", "MediumBlue", "CadetBlue", "MediumOrchid", "Chartreuse", "MediumPurple", "Chocolate", "MediumSeaGreen", "Coral", "MediumSlateBlue", "CornflowerBlue", "MediumSpringGreen", "Cornsilk", "MediumTurquoise", "Crimson", "MediumVioletRed", "Cyan", "MidnightBlue", "DarkBlue", "MintCream", "DarkCyan", "MistyRose", "DarkGoldenrod", "Moccasin", "DarkGray", "NavajoWhite", "DarkGreen", "Navy", "DarkKhaki", "OldLace", "DarkMagenta", "Olive", "DarkOliveGreen", "OliveDrab", "DarkOrange", "Orange", "DarkOrchid", "OrangeRed", "DarkRed", "Orchid", "DarkSalmon", "PaleGoldenrod", "DarkSeaGreen", "PaleGreen", "DarkSlateBlue", "PaleTurquoise", "DarkSlateGray", "PaleVioletRed", "DarkTurquoise", "PapayaWhip", "DarkViolet", "PeachPuff", "DeepPink", "Peru", "DeepSkyBlue", "Pink", "DimGray", "Plum", "DodgerBlue", "PowderBlue", "Firebrick", "Purple", "FloralWhite", "Red", "ForestGreen", "RosyBrown", "Fuchsia", "RoyalBlue", "Gainsboro", "SaddleBrown", "GhostWhite", "Salmon", "Gold", "SandyBrown", "Goldenrod", "SeaGreen", "Gray", "Seashell", "Green", "Sienna", "GreenYellow", "Silver", "Honeydew", "SkyBlue", "HotPink", "SlateBlue", "IndianRed", "SlateGray", "Indigo", "Snow", "Ivory", "SpringGreen", "Khaki", "SteelBlue", "Lavender", "Tan", "LavenderBlush", "Teal", "LawnGreen", "Thistle", "LemonChiffon", "Tomato", "LightBlue", "Turquoise", "LightCoral", "Violet", "LightCyan", "Wheat", "LightGoldenrodYellow", "White", "LightGreen", "WhiteSmoke", "LightGray", "Yellow", "LightPink", "YellowGreen"};
        public Options(Main mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            InitTreeview();
            InitControlsWithProperties(Controls);
        }
        #region Init Treeview
        /// <summary>
        /// http://csharphelper.com/blog/2014/07/use-a-treeview-to-display-property-pages-or-option-pages-in-c/
        /// </summary>
        private void InitTreeview()
        {
            // Expand all tree nodes.
            treeView.ExpandAll();

            // Move the Panels out of the TabControl.
            tabControl.Visible = false;
            foreach (TabPage page in tabControl.TabPages)
            {
                Panel tabPanel = new Panel
                {
                    Name = page.Name + "Panel",
                    Parent = tabControl.Parent,
                    Location = tabControl.Location,
                    Size = page.Size,
                    Visible = false,
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                };

                // Add the Panel to the list.
                while (page.Controls.Count > 0)
                {
                    tabPanel.Controls.Add(page.Controls[0]);
                }
                Panels.Add(tabPanel);
            }
            // Display the first panel.
            DisplayPanel(0);
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DisplayPanel(e.Node.Index);
        }

        /// <summary>
        /// Display the appropriate Panel.
        /// </summary>
        private void DisplayPanel(int index)
        {
            if (Panels.Count < 1) return;

            // If this is the same Panel, do nothing.
            if (VisiblePanel == Panels[index]) return;

            // Hide the previously visible Panel.
            if (VisiblePanel != null) VisiblePanel.Visible = false;

            // Display the appropriate Panel.
            Panels[index].Visible = true;
            VisiblePanel = Panels[index];
        }
        #endregion

        #region Processing Properties
        private void InitControlsWithProperties(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control.GetType().GetMember("Controls") != null)
                {
                    InitControlsWithProperties(control.Controls);
                }
                if (control is ComboBox comboBox)
                {
                    if (comboBox.Name.StartsWith("ColorBox"))
                    {
                        comboBox.Items.AddRange(Colors);
                        comboBox.DrawMode = DrawMode.OwnerDrawVariable;
                        comboBox.DrawItem += new DrawItemEventHandler(ColorBox_DrawItem);
                        comboBox.SelectedIndexChanged += new EventHandler(Control_Changed);
                        try
                        {
                            if (Properties.Settings.Default[comboBox.Name] is Color color) comboBox.SelectedItem = color.Name;
                        }
                        catch (Exception) { }
                    }
                    else if (comboBox.Name.StartsWith("PositionTypeBox"))
                    {
                        foreach (PositionType filterEnum in (PositionType[])Enum.GetValues(typeof(PositionType))) comboBox.Items.Add(filterEnum);
                        comboBox.SelectedIndexChanged += new EventHandler(Control_Changed);
                        try
                        {
                            if (Properties.Settings.Default[comboBox.Name] is PositionType pType) comboBox.SelectedItem = pType;
                        }
                        catch (Exception) { }
                    }
                    else if (comboBox.Name.StartsWith("ImageTypeBox"))
                    {
                        foreach (ImageType filterEnum in (ImageType[])Enum.GetValues(typeof(ImageType))) comboBox.Items.Add(filterEnum);
                        comboBox.SelectedIndexChanged += new EventHandler(Control_Changed);
                        try
                        {
                            if (Properties.Settings.Default[comboBox.Name] is ImageType iType) comboBox.SelectedItem = iType;
                        }
                        catch (Exception) { }
                    }//PixelFormatBoxOutput
                    else if (comboBox.Name.StartsWith("PixelFormatBox"))
                    {
                        comboBox.Items.Add(PixelFormat.DontCare);
                        foreach (PixelFormat filterEnum in (PixelFormat[])Enum.GetValues(typeof(PixelFormat))) if (filterEnum.ToString().StartsWith("Format")) comboBox.Items.Add(filterEnum); //if (filterEnum.ToString().StartsWith("Format")) 
                        comboBox.SelectedIndexChanged += new EventHandler(Control_Changed);
                        try
                        {
                            if (Properties.Settings.Default[comboBox.Name] is PixelFormat pFormat) comboBox.SelectedItem = pFormat;
                        }
                        catch (Exception) { }
                    }
                }
                else if (control is TextBox textBox && control.Name.StartsWith("NumBox"))
                {
                    textBox.TextChanged += new EventHandler(Control_Changed);
                    try
                    {
                        textBox.Text = Properties.Settings.Default[textBox.Name].ToString();
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (control is CheckBox checkBox)
                {
                    try
                    {
                        checkBox.CheckedChanged += new EventHandler(Control_Changed);
                        if (Properties.Settings.Default[checkBox.Name] is bool value) checkBox.Checked = value;
                    }
                    catch (Exception)
                    {
                    }
                } else if (control is NumericUpDown numericUpDown) 
                {
                    try
                    {
                        numericUpDown.ValueChanged += new EventHandler(Control_Changed);
                        if (Properties.Settings.Default[numericUpDown.Name] is int valInt) numericUpDown.Value = valInt;
                        else if (Properties.Settings.Default[numericUpDown.Name] is long valLong) numericUpDown.Value = Convert.ToDecimal(valLong);
                        else if (Properties.Settings.Default[numericUpDown.Name] is float valFloat) numericUpDown.Value = Convert.ToDecimal(valFloat);
                        else if (Properties.Settings.Default[numericUpDown.Name] is byte valByte) numericUpDown.Value = Convert.ToDecimal(valByte);
                    }
                    catch (Exception)
                    {
                    }
                }  
            }
        }
        /// <summary>
        /// https://stackoverflow.com/a/25616698
        /// </summary>
        private void ColorBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                Rectangle rect = e.Bounds; //Rectangle of item
                if (e.Index >= 0)
                {
                    //Get item color name
                    string itemName = ((ComboBox)sender).Items[e.Index].ToString();
                    //Get instance color from item name
                    Color itemColor = Color.FromName(itemName);
                    //Get instance brush with Solid style to draw background
                    Brush brush = new SolidBrush(itemColor);
                    //Draw the background with my brush style and rectangle of item
                    g.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
                    //Get instance a font to draw item name with this style
                    Font itemFont = new Font("Arial", 9, FontStyle.Bold);
                    //Draw the item name
                    g.DrawString(itemName, itemFont, Brushes.Black, rect.X, rect.Top);
                }
            }
        }
        private void Control_Changed(object sender, EventArgs e)
        {
            if (!this.Visible) return;
            if (sender is ComboBox comboBox) //ColorBox_SelectedIndexChanged
            {
                if (comboBox.Name.StartsWith("ColorBox"))
                {
                    string itemName = comboBox.SelectedItem.ToString();
                    Color itemColor = Color.FromName(itemName);
                    Properties.Settings.Default[comboBox.Name] = itemColor;
                    Properties.Settings.Default.Save();
                    //if (mainForm != null)
                    //{
                    //    mainForm.ChangeTheme(mainForm.Controls, false);
                    //    //mainForm.ChangeTheme(this.Controls, false);
                    //}
                }
                else if (comboBox.Name.StartsWith("PositionTypeBox"))
                {
                    PositionType pType = (PositionType)comboBox.SelectedItem;
                    Properties.Settings.Default[comboBox.Name] = pType;
                    Properties.Settings.Default.Save();
                }
                else if (comboBox.Name.StartsWith("ImageTypeBox"))
                {
                    ImageType pType = (ImageType)comboBox.SelectedItem;
                    Properties.Settings.Default[comboBox.Name] = pType;
                    Properties.Settings.Default.Save();
                }//PixelFormatBoxOutput
                else if (comboBox.Name.StartsWith("PixelFormatBox"))
                {
                    PixelFormat pType = (PixelFormat)comboBox.SelectedItem;
                    Properties.Settings.Default[comboBox.Name] = pType;
                    Properties.Settings.Default.Save();
                }
            }
            else if (sender is TextBox numBox) //NumBox_TextChanged
            {
                if (int.TryParse(numBox.Text, out int value))
                {
                    Properties.Settings.Default[numBox.Name] = value;
                    Properties.Settings.Default.Save();
                }
            }
            else if (sender is CheckBox checkBox) //CheckBox_CheckedChanged
            {
                Properties.Settings.Default[checkBox.Name] = checkBox.Checked;
                Properties.Settings.Default.Save();
            }
            else if (sender is NumericUpDown numericUpDown) //NumericUpDown_ValueChanged
            {
                if (numericUpDown.Name.StartsWith("Int"))
                {
                    Properties.Settings.Default[numericUpDown.Name] = (int)numericUpDown.Value;
                    Properties.Settings.Default.Save();
                }
                else if (numericUpDown.Name.StartsWith("Long"))
                {
                    Properties.Settings.Default[numericUpDown.Name] = (long)numericUpDown.Value;
                    Properties.Settings.Default.Save();
                }
                else if (numericUpDown.Name.StartsWith("Float"))
                {
                    Properties.Settings.Default[numericUpDown.Name] = (float)numericUpDown.Value;
                    Properties.Settings.Default.Save();
                }
                else if (numericUpDown.Name.StartsWith("Byte"))
                {
                    Properties.Settings.Default[numericUpDown.Name] = (byte)numericUpDown.Value;
                    Properties.Settings.Default.Save();
                }
            }
            //if (mainForm != null)
            //{
            //    mainForm.Refresh();
            //}
        }
        #endregion
    }
}
