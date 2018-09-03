using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoImageDeltaApp.Models;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using VideoImageDeltaApp;
using System.Text.RegularExpressions;

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

        private void Error(int id)
        {
            DialogResult dr = MessageBox.Show(
                "This shouldn't happen. Error code: " +
                    id.ToString() +
                    "\r\n" +
                    "If you could, report this to [Link] with details on how you got this, it'd really help.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.None
                );
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
            //Panel_Watches.Hide();
            //Panel_Watches_New.Hide();
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

        /**
         * Known bug but don't know how to fix:
         * If you invoke Flicker before a previous flicker expires, it goes berserk.
         */
        private void Flicker(Control form, int milliseconds, Color color)
        {
            var origColor = form.BackColor;
            form.BackColor = color;
            var t = new Timer();
            t.Interval = milliseconds;
            t.Tick += delegate (object a, EventArgs b) {
                form.BackColor = origColor;
            };
            t.Start();
        }

        private void Update_Screens()
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Error(17945);
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
                Error(17952);
            }
            if (ListBox_Screens.SelectedItem == null)
            {
                Error(17953);
            }
            else
            {
                ListBox_WatchZones.Items.Clear();
                var gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                foreach (WatchZone wz in gp.WatchZones)
                {
                    ListBox_WatchZones.Items.Add(wz);
                }
            }
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            Program.GameProfiles.Clear();
            foreach (GameProfile gp in ListBox_GameProfile.Items)
                Program.GameProfiles.Add(gp);
            this.Close();
        }

        private void Button_Plus_GameProfile_Click(object sender, EventArgs e)
        {
            TextBox_GameProfile_New_Name.Text = null;
            Show_Panel(Panel_GameProfile_New, Button_GameProfile_New_Create, TextBox_GameProfile_New_Name);
        }

        private void Button_Import_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "XML Files|*.xml";
            fileDialog.Title = "Select a XML File";

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
                TextBox_Screens_Width.Text = s.Geometry.Width.ToString();
                TextBox_Screens_Height.Text = s.Geometry.Height.ToString();
            }
            else
            {
                ListBox_GameProfile_SelectedIndexChanged(null, null);
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

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = "xml";
                sfd.Filter = "XML Files|*.xml";
                sfd.FileName = ListBox_GameProfile.SelectedItem.ToString(); // Might not always work...
                sfd.Title = "Save Game Profile";
                sfd.ShowDialog();

                XmlDocument document = new XmlDocument();
                StreamWriter stream = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding("iso-8859-7"));
                document.Save(xml);


            }
            else
            {
                Flicker(ListBox_GameProfile, 500, Color.FromArgb(224, 64, 64));
            }
        }

        private void Button_Plus_Screens_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Error(46208);
            }
            else
            {
                TextBox_Screens_New_Name.Text = null;
                TextBox_Screens_New_Width.Text = null;
                TextBox_Screens_New_Height.Text = null;
                Show_Panel(Panel_Screens_New, Button_Screens_New_Create, TextBox_Screens_New_Name);
            }
        }

        private void Button_GameProfile_Rename_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Error(46209);
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
                Flicker(TextBox_GameProfile_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
        }

        private void Button_Screens_New_Create_Click(object sender, EventArgs e)
        {
            string name = TextBox_Screens_New_Name.Text;
            name = name.Trim();
            int width;
            int height;

            if (ListBox_GameProfile.SelectedItem == null)
            {
                Error(17943);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Flicker(TextBox_Screens_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!int.TryParse(TextBox_Screens_New_Width.Text, out width))
            {
                Flicker(TextBox_Screens_New_Width, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!int.TryParse(TextBox_Screens_New_Height.Text, out height))
            {
                Flicker(TextBox_Screens_New_Height, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                Models.Screen item = new Models.Screen(name, new MagickGeometry(width, height));
                var gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                gp.Screens.Add(item);
                Update_Screens();
                ListBox_Screens.SelectedIndex = ListBox_Screens.Items.Count - 1;
                TextBox_Screens_New_Name.Text = null;
                TextBox_Screens_New_Width.Text = null;
                TextBox_Screens_New_Height.Text = null;
                Clean_WatchZones_New();
                Show_Panel(Panel_WatchZones_New, Button_WatchZones_New_Add, TextBox_WatchZones_New_Name);
            }
        }

        private void TextBox_Screens_New_Width_TextChanged(object sender, EventArgs e)
        {
            var text = TextBox_Screens_New_Width.Text;
            int num;
            bool valid = int.TryParse(text, out num);

            if (!valid || num < 0)
            {
                text = Regex.Replace(text, "[^0-9]", "");
                Flicker(TextBox_Screens_New_Width, 250, Color.FromArgb(255, 64, 64));

                if (!int.TryParse(text, out num) && !String.IsNullOrWhiteSpace(text))
                {
                    Error(27418);
                }

                TextBox_Screens_New_Width.Text = text;
            }
        }

        private void TextBox_Screens_New_Height_TextChanged(object sender, EventArgs e)
        {
            var text = TextBox_Screens_New_Height.Text;
            int num;
            bool valid = int.TryParse(text, out num);

            if (!valid)
            {
                text = Regex.Replace(text, "[^0-9]", "");
                Flicker(TextBox_Screens_New_Height, 250, Color.FromArgb(255, 64, 64));

                if (!int.TryParse(text, out num) && !String.IsNullOrWhiteSpace(text))
                {
                    Error(27419);
                }

                TextBox_Screens_New_Height.Text = text;
            }
        }

        private void Button_Screens_Update_Click(object sender, EventArgs e)
        {
            string name = TextBox_Screens_Name.Text;
            name = name.Trim();
            int width;
            int height;

            if (ListBox_GameProfile.SelectedItem == null)
            {
                Error(46210);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Error(46211);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Flicker(TextBox_Screens_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!int.TryParse(TextBox_Screens_Width.Text, out width))
            {
                Flicker(TextBox_Screens_Width, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!int.TryParse(TextBox_Screens_Height.Text, out height))
            {
                Flicker(TextBox_Screens_New_Height, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                var m = (Models.Screen)ListBox_Screens.SelectedItem;
                m.Name = name;
                m.Geometry = new MagickGeometry(width, height);
                Update_Screens();
                ListBox_Screens.SelectedItem = m;
            }
        }

        private void Radio_WatchZones_New_Northwest_Click(object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.Northwest); }
        private void Radio_WatchZones_New_North_Click    (object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.North    ); }
        private void Radio_WatchZones_New_Northeast_Click(object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.Northeast); }
        private void Radio_WatchZones_New_West_Click     (object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.West     ); }
        private void Radio_WatchZones_New_Center_Click   (object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.Center   ); }
        private void Radio_WatchZones_New_East_Click     (object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.East     ); }
        private void Radio_WatchZones_New_Southwest_Click(object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.Southwest); }
        private void Radio_WatchZones_New_South_Click    (object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.South    ); }
        private void Radio_WatchZones_New_Southeast_Click(object sender, EventArgs e) { Gravity_Radio_Manager_New(Gravity.Southeast); }

        private void Radio_WatchZones_Northwest_Click(object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.Northwest); }
        private void Radio_WatchZones_North_Click    (object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.North    ); }
        private void Radio_WatchZones_Northeast_Click(object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.Northeast); }
        private void Radio_WatchZones_West_Click     (object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.West     ); }
        private void Radio_WatchZones_Center_Click   (object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.Center   ); }
        private void Radio_WatchZones_East_Click     (object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.East     ); }
        private void Radio_WatchZones_Southwest_Click(object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.Southwest); }
        private void Radio_WatchZones_South_Click    (object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.South    ); }
        private void Radio_WatchZones_Southeast_Click(object sender, EventArgs e) { Gravity_Radio_Manager(Gravity.Southeast); }

        private Gravity Selected_Gravity_Radio_New()
        {
            // C# is picky about switches...
                 if (Radio_WatchZones_New_Northwest.Checked == true) return Gravity.Northwest;
            else if (Radio_WatchZones_New_North    .Checked == true) return Gravity.North;
            else if (Radio_WatchZones_New_Northeast.Checked == true) return Gravity.Northeast;
            else if (Radio_WatchZones_New_West     .Checked == true) return Gravity.West;
            else if (Radio_WatchZones_New_Center   .Checked == true) return Gravity.Center;
            else if (Radio_WatchZones_New_East     .Checked == true) return Gravity.East;
            else if (Radio_WatchZones_New_Southwest.Checked == true) return Gravity.Southwest;
            else if (Radio_WatchZones_New_South    .Checked == true) return Gravity.South;
            else if (Radio_WatchZones_New_Southeast.Checked == true) return Gravity.Southeast;
                 else return Gravity.Undefined;
        }

        private Gravity Selected_Gravity_Radio()
        {
            // C# is picky about switches...
                 if (Radio_WatchZones_Northwest.Checked == true) return Gravity.Northwest;
            else if (Radio_WatchZones_North    .Checked == true) return Gravity.North;
            else if (Radio_WatchZones_Northeast.Checked == true) return Gravity.Northeast;
            else if (Radio_WatchZones_West     .Checked == true) return Gravity.West;
            else if (Radio_WatchZones_Center   .Checked == true) return Gravity.Center;
            else if (Radio_WatchZones_East     .Checked == true) return Gravity.East;
            else if (Radio_WatchZones_Southwest.Checked == true) return Gravity.Southwest;
            else if (Radio_WatchZones_South    .Checked == true) return Gravity.South;
            else if (Radio_WatchZones_Southeast.Checked == true) return Gravity.Southeast;
            else return Gravity.Undefined;
        }

        private void Gravity_Radio_Manager_New(Gravity grav = Gravity.Undefined)
        {
            Radio_WatchZones_New_Northwest.Checked = false;
            Radio_WatchZones_New_North    .Checked = false;
            Radio_WatchZones_New_Northeast.Checked = false;
            Radio_WatchZones_New_West     .Checked = false;
            Radio_WatchZones_New_Center   .Checked = false;
            Radio_WatchZones_New_East     .Checked = false;
            Radio_WatchZones_New_Southwest.Checked = false;
            Radio_WatchZones_New_South    .Checked = false;
            Radio_WatchZones_New_Southeast.Checked = false;
            if (grav != Gravity.Undefined)
                switch (grav) {
                    case Gravity.Northwest: Radio_WatchZones_New_Northwest.Checked = true; break;
                    case Gravity.North:     Radio_WatchZones_New_North    .Checked = true; break;
                    case Gravity.Northeast: Radio_WatchZones_New_Northeast.Checked = true; break;
                    case Gravity.West:      Radio_WatchZones_New_West     .Checked = true; break;
                    case Gravity.Center:    Radio_WatchZones_New_Center   .Checked = true; break;
                    case Gravity.East:      Radio_WatchZones_New_East     .Checked = true; break;
                    case Gravity.Southwest: Radio_WatchZones_New_Southwest.Checked = true; break;
                    case Gravity.South:     Radio_WatchZones_New_South    .Checked = true; break;
                    case Gravity.Southeast: Radio_WatchZones_New_Southeast.Checked = true; break;
                }
            Update_WatchZones_New_Preview();
        }

        private void Gravity_Radio_Manager(Gravity grav = Gravity.Undefined)
        {
            Radio_WatchZones_Northwest.Checked = false;
            Radio_WatchZones_North    .Checked = false;
            Radio_WatchZones_Northeast.Checked = false;
            Radio_WatchZones_West     .Checked = false;
            Radio_WatchZones_Center   .Checked = false;
            Radio_WatchZones_East     .Checked = false;
            Radio_WatchZones_Southwest.Checked = false;
            Radio_WatchZones_South    .Checked = false;
            Radio_WatchZones_Southeast.Checked = false;
            if (grav != Gravity.Undefined)
                switch (grav)
                {
                    case Gravity.Northwest: Radio_WatchZones_Northwest.Checked = true; break;
                    case Gravity.North:     Radio_WatchZones_North    .Checked = true; break;
                    case Gravity.Northeast: Radio_WatchZones_Northeast.Checked = true; break;
                    case Gravity.West:      Radio_WatchZones_West     .Checked = true; break;
                    case Gravity.Center:    Radio_WatchZones_Center   .Checked = true; break;
                    case Gravity.East:      Radio_WatchZones_East     .Checked = true; break;
                    case Gravity.Southwest: Radio_WatchZones_Southwest.Checked = true; break;
                    case Gravity.South:     Radio_WatchZones_South    .Checked = true; break;
                    case Gravity.Southeast: Radio_WatchZones_Southeast.Checked = true; break;
                }
            //Update_WatchZones_Preview();
        }

        private void Button_Minus_Screens_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Error(17946);
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
            Gravity_Radio_Manager_New(Gravity.Northwest);
            TextBox_WatchZones_New_Name.Text = null;
            Radio_WatchZones_New_Percent.Checked = false;
            Radio_WatchZones_New_Pixels.Checked = true;
            TextBox_WatchZones_New_X.Text = "0";
            TextBox_WatchZones_New_Y.Text = "0";
            TextBox_WatchZones_New_Width.Text = "100";
            TextBox_WatchZones_New_Height.Text = "100";
        }

        private void Clean_WatchZones()
        {
            Gravity_Radio_Manager(Gravity.Northwest);
            TextBox_WatchZones_Name.Text = null;
            Radio_WatchZones_Percent.Checked = false;
            Radio_WatchZones_Pixels.Checked = true;
            TextBox_WatchZones_X.Text = "0";
            TextBox_WatchZones_Y.Text = "0";
            TextBox_WatchZones_Width.Text = "100";
            TextBox_WatchZones_Height.Text = "100";
        }

        private void Button_Plus_WatchZones_Click(object sender, EventArgs e)
        {
            if (ListBox_GameProfile.SelectedItem == null)
            {
                Error(46212);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Error(46213);
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
            Gravity grav = Selected_Gravity_Radio_New();
            bool isPercentage = Radio_WatchZones_New_Percent.Checked;
            double      x;
            double      y;
            double  width;
            double height;

            if (ListBox_GameProfile.SelectedItem == null)
            {
                Error(17944);
            }
            if (ListBox_Screens.SelectedItem == null)
            {
                Error(17945);
            }
            if (grav == Gravity.Undefined)
            {
                Error(87437);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                Flicker(TextBox_Screens_New_Name, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!Double.TryParse(TextBox_WatchZones_New_X.Text, out x))
            {
                Flicker(TextBox_WatchZones_New_X, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!Double.TryParse(TextBox_WatchZones_New_Y.Text, out y))
            {
                Flicker(TextBox_WatchZones_New_Y, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!Double.TryParse(TextBox_WatchZones_New_Width.Text, out width))
            {
                Flicker(TextBox_WatchZones_New_Width, 250, Color.FromArgb(255, 64, 64));
            }
            else if (!Double.TryParse(TextBox_WatchZones_New_Height.Text, out height))
            {
                Flicker(TextBox_WatchZones_New_Height, 250, Color.FromArgb(255, 64, 64));
            }
            else
            {
                GameProfile gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                MagickGeometry geo;
                if (isPercentage)
                {
                    geo = new MagickGeometry((int)x, (int)y, (int)width, (int)height);
                    geo.IsPercentage = false;
                }
                else
                {
                    geo = new MagickGeometry((int)x, (int)y, new Percentage(width), new Percentage(height));
                    geo.IsPercentage = true;
                }
                var wz = new WatchZone(name, (Models.Screen)ListBox_Screens.SelectedItem, geo, grav);
                gp.WatchZones.Add(wz);
                Update_WatchZones();
                ListBox_WatchZones.SelectedIndex = ListBox_WatchZones.Items.Count - 1;
                Clean_WatchZones_New();
                Show_Panel(Panel_WatchZones, Button_WatchZones_Update, TextBox_WatchZones_Name);
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
                Error(46214);
            }
            else if (ListBox_Screens.SelectedItem == null)
            {
                Error(46215);
            }
            else if (TableLayoutPanel_WatchZones_New.Width > 0) // Stop minimize bug
            {
                GameProfile gp = (GameProfile)ListBox_GameProfile.SelectedItem;
                int i = gp.Screens.IndexOf((Models.Screen)ListBox_Screens.SelectedItem);
                int screenWidth = gp.Screens[i].Geometry.Width;
                int screenHeight = gp.Screens[i].Geometry.Height;

                // Shouldn't be hard-coded (among other things...)
                int panelWidth = Math.Min(TableLayoutPanel_WatchZones_New.Width - 310, screenWidth);
                int panelHeight = Math.Min(TableLayoutPanel_WatchZones_New.Height - 7, screenHeight);

                double greaterDivisor = Math.Max((double)screenWidth / panelWidth, (double)screenHeight / panelHeight);
                int newScreenWidth  = (int)(screenWidth / greaterDivisor);
                int newScreenHeight = (int)(screenHeight / greaterDivisor);

                Box_WatchZones_New_Main.Size = new Size(newScreenWidth, newScreenHeight);
                Box_WatchZones_New_Main.Location = new Point(
                    (panelWidth - newScreenWidth) / 2,
                    (panelHeight - newScreenHeight) / 2
                    );

                float cellWidth = (float)newScreenWidth / TableLayoutPanel_WatchZones_New.Width * 100F + 7F;

                TableLayoutPanel_WatchZones_New.ColumnStyles[0].Width = cellWidth;
                TableLayoutPanel_WatchZones_New.ColumnStyles[1].Width = 100F - cellWidth;

                /****************************************/

                Gravity grav      = Selected_Gravity_Radio_New();
                bool isPercentage = Radio_WatchZones_New_Percent.Checked;
                double x      = Double_TryParse_Nullable(TextBox_WatchZones_New_X.Text)      ??  0d;
                double y      = Double_TryParse_Nullable(TextBox_WatchZones_New_Y.Text)      ??  0d;
                double width  = Double_TryParse_Nullable(TextBox_WatchZones_New_Width.Text)  ?? 80d;
                double height = Double_TryParse_Nullable(TextBox_WatchZones_New_Height.Text) ?? 80d;

                Size size = new Size();
                Point point = new Point();
                double scaleWidth  = (double)newScreenWidth / screenWidth;
                double scaleHeight = (double)newScreenHeight / screenHeight;

                AnchorStyles anchor = AnchorStyles_From_Gravity(grav);
                Box_WatchZones_New_Preview.Anchor = anchor;

                     if ( anchor.HasFlag(AnchorStyles.Right )) { x = (double)screenWidth  - x -  width; }
                else if (!anchor.HasFlag(AnchorStyles.Left  )) { x = (double)screenWidth  / 2d + x -  width / 2d; }
                     if ( anchor.HasFlag(AnchorStyles.Bottom)) { y = (double)screenHeight - y - height; }
                else if (!anchor.HasFlag(AnchorStyles.Top   )) { y = (double)screenHeight / 2d + y - height / 2d; }

                if (x < 0d) { x = (double)screenWidth  + x; } // I don't think this even works for x and y.
                if (y < 0d) { y = (double)screenHeight + y; }

                if (!isPercentage)
                {
                    if ( width < 0d) {  width = (double)screenWidth  +  width; }
                    if (height < 0d) { height = (double)screenHeight + height; }
                }
                else
                {
                     width =  width / 100d * screenWidth;
                    height = height / 100d * screenHeight;
                }

                    point.X = (int)(Math.Round(scaleWidth  *      x));
                    point.Y = (int)(Math.Round(scaleHeight *      y));
                 size.Width = (int)(Math.Round(scaleWidth  *  width));
                size.Height = (int)(Math.Round(scaleHeight * height));

                Box_WatchZones_New_Preview.Location = point;
                Box_WatchZones_New_Preview.Size = size;
            }
        }

        private AnchorStyles AnchorStyles_From_Gravity(Gravity grav)
        {
            switch (grav)
            {
                case Gravity.Northwest: return AnchorStyles.Top | AnchorStyles.Left;
                case Gravity.North:     return AnchorStyles.Top;
                case Gravity.Northeast: return AnchorStyles.Top | AnchorStyles.Right;
                case Gravity.West:      return AnchorStyles.Left;
                case Gravity.Center:    return AnchorStyles.None;
                case Gravity.East:      return AnchorStyles.Right;
                case Gravity.Southwest: return AnchorStyles.Bottom | AnchorStyles.Left;
                case Gravity.South:     return AnchorStyles.Bottom;
                case Gravity.Southeast: return AnchorStyles.Bottom | AnchorStyles.Right;
                default:                return AnchorStyles.Top | AnchorStyles.Left;
            }
        }

        private double? Double_TryParse_Nullable(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                double x;
                if (!double.TryParse(s, out x))
                {
                    Error(42069);
                }
                else
                {
                    return x;
                }
            }
            return null;
        }

        private void Radio_WatchZones_New_Pixel_Click(object sender, EventArgs e)
        {
            Radio_WatchZones_New_Percent.Checked = false;
        }

        private void Radio_WatchZones_New_Percent_Click(object sender, EventArgs e)
        {
            Radio_WatchZones_New_Pixels.Checked = false;
        }

        public void Numeric_TextChanged(TextBox box)
        {
            var text = box.Text;
            int num;
            bool valid = int.TryParse(text, out num);
            bool isNegative = false;

            if (!valid)
            {
                if (String.IsNullOrWhiteSpace(text))
                {
                    text = "0";
                }

                if (text.Substring(0, 1) == "-" && text != "-")
                    isNegative = true;
                text = Regex.Replace(text, "[^.0123456789]", "");
                Flicker(box, 250, Color.FromArgb(255, 64, 64));

                if (!int.TryParse(text, out num))
                {
                    text = "0";
                }

                if (isNegative)
                    num = -num;

                box.Text = num.ToString();
            }
        }

        private void TextBox_WatchZones_New_X_TextChanged(object sender, EventArgs e)
        {
            Numeric_TextChanged(TextBox_WatchZones_New_X);
            Update_WatchZones_New_Preview();
        }
        private void TextBox_WatchZones_New_Y_TextChanged(object sender, EventArgs e)
        {
            Numeric_TextChanged(TextBox_WatchZones_New_Y);
            Update_WatchZones_New_Preview();
        }
        private void TextBox_WatchZones_New_Width_TextChanged(object sender, EventArgs e)
        {
            Numeric_TextChanged(TextBox_WatchZones_New_Width);
            Update_WatchZones_New_Preview();
        }
        private void TextBox_WatchZones_New_Height_TextChanged(object sender, EventArgs e)
        {
            Numeric_TextChanged(TextBox_WatchZones_New_Height);
            Update_WatchZones_New_Preview();
        }
    }
}
