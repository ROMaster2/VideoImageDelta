﻿using Hudl.FFmpeg;
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

using Screen = VideoImageDeltaApp.Models.Screen;

namespace VideoImageDeltaApp
{
    public partial class AddVideos : Form
    {
        private bool seenWarning = false;

        public AddVideos()
        {
            InitializeComponent();

            foreach (var v in Program.Videos)
            {
                ListView_Videos.Items.Add(new ListVideo(v));
            }

            foreach (var gp in Program.GameProfiles)
            {
                DropBox_GameProfile.Items.Add(gp);
            }
            if (DropBox_GameProfile.Items.Count > 0)
            {
                DropBox_GameProfile.SelectedIndex = 0;
                DropBox_GameProfile.Enabled = true;
            }
            else
            {
                DropBox_GameProfile.Enabled = false;
            }

            if (DropBox_GameProfile.Items.Count > 0)
            {
                DropBox_GameProfile.SelectedIndex = DropBox_GameProfile.Items.Count - 1;
            }

        }

        private void AddVideos_Load(object sender, EventArgs e)
        {
            Hide_Core();
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            Form_Closing(null, null);
            Close();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Program.Videos.Clear();
            foreach (ListVideo lv in ListView_Videos.Items)
                Program.Videos.Add(lv.Video);
            if (Box_Main.BackgroundImage != null)
                Box_Main.BackgroundImage.Dispose();
        }

        private void Button_Help_Click(object sender, EventArgs e)
        {
            // Todo
        }

        private void Button_Import_Click(object sender, EventArgs e)
        {
            if (SelectedVideo != null)
            {
                var ofd = new OpenFileDialog()
                {
                    Filter = "XML Files|*.xml",
                    Title = "Select an XML File",
                    Multiselect = true
                };

                ofd.ShowDialog();

                if (ofd.CheckFileExists == true)
                {
                    var feeds = new List<Feed>();
                    bool abort = false;
                    foreach (var f in ofd.FileNames)
                    {
retry:
                        VideoProfile vp;
                        XmlSerializer serializer = new XmlSerializer(typeof(VideoProfile));
                        using (StreamReader reader = new StreamReader(f))
                        {
                            try
                            {
                                vp = (VideoProfile)serializer.Deserialize(reader);
                            } catch
                            {
                                DialogResult dr = MessageBox.Show(f + "is either not a Video profile or has been corrupted.",
                                    "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                                if (dr == DialogResult.Abort)
                                {
                                    abort = true;
                                    break;
                                }
                                else if (dr == DialogResult.Retry)
                                {
                                    goto retry;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }

                        bool geoError = false;
                        foreach (Video v in SelectedVideos)
                        {
                            if (v.Geometry.Width != vp.Geometry.Width || v.Geometry.Height != vp.Geometry.Height)
                                geoError = true;
                        }

                        if (geoError)
                        {
                            DialogResult dr = MessageBox.Show("Only profiles with matching Width and Height can be applied.",
                                "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                            if (dr == DialogResult.Abort)
                            {
                                abort = true;
                                break;
                            } else if (dr == DialogResult.Retry)
                            {
                                goto retry;
                            } else
                            {
                                continue;
                            }
                        }
                        if (abort) break;

                        feeds.AddRange(vp.Feeds);
                    }

                    if (!abort)
                    {
                        foreach(var lv in SelectedListVideos)
                        {
                            var i = lv.Index;
                            foreach (Feed f in feeds)
                            {
                                lv.Video.Feeds.Add(f);
                                lv.RefreshValues();
                                ListView_Videos.Items[i] = lv;
                            }
                        }
                        ListBox_Feeds.SelectedIndex = ListBox_Feeds.Items.Count - 1;
                    }
                }
                ofd.Dispose();
            }
            // For some reason, if your cursor is above the preview box when you click OK, it
            // changes the preview box dimensions to where your cursor is. The feeds are
            // unaffected but people might think it imported incorrectly. It probably has to
            // do with the MouseUp/Down stuff.
        }

        private void Button_Export_Click(object sender, EventArgs e)
        {
            if (SelectedVideo != null)
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    FileName = "vidVideoProfiles",
                    DefaultExt = "xml",
                    Filter = "XML Files|*.xml",
                    ValidateNames = true,
                    Title = "Save video profile data"
                };

                sfd.ShowDialog();

                if (sfd.FileName != "")
                {
                    var vp = new VideoProfile("", SelectedVideo);
                    Type t = vp.GetType();
                    XmlSerializer serializer = new XmlSerializer(t);
                    using (TextWriter writer = new StreamWriter(sfd.FileName))
                    {
                        serializer.Serialize(writer, vp);
                    }
                }

                sfd.Dispose();
            }
        }

        // These are used a lot so they're referenced.
        public List<ListVideo> AllListVideos
        {
            get
            {
                var l = new List<ListVideo>();
                foreach (ListVideo lv in ListView_Videos.Items)
                {
                    l.Add(lv);
                }
                return l;
            }
        }

        public List<Video> AllVideos
        {
            get
            {
                var l = new List<Video>();
                foreach (ListVideo lv in AllListVideos)
                {
                    l.Add(lv.Video);
                }
                return l;
            }
        }

        public List<ListVideo> SelectedListVideos
        {
            get
            {
                var l = new List<ListVideo>();
                foreach (ListVideo lv in ListView_Videos.SelectedItems)
                {
                    l.Add(lv);
                }
                return l;
            }
            set
            {
                foreach (ListVideo vlv in value)
                {
                    ListView_Videos.Items[vlv.Index] = vlv;
                }
            }
        }

        public List<Video> SelectedVideos
        {
            get
            {
                var l = new List<Video>();
                foreach (ListVideo lv in ListView_Videos.SelectedItems)
                {
                    l.Add(lv.Video);
                }
                return l;
            }
        }

        public ListVideo SelectedListVideo
        {
            get
            {
                if (ListView_Videos.SelectedItems.Count > 0)
                {
                    return (ListVideo)ListView_Videos.SelectedItems[0];
                } else
                {
                    return null;
                }
            }
            set
            {
                var i = ListView_Videos.SelectedItems[0].Index;
                ListView_Videos.Items[i] = value;
            }
        }

        public Video SelectedVideo
        {
            get
            {
                if (ListView_Videos.SelectedItems.Count > 0)
                {
                    return SelectedListVideo.Video;
                }
                else
                {
                    return null;
                }
            }
        }

        public GameProfile SelectedGameProfile
        {
            get
            {
                if (DropBox_GameProfile.SelectedIndex > -1)
                {
                    return (GameProfile)DropBox_GameProfile.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
        }

        public Feed SelectedFeed
        {
            get
            {
                if (ListBox_Feeds.SelectedItems.Count > 0)
                {
                    return (Feed)ListBox_Feeds.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                var i = SelectedVideo.Feeds.IndexOf(SelectedVideo.Feeds.Where(x => x.Name == value.Name).First());
                SelectedVideo.Feeds[i] = value;
                ListBox_Feeds.SelectedItem = value;
            }
        }

        public List<Screen> CheckedScreens
        {
            get
            {
                if (CheckedListBox_Screens.CheckedItems.Count > 0)
                {
                    var l = new List<Screen>();
                    foreach (Screen lv in CheckedListBox_Screens.CheckedItems)
                    {
                        l.Add(lv);
                    }
                    return l;
                }
                else
                {
                    return null;
                }
            }
        }

        // If nothing is selected, show nothing on the bottom panel.
        public void Hide_Core()
        {
            ListView_Videos.SelectedItems.Clear();
            AcceptButton = null;
            SplitContainer_Core.Panel2Collapsed = true;
        }

        public void Show_Core()
        {
            SplitContainer_Core.Panel2Collapsed = false;
            AcceptButton = Button_Add_Feed;
            if (ListView_Videos.SelectedItems.Count == 1)
            {
                TimeSpan timestamp = new TimeSpan(SelectedVideo.Duration.Ticks * 2L / 3L);
                oldTimestamp = timestamp.ToString().Substring(0, 8);
                TextBox_Timestamp.Text = oldTimestamp;
            }
        }
        
        // Previously caused memory leaks. *Should* be under control now.
        private void Display_Thumbnail(Video v, TimeSpan timestamp)
        {
            if (File.Exists(v.FilePath))
            {
                if (Box_Main.BackgroundImage != null)
                {
                    Box_Main.BackgroundImage.Dispose();
                }
                Image i = v.GetThumbnail(timestamp);
                if (i != null)
                {
                    Box_Main.BackgroundImage = i;
                    Box_Preview.BackColor = Color.FromArgb(127, 255, 0, 255);
                    Label_Failed.Hide();
                } else
                {
                    Box_Main.BackgroundImage = null;
                    Box_Preview.BackColor = Color.Black;
                    Label_Failed.Show();
                }
            } else
            {
                // File can't be found. Options are remove, retry, or ignore (which will cause problems later).
                // Would like "re-assign to different file" or something.
                DialogResult dr = MessageBox.Show(
                    "The video file no longer exists.",
                    "Critical Error",
                    MessageBoxButtons.AbortRetryIgnore,
                    MessageBoxIcon.Stop
                );

                if (dr == DialogResult.Retry)
                {
                    Display_Thumbnail(v, timestamp);
                } else if (dr == DialogResult.Abort)
                {
                    ListView_Videos.SelectedItems[0].Remove();
                }
            }
            Update_Preview_Box();
        }

        private void Update_Preview_Box()
        {
            // Stops minimize bug
            if (Panel_Back.Width > 0 && SplitContainer_Core.Panel2Collapsed == false && SelectedVideo != null)
            {
                double screenWidth = SelectedVideo.Geometry.Width;
                double screenHeight = SelectedVideo.Geometry.Height;

                double panelWidth = Math.Min(Panel_Back.Width, screenWidth);
                double panelHeight = Math.Min(Panel_Back.Height, screenHeight);

                double greaterDivisor = Math.Max(screenWidth / panelWidth, screenHeight / panelHeight);
                double newScreenWidth = screenWidth / greaterDivisor;
                double newScreenHeight = screenHeight / greaterDivisor;

                Box_Main.Size = new Size(
                    (int)newScreenWidth,
                    (int)newScreenHeight
                    );
                Box_Main.Location = new Point(
                    (int)((Panel_Back.Width - newScreenWidth) / 2d),
                    (int)((Panel_Back.Height - newScreenHeight) / 2d)
                    );
                Update_Preview();
            }
        }

        private void Update_Preview()
        {
            double screenWidth = SelectedVideo.Geometry.Width;
            double screenHeight = SelectedVideo.Geometry.Height;

            double x = (double)Numeric_X.Value;
            double y = (double)Numeric_Y.Value;
            double width = (double)Numeric_Width.Value;
            double height = (double)Numeric_Height.Value;

            Size size = new Size();
            Point point = new Point();
            double scaleWidth = Box_Main.Size.Width / screenWidth;
            double scaleHeight = Box_Main.Size.Height / screenHeight;

            if (x < 0d) { x = screenWidth + x; } // I don't think this even works for x and y.
            if (y < 0d) { y = screenHeight + y; }

            if (width < 0d) { width = screenWidth + width; }
            if (height < 0d) { height = screenHeight + height; }

            point.X = (int)(Math.Round(scaleWidth * x));
            point.Y = (int)(Math.Round(scaleHeight * y));
            size.Width = (int)(Math.Round(scaleWidth * width));
            size.Height = (int)(Math.Round(scaleHeight * height));

            Box_Preview.Location = point;
            Box_Preview.Size = size;

        }

        private void Button_Add_Videos_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Add one or more video...",
                Filter = "Video files|*.mp4;*.mkv;*.avi;*.ts;*.wmv",
                Multiselect = true
            };
            ofd.ShowDialog();

            bool added = false;

            foreach (var f in ofd.FileNames)
            {
                // There's probably an array function that does this better...
                bool exists = false;
                foreach (ListVideo lv in ListView_Videos.Items) { if (f == lv.Video.FilePath) exists = true; }
                if (exists) { added = true; continue; }

                Video v = Video.Create(f);
                if (v != null)
                {
                    var lv = new ListVideo(v);
                    ListView_Videos.Items.Add(lv);
                    added = true;
                }
            }

            if (added)
            {
                ListView_Videos.SelectedItems.Clear();

                foreach (ListVideo lv in ListView_Videos.Items)
                {
                    if (ofd.FileNames.Contains(lv.Video.FilePath))
                    {
                        lv.Selected = true;
                    }
                }

                ListView_Videos.Focus();
            }
        }

        private void ListView_Video_Column_Click(object sender, ColumnClickEventArgs e)
        {
            // ListViewItemComparer Isn't found, searching gave no results.
            // It's the only thing that lets this work without a bunch of lines, so fuck.
        }

        private void ListView_Video_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListView_Videos.SelectedItems.Count > 0)
            {
                Show_Core();
                Clean_Inputs();
                ListBox_Feeds.Items.Clear();
                foreach (Feed f in SelectedVideo.Feeds)
                {
                    ListBox_Feeds.Items.Add(f);
                }

                if (SelectedVideo.GameProfile != null && SelectedVideos.All(x => x.GameProfile == SelectedVideo.GameProfile))
                {
                    var i = DropBox_GameProfile.Items.IndexOf(SelectedVideo.GameProfile);
                    DropBox_GameProfile.SelectedIndex = i;
                }
                else
                {
                    DropBox_GameProfile.SelectedIndex = -1;
                }
            }
            else
            {
                Hide_Core();
            }

            if (ListBox_Feeds.SelectedItems.Count > 0)
                FillScreenBox();
            else
            {
                CheckedListBox_Screens.Items.Clear();

            }


            Enable_VandG_Pair();
        }

        private void Clean_Inputs()
        {
            TextBox_Name.Text = null;
            CheckBox_Timer.Checked = false;
            //CheckBox_Perfect.Checked = false;
            Numeric_X.Value = 0m;
            Numeric_Y.Value = 0m;
            Numeric_Width.Value = 100m;
            Numeric_Height.Value = 100m;
            Numeric_Game_Width.Value = 0m;
            Numeric_Game_Height.Value = 0m;
        }

        private void Update_Inputs()
        {
            Feed f = (Feed)ListBox_Feeds.SelectedItem;
            if (f != null)
            {
                TextBox_Name.Text = f.Name;
                CheckBox_Timer.Checked = f.UseOCR;
                //CheckBox_Perfect.Checked = f.AccurateCapture;
                Numeric_X.Value = (decimal)f.Geometry.X;
                Numeric_Y.Value = (decimal)f.Geometry.Y;
                Numeric_Width.Value = (decimal)f.Geometry.Width;
                Numeric_Height.Value = (decimal)f.Geometry.Height;
                if (f.GameGeometry != null && CheckBox_Advanced.Checked)
                {
                    Numeric_Game_Width.Value = (decimal)f.GameGeometry.Width;
                    Numeric_Game_Height.Value = (decimal)f.GameGeometry.Height;
                }
                Update_Preview();
            }
            else
            {
                Clean_Inputs();
            }
        }

        private void ListView_Video_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && ListView_Videos.SelectedItems.Count > 0)
            {
                string plural = "this video";
                if (ListView_Videos.SelectedItems.Count > 1)
                    plural = "these videos";

                DialogResult dr = MessageBox.Show(
                    "Are you sure you want to remove " + plural + "?",
                    "Question",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    );

                if (dr == DialogResult.Yes)
                {
                    foreach (ListVideo lv in ListView_Videos.SelectedItems)
                    {
                        ListView_Videos.Items.Remove(lv);
                    }
                }
            }
        }

        private void Numeric_ValueChanged(object sender, EventArgs e)
        {
            if (!selectionActive)
                Update_Preview();
        }

        // If the window is mazimized or restored, do thing.
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x0112)
            {
                if (m.WParam == new IntPtr(0xF030) || m.WParam == new IntPtr(0xF120))
                {
                    Update_Preview_Box();
                }
            }
        }

        private void Resize_Box(object sender, EventArgs e)
        {
            Update_Preview_Box();
        }

        private void CheckBox_Timer_CheckedChanged(object sender, EventArgs e)
        {
            //CheckBox_Perfect.Enabled = !CheckBox_Timer.Checked;
            //CheckBox_Perfect.Checked = CheckBox_Timer.Checked;
        }

        /******************************************************************/

        private Point selectionStart = new Point(0, 0);
        private Point selectionEnd = new Point(0, 0);
        private bool selectionActive = false;

        private void Box_Main_MouseDown(object sender, MouseEventArgs e)
        {
            decimal widthMultiplier = (decimal)SelectedVideo.Geometry.Width / Box_Main.Width;
            decimal heightMultiplier = (decimal)SelectedVideo.Geometry.Height / Box_Main.Height;
            Numeric_X.Value = selectionStart.X * widthMultiplier;
            Numeric_Y.Value = selectionStart.Y * heightMultiplier;

            Update_Preview();

            selectionStart = e.Location;
            selectionActive = true;
        }

        private void Box_Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectionActive)
            {
                Click_Resize_Preview(e);
            }
        }

        private void Box_Main_MouseUp(object sender, MouseEventArgs e)
        {
            selectionEnd = e.Location;
            selectionActive = false;
            Click_Resize_Preview(e);
        }

        private void Click_Resize_Preview(MouseEventArgs e)
        {
            decimal screenWidth = (decimal)SelectedVideo.Geometry.Width;
            decimal screenHeight = (decimal)SelectedVideo.Geometry.Height;
            decimal widthMultiplier = screenWidth / Box_Main.Width;
            decimal heightMultiplier = screenHeight / Box_Main.Height;

            int mouseX = Math.Min(Math.Max(0, e.Location.X), (int)Math.Round(screenWidth / widthMultiplier));
            int mouseY = Math.Min(Math.Max(0, e.Location.Y), (int)Math.Round(screenHeight / heightMultiplier));

            Numeric_X.Value = Math.Max(0m, Math.Round(Math.Min(selectionStart.X, mouseX) * widthMultiplier));
            Numeric_Y.Value = Math.Max(0m, Math.Round(Math.Min(selectionStart.Y, mouseY) * heightMultiplier));
            Numeric_Width.Value =
                Math.Min(screenWidth - Numeric_X.Value, Math.Round(Math.Abs(selectionStart.X - mouseX) * widthMultiplier));
            Numeric_Height.Value =
                Math.Min(screenHeight - Numeric_Y.Value, Math.Round(Math.Abs(selectionStart.Y - mouseY) * heightMultiplier));

            Update_Preview();
        }

        string oldTimestamp = "00:00:03";

        private void TextBox_Timestamp_TextChanged(object sender, EventArgs e)
        {
            string str = TextBox_Timestamp.Text;
            bool isValid = Validate_Timestamp_Text(ref str);
            if (isValid)
            {
                oldTimestamp = str;
                TimeSpan timestamp = TimeSpan.ParseExact(str, @"hh\:mm\:ss", CultureInfo.CurrentCulture);
                if (SelectedVideo.Duration > timestamp)
                {
                    TextBox_Timestamp.BackColor = Color.White;
                    Display_Thumbnail(SelectedVideo, timestamp);
                    TrackBar_Thumbnail.Value = (int)Math.Round(((timestamp.TotalSeconds / SelectedVideo.Duration.TotalSeconds) * 100 ));
                } else
                {
                    TextBox_Timestamp.BackColor = Color.FromArgb(224, 64, 64);
                }
            } else
            {
                TextBox_Timestamp.BackColor = Color.FromArgb(224, 64, 64);
            }
        }

        private bool Validate_Timestamp_Text(ref string str)
        {
            bool isValid = Regex.IsMatch(str, @"^(?:(?:([01]?\d|2[0-3]):)?([0-5]\d):)?([0-5]\d)$");
            if (isValid && str.Length < 8)
            {
                int len = str.Length;
                str = "00:00:00".Substring(0, 8 - len) + str;
                TextBox_Timestamp.Text = str;
                TextBox_Timestamp.SelectionStart = 8 - len;
            }
            return isValid;
        }

        private void Button_Add_Feed_Click(object sender, EventArgs e)
        {
            string name = TextBox_Name.Text;
            name = name.Trim();
            bool useOCR = CheckBox_Timer.Checked;
            //bool accurateCapture = CheckBox_Perfect.Checked;

            int x = (int)Numeric_X.Value;
            int y = (int)Numeric_Y.Value;
            int width = (int)Numeric_Width.Value;
            int height = (int)Numeric_Height.Value;
            Geometry geo = new Geometry(x, y, width, height);
            Geometry gameGeo = null;

            if (CheckBox_Advanced.Checked && Numeric_Game_Width.Value > 0m && Numeric_Game_Height.Value > 0m)
            {
                gameGeo = new Geometry((int)Numeric_Game_Width.Value, (int)Numeric_Game_Height.Value);
            }

            int exists = -1;
            for (int i = 0; i < SelectedVideo.Feeds.Count; i++)
            {
                if (SelectedVideo.Feeds[i].Name.ToLower() == name.ToLower())
                {
                    exists = i;
                    break;
                }
            }

            var feed = new Feed(name, useOCR, geo, gameGeo);

            if (String.IsNullOrWhiteSpace(name))
            {
                Utilities.Flicker(TextBox_Name, 500, Color.FromArgb(255, 64, 64));
            }
            else if (exists >= 0)
            {
                DialogResult dr = MessageBox.Show(
                    "Update existing feed?",
                    "Question",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dr == DialogResult.Yes)
                {
                    SelectedVideo.Feeds[exists] = feed;
                    SelectedListVideo.RefreshValues();
                    ListBox_Feeds.Items[exists] = feed;
                }
            }
            else
            {
                SelectedVideo.Feeds.Add(feed);
                SelectedListVideo.RefreshValues();
                ListBox_Feeds.Items.Add(feed);
            }
        }

        private void ListBox_Feeds_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_Inputs();
            if (ListBox_Feeds.SelectedItems.Count > 0)
                FillScreenBox();
            else
                CheckedListBox_Screens.Items.Clear();
        }

        private void TextBox_Auto_Selector(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void Numeric_Auto_Selector(object sender, EventArgs e)
        {
            int len = ((NumericUpDown)sender).Value.ToString().Length;
            ((NumericUpDown)sender).Select(0, len);
        }

        // Used over ValueChanged to avoid feedback loop
        private void TrackBar_Thumbnail_MouseUp(object sender, MouseEventArgs e)
        {
            TextBox_Timestamp.Text =
                TimeSpan.FromSeconds(SelectedVideo.Duration.TotalSeconds * TrackBar_Thumbnail.Value / 100)
                .ToString()
                .Substring(0 ,8);
        }

        private void TrackBar_Thumbnail_KeyUp(object sender, KeyEventArgs e)
        {
            TrackBar_Thumbnail_MouseUp(null, null);
        }

        private void Button_Minus_Feeds_Click(object sender, EventArgs e)
        {
            if (ListBox_Feeds.SelectedItem != null)
            {
                SelectedVideo.Feeds.RemoveAt(ListBox_Feeds.SelectedIndex);
                ListBox_Feeds.Items.RemoveAt(ListBox_Feeds.SelectedIndex);
                SelectedListVideo.RefreshValues();
            }
        }

        private void CheckBox_Advanced_CheckedChanged(object sender, EventArgs e)
        {
            if (!seenWarning)
            {
                DialogResult dr = MessageBox.Show("Only use these options if you understand what they do.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                seenWarning = true;
            }
            if (CheckBox_Advanced.Checked)
            {
                Label_Game_Width.Show();
                Label_Game_Height.Show();
                Numeric_Game_Width.Show();
                Numeric_Game_Height.Show();
            } else
            {
                Label_Game_Width.Hide();
                Label_Game_Height.Hide();
                Numeric_Game_Width.Hide();
                Numeric_Game_Height.Hide();
                Numeric_Game_Width.Value = 0m;
                Numeric_Game_Height.Value = 0m;
            }
        }

        private void SplitContainer_Core_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Update_Preview_Box();
        }

        private void Enable_VandG_Pair()
        {
            Button_Pair_VandG.Enabled = (ListView_Videos.SelectedItems.Count > 0 && DropBox_GameProfile.SelectedIndex > -1);

            if (SelectedListVideos != null && SelectedGameProfile != null && SelectedVideos.All(x => x.GameProfile == SelectedGameProfile))
            {
                //FillScreenBox();
                Button_Pair_VandG.Text = "Unpair";
            } else
            {
                CheckedListBox_Screens.Items.Clear();
                Button_Pair_VandG.Text = "Pair";
            }

        }

        private void ComboBox_GameProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enable_VandG_Pair();
        }

        private void Button_Pair_VandG_Click(object sender, EventArgs e)
        {
            if (SelectedVideos.All(x => x.GameProfile == SelectedGameProfile))
            {
                DialogResult dr = MessageBox.Show(
                    "Are you sure you want to unpair the Video(s) from the Game Profile?" +
                    "\n\rThis will remove all Feed-Screen connections.",
                    "Question",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    );
                if (dr == DialogResult.Yes)
                {
                    foreach (var lv in SelectedListVideos)
                    {
                        var i = lv.Index;
                        ((ListVideo)ListView_Videos.Items[i]).Video.GameProfile = null;
                        ((ListVideo)ListView_Videos.Items[i]).RefreshValues();
                    }
                }
            }
            else
            { // Todo: Add something that prompts for replacing.
                var lvs = SelectedListVideos;
                var toRemove = new List<int>(); // Can't do it inline...
                foreach (var lv in lvs)
                {
                    if (lv.Video.GameProfile != SelectedGameProfile)
                    {
                        lv.Video.GameProfile = SelectedGameProfile;

                        foreach (var f in lv.Video.Feeds)
                        {
                            f.GameProfile = SelectedGameProfile;

                            if (CheckBox_AutoMatch.Checked)
                            {
                                var l = SelectedGameProfile.Screens.Where(x => x.Name == f.Name);
                                if (l.Count() > 0) 
                                    f.Screens.Add(l.First());
                            }
                        }
                        lv.RefreshValues();
                    }
                    else
                    {
                        toRemove.Add(lvs.IndexOf(lv));
                    }
                }
                toRemove.Reverse();
                foreach (var i in toRemove)
                {
                    lvs.RemoveAt(i);
                }
                SelectedListVideos = lvs;
                //FillScreenBox();
            }
        }

        private void FillScreenBox()
        {
            CheckedListBox_Screens.Items.Clear();
            if (SelectedGameProfile != null && SelectedVideo != null && SelectedVideo.Feeds != null)
            {
                foreach (var s in SelectedGameProfile.Screens)
                {
                    CheckedListBox_Screens.Items.Add(s);

                    if (SelectedVideo.Feeds.Any(x => x.Screens.Any(y => y == s))) // Is this right?
                        CheckedListBox_Screens_ItemCheckManuel(CheckedListBox_Screens.Items.Count - 1, CheckState.Checked);

                    DropBox_Watch_Preview.Items.Clear();
                    var dropDownWidth = 168;
                    foreach (var wz in s.WatchZones)
                    {
                        foreach (var w in wz.Watches)
                        {
                            foreach (var wi in w.Images)
                            {
                                wi.SetName(s, wz, w); // Help, need not-hacky method to display a better string.
                                DropBox_Watch_Preview.Items.Add(wi);
                                dropDownWidth = Math.Max(dropDownWidth, TextRenderer.MeasureText(wi.Name, DropBox_Watch_Preview.Font).Width);
                            }
                        }
                    }
                    DropBox_Watch_Preview.DropDownWidth = dropDownWidth;
                }
            }
        }

        private void CheckedListBox_Screens_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Switch off event handler
            CheckedListBox_Screens.ItemCheck -= CheckedListBox_Screens_ItemCheck;
            CheckedListBox_Screens.SetItemCheckState(e.Index, e.NewValue);
            // Switch on event handler
            CheckedListBox_Screens.ItemCheck += CheckedListBox_Screens_ItemCheck;

            ((Feed)ListBox_Feeds.SelectedItem).Screens = CheckedScreens;
            foreach (ListVideo lv in ListView_Videos.SelectedItems)
            {
                var i = lv.Index;
                var i2 = ((ListVideo)ListView_Videos.Items[i]).Video.Feeds.IndexOf((Feed)ListBox_Feeds.SelectedItem);
                foreach (Screen s in CheckedListBox_Screens.CheckedItems)
                {
                    ((Feed)((ListVideo)ListView_Videos.Items[i]).Video.Feeds[i2]).AddScreen(s);
                }
            }
            //var tmp = SelectedVideo.Feeds;
        }

        private void CheckedListBox_Screens_ItemCheckManuel(int index, CheckState state)
        {
            CheckedListBox clb = CheckedListBox_Screens;
            // Switch off event handler
            clb.ItemCheck -= CheckedListBox_Screens_ItemCheck;
            clb.SetItemCheckState(index, state);
            // Switch on event handler
            clb.ItemCheck += CheckedListBox_Screens_ItemCheck;
        }
    }

}
