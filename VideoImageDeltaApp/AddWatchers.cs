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
                ListBox_GameProfiles.Items.Add(gp);

            if (ListBox_GameProfiles.Items.Count > 0)
            {
                ListBox_GameProfiles.SelectedIndex = 0;
            }

        }

        public GameProfile SelectedGameProfile
        {
            get
            {
                if (ListBox_GameProfiles.SelectedItem != null)
                {
                    return (GameProfile)ListBox_GameProfiles.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ListBox_GameProfiles.SelectedItem = value;
            }
        }

        public Models.Screen SelectedScreen
        {
            get
            {
                if (ListBox_Screens.SelectedItem != null)
                {
                    return (Models.Screen)ListBox_Screens.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ListBox_Screens.SelectedItem = value;
            }
        }

        public WatchZone SelectedWatchZone
        {
            get
            {
                if (ListBox_WatchZones.SelectedItem != null)
                {
                    return (WatchZone)ListBox_WatchZones.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ListBox_WatchZones.SelectedItem = value;
            }
        }

        public Watcher SelectedWatcher
        {
            get
            {
                if (ListBox_Watches.SelectedItem != null)
                {
                    return (Watcher)ListBox_Watches.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ListBox_Watches.SelectedItem = value;
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

        private void Update_GameProfiles()
        {
            var selectedGP = SelectedGameProfile;
            var gps = new List<GameProfile>();
            foreach (GameProfile gp in ListBox_GameProfiles.Items)
            {
                gps.Add(gp);
            }
            ListBox_Watches.Items.Clear();
            ListBox_WatchZones.Items.Clear();
            ListBox_Screens.Items.Clear();
            ListBox_GameProfiles.Items.Clear();
            foreach (var gp in gps)
            {
                ListBox_GameProfiles.Items.Add(gp);
            }
            int i = ListBox_GameProfiles.Items.IndexOf(selectedGP);
            ListBox_GameProfiles.SelectedIndex = i;
        }

        private void Update_Screens()
        {
            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(17945);
            }
            else
            {
                ListBox_Watches.Items.Clear();
                ListBox_WatchZones.Items.Clear();
                ListBox_Screens.Items.Clear();
                var gp = SelectedGameProfile;
                foreach (Models.Screen s in gp.Screens)
                {
                    ListBox_Screens.Items.Add(s);
                }
            }
        }

        private void Update_WatchZones()
        {
            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(17952);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(17953);
            }
            else
            {
                ListBox_Watches.Items.Clear();
                ListBox_WatchZones.Items.Clear();
                foreach (WatchZone wz in SelectedScreen.WatchZones)
                {
                    ListBox_WatchZones.Items.Add(wz);
                }
            }
        }

        private void Update_Watches()
        {
            if (ListBox_GameProfiles.SelectedItem == null)
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
                var wz = SelectedWatchZone;
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
            foreach (GameProfile gp in ListBox_GameProfiles.Items)
                Program.GameProfiles.Add(gp);
        }

        private void Button_Plus_GameProfile_Click(object sender, EventArgs e)
        {
            TextBox_GameProfile_New_Name.Text = null;
            Show_Panel(Panel_GameProfile_New, Button_GameProfile_New_Create, TextBox_GameProfile_New_Name);
        }

        private void Button_Import_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "XML Files|*.xml",
                Title = "Select an XML File",
                Multiselect = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var gps = new List<GameProfile>();
                foreach (var f in ofd.FileNames)
                {
                    GameProfile gp;
                    XmlSerializer serializer = new XmlSerializer(typeof(GameProfile));
                    using (StreamReader reader = new StreamReader(f))
                    {
                        gp = (GameProfile)serializer.Deserialize(reader);
                    }
                    ListBox_GameProfiles.Items.Add(gp);
                }
                ListBox_GameProfiles.SelectedIndex = ListBox_GameProfiles.Items.Count - 1;
                Update_GameProfiles();
            }

            ofd.Dispose();
        }

        private void Button_Minus_GameProfile_Click(object sender, EventArgs e)
        {
            // I tried to make this a function that can be applied to other methods. It didn't work. It wouldn't accept Type for the overload.
            // Otherwise I don't know how to get it to work without passing through an unnecessary amount of variables.
            if (ListBox_GameProfiles.SelectedItem != null)
            {
                ListBox_Watches.Items.Clear();
                ListBox_WatchZones.Items.Clear();
                ListBox_Screens.Items.Clear();
                Hide_Screens();
                ListBox_GameProfiles.Items.RemoveAt(ListBox_GameProfiles.SelectedIndex);
            }
        }

        private void ListBox_GameProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox_GameProfiles.SelectedItem != null)
            {
                Show_Screens();
                Show_Panel(Panel_GameProfile, Button_GameProfile_Rename, TextBox_GameProfile_Name);
                var gp = SelectedGameProfile;
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
                TextBox_Screens_Name.Text = SelectedScreen.Name;
                Numeric_Screens_Width.Value = (decimal)SelectedScreen.Geometry.Width;
                Numeric_Screens_Height.Value = (decimal)SelectedScreen.Geometry.Height;
                CheckBox_Screens_Advanced.Checked = SelectedScreen.UseAdvanced;
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
                var wz = SelectedWatchZone;
                TextBox_WatchZones_Name.Text = wz.Name;
                ScaleType_Radio_Manager(wz.ScaleType);
                Numeric_WatchZones_X.Value = (decimal)wz.Geometry.X;
                Numeric_WatchZones_Y.Value = (decimal)wz.Geometry.Y;
                Numeric_WatchZones_Width.Value = (decimal)wz.Geometry.Width;
                Numeric_WatchZones_Height.Value = (decimal)wz.Geometry.Height;
                if (!SelectedScreen.UseAdvanced)
                {
                    GroupBox_WatchZones_ScaleType.Hide();
                    Label_WatchZones_Anchor.Hide();
                    TableLayoutPanel_WatchZones_Anchor.Hide();
                }
                else
                {
                    GroupBox_WatchZones_ScaleType.Show();
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
            if (ListBox_GameProfiles.SelectedItem != null)
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    DefaultExt = "xml",
                    Filter = "XML Files|*.xml",
                    FileName = ListBox_GameProfiles.SelectedItem.ToString(), // Might not always work...
                    Title = "Save Game Profile",
                };
                sfd.ShowDialog();

                if (sfd.FileName != "")
                {
                    Type t = typeof(GameProfile);
                    XmlSerializer serializer = new XmlSerializer(t);
                    using (TextWriter writer = new StreamWriter(sfd.FileName))
                    {
                        serializer.Serialize(writer, SelectedGameProfile);
                    }

                }

                sfd.Dispose();
            }
            else
            {
                Utilities.Flicker(ListBox_GameProfiles, 500, Color.FromArgb(224, 64, 64));
            }
        }

        private void Button_Plus_Screens_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(46208);
            }
            else
            {
                TextBox_Screens_New_Name.Text = null;
                CheckBox_Screens_New_Advanced.Checked = false;
                Numeric_Screens_New_Width.Value = 640m;
                Numeric_Screens_New_Height.Value = 480m;
                Show_Panel(Panel_Screens_New, Button_Screens_New_Create, TextBox_Screens_New_Name);
            }
        }

        private void Button_GameProfile_Rename_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(46209);
            }
            else
            {
                var text = TextBox_GameProfile_Name.Text.Trim();
                var gp = SelectedGameProfile;
                if (!String.IsNullOrWhiteSpace(text))
                {
                    gp.Name = text;
                }
                SelectedGameProfile = gp;
                Update_GameProfiles();
                ListBox_GameProfiles.Select();
            }
        }

        private void Button_GameProfile_New_Create_Click(object sender, EventArgs e)
        {
            string name = TextBox_GameProfile_New_Name.Text;
            name = name.Trim();
            if (!String.IsNullOrWhiteSpace(name))
            {
                GameProfile item = new GameProfile(name);
                ListBox_GameProfiles.Items.Add(item);
                ListBox_GameProfiles.SelectedIndex = ListBox_GameProfiles.Items.Count - 1;
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
            bool useAdvanced = CheckBox_Screens_New_Advanced.Checked;
            double width = (double)Numeric_Screens_New_Width.Value;
            double height = (double)Numeric_Screens_New_Height.Value;

            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(17943);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_Screens_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                Models.Screen item;
                item = new Models.Screen(name, useAdvanced, new GeometryOld(width, height));
                var gp = SelectedGameProfile;
                gp.Screens.Add(item);
                Update_Screens();
                ListBox_Screens.SelectedIndex = ListBox_Screens.Items.Count - 1;
                TextBox_Screens_New_Name.Text = null;
                CheckBox_Screens_New_Advanced.Checked = false;
                Numeric_Screens_New_Width.Value = 640m;
                Numeric_Screens_New_Height.Value = 480m;
                Clean_WatchZones_New();
                Show_Panel(Panel_WatchZones_New, Button_WatchZones_New_Add, TextBox_WatchZones_New_Name);
            }
        }

        private void Button_Screens_Update_Click(object sender, EventArgs e)
        {
            string name = TextBox_Screens_Name.Text;
            name = name.Trim();
            bool useAdvanced = CheckBox_Screens_Advanced.Checked;
            double width = (double)Numeric_Screens_Width.Value;
            double height = (double)Numeric_Screens_Height.Value;

            if (ListBox_GameProfiles.SelectedItem == null)
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
                var s = SelectedScreen;
                s.Name = name;
                s.UseAdvanced = useAdvanced;
                s.Geometry = new GeometryOld(width, height);
                Update_Screens();
                ListBox_Screens.SelectedItem = s;
            }
        }

        private void Radio_WatchZones_New_NoScale_Click(object sender, EventArgs e) { ScaleType_Radio_Manager_New(ScaleType.NoScale); }
        private void Radio_WatchZones_New_KeepRatio_Click(object sender, EventArgs e) { ScaleType_Radio_Manager_New(ScaleType.KeepRatio); }
        private void Radio_WatchZones_New_Scale_Click(object sender, EventArgs e) { ScaleType_Radio_Manager_New(ScaleType.Scale); }

        private void Radio_WatchZones_NoScale_Click(object sender, EventArgs e) { ScaleType_Radio_Manager(ScaleType.NoScale); }
        private void Radio_WatchZones_KeepRatio_Click(object sender, EventArgs e) { ScaleType_Radio_Manager(ScaleType.KeepRatio); }
        private void Radio_WatchZones_Scale_Click(object sender, EventArgs e) { ScaleType_Radio_Manager(ScaleType.Scale); }

        private ScaleType Selected_ScaleType_Radio_New()
        {
            if (Radio_WatchZones_New_NoScale.Checked == true) return ScaleType.NoScale;
            else if (Radio_WatchZones_New_KeepRatio.Checked == true) return ScaleType.KeepRatio;
            else if (Radio_WatchZones_New_Scale.Checked == true) return ScaleType.Scale;
            else return ScaleType.Undefined;
        }

        private ScaleType Selected_ScaleType_Radio()
        {
            if (Radio_WatchZones_NoScale.Checked == true) return ScaleType.NoScale;
            else if (Radio_WatchZones_KeepRatio.Checked == true) return ScaleType.KeepRatio;
            else if (Radio_WatchZones_Scale.Checked == true) return ScaleType.Scale;
            else return ScaleType.Undefined;
        }

        private void ScaleType_Radio_Manager_New(ScaleType ScaleType = ScaleType.Undefined)
        {
            if (ScaleType == ScaleType.Undefined || ScaleType == ScaleType.Scale)
            {
                Label_WatchZones_New_Anchor.Hide();
                TableLayoutPanel_WatchZones_New_Anchor.Hide();
                Anchor_Radio_Manager_New(Models.Anchor.Undefined);
            }
            else
            {
                Label_WatchZones_New_Anchor.Show();
                TableLayoutPanel_WatchZones_New_Anchor.Show();
                if (Selected_Anchor_Radio_New() == Models.Anchor.Undefined)
                    Anchor_Radio_Manager_New(Models.Anchor.TopLeft);
            }

            Radio_WatchZones_New_NoScale.Checked = false;
            Radio_WatchZones_New_KeepRatio.Checked = false;
            Radio_WatchZones_New_Scale.Checked = false;
            if (ScaleType != ScaleType.Undefined)
                switch (ScaleType)
                {
                    case ScaleType.NoScale: Radio_WatchZones_New_NoScale.Checked = true; break;
                    case ScaleType.KeepRatio: Radio_WatchZones_New_KeepRatio.Checked = true; break;
                    case ScaleType.Scale: Radio_WatchZones_New_Scale.Checked = true; break;
                }
        }

        private void ScaleType_Radio_Manager(ScaleType ScaleType = ScaleType.Undefined)
        {
            if (ScaleType == ScaleType.Undefined || ScaleType == ScaleType.Scale)
            {
                Label_WatchZones_Anchor.Hide();
                TableLayoutPanel_WatchZones_Anchor.Hide();
                Anchor_Radio_Manager(Models.Anchor.Undefined);
            }
            else
            {
                Label_WatchZones_Anchor.Show();
                TableLayoutPanel_WatchZones_Anchor.Show();
                if (Selected_Anchor_Radio() == Models.Anchor.Undefined)
                    Anchor_Radio_Manager(Models.Anchor.TopLeft);
            }

            Radio_WatchZones_NoScale.Checked = false;
            Radio_WatchZones_KeepRatio.Checked = false;
            Radio_WatchZones_Scale.Checked = false;
            if (ScaleType != ScaleType.Undefined)
                switch (ScaleType)
                {
                    case ScaleType.NoScale: Radio_WatchZones_NoScale.Checked = true; break;
                    case ScaleType.KeepRatio: Radio_WatchZones_KeepRatio.Checked = true; break;
                    case ScaleType.Scale: Radio_WatchZones_Scale.Checked = true; break;
                }
        }

        private void Radio_WatchZones_New_TopLeft_Click(object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.TopLeft); }
        private void Radio_WatchZones_New_Top_Click    (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Top); }
        private void Radio_WatchZones_New_TopRight_Click(object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.TopRight); }
        private void Radio_WatchZones_New_Left_Click     (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Left); }
        private void Radio_WatchZones_New_Center_Click   (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Center); }
        private void Radio_WatchZones_New_Right_Click     (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Right); }
        private void Radio_WatchZones_New_BottomLeft_Click(object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.BottomLeft); }
        private void Radio_WatchZones_New_Bottom_Click    (object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.Bottom); }
        private void Radio_WatchZones_New_BottomRight_Click(object sender, EventArgs e) { Anchor_Radio_Manager_New(Models.Anchor.BottomRight); }

        private void Radio_WatchZones_TopLeft_Click(object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.TopLeft); }
        private void Radio_WatchZones_Top_Click    (object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.Top); }
        private void Radio_WatchZones_TopRight_Click(object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.TopRight); }
        private void Radio_WatchZones_Left_Click     (object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.Left); }
        private void Radio_WatchZones_Center_Click   (object sender, EventArgs e) { Anchor_Radio_Manager(Models.Anchor.Center); }
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
            else if (Radio_WatchZones_New_Center   .Checked == true) return Models.Anchor.Center;
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
            else if (Radio_WatchZones_Center   .Checked == true) return Models.Anchor.Center;
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
                    case Models.Anchor.Center:    Radio_WatchZones_New_Center   .Checked = true; break;
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
                    case Models.Anchor.Center:    Radio_WatchZones_Center   .Checked = true; break;
                    case Models.Anchor.Right:      Radio_WatchZones_Right     .Checked = true; break;
                    case Models.Anchor.BottomLeft: Radio_WatchZones_BottomLeft.Checked = true; break;
                    case Models.Anchor.Bottom:     Radio_WatchZones_Bottom    .Checked = true; break;
                    case Models.Anchor.BottomRight: Radio_WatchZones_BottomRight.Checked = true; break;
                }
            Update_WatchZones_Preview();
        }

        private void Button_Minus_Screens_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(17946);
            }
            else if (ListBox_Screens.SelectedItem != null)
            {
                GameProfile gp = SelectedGameProfile;
                gp.Screens.Remove((Models.Screen)ListBox_Screens.SelectedItem);
                Update_Screens();
                Hide_WatchZones();
                Show_Panel(Panel_Screens_New, Button_Screens_New_Create, TextBox_Screens_New_Name);
            }
        }

        private void Clean_WatchZones_New()
        {
            if (!SelectedScreen.UseAdvanced)
            {
                ScaleType_Radio_Manager_New(ScaleType.Undefined);
                GroupBox_WatchZones_New_ScaleType.Hide();
            }
            else
            {
                ScaleType_Radio_Manager_New(ScaleType.Scale);
                GroupBox_WatchZones_New_ScaleType.Show();
                //Anchor_Radio_Manager_New(Models.Anchor.TopLeft);
            }

            Anchor_Radio_Manager_New(Models.Anchor.Undefined);
            Label_WatchZones_New_Anchor.Hide();
            TableLayoutPanel_WatchZones_New_Anchor.Hide();

            TextBox_WatchZones_New_Name.Text = null;
            Numeric_WatchZones_New_X.Value = 0m;
            Numeric_WatchZones_New_Y.Value = 0m;
            Numeric_WatchZones_New_Width.Value = 100m;
            Numeric_WatchZones_New_Height.Value = 100m;

            Box_WatchZones_New_Main.BackgroundImage = null;
            Box_WatchZones_New_Preview.BackColor = Color.Black;
            Button_WatchZones_New_SSAAI.Enabled = false;
        }

        private void Clean_WatchZones()
        {
            if (!SelectedScreen.UseAdvanced)
            {
                ScaleType_Radio_Manager(ScaleType.Undefined);
                GroupBox_WatchZones_ScaleType.Hide();

                Anchor_Radio_Manager(Models.Anchor.Undefined);
                Label_WatchZones_Anchor.Hide();
                TableLayoutPanel_WatchZones_Anchor.Hide();
            }
            else
            {
                ScaleType_Radio_Manager(ScaleType.Scale);
                GroupBox_WatchZones_ScaleType.Show();

                Anchor_Radio_Manager(Models.Anchor.TopLeft);
                Label_WatchZones_Anchor.Show();
                TableLayoutPanel_WatchZones_Anchor.Show();
            }

            TextBox_WatchZones_Name.Text = null;
            Numeric_WatchZones_X.Value = 0m;
            Numeric_WatchZones_Y.Value = 0m;
            Numeric_WatchZones_Width.Value = 100m;
            Numeric_WatchZones_Height.Value = 100m;

            Box_WatchZones_Main.BackgroundImage = null;
            Box_WatchZones_Preview.BackColor = Color.Black;
            //Button_WatchZones_SSAAI.Enabled = false; // Why did I comment this out?
        }

        private void Clean_Watches_New()
        {
            Anchor_Radio_Manager_New(Models.Anchor.TopLeft);
            TextBox_Watches_New_Name.Text = null;
            Numeric_Watches_New_Frequency.Value = 10;
            PictureBox_Watches_New.Image = null;
            ListBox_Watches_New_Images.Items.Clear();

            var wz = SelectedWatchZone;
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
            if (ListBox_GameProfiles.SelectedItem == null)
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
            ScaleType scaleType;
            Models.Anchor anchor;
            double      x = (double)Numeric_WatchZones_New_X.Value; 
            double      y = (double)Numeric_WatchZones_New_Y.Value;
            double  width = (double)Numeric_WatchZones_New_Width.Value;
            double height = (double)Numeric_WatchZones_New_Height.Value;

            if (SelectedScreen.UseAdvanced)
            {
                scaleType = Selected_ScaleType_Radio_New();
                if (scaleType != ScaleType.Scale)
                {
                    anchor = Selected_Anchor_Radio_New();
                }
                else
                {
                    anchor = Models.Anchor.Undefined;
                }
            }
            else
            {
                scaleType = ScaleType.Undefined;
                anchor = Models.Anchor.Undefined;
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_Screens_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                var s = SelectedScreen;
                GeometryOld geo;
                geo = new GeometryOld(x, y, width, height, anchor);
                var wz = new WatchZone(name, scaleType, geo);
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
            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(46214);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(46215);
            }
            else if (TableLayoutPanel_WatchZones_New.Visible && TableLayoutPanel_WatchZones_New.Width > 0 && Panel_WatchZones_New.Visible == true) // Stop minimize bug
            {
                double screenWidth = 640d;
                double screenHeight = 480d;
                if (!SelectedScreen.Geometry.HasSize())
                {
                    screenWidth = SelectedScreen.Geometry.Width;
                    screenHeight = SelectedScreen.Geometry.Height;
                }

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
                         if ( anchor.HasFlag(Models.Anchor.Right))  { x = screenWidth  + x -  width; }
                    else if (!anchor.HasFlag(Models.Anchor.Left))   { x = screenWidth  / 2d + x -  width / 2d; }
                         if ( anchor.HasFlag(Models.Anchor.Bottom)) { y = screenHeight + y - height; }
                    else if (!anchor.HasFlag(Models.Anchor.Top))    { y = screenHeight / 2d + y - height / 2d; }
                }

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
            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(46314);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(46315);
            }
            else if (TableLayoutPanel_WatchZones.Visible && TableLayoutPanel_WatchZones.Width > 0 && Panel_WatchZones.Visible == true) // Stops minimize bug
            {
                double screenWidth = 640d;
                double screenHeight = 480d;
                if (!SelectedScreen.Geometry.HasSize())
                {
                    screenWidth = SelectedScreen.Geometry.Width;
                    screenHeight = SelectedScreen.Geometry.Height;
                }

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
                         if ( anchor.HasFlag(Models.Anchor.Right))  { x = screenWidth  + x -  width; }
                    else if (!anchor.HasFlag(Models.Anchor.Left))   { x = screenWidth  / 2d + x -  width / 2d; }
                         if ( anchor.HasFlag(Models.Anchor.Bottom)) { y = screenHeight + y - height; }
                    else if (!anchor.HasFlag(Models.Anchor.Top))    { y = screenHeight / 2d + y - height / 2d; }
                }

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
            if (ListBox_GameProfiles.SelectedItem == null)
            {
                Utilities.Error(18046);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Utilities.Error(18046);
            }
            else if (ListBox_WatchZones.SelectedItem != null)
            {
                Models.Screen s = SelectedScreen;
                s.WatchZones.Remove((WatchZone)ListBox_WatchZones.SelectedItem);
                Update_WatchZones();
                Clean_WatchZones_New();
                Hide_Watches();
                Show_Panel(Panel_WatchZones_New, Button_WatchZones_New_Add, TextBox_WatchZones_New_Name);
            }
        }

        private void Button_WatchZones_Update_Click(object sender, EventArgs e)
        {
            string name = TextBox_WatchZones_Name.Text;
            name = name.Trim();
            ScaleType scaleType;
            Models.Anchor anchor;
            double x = (double)Numeric_WatchZones_X.Value;
            double y = (double)Numeric_WatchZones_Y.Value;
            double width = (double)Numeric_WatchZones_Width.Value;
            double height = (double)Numeric_WatchZones_Height.Value;

            if (SelectedScreen.UseAdvanced)
            {
                scaleType = Selected_ScaleType_Radio();
                if (scaleType != ScaleType.Scale)
                {
                    anchor = Selected_Anchor_Radio();
                }
                else
                {
                    anchor = Models.Anchor.Undefined;
                }
            }
            else
            {
                scaleType = ScaleType.Undefined;
                anchor = Models.Anchor.Undefined;
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_Screens_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                var wz = SelectedWatchZone;
                wz.Name = name;
                wz.ScaleType = scaleType;
                wz.Geometry = new GeometryOld(x, y, width, height, anchor);
                //Update_WatchZones();
                ListBox_WatchZones.SelectedItem = wz;
            }
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
            if (ListBox_GameProfiles.SelectedItem == null)
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
            if (ListBox_GameProfiles.SelectedItem == null)
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
            double frequency = (double)Numeric_Watches_New_Frequency.Value;
            List<WatchImage> Images = new List<WatchImage>();

            if (ListBox_Watches_New_Images.Items.Count <= 0)
            {
                Utilities.Flicker(ListBox_Watches_New_Images, 500, Color.FromArgb(255, 64, 64));
            }
            else if (ListBox_GameProfiles.SelectedItem == null)
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
            else
            {
                var wz = SelectedWatchZone;
                var w = new Watcher(name, frequency);
                foreach (WatchImage i in ListBox_Watches_New_Images.Items)
                {
                    i.Clear();
                    w.WatchImages.Add(i);
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
                var w = SelectedWatcher;
                TextBox_Watches_Name.Text = w.Name;
                Numeric_Watches_Frequency.Value = (decimal)w.Frequency;
                foreach (var i in w.WatchImages)
                {
                    ListBox_Watches_Images.Items.Add(i);
                }
                ListBox_Watches_Images.SelectedIndex = ListBox_Watches_Images.Items.Count - 1;


                var wz = SelectedWatchZone;
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
            if (ListBox_GameProfiles.SelectedItem == null)
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
                ListBox_Watches.Items.Remove((Watcher)ListBox_Watches.SelectedItem);
                Update_Watches();
                Clean_Watches_New();
                Show_Panel(Panel_Watches_New, Button_Watches_New_Create, TextBox_Watches_New_Name);
            }
        }

        private void Button_Plus_Watches_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfiles.SelectedItem == null)
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

            if (sfd.FileName != "")
            {
                var image = Box_WatchZones_New_Main.BackgroundImage;

                double screenWidth = 640d;
                double screenHeight = 480d;
                if (!SelectedScreen.Geometry.HasSize())
                {
                    screenWidth = SelectedScreen.Geometry.Width;
                    screenHeight = SelectedScreen.Geometry.Height;
                }

                Models.Anchor anchor = Selected_Anchor_Radio_New();
                double x = (double)Numeric_WatchZones_New_X.Value;
                double y = (double)Numeric_WatchZones_New_Y.Value;
                double width = (double)Numeric_WatchZones_New_Width.Value;
                double height = (double)Numeric_WatchZones_New_Height.Value;

                double scaleWidth = image.Width / screenWidth;
                double scaleHeight = image.Height / screenHeight;

                if (anchor != Models.Anchor.Undefined)
                {
                    if (anchor.HasFlag(Models.Anchor.Right)) { x = screenWidth + x - width; }
                    else if (!anchor.HasFlag(Models.Anchor.Left)) { x = screenWidth / 2d + x - width / 2d; }
                    if (anchor.HasFlag(Models.Anchor.Bottom)) { y = screenHeight + y - height; }
                    else if (!anchor.HasFlag(Models.Anchor.Top)) { y = screenHeight / 2d + y - height / 2d; }
                }

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
            sfd.Dispose();
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

            if (sfd.FileName != "")
            {
                var image = Box_WatchZones_Main.BackgroundImage;

                double screenWidth = 640d;
                double screenHeight = 480d;
                if (!SelectedScreen.Geometry.HasSize())
                {
                    screenWidth = SelectedScreen.Geometry.Width;
                    screenHeight = SelectedScreen.Geometry.Height;
                }

                Models.Anchor anchor = Selected_Anchor_Radio();
                double x = (double)Numeric_WatchZones_X.Value;
                double y = (double)Numeric_WatchZones_Y.Value;
                double width = (double)Numeric_WatchZones_Width.Value;
                double height = (double)Numeric_WatchZones_Height.Value;

                double scaleWidth = image.Width / screenWidth;
                double scaleHeight = image.Height / screenHeight;

                if (anchor != Models.Anchor.Undefined)
                {
                    if (anchor.HasFlag(Models.Anchor.Right)) { x = screenWidth + x - width; }
                    else if (!anchor.HasFlag(Models.Anchor.Left)) { x = screenWidth / 2d + x - width / 2d; }
                    if (anchor.HasFlag(Models.Anchor.Bottom)) { y = screenHeight + y - height; }
                    else if (!anchor.HasFlag(Models.Anchor.Top)) { y = screenHeight / 2d + y - height / 2d; }
                }

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
            sfd.Dispose();
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

        // If the window is mazimized or restored, do thing.
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x0112)
            {
                if (m.WParam == new IntPtr(0xF030) || m.WParam == new IntPtr(0xF120))
                {
                    if (SelectedGameProfile != null && SelectedScreen != null)
                    {
                        Update_WatchZones_New_Preview();
                        Update_WatchZones_Preview();
                    }
                }
            }
        }

    }
}
