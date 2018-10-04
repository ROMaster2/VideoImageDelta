using ImageMagick;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using VideoImageDeltaApp.Models;

using Screen = VideoImageDeltaApp.Models.Screen;

namespace VideoImageDeltaApp.Forms
{
    public partial class AddVideos : Form
    {
        private bool seenWarning = false;

        private static Image thumb1;
        private static Image thumb2;
        private static Image thumb3;

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
                //DropBox_GameProfile.SelectedIndex = 0;
                DropBox_GameProfile.Enabled = true;
            }
            else
            {
                DropBox_GameProfile.Enabled = false;
            }

            if (DropBox_GameProfile.Items.Count > 0)
            {
                //DropBox_GameProfile.SelectedIndex = DropBox_GameProfile.Items.Count - 1;
            }

        }

        private void AddVideos_Load(object sender, EventArgs e)
        {
            Hide_Core();
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Program.Videos.Clear();
            foreach (ListVideo lv in ListView_Videos.Items)
                Program.Videos.Add(lv.Video);
            if (Box_Main.BackgroundImage != null)
                Box_Main.BackgroundImage.Dispose();
            if (thumb1 != null) thumb1.Dispose();
            if (thumb2 != null) thumb2.Dispose();
            if (thumb3 != null) thumb3.Dispose();
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
                        VideoProfile videoProfile;
                        XmlSerializer serializer = new XmlSerializer(typeof(VideoProfile));
                        using (StreamReader reader = new StreamReader(f))
                        {
                            try
                            {
                                videoProfile = (VideoProfile)serializer.Deserialize(reader);
                            } catch
                            {
                                DialogResult dr = MessageBox.Show(f + " is either not a Video profile or has been corrupted.",
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
                        foreach (Video video in SelectedVideos)
                        {
                            if (video.Geometry.Width != videoProfile.Geometry.Width || video.Geometry.Height != videoProfile.Geometry.Height)
                            {
                                geoError = true;
                                break;
                            }
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

                        if (abort)
                        {
                            break;
                        }

                        if (SelectedVideo.GameProfile != null)
                        {
                            videoProfile.Feeds.ForEach(x => x.GameProfile = SelectedVideo.GameProfile);
                            if (CheckBox_AutoMatch.Checked)
                            {
                                foreach (var feed in videoProfile.Feeds)
                                {
                                    var l = SelectedGameProfile.Screens.Where(y => y.Name == feed.Name);
                                    if (l.Count() > 0)
                                        feed.AddScreen(l.First());
                                }
                            }
                        }

                        feeds.AddRange(videoProfile.Feeds);
                    }

                    if (!abort)
                    {
                        foreach (var lv in SelectedListVideos)
                        {
                            var i = lv.Index;
                            foreach (Feed f in feeds)
                            {
                                lv.Video.Feeds.Add(f);
                                lv.RefreshValues();
                                ListView_Videos.Items[i] = lv;
                            }
                        }
                        if (ListBox_Feeds.Items.Count > 0)
                            ListBox_Feeds.SelectedIndex = 0;
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
                    var vp = new VideoProfile(SelectedVideo);
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
        public IReadOnlyList<ListVideo> AllListVideos
        {
            get
            {
                var l = new List<ListVideo>();
                foreach (ListVideo lv in ListView_Videos.Items)
                {
                    l.Add(lv);
                }
                return l.AsReadOnly();
            }
        }

        public IReadOnlyList<ListVideo> SelectedListVideos
        {
            get
            {
                var l = new List<ListVideo>();
                foreach (ListVideo lv in ListView_Videos.SelectedItems)
                {
                    l.Add(lv);
                }
                return l.AsReadOnly();
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

        public IReadOnlyList<Video> AllVideos
        {
            get
            {
                var l = new List<Video>();
                foreach (ListVideo lv in AllListVideos)
                {
                    l.Add(lv.Video);
                }
                return l.AsReadOnly();
            }
        }

        public IReadOnlyList<Video> SelectedVideos
        {
            get
            {
                var l = new List<Video>();
                foreach (ListVideo lv in ListView_Videos.SelectedItems)
                {
                    l.Add(lv.Video);
                }
                return l.AsReadOnly();
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
                ListBox_Feeds.SelectedItem = value;
                var s = SelectedVideo.Feeds.Where(x => x.Name == value.Name).First();
                var i = SelectedVideo.Feeds.IndexOf(s);
                SelectedVideo.Feeds[i] = value;
            }
        }

        public IReadOnlyList<Screen> CheckedScreens
        {
            get
            {
                //if (CheckedListBox_Screens.CheckedItems.Count > 0)
                //{
                var l = new List<Screen>();
                foreach (Screen lv in CheckedListBox_Screens.CheckedItems)
                {
                    l.Add(lv);
                }
                return l.AsReadOnly();
                //}
                //else
                //{
                //    return null;
                //}
            }
        }

        public PreviewType SelectedPreviewType
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DropBox_Preview_Type.Text))
                    return (PreviewType)Enum.Parse(typeof(PreviewType), DropBox_Preview_Type.Text);
                else
                    return PreviewType.Video;
            }
        }


        public Geometry PreviewBoxGeometryBefore = new Geometry(640, 480);
        public Geometry PreviewBoxGeometryRescale = new Geometry(0,0);
        public Geometry PreviewBoxGeometryAfter = new Geometry(0, 0);

        public void CalcPreviewBoxGeometry(bool force = false)
        {
            double x = (double)Numeric_X.Value;
            double y = (double)Numeric_Y.Value;
            double width = Numeric_Width.Value > 0 ?
                (double)Numeric_Width.Value : SelectedVideo.Geometry.Width - (double)Numeric_Width.Value;
            double height = Numeric_Height.Value > 0 ?
                (double)Numeric_Height.Value : SelectedVideo.Geometry.Height - (double)Numeric_Height.Value;
            double gameWidth = (double)Numeric_Game_Width.Value;
            double gameHeight = (double)Numeric_Game_Height.Value;

            // Look, I just don't know where to put these methods sometimes.
            if (force)
            {
                PreviewBoxGeometryBefore = new Geometry(x, y, width, height);
                if (gameWidth > 0 && gameHeight > 0)
                    PreviewBoxGeometryRescale = new Geometry(gameWidth, gameHeight);
                else
                    PreviewBoxGeometryRescale = PreviewBoxGeometryBefore;

                if (DropBox_Watch_Preview.SelectedIndex > -1 && SelectedGameProfile != null)
                {
                    // Using strings to store this stuff is a bad idea...
                    string[] words = DropBox_Watch_Preview.Text.Split('/');
                    if (words.Count() != 3)
                        throw new Exception("Slashes were used on variable names.");
                    var sl = SelectedGameProfile.Screens.Where(z => z.Name == words[0]);
                    if (sl.Count() > 0)
                    {
                        var wzl = sl.First().WatchZones.Where(z => z.Name == words[1]);
                        if (wzl.Count() > 0)
                            PreviewBoxGeometryAfter = wzl.First().Geometry;
                        else
                            throw new Exception("Despite being selectable, the WatchZone does not exist.");
                    }
                    else
                        throw new Exception("Despite being selectable, the Screen does not exist.");
                }
                else
                {
                    PreviewBoxGeometryAfter = PreviewBoxGeometryRescale;
                }
            }
            else
                switch (SelectedPreviewType)
                {
                    case PreviewType.Video:
                        PreviewBoxGeometryBefore = SelectedVideo.Geometry;
                        PreviewBoxGeometryRescale = PreviewBoxGeometryBefore;
                        PreviewBoxGeometryAfter = PreviewBoxGeometryRescale;
                        break;
                    case PreviewType.Feed:
                        PreviewBoxGeometryBefore = new Geometry(x, y, width, height);
                        PreviewBoxGeometryRescale = PreviewBoxGeometryBefore;
                        PreviewBoxGeometryAfter = PreviewBoxGeometryRescale;
                        break;
                    case PreviewType.Screen:
                        PreviewBoxGeometryBefore = new Geometry(x, y, width, height);
                        if (gameWidth > 0 && gameHeight > 0)
                            PreviewBoxGeometryRescale = new Geometry(gameWidth, gameHeight);
                        else
                            PreviewBoxGeometryRescale = PreviewBoxGeometryBefore;
                        PreviewBoxGeometryAfter = PreviewBoxGeometryRescale;
                        break;
                    case PreviewType.WatchZone:
                        PreviewBoxGeometryBefore = new Geometry(x, y, width, height);
                        if (gameWidth > 0 && gameHeight > 0)
                            PreviewBoxGeometryRescale = new Geometry(gameWidth, gameHeight);
                        else
                            PreviewBoxGeometryRescale = PreviewBoxGeometryBefore;

                        if (DropBox_Watch_Preview.SelectedIndex > -1 && SelectedGameProfile != null)
                        {
                            // Using strings to store this stuff is a bad idea...
                            string[] words = DropBox_Watch_Preview.Text.Split('/');
                            if (words.Count() != 3)
                                throw new Exception("Slashes were used on variable names.");
                            var sl = SelectedGameProfile.Screens.Where(z => z.Name == words[0]);
                            if (sl.Count() > 0)
                            {
                                var wzl = sl.First().WatchZones.Where(z => z.Name == words[1]);
                                if (wzl.Count() > 0)
                                    PreviewBoxGeometryAfter = wzl.First().WithoutScale(
                                        SelectedFeed.Geometry,
                                        PreviewBoxGeometryBefore,
                                        PreviewBoxGeometryRescale);
                                else
                                    throw new Exception("Despite being selectable, the WatchZone does not exist.");
                            }
                            else
                                throw new Exception("Despite being selectable, the Screen does not exist.");
                        }
                        else
                        {
                            PreviewBoxGeometryAfter = PreviewBoxGeometryRescale;
                        }
                        break;
                    default:
                        PreviewBoxGeometryBefore = SelectedVideo.Geometry;
                        PreviewBoxGeometryRescale = PreviewBoxGeometryBefore;
                        PreviewBoxGeometryAfter = PreviewBoxGeometryRescale;
                        break;
                }

        }

        public Image RescaleThumbnail(Image image, bool force = false)
        {
            CalcPreviewBoxGeometry(force);
            if (force || (!PreviewBoxGeometryBefore.IsBlank && !PreviewBoxGeometryBefore.Equals(SelectedVideo.Geometry)))
            {
                using (var mi = new MagickImage((Bitmap)image))
                {

                    int x = (int)PreviewBoxGeometryBefore.X;
                    int y = (int)PreviewBoxGeometryBefore.Y;
                    int w = (int)PreviewBoxGeometryBefore.Width;
                    int h = (int)PreviewBoxGeometryBefore.Height;
                    mi.Crop(new MagickGeometry(x, y, w, h), Gravity.Northwest);
                    mi.RePage();
                    if (force || (!PreviewBoxGeometryRescale.IsBlank && !PreviewBoxGeometryRescale.Equals(SelectedVideo.Geometry)))
                    {
                        w = (int)PreviewBoxGeometryRescale.Width;
                        h = (int)PreviewBoxGeometryRescale.Height;
                        mi.Resize(w, h);
                        mi.RePage();
                        if (force || (!PreviewBoxGeometryAfter.IsBlank && !PreviewBoxGeometryAfter.Equals(PreviewBoxGeometryRescale)))
                        {
                            x = (int)PreviewBoxGeometryAfter.X;
                            y = (int)PreviewBoxGeometryAfter.Y;
                            w = (int)PreviewBoxGeometryAfter.Width;
                            h = (int)PreviewBoxGeometryAfter.Height;
                            Gravity g = PreviewBoxGeometryAfter.Anchor.ToGravity();
                            mi.Crop(new MagickGeometry(x, y, w, h), g);
                            mi.RePage();
                            return mi.ToBitmap();
                        }
                        else
                            return mi.ToBitmap();
                    }
                    else
                        return mi.ToBitmap();
                }
            }
            else
                return image;
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
                oldTimestamp = timestamp.ToString();
                if (oldTimestamp.Length == 8) oldTimestamp += ".0000000";
                oldTimestamp = oldTimestamp.ToString().Substring(0, 12);
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

                thumb1 = v.GetThumbnail(timestamp);
                thumb2 = (Bitmap)RescaleThumbnail((Image)thumb1.Clone(), true);
                

                if (SelectedGameProfile != null && SelectedGameProfile.Screens.Count > 0 && DropBox_Watch_Preview.SelectedIndex > -1)
                {
                    // Using strings to store this stuff is a bad idea...
                    string[] words = DropBox_Watch_Preview.Text.Split('/');
                    if (words.Count() != 3)
                        throw new Exception("Slashes were used on variable names.");
                    var sl = SelectedGameProfile.Screens.Where(z => z.Name == words[0]);
                    if (sl.Count() > 0)
                    {
                        var wzl = sl.First().WatchZones.Where(z => z.Name == words[1]);
                        if (wzl.Count() > 0)
                        {
                            var words2 = words[2].Split(new string[] { " - " }, StringSplitOptions.None);
                            if (words2.Count() != 2)
                                throw new Exception("\" - \" was used in a Watcher or File name.");
                            var wl = wzl.First().Watches.Where(z => z.Name == words2[0]);
                            if (wl.Count() > 0)
                            {
                                var il = wl.First().WatchImages.Where(z => z.FileName == words2[1]);
                                if (il.Count() > 0)
                                {
                                    thumb3 = (Bitmap)il.First().Image;
                                }
                                else
                                    throw new Exception("The Image no longer exists." +
                                        "\r\nException handling for this hasn't been fully implemented. Hope you don't crash!");
                            }
                            else
                                throw new Exception("Despite being selectable, the Watcher does not exist.");
                        }
                        else
                            throw new Exception("Despite being selectable, the WatchZone does not exist.");
                    }
                    else
                        throw new Exception("Despite being selectable, the Screen does not exist.");

                    if (CheckBox_Timer.Checked)
                    {
                        var t = SelectedFeed.Geometry;
                        Utilities.ReadImage(thumb1, SelectedFeed.Geometry, out string str, out float confidence);

                        Label_Delta.Text = str;

                        // Does it at LEAST contain a number?
                        bool iTried = Regex.IsMatch(str, @".*[0-9].*");
                        if (iTried)
                        {
                            // Is it a timestamp? eg 01:23:45.678
                            // Anything longer than 100 hours won't get picked up
                            // But there's really not many videos over that length that will use this, right?
                            // Even if there is, just fill it out manually...
                            // Honestly we should filter out everything that doesn't fit this pattern as well.
                            bool isValid = Utilities.ValidateTimeOCR(str);

                            if (!isValid) confidence /= 2f;

                            if (confidence >= 0.9) Label_Delta_Number.ForeColor = Color.Cyan;
                            else if (confidence >= 0.75) Label_Delta_Number.ForeColor = Color.Green;
                            else if (confidence >= 0.75) Label_Delta_Number.ForeColor = Color.FromArgb(255, 192, 0);
                            else if (confidence >= 0.5) Label_Delta_Number.ForeColor = Color.OrangeRed;
                            else Label_Delta_Number.ForeColor = Color.DarkRed;


                            var delta = Math.Round(confidence * 100f, 4);
                            Label_Delta_Number.Text = delta.ToString() + "%";
                            Label_Delta_Number.Show();
                        } 
                        else
                        { 
                            Label_Delta_Number.Text = "0%";
                            Label_Delta_Number.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        Label_Delta.Text = "Delta Check";
                        using (MagickImage mi2 = new MagickImage((Bitmap)thumb2))
                        {
                            using (MagickImage mi1 = new MagickImage((Bitmap)thumb3))
                            {
                                // Make error metric selectable in the future.
                                if (mi1.HasAlpha)
                                {
                                    mi2.Composite(mi1, CompositeOperator.CopyAlpha);
                                }
                                double delta = mi1.Compare(mi2, ErrorMetric.PeakSignalToNoiseRatio);
                                if (delta >= 16) Label_Delta_Number.ForeColor = Color.Cyan;
                                else if (delta >= 8) Label_Delta_Number.ForeColor = Color.Green;
                                else if (delta >= 4) Label_Delta_Number.ForeColor = Color.FromArgb(255, 192, 0);
                                else if (delta >= 2) Label_Delta_Number.ForeColor = Color.OrangeRed;
                                else Label_Delta_Number.ForeColor = Color.DarkRed;

                                delta = Math.Round(delta, 4);
                                Label_Delta_Number.Text = delta.ToString();
                                Label_Delta_Number.Show();
                                if (CheckBox_Display.Checked)
                                {
                                    mi1.Composite(mi2, CompositeOperator.Difference);
                                    thumb1 = ((MagickImage)mi1.Clone()).ToBitmap();
                                }
                            }
                        }
                    }
                    thumb2 = null;
                    thumb3 = null;
                }
                else
                {
                    Label_Delta.Text = "Delta Check";
                    Label_Delta_Number.Text = null;
                }
                if (!CheckBox_Display.Checked)
                    thumb1 = RescaleThumbnail(thumb1);

                if (thumb1 != null)
                {
                    Box_Main.BackgroundImage = thumb1;
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
                double screenWidth = PreviewBoxGeometryBefore.Width;
                double screenHeight = PreviewBoxGeometryBefore.Height;
                if (SelectedPreviewType == PreviewType.Screen && !PreviewBoxGeometryRescale.IsBlank)
                {
                    screenWidth = PreviewBoxGeometryRescale.Width;
                    screenHeight = PreviewBoxGeometryRescale.Height;

                }
                else if (SelectedPreviewType == PreviewType.WatchZone &&
                    DropBox_Watch_Preview.Text != null &&
                    !PreviewBoxGeometryAfter.IsBlank)
                {
                    screenWidth = PreviewBoxGeometryAfter.Width;
                    screenHeight = PreviewBoxGeometryAfter.Height;
                }
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
            double screenWidth = PreviewBoxGeometryBefore.Width;
            double screenHeight = PreviewBoxGeometryBefore.Height;

            double x = (double)Numeric_X.Value;
            double y = (double)Numeric_Y.Value;
            double width = (double)Numeric_Width.Value;
            double height = (double)Numeric_Height.Value;

            double reScaleWidth = 1;
            double reScaleHeight = 1;

            if (SelectedPreviewType == PreviewType.Feed)
            {
                x -= PreviewBoxGeometryBefore.X;
                y -= PreviewBoxGeometryBefore.Y;
            }
            else if (SelectedPreviewType == PreviewType.Screen && !PreviewBoxGeometryRescale.IsBlank)
            {
                x = 0;
                y = 0;
                screenWidth = PreviewBoxGeometryRescale.Width;
                screenHeight = PreviewBoxGeometryRescale.Height;
                reScaleWidth = PreviewBoxGeometryRescale.Width / PreviewBoxGeometryBefore.Width;
                reScaleHeight = PreviewBoxGeometryRescale.Height / PreviewBoxGeometryBefore.Height;
            }
            else if (SelectedPreviewType == PreviewType.WatchZone &&
                DropBox_Watch_Preview.Text != null &&
                !PreviewBoxGeometryAfter.IsBlank)
            {
                // Anchors make this complicated
                // Width and Height are good but X and Y need work.
                reScaleWidth = PreviewBoxGeometryRescale.Width / PreviewBoxGeometryBefore.Width;
                reScaleHeight = PreviewBoxGeometryRescale.Height / PreviewBoxGeometryBefore.Height;
                //var tmp = PreviewBoxGeometryAfter.WithoutAnchor(PreviewBoxGeometryRescale);
                x = 0;
                y = 0;
                screenWidth = PreviewBoxGeometryAfter.Width;
                screenHeight = PreviewBoxGeometryAfter.Height;
            }


            Size size = new Size();
            Point point = new Point();
            double scaleWidth = Box_Main.Size.Width / screenWidth * reScaleWidth;
            double scaleHeight = Box_Main.Size.Height / screenHeight * reScaleHeight;

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

            if (SelectedVideo != null && SelectedVideo.GameProfile != null && ListBox_Feeds.SelectedItems.Count > 0)
                FillScreenBox();
            else
                CheckedListBox_Screens.Items.Clear();


            Enable_VandG_Pair();
        }

        private void Clean_Inputs()
        {
            TextBox_Name.Text = null;
            CheckBox_Timer.Checked = false;
            Numeric_X.Value = 0m;
            Numeric_Y.Value = 0m;
            Numeric_Width.Value = 0m;
            Numeric_Height.Value = 0m;
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
                Numeric_X.Value = (decimal)f.Geometry.X;
                Numeric_Y.Value = (decimal)f.Geometry.Y;
                Numeric_Width.Value = (decimal)f.Geometry.Width;
                Numeric_Height.Value = (decimal)f.Geometry.Height;
                if (!f.GameGeometry.IsBlank)
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
            if (CheckBox_Timer.Checked)
            {
                Numeric_Game_Width.Enabled = false;
                Numeric_Game_Height.Enabled = false;
                Numeric_Game_Width.Value = 0L;
                Numeric_Game_Height.Value = 0L;
            }
            else
            {
                Numeric_Game_Width.Enabled = true;
                Numeric_Game_Height.Enabled = true;
            }
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
            bool isValid = Validate_Timestamp_Text(str) && SelectedVideo != null;
            if (isValid)
            {
                oldTimestamp = str;
                TimeSpan timestamp = TimeSpan.Parse(str);
                if (SelectedVideo.Duration > timestamp)
                {
                    TextBox_Timestamp.BackColor = Color.White;
                    Display_Thumbnail(SelectedVideo, timestamp);
                    TrackBar_Timestamp.Value =
                        (int)Math.Round(timestamp.TotalSeconds / SelectedVideo.Duration.TotalSeconds * TrackBar_Timestamp.Maximum);
                } else
                {
                    TextBox_Timestamp.BackColor = Color.FromArgb(224, 64, 64);
                }
            } else
            {
                TextBox_Timestamp.BackColor = Color.FromArgb(224, 64, 64);
            }
        }

        private bool Validate_Timestamp_Text(string str)
        {
            //bool isValid = Regex.IsMatch(str, @"^(?:(?:([01]?\d|2[0-3]):)?([0-5]\d):)?([0-5]\d)$");
            return TimeSpan.TryParse(str, out TimeSpan time);
        }

        private void Button_Add_Feed_Click(object sender, EventArgs e)
        {
            string name = TextBox_Name.Text;
            name = name.Trim();
            bool useOCR = CheckBox_Timer.Checked;

            int x = (int)Numeric_X.Value;
            int y = (int)Numeric_Y.Value;
            int width = (int)Numeric_Width.Value;
            int height = (int)Numeric_Height.Value;
            Geometry geo = new Geometry(x, y, width, height);
            Geometry gameGeo = new Geometry(0,0);

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

            var feed = new Feed(name, useOCR, geo, gameGeo)
            {
                GameProfile = SelectedGameProfile
            };

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
            if (SelectedVideo != null && SelectedVideo.GameProfile != null && ListBox_Feeds.SelectedItems.Count > 0)
            {
                FillScreenBox();
            }
            else
            {
                CheckedListBox_Screens.Items.Clear();
            }
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
        private void TrackBar_Timestamp_MouseUp(object sender, MouseEventArgs e)
        {
            var seconds = SelectedVideo.Duration.TotalSeconds * TrackBar_Timestamp.Value / TrackBar_Timestamp.Maximum;
            seconds = Math.Floor(seconds * SelectedVideo.FrameRate) / SelectedVideo.FrameRate;
            var timestamp = TimeSpan.FromSeconds(Math.Min(SelectedVideo.Duration.TotalSeconds, Math.Max(seconds, 0d)));
            TextBox_Timestamp.Text = timestamp.ToString(@"hh\:mm\:ss\.fff", CultureInfo.CurrentCulture);
        }

        private void TrackBar_Timestamp_KeyUp(object sender, KeyEventArgs e)
        {
            TrackBar_Timestamp_MouseUp(null, null);
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
                //DialogResult dr = MessageBox.Show("Only use these options if you understand what they do.",
                //    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                seenWarning = true;
            }
            if (CheckBox_Advanced.Checked)
            {
                Numeric_X.DecimalPlaces = 2;
                Numeric_Y.DecimalPlaces = 2;
                Numeric_Width.DecimalPlaces = 2;
                Numeric_Height.DecimalPlaces = 2;
                Label_Game_Width.Show();
                Label_Game_Height.Show();
                Numeric_Game_Width.Show();
                Numeric_Game_Height.Show();
            } else
            {
                Numeric_X.DecimalPlaces = 0;
                Numeric_Y.DecimalPlaces = 0;
                Numeric_Width.DecimalPlaces = 0;
                Numeric_Height.DecimalPlaces = 0;
                Label_Game_Width.Hide();
                Label_Game_Height.Hide();
                Numeric_Game_Width.Hide();
                Numeric_Game_Height.Hide();
                Numeric_Game_Width.Value = 0m; // Keep?
                Numeric_Game_Height.Value = 0m;
            }
        }

        private void SplitContainer_Core_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Update_Preview_Box();
        }

        private void Enable_VandG_Pair()
        {
            Button_Pair_VandG.Enabled = ListView_Videos.SelectedItems.Count > 0 && DropBox_GameProfile.SelectedIndex > -1;

            if (SelectedListVideos != null && SelectedGameProfile != null && SelectedVideos.All(x => x.GameProfile == SelectedGameProfile))
            {
                Button_Pair_VandG.Text = "Unpair";
            } else
            {
                Button_Pair_VandG.Text = "Pair";
            }

        }

        private void DropBox_GameProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enable_VandG_Pair();
            if (SelectedVideo != null && SelectedVideo.GameProfile != null && ListBox_Feeds.SelectedItems.Count > 0)
                FillScreenBox();
            else
                CheckedListBox_Screens.Items.Clear();
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
                        lv.Video.GameProfile = null;

                        foreach (var f in lv.Video.Feeds)
                        {
                            f.GameProfile = null;
                            f.ClearScreens();
                        }

                        lv.RefreshValues();
                    }
                }
            }
            else
            {
                bool replace = true;

                if (!SelectedVideos.Any(x => x.GameProfile != null || x.GameProfile != SelectedGameProfile))
                {
                    DialogResult dr = MessageBox.Show(
                        "Do you want to replace the currently applied GameProfile(s)?" +
                        "\n\rThis will remove all Feed-Screen connections.",
                        "Question",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                        );
                    replace = dr == DialogResult.Yes;
                }

                if (replace)
                {
                    foreach (var lv in SelectedListVideos)
                    {
                        lv.Video.GameProfile = SelectedGameProfile;

                        foreach (var f in lv.Video.Feeds)
                        {
                            f.GameProfile = SelectedGameProfile;

                            if (CheckBox_AutoMatch.Checked)
                            {
                                var l = SelectedGameProfile.Screens.Where(x => x.Name == f.Name);
                                if (l.Count() > 0)
                                    f.AddScreen(l.First());
                            }
                        }
                        lv.RefreshValues();
                    }
                }
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

                    // Okay, the "SelectedVideo.GameProfile != null" shouldn't be necessary. There's a bug afoot.
                    if (SelectedVideo.GameProfile != null &&
                        SelectedFeed != null &&
                        SelectedFeed.Screens.Any(y => y == s)) // Is this right?
                            CheckedListBox_Screens.SetItemCheckState(CheckedListBox_Screens.Items.Count - 1, CheckState.Checked);

                    DropBox_Watch_Preview.Items.Clear();
                    var dropDownWidth = 168;
                    foreach (var wz in s.WatchZones)
                    {
                        foreach (var w in wz.Watches)
                        {
                            foreach (var wi in w.WatchImages)
                            {
                                wi.SetName(s, wz, w); // Help, need not-hacky method to display a better string.
                                DropBox_Watch_Preview.Items.Add(wi);
                                dropDownWidth = Math.Max(dropDownWidth, TextRenderer.MeasureText(wi.Name, DropBox_Watch_Preview.Font).Width);
                            }
                        }
                    }
                    DropBox_Watch_Preview.DropDownWidth = dropDownWidth;
                    DropBox_Watch_Preview.Enabled = true;
                    if (DropBox_Watch_Preview.Items.Count > 0)
                    {
                        DropBox_Watch_Preview.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                DropBox_Watch_Preview.Enabled = false;
                DropBox_Watch_Preview.SelectedIndex = -1;
            }
        }

        // ItemCheck was causing all kinds of problems.
        private void CheckedListBox_Screens_MouseUp(object sender, MouseEventArgs e)
        {
            if (SelectedFeed != null && CheckedListBox_Screens.Items.Count > 0)
            {
                SelectedFeed.ClearScreens();
                var l = SelectedVideo.Feeds.Where(x => x.Name == SelectedFeed.Name).First();
                var i = SelectedVideo.Feeds.IndexOf(l);
                SelectedVideo.Feeds[i].ClearScreens();
                foreach (Screen s in CheckedListBox_Screens.CheckedItems)
                {
                    //SelectedFeed._Screens.Add(s.Name);
                    // I'm desperate here...
                    for (int n = 0; n < SelectedVideo.Feeds.Count(); n++)
                    {
                        if (n == i)
                        {
                            SelectedVideo.Feeds[n].AddScreen(s);
                        }
                    }
                    //SelectedVideo.Feeds[i]._Screens.Add(s.Name);
                }
                SelectedListVideo.RefreshValues();
            }
        }

        private void ListBox_Feeds_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && SelectedFeed != null)
            {
                DialogResult dr = MessageBox.Show(
                    "Are you sure you want to remove this feed?",
                    "Question",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    );

                if (dr == DialogResult.Yes)
                {
                    SelectedVideo.Feeds.Remove(SelectedFeed);
                    ListBox_Feeds.Items.Remove(SelectedFeed);
                    SelectedListVideo.RefreshValues();
                }
            }
        }

        private void DropBox_Watch_Preview_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_Timestamp_TextChanged(null, null);
        }

        private void DropBox_Preview_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_Timestamp_TextChanged(null, null);
        }

        private void DropBox_Watch_Preview_TextChanged(object sender, EventArgs e)
        {
            Button_AutoAlign.Enabled = DropBox_Watch_Preview.SelectedIndex > -1;
            CheckBox_Display.Enabled = DropBox_Watch_Preview.SelectedIndex > -1;
        }

        private void CheckBox_Display_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Display.Checked)
            {
                Box_Preview.Hide();
                DropBox_Preview_Type.SelectedIndex = DropBox_Preview_Type.Items.Count - 1;
            }
            else
                Box_Preview.Show();
            TextBox_Timestamp_TextChanged(null, null);
        }

        private void Button_AutoAlign_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Not ready yet lol",
                "Nope", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }

}
