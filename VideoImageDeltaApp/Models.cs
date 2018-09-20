using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
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

using Anchor = VideoImageDeltaApp.Models.Anchor;

namespace VideoImageDeltaApp.Models
{
    [Flags]
    public enum Anchor
    {
        Undefined = -1,
        Center = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right
    }

    public class Geometry
    {
        #region Constructors
        public Geometry() { }
        public Geometry(double widthAndHeight)
        {
            Width = widthAndHeight;
            Height = widthAndHeight;
        }
        public Geometry(double width, double height)
        {
            Width = width;
            Height = height;
        }
        public Geometry(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public Geometry(double widthAndHeight, Anchor anchor)
        {
            Width = widthAndHeight;
            Height = widthAndHeight;
            Anchor = anchor;
        }
        public Geometry(double width, double height, Anchor anchor)
        {
            Width = width;
            Height = height;
            Anchor = anchor;
        }
        public Geometry(double x, double y, double width, double height, Anchor anchor)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Anchor = anchor;
        }
        #endregion

        public double X { get; set; } = 0d;
        public double Y { get; set; } = 0d;
        public double Width { get; set; } = 0d;
        public double Height { get; set; } = 0d;
        public Anchor Anchor { get; set; } = Anchor.Undefined;

        public double Ratio
        {
            get
            {
                if (Width != 0d && Height != 0d)
                {
                    return Width / Height;
                }
                else
                {
                    return 0d;
                }
            }
        }

        public bool HasPoint()
        {
            return (X != 0) || (Y != 0);
        }

        public bool IsEmpty()
        {
            return (Width != 0) && (Height != 0);
        }

        public bool HasAnchor()
        {
            return Anchor != Anchor.Undefined;
        }

        public Geometry WithoutAnchor(Geometry baseGeometry)
        {
            double _x;
            double _y;

            if (Anchor == Anchor.Undefined || 
                Anchor == Anchor.Center ||
                Anchor.HasFlag(Anchor.Left))
            {
                _x = X;
            } else if (Anchor.HasFlag(Anchor.Right))
            {
                _x = baseGeometry.Width - Width + X;
            } else
            {
                _x = (baseGeometry.Width - Width) / 2d + X;
            }

            if (Anchor == Anchor.Undefined || 
                Anchor == Anchor.Center ||
                Anchor.HasFlag(Anchor.Top))
            {
                _y = Y;
            }
            else if (Anchor.HasFlag(Anchor.Bottom))
            {
                _y = baseGeometry.Height - Height + Y;
            }
            else
            {
                _y = (baseGeometry.Height - Height) / 2d + Y;
            }

            return new Geometry(_x, _y, Width, Height, Anchor.Undefined);
        }
    }

    /*
    // Pair by names for now. How can you avoid unnecessary duplication and desyncs from changes?
    public class Pair
    {
        public Pair (Video video, GameProfile gameProfile, bool auto = true)
        {
            Video = video;
            GameProfile = gameProfile;
        }

        private string _Video;
        public Video Video
        {
            get
            {
                return Program.Videos.Where(x => x.FilePath == _Video).First();
            }
            set
            {
                _Video = value.FilePath;
            }
        }

        private string _GameProfile;
        public GameProfile GameProfile
        {
            get
            {
                var l = Program.GameProfiles.Where(x => x.Name == _GameProfile);
                if (l.Count() == 0)
                {
                    return new GameProfile("Not set");
                }
                else
                    return l.First();
            }
            set
            {
                if (value == null)
                    _GameProfile = null;
                else
                    _GameProfile = value.Name;
            }
        }

        public List<SubPair> SubPairs { get; set; } = new List<SubPair>();

        public class SubPair
        {
            public SubPair(Feed feed, Screen screen)
            {
                Feed = feed;
                Screen = screen;
            }

            private string _Feed;
            public Feed Feed
            {
                get
                {
                    var v = Program.Videos.Where(x => x.FilePath == _Video).First();
                    return v.Feeds.Where(x => x.Name == _Feed).First();
                }
                set
                {
                    _Feed = value.Name;
                }
            }

            private string _Screen;
            public Screen Screen
            {
                get
                {
                    var gp = Program.GameProfiles.Where(x => x.Name == _GameProfile).First();
                    return gp.Screens.Where(x => x.Name == _Screen).First();
                }
                set
                {
                    _Screen = value.Name;
                }
            }

        }
    }
    */
    public class VideoProfile
    {
        public VideoProfile(string name, Video video)
        {
            Name = name;
            Geometry = video.Geometry;
            Feeds = video.Feeds;
        }

        public VideoProfile(string name, Geometry geometry, List<Feed> feeds)
        {
            Name = name;
            Geometry = geometry;
            Feeds = feeds;
        }

        internal VideoProfile() { }

        public string Name;
        // Todo: Add set condition that Feed geometry MUST be within the video geometry.
        public Geometry Geometry;
        public List<Feed> Feeds;

        override public string ToString()
        {
            return Name;
        }
    }

    public class Video
    {
        public static Video Create(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var tmp = Resource.From(filePath);
                    tmp.LoadMetadata();
                    var tmp2 = (VideoStream)tmp.Streams.First(x => x.ResourceIndicator == "v");
                    return new Video(filePath, tmp2.Info.VideoMetadata);
                }
                catch (InvalidOperationException e)
                {
                    DialogResult dr = MessageBox.Show(
                        "Sorry, but this file type is currently not supported.",
                        e.ToString(),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                }
                return null;
            }
            else
            {
                Utilities.Error(28641);
                return null;
            }

        }

        private Video(string filePath, VideoStreamMetadata metaData)
        {
            FilePath = filePath;
            if (File.Exists(filePath))
            {
                RawMetadata = metaData;

                Duration = RawMetadata.Duration;
                if (Duration == TimeSpan.Zero) Duration = RawFFmpeg.GetDuration(filePath);

                Geometry = new Geometry(RawMetadata.Width, RawMetadata.Height);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        internal Video() { }

        public string FilePath { get; }
        public VideoStreamMetadata RawMetadata { get; }
        public TimeSpan Duration { get; }
        public Geometry Geometry { get; }
        public Geometry AdjustedGeometry { get; set; }
        public double FrameRate {
            get
            {
                var rfr = RawMetadata.RFrameRate;
                return (double)rfr.Numerator / rfr.Denominator;
            }
        }

        public int FrameCount
        {
            get
            {
                return Convert.ToInt32(FrameRate * Duration.TotalSeconds);
            }
        }

        private string _GameProfile;
        public GameProfile GameProfile
        {
            get
            {
                var l = Program.GameProfiles.Where(x => x.Name == _GameProfile);
                if (l.Count() == 0)
                    return null;
                else
                    return l.First();
            }
            set
            {
                if (value == null)
                    _GameProfile = null;
                else
                    _GameProfile = value.Name;
            }
        }

        public List<Feed> Feeds = new List<Feed>();

        public bool IsSynced()
        {
            return Feeds.Count > 0 && Feeds.All(x => x.GameProfile != null) && Feeds.All(x => x.Screens.Count > 0);
        }
        
        public Image GetThumbnail(TimeSpan timestamp)
        {
            // Would use Hudl but figuring out the syntax is a pain. Documentation kinda sucks.
            //return RawFFmpeg.GetThumbnail(this, timestamp);
            return RawFFmpeg.GetThumbnail(FilePath, new System.Windows.Size(Geometry.Width, Geometry.Height),timestamp);
        }

        // Full name would be CalculateChildAdjustedGeometry, but that's a mouthful and too many characters.
        /// <summary>
        /// Cache the geometry adjustments needed to position and size images to their spot on the video.
        /// This is done so it's not crunched every single process cycle.
        /// Warning: Could break if negative offsets are used. Need to test.
        /// WARNING: This alters the GameProfile's WatchZone's AdjGeo, which affects EVERY VIDEO using that GameProfile.
        /// Todo: That MUST be fixed.
        /// </summary>
        public void CalcChildAdjGeo()
        {
            double x = 32768d;
            double y = 32768d;
            double width = -32768d;
            double height = -32768d;
            foreach (var f in Feeds)
            {
                x = Math.Min(x, f.Geometry.X);
                y = Math.Min(y, f.Geometry.Y);
                width = Math.Max(width, f.Geometry.X + f.Geometry.Width);
                height = Math.Max(height, f.Geometry.Y + f.Geometry.Height);
            }
            width -= x;
            height -= y;

            AdjustedGeometry = new Geometry(x, y, width, height, Anchor.Undefined);

            foreach (var f in Feeds)
            {
                foreach (var s in f.Screens)
                {
                    Geometry GameGeometry = s.Geometry;
                    if (f.GameGeometry.Width > 0d && f.GameGeometry.Height > 0d) GameGeometry = f.GameGeometry;

                    double xScale = GameGeometry.Width / f.Geometry.Width;
                    double yScale = GameGeometry.Height / f.Geometry.Height;
                    foreach (var wz in s.WatchZones)
                    {
                        double _x = wz.Geometry.X;
                        double _y = wz.Geometry.Y;
                        double _width = wz.Geometry.Width;
                        double _height = wz.Geometry.Height;

                        if (wz.ScaleType == ScaleType.Scale)
                        {
                            _x *= xScale;
                            _y *= yScale;
                            _width *= xScale;
                            _height *= yScale;
                        }
                        else if (wz.ScaleType == ScaleType.KeepRatio)
                        {
                            double scale = Math.Min(xScale, yScale); // Min or Max?
                            _x *= scale;
                            _y *= scale;
                            _width *= scale;
                            _height *= scale;
                        }

                        var newGeo = new Geometry(_x, _y, _width, _height, wz.Geometry.Anchor).WithoutAnchor(GameGeometry);

                        newGeo.X = newGeo.X / GameGeometry.Width * f.Geometry.Width + f.Geometry.X - this.AdjustedGeometry.X;
                        newGeo.Y = newGeo.Y / GameGeometry.Height * f.Geometry.Height + f.Geometry.Y - this.AdjustedGeometry.Y;
                        newGeo.Width = newGeo.Width / GameGeometry.Width * f.Geometry.Width;
                        newGeo.Height = newGeo.Height / GameGeometry.Height * f.Geometry.Height;

                        wz.AdjustedGeometry = newGeo;
                    }
                }
            }
        }

        override public string ToString()
        {
            return FilePath;
        }

    }

    public class Feed
    {
        public Feed(string name, bool useOCR, Geometry geometry, Geometry gameGeometry = null)
        {
            Name = name;
            UseOCR = useOCR;
            Geometry = geometry;
            GameGeometry = gameGeometry;
        }

        internal Feed() { }

        public string Name { get; set; }
        [XmlElement("IsTimer")]
        public bool UseOCR { get; set; }
        public Geometry Geometry { get; set; }
        public Geometry GameGeometry { get; set; }

        public string FullName
        {
            get
            {
                string start = Geometry.Width.ToString() + "x" + Geometry.Height.ToString();
                string middle = null;
                if (Geometry.HasPoint() || Geometry.HasAnchor())
                    middle = Utilities.PrefixNumber((decimal)Geometry.X) + Utilities.PrefixNumber((decimal)Geometry.Y);
                return Name + " - " + start + middle;
            }
        }

        private string _GameProfile;
        public GameProfile GameProfile
        {
            get
            {
                var l = Program.GameProfiles.Where(x => x.Name == _GameProfile);
                if (l.Count() == 0)
                    return null;
                else
                    return l.First();
            }
            set
            {
                if (value == null)
                    _GameProfile = null;
                else
                    _GameProfile = value.Name;
            }
        }

        // Would be private, but then methods like Add and Clear won't passthrough...
        private List<string> _Screens = new List<string>();
        public IReadOnlyList<string> ScreensRaw
        {
            get
            {
                return _Screens.AsReadOnly();
            }
        }
        public IReadOnlyList<Screen> Screens
        {
            get
            {
                var gps = Program.GameProfiles.Where(x => x.Name == _GameProfile);
                if (gps.Count() == 0)
                    throw new ArgumentException("Game Profile is not set.");
                    //return new List<Screen>().AsReadOnly();
                else
                {
                    var ss = new List<Screen>();
                    foreach (var s in _Screens)
                    {
                        ss.Add(gps.First().Screens.Where(x => x.Name == s).First());
                    }
                    return ss.AsReadOnly();
                }
            }
        }

        public void AddScreen(Screen screen)
        {
            if (!_Screens.Exists(x => x == screen.Name))
                _Screens.Add(screen.Name);
        }
        public void ClearScreens()
        {
            _Screens.Clear();
        }

        override public string ToString()
        {
            return FullName;
        }

    }

    public class GameProfile
    {
        public GameProfile(string name)
        {
            Name = name;
        }

        public GameProfile() { }

        public string Name { get; set; }
        public List<Screen> Screens { get; set; } = new List<Screen>();

        public void Serialize(string output)
        {
            Type t = GetType();
            XmlSerializer serializer = new XmlSerializer(t);
            using (TextWriter writer = new StreamWriter(output))
            {
                serializer.Serialize(writer, this);
            }
        }

        override public string ToString()
        {
            return Name;
        }

    }

    public class Screen
    {
        public Screen(string name, Geometry geometry)
        {
            Name = name;
            UseAdvanced = false;
            Geometry = null;
        }

        public Screen(string name, bool useAdvanced, Geometry geometry)
        {
            Name = name;
            UseAdvanced = useAdvanced;
            Geometry = geometry;
        }

        internal Screen() { }

        public string Name { get; set; }
        public bool UseAdvanced { get; set; }
        public Geometry Geometry { get; set; }
        public List<WatchZone> WatchZones { get; set; } = new List<WatchZone>();

        override public string ToString()
        {
            return Name;
        }

    }

    public enum ScaleType
    {
        Undefined = -1,
        NoScale = 0,
        KeepRatio = 1,
        Scale = 2,
    }

    public class WatchZone
    {
        public WatchZone(string name, ScaleType scaleType, Geometry geometry)
        {
            Name = name;
            ScaleType = scaleType;
            Geometry = geometry;
        }

        internal WatchZone() { }

        public string Name { get; set; }
        public ScaleType ScaleType { get; set; }
        public Geometry Geometry { get; set; }
        public List<Watcher> Watches { get; set; } = new List<Watcher>();

        public Geometry AdjustedGeometry { get; set; } = null;

        override public string ToString()
        {
            return Name;
        }

    }

    public class Watcher
    {
        public Watcher(string name, double frequency)
        {
            Name = name;
            Frequency = frequency;
        }
        public Watcher(string name)
        {
            Name = name;
        }

        internal Watcher() { }

        public string Name { get; set; }
        public double Frequency { get; set; } = 1d;
        public List<WatchImage> Images { get; set; } = new List<WatchImage>(); // To expand

        override public string ToString()
        {
            return Name;
        }

    }

    public class WatchImage // Just to give it a name in the boxes...mostly
    {
        public WatchImage(string filePath)
        {
            FilePath = filePath;
        }

        internal WatchImage() { }

        public string FilePath { get; set; }
        private Image image;
        public Image Image
        {
            get
            {
                if (image == null)
                {
                    image = Image.FromFile(@FilePath);
                }
                return image;
            }
        }

        public string FileName
        {
            get
            {
                var i = FilePath.LastIndexOf('\\');
                return FilePath.Substring(i + 1, FilePath.Length - i - 1);
            }
        }

        [XmlIgnore]
        public string Name { get; internal set; }

        public void SetName(Screen screen, WatchZone watchZone, Watcher watcher)
        {
            Name = screen.Name + "/" + watchZone.Name + "/" + watcher.Name + " - " + FileName;
        }

        public void Clear()
        {
            image = null;
        }

        override public string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Name))
                return Name;
            else
                return FileName;
        }

    }

    public enum PreviewType
    {
        Video,
        Feed,
        Screen,
        WatchZone,
        Watcher
    }

    public class ListVideo : ListViewItem
    {
        public ListVideo(Video video) : base()
        {
            Video = video;
            Text = Video.FilePath;
            SubItems.Add((Math.Round(Video.FrameRate * 1000d) / 1000d).ToString());
            SubItems.Add(Video.Geometry.Width.ToString());
            SubItems.Add(Video.Geometry.Height.ToString());
            SubItems.Add(Video.Duration.ToString().Substring(0, 8));
            SubItems.Add("");
            SubItems.Add("");
            SubItems.Add("");
            RefreshValues();
        }

        public Video Video { get; }

        public void RefreshValues()
        {
            // What's a better method? lol
            SubItems.RemoveAt(SubItems.Count - 1);
            SubItems.RemoveAt(SubItems.Count - 1);
            SubItems.RemoveAt(SubItems.Count - 1);

            if (Video.Feeds.Count > 1)
            {
                string s = null;
                foreach (Feed f in Video.Feeds)
                {
                    if (s != null)
                    {
                        s = s + ", " + f.Name;
                    }
                    else
                    {
                        s = f.Name;
                    }
                }
                SubItems.Add(s);
            }
            else if (Video.Feeds.Count == 1)
            {
                SubItems.Add(Video.Feeds[0].ToString());
            }
            else
            {
                SubItems.Add("None Set");
            }

            if (Video.GameProfile != null)
                SubItems.Add(Video.GameProfile.Name);
            else
                SubItems.Add("Not Set");

            SubItems.Add(Video.IsSynced() ? "Yes" : "No");

        }
    }
}
