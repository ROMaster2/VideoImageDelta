using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
//using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Hudl.FFprobe;
using Hudl.FFprobe.Command;
using Hudl.FFprobe.Metadata;
using Hudl.FFprobe.Metadata.Models;
using ImageMagick;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using VideoImageDeltaApp;
using VideoImageDeltaApp.Forms;
using VideoImageDeltaApp.Models;

namespace VideoImageDeltaApp.Forms
{
    public partial class AddWatchers : Form
    {
        public AddWatchers()
        {
            InitializeComponent();

            Show_Panel(Panel_Blank);
            Hide_Screens();

            foreach (var gp in Program.GameProfiles)
                ListBox_GameProfile.Items.Add(gp);

            if (ListBox_GameProfile.Items.Count > 0)
            {
                ListBox_GameProfile.SelectedIndex = 0;
            }

        }

        private void Hide_Screens()
        {
            Hide_WatchZones();
            ListBox_Screens.ClearSelected();
            ListBox_Screens.Hide();
            Button_Plus_Screens.Hide();
            Button_Minus_Screens.Hide();
            Label_Screens.Hide();
        }

        private void Hide_WatchZones()
        {
            Hide_Watches();
            ListBox_WatchZones.ClearSelected();
            ListBox_WatchZones.Hide();
            Button_Plus_WatchZones.Hide();
            Button_Minus_WatchZones.Hide();
            Label_WatchZones.Hide();
        }

        private void Hide_Watches()
        {
            ListBox_Watches.ClearSelected();
            ListBox_Watches.Hide();
            Button_Plus_Watches.Hide();
            Button_Minus_Watches.Hide();
            Label_Watches.Hide();
        }

        private void Show_Screens()
        {
            ListBox_Screens.Show();
            Button_Plus_Screens.Show();
            Button_Minus_Screens.Show();
            Label_Screens.Show();
        }

        private void Show_WatchZones()
        {
            Show_Screens();
            ListBox_WatchZones.Show();
            Button_Plus_WatchZones.Show();
            Button_Minus_WatchZones.Show();
            Label_WatchZones.Show();
        }

        private void Show_Watches()
        {
            Show_WatchZones();
            ListBox_Watches.Show();
            Button_Plus_Watches.Show();
            Button_Minus_Watches.Show();
            Label_Watches.Show();
        }

        private void Show_Panel(Panel panel, Button acceptButton = null, TextBox focusBox = null)
        {
            Panel_Blank.Hide();
            Panel_GameProfile.Hide();
            Panel_GameProfile_New.Hide();
            Panel_Screens.Hide();
            Panel_Screens_New.Hide();
            Panel_WatchZones.Hide();
            Panel_WatchZones_New.Hide();
            Panel_Watches.Hide();
            Panel_Watches_New.Hide();
            panel.Show();
            panel.BringToFront();
            if (acceptButton != null)
            {
                this.AcceptButton = acceptButton;
            }
            if (focusBox != null)
            {
                focusBox.Focus();
            }
        }

        private void Update_Screens()
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(17945);
            }
            else
            {
                ListBox_Screens.Items.Clear();
                var gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                foreach (Models.Screen s in gp.Screens)
                {
                    ListBox_Screens.Items.Add(s);
                }
            }
        }

        private void Update_WatchZones()
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(17952);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(17953);
            }
            else
            {
                ListBox_WatchZones.Items.Clear();
                var s = (Models.Screen)ListBox_Screens.SelectedItem;
                foreach (WatchZone wz in s.WatchZones)
                {
                    ListBox_WatchZones.Items.Add(wz);
                }
            }
        }

        private void Update_Watches()
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(17954);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(17955);
            }
            else if (ListBox_WatchZones.SelectedItem == null)
            {
                Utilities.Error(17956);
            }
            else
            {
                ListBox_Watches.Items.Clear();
                var wz = (WatchZone)ListBox_WatchZones.SelectedItem;
                foreach (Watcher w in wz.Watches)
                {
                    ListBox_Watches.Items.Add(w);
                }
                var ratio = wz.Geometry.Ratio;
                ratio = ratio / (200d / 120d);
                if (ratio > 1d)
                {
                    PictureBox_Watches_New.Height = (int)(120 / ratio);
                    PictureBox_Watches_New.Location = new Point(15, 18 + (int)((120d - (120d / ratio)) / 2d));
                }
                else
                {
                    PictureBox_Watches_New.Width = (int)(200 * ratio);
                    PictureBox_Watches_New.Location = new Point(15 + (int)((200d - (200d * ratio)) / 2d), 18);
                }

            }
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            Form_Closing(null, null);
            this.Close();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Program.GameProfiles.Clear();
            foreach (GameProfile gp in ListBox_GameProfile.Items)
                Program.GameProfiles.Add(gp);
        }

        private void Button_Plus_GameProfile_Click(object sender, EventArgs e)
        {
            TextBox_GameProfile_New_Name.Text = null;
            Show_Panel(Panel_GameProfile_New, Button_GameProfile_New_Create, TextBox_GameProfile_New_Name);
        }

        private void Button_Import_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "XML Files|*.xml",
                Title = "Select a XML File"
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                // Process XML
            }

        }

        private void Button_Minus_GameProfile_Click(object sender, EventArgs e)
        {
            // I tried to make this a function that can be applied to other methods. It didn't work. It wouldn't accept Type for the overload.
            // Otherwise I don't know how to get it to work without passing through an unnecessary amount of variables.
            if (ListBox_GameProfile.SelectedItem != null)
            {
                ListBox_GameProfile.Items.RemoveAt(ListBox_GameProfile.SelectedIndex);
            }
        }

        private void ListBox_GameProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem != null)
            {
                Show_Screens();
                Show_Panel(Panel_GameProfile, Button_GameProfile_Rename, TextBox_GameProfile_Name);
                var gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                TextBox_GameProfile_Name.Text = gp.Name;
                Update_Screens();
                Hide_WatchZones();
            }
            else
            {
                Hide_Screens();
                Show_Panel(Panel_GameProfile_New, Button_GameProfile_New_Create, TextBox_GameProfile_New_Name);
            }
        }

        private void ListBox_Screens_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox_Screens.SelectedItem != null)
            {
                Show_WatchZones();
                Show_Panel(Panel_Screens, Button_Screens_Update, TextBox_Screens_Name);
                var s = (Models.Screen)ListBox_Screens.SelectedItem;
                TextBox_Screens_Name.Text = s.Name;
                ScreenType_Radio_Manager(s.ScreenType);
                Numeric_Screens_Width.Value = (decimal)s.Geometry.Width;
                Numeric_Screens_Height.Value = (decimal)s.Geometry.Height;
                Update_WatchZones();
                Hide_Watches();
            }
            else
            {
                Hide_WatchZones();
                ListBox_GameProfile_SelectedIndexChanged(null, null);
            }
        }

        private void ListBox_WatchZones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox_WatchZones.SelectedItem != null)
            {
                Show_Watches();
                Show_Panel(Panel_WatchZones, Button_WatchZones_Update, TextBox_WatchZones_Name);
                var wz = (WatchZone)ListBox_WatchZones.SelectedItem;
                TextBox_WatchZones_Name.Text = wz.Name;
                Numeric_WatchZones_X.Value = (decimal)wz.Geometry.X;
                Numeric_WatchZones_Y.Value = (decimal)wz.Geometry.Y;
                Numeric_WatchZones_Width.Value = (decimal)wz.Geometry.Width;
                Numeric_WatchZones_Height.Value = (decimal)wz.Geometry.Height;
                var s = (Models.Screen)ListBox_Screens.SelectedItem;
                if (s.ScreenType != ScreenType.Dynamic)
                {
                    Label_WatchZones_Anchor.Hide();
                    TableLayoutPanel_WatchZones_Anchor.Hide();
                }
                else
                {
                    Label_WatchZones_Anchor.Show();
                    TableLayoutPanel_WatchZones_Anchor.Show();
                }
                Anchor_Radio_Manager(wz.Geometry.Anchor);
                Update_Watches();
            }
            else
            {
                Hide_Watches();
                ListBox_Screens_SelectedIndexChanged(null, null);
            }
        }

        private void Button_Export_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem != null)
            {
                var item = @ListBox_GameProfile.SelectedItem;
                var type = item.GetType();
                XmlSerializer xsSubmit = new XmlSerializer(type);
                var xml = "";

                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, item);
                        xml = sww.ToString();
                    }
                }

                SaveFileDialog sfd = new SaveFileDialog()
                {
                    DefaultExt = "xml",
                    Filter = "XML Files|*.xml",
                    FileName = ListBox_GameProfile.SelectedItem.ToString(), // Might not always work...
                    Title = "Save Game Profile",
                };
                sfd.ShowDialog();

                XmlDocument document = new XmlDocument();
                StreamWriter stream = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding("iso-8859-7"));
                document.Save(xml);


            }
            else
            {
                Utilities.Flicker(ListBox_GameProfile, 500, Color.FromArgb(224, 64, 64));
            }
        }

        private void Button_Plus_Screens_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(46208);
            }
            else
            {
                TextBox_Screens_New_Name.Text = null;
                ScreenType_Radio_Manager_New(ScreenType.Static);
                Numeric_Screens_New_Width.Value = 640;
                Numeric_Screens_New_Height.Value = 480;
                Show_Panel(Panel_Screens_New, Button_Screens_New_Create, TextBox_Screens_New_Name);
            }
        }

        private void Button_GameProfile_Rename_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(46209);
            }
            else
            {
                var text = TextBox_GameProfile_Name.Text.Trim();
                var gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                if (!String.IsNullOrWhiteSpace(text))
                {
                    gp.Name = text;
                }
                ListBox_GameProfile.Select();
            }
        }

        private void Button_GameProfile_New_Create_Click(object sender, EventArgs e)
        {
            string name = TextBox_GameProfile_New_Name.Text;
            name = name.Trim();
            if (!String.IsNullOrWhiteSpace(name))
            {
                GameProfile item = new GameProfile(name);
                ListBox_GameProfile.Items.Add(item);
                ListBox_GameProfile.SelectedIndex = ListBox_GameProfile.Items.Count - 1;
                Show_Panel(Panel_Screens_New, Button_Screens_New_Create, TextBox_Screens_New_Name);
            } else
            {
                Utilities.Flicker(TextBox_GameProfile_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
        }

        private void Button_Screens_New_Create_Click(object sender, EventArgs e)
        {
            string name = TextBox_Screens_New_Name.Text;
            name = name.Trim();
            ScreenType screenType = Selected_ScreenType_Radio_New();
            short width = (short)Numeric_Screens_New_Width.Value;
            short height = (short)Numeric_Screens_New_Height.Value;

            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(17943);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_Screens_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                Models.Screen item = new Models.Screen(name, screenType, new Geometry(width, height));
                var gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                gp.Screens.Add(item);
                Update_Screens();
                ListBox_Screens.SelectedIndex = ListBox_Screens.Items.Count - 1;
                TextBox_Screens_New_Name.Text = null;
                ScreenType_Radio_Manager_New();
                Numeric_Screens_New_Width.Value = 640;
                Numeric_Screens_New_Height.Value = 480;
                Clean_WatchZones_New();
                Show_Panel(Panel_WatchZones_New, Button_WatchZones_New_Add, TextBox_WatchZones_New_Name);
            }
        }

        private void Button_Screens_Update_Click(object sender, EventArgs e)
        {
            string name = TextBox_Screens_Name.Text;
            name = name.Trim();
            ScreenType screenType = Selected_ScreenType_Radio();
            short width = (short)Numeric_Screens_Width.Value;
            short height = (short)Numeric_Screens_Height.Value;

            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(46210);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(46211);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_Screens_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                var m = (Models.Screen)ListBox_Screens.SelectedItem;
                m.Name = name;
                m.ScreenType = screenType;
                m.Geometry = new Geometry(width, height);
                Update_Screens();
                ListBox_Screens.SelectedItem = m;
            }
        }

        private void Radio_Screens_New_Static_Click(object sender, EventArgs e) { ScreenType_Radio_Manager_New(ScreenType.Static); }
        private void Radio_Screens_New_Ratio_Click(object sender, EventArgs e) { ScreenType_Radio_Manager_New(ScreenType.Ratio); }
        private void Radio_Screens_New_Dynamic_Click(object sender, EventArgs e) { ScreenType_Radio_Manager_New(ScreenType.Dynamic); }

        private void Radio_Screens_Static_Click(object sender, EventArgs e) { ScreenType_Radio_Manager(ScreenType.Static); }
        private void Radio_Screens_Ratio_Click(object sender, EventArgs e) { ScreenType_Radio_Manager(ScreenType.Ratio); }
        private void Radio_Screens_Dynamic_Click(object sender, EventArgs e) { ScreenType_Radio_Manager(ScreenType.Dynamic); }

        private ScreenType Selected_ScreenType_Radio_New()
        {
            if (Radio_Screens_New_Static.Checked == true) return ScreenType.Static;
            else if (Radio_Screens_New_Ratio.Checked == true) return ScreenType.Ratio;
            else if (Radio_Screens_New_Dynamic.Checked == true) return ScreenType.Dynamic;
            else return ScreenType.Undefined;
        }

        private ScreenType Selected_ScreenType_Radio()
        {
            if (Radio_Screens_Static.Checked == true) return ScreenType.Static;
            else if (Radio_Screens_Ratio.Checked == true) return ScreenType.Ratio;
            else if (Radio_Screens_Dynamic.Checked == true) return ScreenType.Dynamic;
            else return ScreenType.Undefined;
        }

        private void ScreenType_Radio_Manager_New(ScreenType screenType = ScreenType.Undefined)
        {
            Radio_Screens_New_Static.Checked = false;
            Radio_Screens_New_Ratio.Checked = false;
            Radio_Screens_New_Dynamic.Checked = false;
            if (screenType != ScreenType.Undefined)
                switch (screenType)
                {
                    case ScreenType.Static: Radio_Screens_New_Static.Checked = true; break;
                    case ScreenType.Ratio: Radio_Screens_New_Ratio.Checked = true; break;
                    case ScreenType.Dynamic: Radio_Screens_New_Dynamic.Checked = true; break;
                }
        }

        private void ScreenType_Radio_Manager(ScreenType screenType = ScreenType.Undefined)
        {
            Radio_Screens_Static.Checked = false;
            Radio_Screens_Ratio.Checked = false;
            Radio_Screens_Dynamic.Checked = false;
            if (screenType != ScreenType.Undefined)
                switch (screenType)
                {
                    case ScreenType.Static: Radio_Screens_Static.Checked = true; break;
                    case ScreenType.Ratio: Radio_Screens_Ratio.Checked = true; break;
                    case ScreenType.Dynamic: Radio_Screens_Dynamic.Checked = true; break;
                }
        }

        private void Radio_WatchZones_New_TopLeft_Click(object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.TopLeft); }
        private void Radio_WatchZones_New_Top_Click    (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Top); }
        private void Radio_WatchZones_New_TopRight_Click(object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.TopRight); }
        private void Radio_WatchZones_New_Left_Click     (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Left); }
        private void Radio_WatchZones_New_Center_Click   (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.None); }
        private void Radio_WatchZones_New_Right_Click     (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Right); }
        private void Radio_WatchZones_New_BottomLeft_Click(object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.BottomLeft); }
        private void Radio_WatchZones_New_Bottom_Click    (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Bottom); }
        private void Radio_WatchZones_New_BottomRight_Click(object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.BottomRight); }

        private void Radio_WatchZones_TopLeft_Click(object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.TopLeft); }
        private void Radio_WatchZones_Top_Click    (object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.Top); }
        private void Radio_WatchZones_TopRight_Click(object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.TopRight); }
        private void Radio_WatchZones_Left_Click     (object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.Left); }
        private void Radio_WatchZones_Center_Click   (object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.None); }
        private void Radio_WatchZones_Right_Click     (object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.Right); }
        private void Radio_WatchZones_BottomLeft_Click(object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.BottomLeft); }
        private void Radio_WatchZones_Bottom_Click    (object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.Bottom); }
        private void Radio_WatchZones_BottomRight_Click(object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.BottomRight); }

        private Models.Anchor Selected_Anchor_Radio_New()
        {
            // C# is picky about switches...
                 if (Radio_WatchZones_New_TopLeft.Checked == true) return Models.Anchor.TopLeft;
            else if (Radio_WatchZones_New_Top    .Checked == true) return Models.Anchor.Top;
            else if (Radio_WatchZones_New_TopRight.Checked == true) return Models.Anchor.TopRight;
            else if (Radio_WatchZones_New_Left     .Checked == true) return Models.Anchor.Left;
            else if (Radio_WatchZones_New_Center   .Checked == true) return Models.Anchor.None;
            else if (Radio_WatchZones_New_Right     .Checked == true) return Models.Anchor.Right;
            else if (Radio_WatchZones_New_BottomLeft.Checked == true) return Models.Anchor.BottomLeft;
            else if (Radio_WatchZones_New_Bottom    .Checked == true) return Models.Anchor.Bottom;
            else if (Radio_WatchZones_New_BottomRight.Checked == true) return Models.Anchor.BottomRight;
                 else return Models.Anchor.Undefined;
        }

        private Models.Anchor Selected_Anchor_Radio()
        {
            // C# is picky about switches...
                 if (Radio_WatchZones_TopLeft.Checked == true) return Models.Anchor.TopLeft;
            else if (Radio_WatchZones_Top    .Checked == true) return Models.Anchor.Top;
            else if (Radio_WatchZones_TopRight.Checked == true) return Models.Anchor.TopRight;
            else if (Radio_WatchZones_Left     .Checked == true) return Models.Anchor.Left;
            else if (Radio_WatchZones_Center   .Checked == true) return Models.Anchor.None;
            else if (Radio_WatchZones_Right     .Checked == true) return Models.Anchor.Right;
            else if (Radio_WatchZones_BottomLeft.Checked == true) return Models.Anchor.BottomLeft;
            else if (Radio_WatchZones_Bottom    .Checked == true) return Models.Anchor.Bottom;
            else if (Radio_WatchZones_BottomRight.Checked == true) return Models.Anchor.BottomRight;
            else return Models.Anchor.Undefined;
        }

        private void Anchor_Radio_Manager_New(Models.Anchor anchor)
        {
            Radio_WatchZones_New_TopLeft.Checked = false;
            Radio_WatchZones_New_Top    .Checked = false;
            Radio_WatchZones_New_TopRight.Checked = false;
            Radio_WatchZones_New_Left     .Checked = false;
            Radio_WatchZones_New_Center   .Checked = false;
            Radio_WatchZones_New_Right     .Checked = false;
            Radio_WatchZones_New_BottomLeft.Checked = false;
            Radio_WatchZones_New_Bottom    .Checked = false;
            Radio_WatchZones_New_BottomRight.Checked = false;
            if (anchor != Models.Anchor.Undefined)
                switch (anchor) {
                    case Models.Anchor.TopLeft: Radio_WatchZones_New_TopLeft.Checked = true; break;
                    case Models.Anchor.Top:     Radio_WatchZones_New_Top    .Checked = true; break;
                    case Models.Anchor.TopRight: Radio_WatchZones_New_TopRight.Checked = true; break;
                    case Models.Anchor.Left:      Radio_WatchZones_New_Left     .Checked = true; break;
                    case Models.Anchor.None:    Radio_WatchZones_New_Center   .Checked = true; break;
                    case Models.Anchor.Right:      Radio_WatchZones_New_Right     .Checked = true; break;
                    case Models.Anchor.BottomLeft: Radio_WatchZones_New_BottomLeft.Checked = true; break;
                    case Models.Anchor.Bottom:     Radio_WatchZones_New_Bottom    .Checked = true; break;
                    case Models.Anchor.BottomRight: Radio_WatchZones_New_BottomRight.Checked = true; break;
                }
            Update_WatchZones_New_Preview();
        }

        private void Anchor_Radio_Manager(Models.Anchor anchor)
        {
            Radio_WatchZones_TopLeft.Checked = false;
            Radio_WatchZones_Top    .Checked = false;
            Radio_WatchZones_TopRight.Checked = false;
            Radio_WatchZones_Left     .Checked = false;
            Radio_WatchZones_Center   .Checked = false;
            Radio_WatchZones_Right     .Checked = false;
            Radio_WatchZones_BottomLeft.Checked = false;
            Radio_WatchZones_Bottom    .Checked = false;
            Radio_WatchZones_BottomRight.Checked = false;
            if (anchor != Models.Anchor.Undefined)
                switch (anchor)
                {
                    case Models.Anchor.TopLeft: Radio_WatchZones_TopLeft.Checked = true; break;
                    case Models.Anchor.Top:     Radio_WatchZones_Top    .Checked = true; break;
                    case Models.Anchor.TopRight: Radio_WatchZones_TopRight.Checked = true; break;
                    case Models.Anchor.Left:      Radio_WatchZones_Left     .Checked = true; break;
                    case Models.Anchor.None:    Radio_WatchZones_Center   .Checked = true; break;
                    case Models.Anchor.Right:      Radio_WatchZones_Right     .Checked = true; break;
                    case Models.Anchor.BottomLeft: Radio_WatchZones_BottomLeft.Checked = true; break;
                    case Models.Anchor.Bottom:     Radio_WatchZones_Bottom    .Checked = true; break;
                    case Models.Anchor.BottomRight: Radio_WatchZones_BottomRight.Checked = true; break;
                }
            Update_WatchZones_Preview();
        }

        private void Button_Minus_Screens_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(17946);
            }
            else if (ListBox_Screens.SelectedItem != null)
            {
                GameProfile gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                gp.Screens.Remove((Models.Screen)ListBox_Screens.SelectedItem);
                Update_Screens();
                Show_Panel(Panel_Screens_New, Button_Screens_New_Create, TextBox_Screens_New_Name);
            }
        }

        private void Clean_WatchZones_New()
        {
            var s = (Models.Screen)ListBox_Screens.SelectedItem;
            Models.Anchor anchor = Models.Anchor.TopLeft;
            if (s.ScreenType != ScreenType.Dynamic)
            {
                anchor = Models.Anchor.Undefined;
                Label_WatchZones_New_Anchor.Hide();
                TableLayoutPanel_WatchZones_New_Anchor.Hide();
            } else
            {
                Label_WatchZones_New_Anchor.Show();
                TableLayoutPanel_WatchZones_New_Anchor.Show();
            }
            Anchor_Radio_Manager_New(anchor);
            TextBox_WatchZones_New_Name.Text = null;
            Numeric_WatchZones_New_X.Value = 0;
            Numeric_WatchZones_New_Y.Value = 0;
            Numeric_WatchZones_New_Width.Value = 100;
            Numeric_WatchZones_New_Height.Value = 100;

            Box_WatchZones_New_Main.BackgroundImage = null;
            Box_WatchZones_New_Preview.BackColor = Color.Black;
            Button_WatchZones_New_SSAAI.Enabled = false;
        }

        private void Clean_WatchZones()
        {
            var s = (Models.Screen)ListBox_Screens.SelectedItem;
            Models.Anchor anchor = Models.Anchor.TopLeft;
            if (s.ScreenType != ScreenType.Dynamic)
            {
                anchor = Models.Anchor.Undefined;
                Label_WatchZones_Anchor.Hide();
                TableLayoutPanel_WatchZones_Anchor.Hide();
            } else
            {
                Label_WatchZones_Anchor.Show();
                TableLayoutPanel_WatchZones_Anchor.Show();
            }
            Anchor_Radio_Manager(anchor);
            TextBox_WatchZones_Name.Text = null;
            Numeric_WatchZones_X.Value = 0;
            Numeric_WatchZones_Y.Value = 0;
            Numeric_WatchZones_Width.Value = 100;
            Numeric_WatchZones_Height.Value = 100;

            Box_WatchZones_Main.BackgroundImage = null;
            Box_WatchZones_Preview.BackColor = Color.Black;
            //Button_WatchZones_SSAAI.Enabled = false;
        }

        private void Clean_Watches_New()
        {
            Anchor_Radio_Manager_New(Models.Anchor.TopLeft);
            TextBox_Watches_New_Name.Text = null;
            Numeric_Watches_New_Frequency.Value = 10;
            Numeric_Watches_New_Rescan_Range.Value = 1000;
            ComboBox_Watches_New_Rescan_Type.Text = "Both";
            PictureBox_Watches_New.Image = null;
            ListBox_Watches_New_Images.Items.Clear();

            var wz = (WatchZone)ListBox_WatchZones.SelectedItem;
            var ratio = wz.Geometry.Ratio;
            ratio = ratio / (200d / 120d);
            if (ratio > 1d)
            {
                PictureBox_Watches_New.Height = (int)(120 / ratio);
                PictureBox_Watches_New.Location = new Point(15, 18 + (int)Math.Round((120d - (120d / ratio)) / 2d));
            } else
            {
                PictureBox_Watches_New.Width = (int)(200 * ratio);
                PictureBox_Watches_New.Location = new Point(15 + (int)Math.Round((200d - (200d * ratio)) / 2d), 18);
            }
            PictureBox_Watches_New.BorderStyle = BorderStyle.FixedSingle;
        }

        private void Button_Plus_WatchZones_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(46212);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(46213);
            }
            else
            {
                Clean_WatchZones_New();
                Show_Panel(Panel_WatchZones_New, Button_WatchZones_New_Add, TextBox_WatchZones_New_Name);
            }
        }

        private void Button_WatchZones_New_Add_Click(object sender, EventArgs e)
        {
            string name = TextBox_WatchZones_New_Name.Text;
            name = name.Trim();
            Models.Anchor anchor = Selected_Anchor_Radio_New();
            double      x = (double)Numeric_WatchZones_New_X.Value; 
            double      y = (double)Numeric_WatchZones_New_Y.Value;
            double  width = (double)Numeric_WatchZones_New_Width.Value;
            double height = (double)Numeric_WatchZones_New_Height.Value;

            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(17944);
            }
            if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(17945);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_Screens_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                var s = (Models.Screen)ListBox_Screens.SelectedItem;
                Geometry geo;
                geo = new Geometry(x, y, width, height, anchor);
                var wz = new WatchZone(name, geo);
                s.WatchZones.Add(wz);
                Update_WatchZones();
                ListBox_WatchZones.SelectedIndex = ListBox_WatchZones.Items.Count - 1;
                Clean_WatchZones_New();
                Clean_Watches_New();
                Show_Watches();
                Show_Panel(Panel_Watches_New, Button_Watches_New_Create, TextBox_Watches_New_Name);
            }
        }

        private void Panel_WatchZones_New_VisibleChanged(object sender, EventArgs e)
        {
            if (Panel_WatchZones_New.Visible)
            {
                Update_WatchZones_New_Preview();
            }
        }

        private void Update_WatchZones_New_Preview() // Add async later
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(46214);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(46215);
            }
            else if (TableLayoutPanel_WatchZones_New.Width > 0 && Panel_WatchZones_New.Visible == true) // Stop minimize bug
            {
                GameProfile gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                int i = gp.Screens.IndexOf((Models.Screen)ListBox_Screens.SelectedItem);
                double screenWidth = gp.Screens[i].Geometry.Width;
                double screenHeight = gp.Screens[i].Geometry.Height;

                // Shouldn't be hard-coded (among other things...)
                double panelWidth  = Math.Min(TableLayoutPanel_WatchZones_New.Width  - 310d, screenWidth);
                double panelHeight = Math.Min(TableLayoutPanel_WatchZones_New.Height -   7d, screenHeight);

                double greaterDivisor = Math.Max(screenWidth / panelWidth, screenHeight / panelHeight);
                double newScreenWidth  = screenWidth  / greaterDivisor;
                double newScreenHeight = screenHeight / greaterDivisor;

                Box_WatchZones_New_Main.Size = new Size(
                    (int)newScreenWidth,
                    (int)newScreenHeight
                    );
                Box_WatchZones_New_Main.Location = new Point(
                    (int)((panelWidth - newScreenWidth) / 2d),
                    (int)((panelHeight - newScreenHeight) / 2d)
                    );

                float cellWidth = (float)newScreenWidth / TableLayoutPanel_WatchZones_New.Width * 100F + 7F;

                TableLayoutPanel_WatchZones_New.ColumnStyles[0].Width = cellWidth;
                TableLayoutPanel_WatchZones_New.ColumnStyles[1].Width = 100F - cellWidth;

                /****************************************/

                Models.Anchor anchor = Selected_Anchor_Radio_New();
                double      x = (double)Numeric_WatchZones_New_X.Value;
                double      y = (double)Numeric_WatchZones_New_Y.Value;
                double  width = (double)Numeric_WatchZones_New_Width.Value;
                double height = (double)Numeric_WatchZones_New_Height.Value;

                Size size = new Size();
                Point point = new Point();
                double scaleWidth  = newScreenWidth / screenWidth;
                double scaleHeight = newScreenHeight / screenHeight;

                //Box_WatchZones_New_Preview.Anchor = (AnchorStyles)anchor;

                if (anchor != Models.Anchor.Undefined)
                {
                         if ( anchor.HasFlag(Models.Anchor.Right))  { x = screenWidth  - x -  width; }
                    else if (!anchor.HasFlag(Models.Anchor.Left))   { x = screenWidth  / 2d + x -  width / 2d; }
                         if ( anchor.HasFlag(Models.Anchor.Bottom)) { y = screenHeight - y - height; }
                    else if (!anchor.HasFlag(Models.Anchor.Top))    { y = screenHeight / 2d + y - height / 2d; }
                }

                if (x < 0d) { x = screenWidth  + x; } // I don't think this even works for x and y.
                if (y < 0d) { y = screenHeight + y; }

                if ( width < 0d) {  width = screenWidth  +  width; }
                if (height < 0d) { height = screenHeight + height; }

                    point.X = (int)(Math.Round(scaleWidth  *      x));
                    point.Y = (int)(Math.Round(scaleHeight *      y));
                 size.Width = (int)(Math.Round(scaleWidth  *  width));
                size.Height = (int)(Math.Round(scaleHeight * height));

                Box_WatchZones_New_Preview.Location = point;
                Box_WatchZones_New_Preview.Size = size;
            }
        }

        private void Update_WatchZones_Preview()
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(46314);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(46315);
            }
            else if (TableLayoutPanel_WatchZones.Width > 0 && Panel_WatchZones.Visible == true) // Stops minimize bug
            {
                GameProfile gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                int i = gp.Screens.IndexOf((Models.Screen)ListBox_Screens.SelectedItem);
                double screenWidth = gp.Screens[i].Geometry.Width;
                double screenHeight = gp.Screens[i].Geometry.Height;

                // Shouldn't be hard-coded (among other things...)
                double panelWidth = Math.Min(TableLayoutPanel_WatchZones.Width - 310d, screenWidth);
                double panelHeight = Math.Min(TableLayoutPanel_WatchZones.Height - 7d, screenHeight);

                double greaterDivisor = Math.Max(screenWidth / panelWidth, screenHeight / panelHeight);
                double newScreenWidth = screenWidth / greaterDivisor;
                double newScreenHeight = screenHeight / greaterDivisor;

                Box_WatchZones_Main.Size = new Size(
                    (int)newScreenWidth,
                    (int)newScreenHeight
                    );
                Box_WatchZones_Main.Location = new Point(
                    (int)((panelWidth - newScreenWidth) / 2d),
                    (int)((panelHeight - newScreenHeight) / 2d)
                    );

                float cellWidth = (float)newScreenWidth / TableLayoutPanel_WatchZones.Width * 100F + 7F;

                TableLayoutPanel_WatchZones.ColumnStyles[0].Width = cellWidth;
                TableLayoutPanel_WatchZones.ColumnStyles[1].Width = 100F - cellWidth;

                /****************************************/

                Models.Anchor anchor = Selected_Anchor_Radio();
                double      x = (double)Numeric_WatchZones_X.Value;
                double      y = (double)Numeric_WatchZones_Y.Value;
                double  width = (double)Numeric_WatchZones_Width.Value;
                double height = (double)Numeric_WatchZones_Height.Value;

                Size size = new Size();
                Point point = new Point();
                double scaleWidth  = newScreenWidth  / screenWidth;
                double scaleHeight = newScreenHeight / screenHeight;

                //Box_WatchZones_Preview.Anchor = (AnchorStyles)anchor;

                if (anchor != Models.Anchor.Undefined)
                {
                         if ( anchor.HasFlag(Models.Anchor.Right))  { x = screenWidth  - x -  width; }
                    else if (!anchor.HasFlag(Models.Anchor.Left))   { x = screenWidth  / 2d + x -  width / 2d; }
                         if ( anchor.HasFlag(Models.Anchor.Bottom)) { y = screenHeight - y - height; }
                    else if (!anchor.HasFlag(Models.Anchor.Top))    { y = screenHeight / 2d + y - height / 2d; }
                }

                if (x < 0d) { x = screenWidth + x; } // I don't think this even works for x and y.
                if (y < 0d) { y = screenHeight + y; }

                if (width < 0d) { width = screenWidth + width; }
                if (height < 0d) { height = screenHeight + height; }

                point.X = (int)(Math.Round(scaleWidth * x));
                point.Y = (int)(Math.Round(scaleHeight * y));
                size.Width = (int)(Math.Round(scaleWidth * width));
                size.Height = (int)(Math.Round(scaleHeight * height));

                Box_WatchZones_Preview.Location = point;
                Box_WatchZones_Preview.Size = size;
            }
        }

        private void Button_Minus_WatchZones_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(18046);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(18046);
            }
            else if (ListBox_WatchZones.SelectedItem != null)
            {
                Models.Screen s = (Models.Screen)ListBox_Screens.SelectedItem;
                s.WatchZones.Remove((WatchZone)ListBox_WatchZones.SelectedItem);
                Update_WatchZones();
                Clean_WatchZones_New();
                Show_Panel(Panel_WatchZones_New, Button_WatchZones_New_Add, TextBox_WatchZones_New_Name);
            }
        }

        private void Button_WatchZones_Update_Click(object sender, EventArgs e)
        {
            string name = TextBox_WatchZones_Name.Text;
            name = name.Trim();
            Models.Anchor anchor = Selected_Anchor_Radio();
            short      x = (short)Numeric_WatchZones_X.Value;
            short      y = (short)Numeric_WatchZones_Y.Value;
            short  width = (short)Numeric_WatchZones_Width.Value;
            short height = (short)Numeric_WatchZones_Height.Value;

            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(48210);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(48211);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(48212);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_WatchZones_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                var wz = (WatchZone)ListBox_WatchZones.SelectedItem;
                wz.Name = name;
                wz.Geometry = new Geometry(x, y, width, height, anchor);
                Update_WatchZones();
                ListBox_WatchZones.SelectedItem = wz;
            }
        }

        private void CheckBox_Watches_New_Frequency_Click(object sender, EventArgs e)
        {
            Numeric_Watches_New_Frequency.Enabled = !CheckBox_Watches_New_Frequency.Checked;
        }

        private void CheckBox_Watches_Frequency_Click(object sender, EventArgs e)
        {
            Numeric_Watches_Frequency.Enabled = !CheckBox_Watches_Frequency.Checked;
        }

        private void ListBox_Watches_New_Images_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox_Watches_New_Images.SelectedItem != null)
            {
                var i = (WatchImage)ListBox_Watches_New_Images.SelectedItem;
                PictureBox_Watches_New.Image = i.Image;
                PictureBox_Watches_New.SizeMode = PictureBoxSizeMode.StretchImage;
                //PictureBox_Watches_New.Load();
            }
            else
            {
                PictureBox_Watches_New.Image = null;
            }
        }

        private void Button_Minus_Watches_New_Images_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(90001);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(90002);
            }
            else if (ListBox_WatchZones.SelectedItem == null)
            {
                Utilities.Error(90003);
            }
            else if (ListBox_Watches_New_Images.SelectedItem != null)
            {
                ListBox_Watches_New_Images.Items.Remove((WatchImage)ListBox_Watches_New_Images.SelectedItem);
                if (ListBox_Watches_New_Images.Items.Count > 0)
                {
                    ListBox_Watches_New_Images.SelectedIndex = ListBox_Watches_New_Images.Items.Count - 1;
                }
                //Update_Watches_New_Images();
            }
        }

        private void Button_Plus_Watches_New_Images_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Add one or more images",
                Filter = "Image files|*.png;*.jpg;*.jpeg|All files|*.*",
                Multiselect = true
            };
            ofd.ShowDialog();

            foreach (var f in ofd.FileNames)
            {
                var i = new WatchImage(f);
                ListBox_Watches_New_Images.Items.Add(i);
            }

            ListBox_Watches_New_Images.SelectedIndex = ListBox_Watches_New_Images.Items.Count - 1;

        }

        private void ListBox_Watches_Images_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox_Watches_Images.SelectedItem != null)
            {
                var i = (WatchImage)ListBox_Watches_Images.SelectedItem;
                PictureBox_Watches.Image = (Bitmap)i.Image;
                PictureBox_Watches.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                PictureBox_Watches.Image = null;
            }
        }

        private void Button_Minus_Watches_Images_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(90001);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(90002);
            }
            else if (ListBox_WatchZones.SelectedItem == null)
            {
                Utilities.Error(90003);
            }
            else if (ListBox_Watches_Images.SelectedItem != null)
            {
                ListBox_Watches_Images.Items.Remove((WatchImage)ListBox_Watches_Images.SelectedItem);
                if (ListBox_Watches_Images.Items.Count > 0)
                {
                    ListBox_Watches_Images.SelectedIndex = ListBox_Watches_Images.Items.Count - 1;
                }
                //Update_Watches_Images();
            }
        }

        private void Button_Plus_Watches_Images_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Add one or more images",
                Filter = "Image files|*.png;*.jpg;*.jpeg|All files|*.*",
                Multiselect = true
            };
            ofd.ShowDialog();

            foreach (var f in ofd.FileNames)
            {
                var i = new WatchImage(f);
                ListBox_Watches_Images.Items.Add(i);
            }

            ListBox_Watches_Images.SelectedIndex = ListBox_Watches_Images.Items.Count - 1;

        }

        private void Button_Watches_New_Create_Click(object sender, EventArgs e)
        {
            string name = TextBox_Watches_New_Name.Text;
            name = name.Trim();
            int frequency = (int)Numeric_Watches_New_Frequency.Value;
            ScanType rescanType;
            int rescanRange = (int)Numeric_Watches_New_Rescan_Range.Value;
            List<WatchImage> Images = new List<WatchImage>();

            if (ListBox_Watches_New_Images.Items.Count <= 0)
            {
                Utilities.Flicker(ListBox_Watches_New_Images, 500, Color.FromArgb(255, 64, 64));
            }
            else if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(27944);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(27945);
            }
            else if (ListBox_WatchZones.SelectedItem == null)
            {
                Utilities.Error(27945);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_Screens_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!ScanType.TryParse(ComboBox_Watches_New_Rescan_Type.Text, out rescanType))
            {
                Utilities.Flicker(ComboBox_Watches_New_Rescan_Type, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                var wz = (WatchZone)ListBox_WatchZones.SelectedItem;
                var w = new Watcher(name, frequency, rescanType, TimeSpan.FromMilliseconds(rescanRange));
                foreach (WatchImage i in ListBox_Watches_New_Images.Items)
                {
                    i.Clear();
                    w.Images.Add(i);
                }
                wz.Watches.Add(w);
                Update_Watches();
                ListBox_Watches.SelectedIndex = ListBox_Watches.Items.Count - 1;
                Clean_Watches_New();
            }
        }

        private void ListBox_Watches_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox_Watches_Images.Items.Clear();
            ListBox_Watches_New_Images.Items.Clear();
            if (ListBox_Watches.SelectedItem != null)
            {
                Show_Panel(Panel_Watches, Button_Watches_Update, TextBox_Watches_Name);
                var w = (Watcher)ListBox_Watches.SelectedItem;
                TextBox_Watches_Name.Text = w.Name;
                if (w.Frequency == null)
                {
                    CheckBox_Watches_Frequency.Checked = true;
                    Numeric_Watches_Frequency.Enabled = false;
                    Numeric_Watches_Frequency.Value = 0;
                }
                else
                {
                    CheckBox_Watches_Frequency.Checked = false;
                    Numeric_Watches_Frequency.Enabled = true;
                    Numeric_Watches_Frequency.Value = (decimal)w.Frequency;
                }
                Numeric_Watches_Rescan_Range.Value = (decimal)w.RescanRange.TotalMilliseconds;
                ComboBox_Watches_Rescan_Type.Text = w.RescanType.ToString();
                foreach (var i in w.Images)
                {
                    ListBox_Watches_Images.Items.Add(i);
                }
                ListBox_Watches_Images.SelectedIndex = ListBox_Watches_Images.Items.Count - 1;


                var wz = (WatchZone)ListBox_WatchZones.SelectedItem;
                var ratio = wz.Geometry.Ratio;
                ratio = ratio / (200d / 120d);
                if (ratio > 1d)
                {
                    PictureBox_Watches.Height = (int)(120 / ratio);
                    PictureBox_Watches.Location = new Point(15, 18 + (int)((120d - (120d / ratio)) / 2d));
                }
                else
                {
                    PictureBox_Watches.Width = (int)(200 * ratio);
                    PictureBox_Watches.Location = new Point(15 + (int)((200d - (200d * ratio)) / 2d), 18);
                }
            }
            else
            {
                Hide_Watches();
                ListBox_WatchZones_SelectedIndexChanged(null, null);
            }
        }

        private void Button_Minus_Watches_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(18057);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(18058);
            }
            else if (ListBox_WatchZones.SelectedItem == null)
            {
                Utilities.Error(18059);
            }
            else if (ListBox_Watches.SelectedItem != null)
            {
                WatchZone wz = (WatchZone)ListBox_WatchZones.SelectedItem;
                wz.Watches.Remove((Watcher)ListBox_Watches.SelectedItem);
                Update_Watches();
                Clean_Watches_New();
                Show_Panel(Panel_Watches_New, Button_Watches_New_Create, TextBox_Watches_New_Name);
            }
        }

        private void Button_Plus_Watches_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Utilities.Error(48212);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(48213);
            }
            else if (ListBox_WatchZones.SelectedItem == null)
            {
                Utilities.Error(48213);
            }
            else
            {
                Clean_Watches_New();
                Show_Panel(Panel_Watches_New, Button_Watches_New_Create, TextBox_Watches_New_Name);
            }
        }

        private void Button_WatchZones_New_URI_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select an image",
                Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp|All files|*.*",
                Multiselect = false
            };
            ofd.ShowDialog();

            if (File.Exists(ofd.FileName))
            {
                var i = new WatchImage(ofd.FileName);
                Box_WatchZones_New_Main.BackgroundImage = i.Image;
                Box_WatchZones_New_Preview.BackColor = Color.FromArgb(127, 255, 0, 255);
                Button_WatchZones_New_SSAAI.Enabled = true;
            }
        }

        private void Button_WatchZones_New_SSAAI_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                DefaultExt = "png",
                Filter = "PNG Image|*.png|JPEG Image|*.jpg",
                Title = "Save Image"
            };
            sfd.ShowDialog();

            var image = Box_WatchZones_New_Main.BackgroundImage;

            GameProfile gp = (GameProfile)ListBox_GameProfile.SelectedItem;
            int i = gp.Screens.IndexOf((Models.Screen)ListBox_Screens.SelectedItem);
            double screenWidth = gp.Screens[i].Geometry.Width;
            double screenHeight = gp.Screens[i].Geometry.Height;

            Models.Anchor anchor = Selected_Anchor_Radio_New();
            double      x = (double)Numeric_WatchZones_New_X.Value;
            double      y = (double)Numeric_WatchZones_New_Y.Value;
            double  width = (double)Numeric_WatchZones_New_Width.Value;
            double height = (double)Numeric_WatchZones_New_Height.Value;

            double scaleWidth = image.Width / screenWidth;
            double scaleHeight = image.Height / screenHeight;

            if (anchor != Models.Anchor.Undefined)
            {
                if (anchor.HasFlag(Models.Anchor.Right)) { x = screenWidth - x - width; }
                else if (!anchor.HasFlag(Models.Anchor.Left)) { x = screenWidth / 2d + x - width / 2d; }
                if (anchor.HasFlag(Models.Anchor.Bottom)) { y = screenHeight - y - height; }
                else if (!anchor.HasFlag(Models.Anchor.Top)) { y = screenHeight / 2d + y - height / 2d; }
            }

            if (x < 0d) { x = screenWidth + x; } // I don't think this even works for x and y.
            if (y < 0d) { y = screenHeight + y; }

            if (width < 0d) { width = screenWidth + width; }
            if (height < 0d) { height = screenHeight + height; }

                 x *= scaleWidth;
                 y *= scaleHeight;
             width *= scaleWidth;
            height *= scaleHeight;

            var image2 = new MagickImage((Bitmap)image);
            // Wanted to do distorted rescaling. Don't think the library can do it.
            var geo = new MagickGeometry((int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(width), (int)Math.Round(height));
            image2.Crop(geo, Gravity.Northwest);
            image2.Write(sfd.FileName);
        }

        private void Button_WatchZones_URI_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select an image",
                Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp|All files|*.*",
                Multiselect = false
            };
            ofd.ShowDialog();

            if (File.Exists(ofd.FileName))
            {
                var i = new WatchImage(ofd.FileName);
                Box_WatchZones_Main.BackgroundImage = i.Image;
                Box_WatchZones_Preview.BackColor = Color.FromArgb(127, 255, 0, 255);
                Button_WatchZones_SSAAI.Enabled = true;
            }
        }

        private void Button_WatchZones_SSAAI_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                DefaultExt = "png",
                Filter = "PNG Image|*.png|JPEG Image|*.jpg",
                Title = "Save Image"
            };
            sfd.ShowDialog();

            var image = Box_WatchZones_Main.BackgroundImage;

            GameProfile gp = (GameProfile)ListBox_GameProfile.SelectedItem;
            int i = gp.Screens.IndexOf((Models.Screen)ListBox_Screens.SelectedItem);
            double screenWidth = gp.Screens[i].Geometry.Width;
            double screenHeight = gp.Screens[i].Geometry.Height;

            Models.Anchor anchor = Selected_Anchor_Radio();
            double x = (double)Numeric_WatchZones_X.Value;
            double y = (double)Numeric_WatchZones_Y.Value;
            double width = (double)Numeric_WatchZones_Width.Value;
            double height = (double)Numeric_WatchZones_Height.Value;

            double scaleWidth = image.Width / screenWidth;
            double scaleHeight = image.Height / screenHeight;

            if (anchor != Models.Anchor.Undefined)
            {
                if (anchor.HasFlag(Models.Anchor.Right)) { x = screenWidth - x - width; }
                else if (!anchor.HasFlag(Models.Anchor.Left)) { x = screenWidth / 2d + x - width / 2d; }
                if (anchor.HasFlag(Models.Anchor.Bottom)) { y = screenHeight - y - height; }
                else if (!anchor.HasFlag(Models.Anchor.Top)) { y = screenHeight / 2d + y - height / 2d; }
            }

            if (x < 0d) { x = screenWidth + x; } // I don't think this even works for x and y.
            if (y < 0d) { y = screenHeight + y; }

            if (width < 0d) { width = screenWidth + width; }
            if (height < 0d) { height = screenHeight + height; }

            x *= scaleWidth;
            y *= scaleHeight;
            width *= scaleWidth;
            height *= scaleHeight;

            var image2 = new MagickImage((Bitmap)image);
            // Wanted to do distorted rescaling. Don't think the library can do it.
            var geo = new MagickGeometry((int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(width), (int)Math.Round(height));
            image2.Crop(geo, Gravity.Northwest);
            image2.Write(sfd.FileName);
        }

        private void Numeric_WatchZones_New_X_ValueChanged(object sender, EventArgs e)
        {
            Update_WatchZones_New_Preview();
        }
        private void Numeric_WatchZones_New_Y_ValueChanged(object sender, EventArgs e)
        {
            Update_WatchZones_New_Preview();
        }
        private void Numeric_WatchZones_New_Width_ValueChanged(object sender, EventArgs e)
        {
            Update_WatchZones_New_Preview();
        }
        private void Numeric_WatchZones_New_Height_ValueChanged(object sender, EventArgs e)
        {
            Update_WatchZones_New_Preview();
        }
        private void Numeric_WatchZones_X_ValueChanged(object sender, EventArgs e)
        {
            Update_WatchZones_Preview();
        }
        private void Numeric_WatchZones_Y_ValueChanged(object sender, EventArgs e)
        {
            Update_WatchZones_Preview();
        }
        private void Numeric_WatchZones_Width_ValueChanged(object sender, EventArgs e)
        {
            Update_WatchZones_Preview();
        }
        private void Numeric_WatchZones_Height_ValueChanged(object sender, EventArgs e)
        {
            Update_WatchZones_Preview();
        }

        // I kind of figured this out right at the end...
        private void TextBox_Auto_Selector(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void ComboBox_Auto_Selector(object sender, EventArgs e)
        {
            ((ComboBox)sender).SelectAll();
        }

        private void Numeric_Auto_Selector(object sender, EventArgs e)
        {
            int len = ((NumericUpDown)sender).Value.ToString().Length;
            ((NumericUpDown)sender).Select(0, len);
        }

    }
}
