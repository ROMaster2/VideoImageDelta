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

namespace VideoImageDeltaApp.Models
{
    [Flags]
    public enum Anchor
    {
        Undefined = -1,
        None = 0,
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
        public Anchor Anchor { get; set; } = Models.Anchor.Undefined;

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

        public static void AdjustVideo(Video v)
        {
            double x = 32768;
            double y = 32768;
            double width = -32768;
            double height = -32768;
            foreach (var f in v.Feeds)
            {
                x = Math.Min(x, f.Geometry.X);
                y = Math.Min(y, f.Geometry.Y);
                width = Math.Max(width, f.Geometry.X + f.Geometry.Width);
                height = Math.Max(height, f.Geometry.Y + f.Geometry.Height);
            }
            width -= x;
            height -= y;

            v.AdjustedGeometry = new Geometry(x, y, width, height);
        }
        /*
        public static Geometry AdjustWatchZone(Video v, Feed f, Screen s, WatchZone wz)
        {
            if (v.AdjustedGeometry == null) AdjustVideo(v);

            Geometry GameGeometry = s.Geometry;
            if (f.GameGeometry.Width != 0 && f.GameGeometry.Height != 0)
                GameGeometry = f.GameGeometry;

            double xScale = GameGeometry.Width / f.Geometry.Width;
            double yScale = GameGeometry.Height / f.Geometry.Height;
        }
        */
    }

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

        public List<Feed> Feeds { get; set; } = new List<Feed>();
        
        public Image GetThumbnail(TimeSpan timestamp)
        {
            // Would use Hudl but figuring out the syntax is a pain. Documentation kinda sucks.
//            return RawFFmpeg.GetThumbnail(this, timestamp);
            return RawFFmpeg.GetThumbnail(FilePath, new System.Windows.Size(Geometry.Width, Geometry.Height),timestamp);
        }

        override public string ToString()
        {
            return FilePath;
        }

    }

    public class Feed
    {
        public Feed(string name, bool useOCR, bool accurateCapture, Geometry geometry, Geometry gameGeometry = null)
        {
            Name = name;
            UseOCR = useOCR;
            AccurateCapture = accurateCapture;
            Geometry = geometry;
            GameGeometry = gameGeometry;
        }

        internal Feed() { }

        public string Name { get; set; }
        [XmlElement("IsTimer")]
        public bool UseOCR { get; set; }
        [XmlElement("PerfectCapture")]
        public bool AccurateCapture { get; set; }
        public Geometry Geometry { get; set; }
        public Geometry GameGeometry { get; set; }

        public Screen Screen { get; set; } // Experimenting

        override public string ToString()
        { // Figure out how to get + and - to appear based on value.
            return Geometry.Width.ToString() + "x" + Geometry.Height.ToString() + " " + Name;
        }

    }
    /*
    public class VideoProfilePair
    {
        public VideoProfilePair(Video video, GameProfile gameProfile)
        {
            Video = video;
            GameProfile = gameProfile;
        }

        public Video Video;
        public GameProfile GameProfile;
    }

    public class FeedScreenPair
    {
        public FeedScreenPair(Feed feed, Screen screen)
        {
            Feed = feed;
            Screen = screen;
        }

        public Feed Feed;
        public Screen Screen;
    }
    */
    public class GameProfile
    {
        public GameProfile(string name)
        {
            Name = name;
        }

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

    public enum ScreenType // To remove?
    {
        Undefined = -1,
        Static = 0,
        Ratio = 1,
        Dynamic = 2,
    }

    public class Screen
    {
        public Screen(string name, ScreenType screenType, Geometry geometry)
        {
            Name = name;
            ScreenType = screenType;
            Geometry = geometry;
        }

        public string Name { get; set; }
        public ScreenType ScreenType { get; set; } // To remove?
        public Geometry Geometry { get; set; }
        public List<WatchZone> WatchZones { get; set; } = new List<WatchZone>();

        override public string ToString()
        {
            return Name;
        }

    }

    public class WatchZone
    {
        public WatchZone(string name, Geometry geometry)
        {
            Name = name;
            Geometry = geometry;
        }

        public string Name { get; set; }
        public Geometry Geometry { get; set; }
        public List<Watcher> Watches { get; set; } = new List<Watcher>();

        public Geometry AdjustedGeometry { get; set; } = null;

        override public string ToString()
        {
            return Name;
        }

    }

    [Flags]
    public enum ScanType
    {
        None = 0,
        BeforeStart = 1,
        AfterStart = 2,
        During = 4,
        BeforeEnd = 8,
        AfterEnd = 16,
        Before = BeforeStart | BeforeEnd,
        After = AfterStart | AfterEnd,
        Start = BeforeStart | AfterStart,
        End = BeforeEnd | AfterEnd,
        Both = Before | After,
        Everything = Before | During | After,
    }

    public class Watcher
    {
        #region Constructors
        public Watcher(string name, int frequency, ScanType rescanType, TimeSpan rescanRange)
        {
            Name = name;
            Frequency = frequency;
            RescanType = rescanType;
            RescanRange = rescanRange;
        }
        public Watcher(string name, ScanType rescanType, TimeSpan rescanRange)
        {
            Name = name;
            RescanType = rescanType;
            RescanRange = rescanRange;
        }
        public Watcher(string name, int frequency, TimeSpan rescanRange)
        {
            Name = name;
            Frequency = frequency;
            RescanRange = rescanRange;
        }
        public Watcher(string name, TimeSpan rescanRange)
        {
            Name = name;
            RescanRange = rescanRange;
        }
        public Watcher(string name, int frequency, ScanType rescanType)
        {
            Name = name;
            Frequency = frequency;
            RescanType = rescanType;
        }
        public Watcher(string name, ScanType rescanType)
        {
            Name = name;
            RescanType = rescanType;
        }
        public Watcher(string name, int frequency)
        {
            Name = name;
            Frequency = frequency;
        }
        public Watcher(string name)
        {
            Name = name;
        }
        #endregion

        public string Name { get; set; }
        public double? Frequency { get; set; } = 1d; // null = match video framerate
        public ScanType RescanType { get; set; } = ScanType.Both;
        public TimeSpan RescanRange { get; set; } = TimeSpan.FromSeconds(1d);
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

        public void Clear()
        {
            image = null;
        }

        override public string ToString()
        {
            var i = FilePath.LastIndexOf('\\');
            return FilePath.Substring(i + 1, FilePath.Length - i - 1);
        }

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
            UpdateFeeds();
        }

        public Video Video { get; }

        public void UpdateFeeds()
        {
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
                SubItems.Add("Not Set");
            }

        }
    }

}
