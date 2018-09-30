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

using Microsoft.VisualBasic.Devices;
using Anchor = VideoImageDeltaApp.Models.Anchor;
using System.Runtime.InteropServices;

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
        public Geometry() { }
        public Geometry(double width, double height, Anchor anchor = Anchor.Undefined)
        {
            Width = width;
            Height = height;
            Anchor = anchor;
        }
        public Geometry(double x, double y, double width, double height, Anchor anchor = Anchor.Undefined)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Anchor = anchor;
        }

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

        // Only Northwest for now
        public void UpdateRelativeToPoint(double x, double y)
        {
            X = X - x;
            Y = Y - y;
        }
        public void UpdateRelativeToPoint(Geometry geo)
        {
            X = X - geo.X;
            Y = Y - geo.Y;
        }
        public Geometry RelativeToPoint(double x, double y)
        {
            return new Geometry(Width, Height)
            {
                X = X - x,
                Y = Y - y
            };
        }
        public Geometry RelativeToPoint(Geometry geo)
        {
            return new Geometry(Width, Height)
            {
                X = X - geo.X,
                Y = Y - geo.Y
            };
        }

        // Can't be in the namespace, so...
        public static Gravity AnchorToGravity(Anchor anchor)
        {
            switch (anchor)
            {
                case Anchor.TopLeft: return Gravity.Northwest;
                case Anchor.Top: return Gravity.North;
                case Anchor.TopRight: return Gravity.Northeast;
                case Anchor.Left: return Gravity.West;
                case Anchor.Center: return Gravity.Center;
                case Anchor.Right: return Gravity.East;
                case Anchor.BottomLeft: return Gravity.Southwest;
                case Anchor.Bottom: return Gravity.South;
                case Anchor.BottomRight: return Gravity.Southeast;
                default: return Gravity.Undefined;
            }
        }

        public static Anchor GravityToAnchor(Gravity gravity)
        {
            switch (gravity)
            {
                case Gravity.Northwest: return Anchor.TopLeft;
                case Gravity.North: return Anchor.Top;
                case Gravity.Northeast: return Anchor.TopRight;
                case Gravity.West: return Anchor.Left;
                case Gravity.Center: return Anchor.Center;
                case Gravity.East: return Anchor.Right;
                case Gravity.Southwest: return Anchor.BottomLeft;
                case Gravity.South: return Anchor.Bottom;
                case Gravity.Southeast: return Anchor.BottomRight;
                default: return Anchor.Undefined;
            }
        }

        public MagickGeometry ToMagick()
        {
            return new MagickGeometry((int)X, (int)Y, (int)Width, (int)Height);
        }

        public Tesseract.Rect ToTesseract()
        {
            return new Tesseract.Rect((int)X, (int)Y, (int)Width, (int)Height);
        }

        public string ToFFmpegString()
        {
            return Width.ToString() + ':' + Height.ToString() + ':' + X.ToString() + ':' + Y.ToString();
        }

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
                ffprobeType tmp = RawFFmpeg.GetRawMetadata(filePath);
                return new Video(filePath, tmp);
            }
            else
            {
                Utilities.Error(28641);
                return null;
            }

        }

        private Video(string filePath, ffprobeType metaData)
        {
            FilePath = filePath;
            if (File.Exists(filePath))
            {
                RawMetadata = metaData;

                Duration = TimeSpan.FromSeconds(
            (double)RawVideoMetadata.duration_ts * (double)Utilities.DivideString(RawVideoMetadata.time_base)
            );
                if (Duration == TimeSpan.Zero) Duration = RawFFmpeg.GetDuration(FilePath);

                Geometry = new Geometry(RawVideoMetadata.width, RawVideoMetadata.height);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        internal Video() { }

        public string FilePath;
        public ffprobeType RawMetadata { get; }
        public Geometry Geometry;
        public Geometry ThumbnailGeometry { get; set; }

        public streamType RawVideoMetadata { get { return RawMetadata.streams.Where(x => x.codec_type == "video").First(); } }
        public TimeSpan Duration { get; }
        public double FrameRate { get { return (double)Utilities.DivideString(RawVideoMetadata.r_frame_rate); } }
        //public double IFrameRate { get { return (double)Utilities.DivideString(RawVideoMetadata.); } }

        public int FrameCount { get { return (int)(FrameRate * Duration.TotalSeconds); } }

        [XmlIgnore]
        private string _GameProfile;
        [XmlIgnore]
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

        [XmlIgnore]
        public List<Screen> Screens
        { get { var a = new List<Screen>();     a.AddRange(     Feeds.SelectMany(f  => f.Screens));     return a; } }
        [XmlIgnore]
        public List<WatchZone> WatchZones
        { get { var a = new List<WatchZone>();  a.AddRange(   Screens.SelectMany(s  => s.WatchZones));  return a; } }
        [XmlIgnore]
        public List<Watcher> Watches
        { get { var a = new List<Watcher>();    a.AddRange(WatchZones.SelectMany(wz => wz.Watches));    return a; } }
        [XmlIgnore]
        public List<WatchImage> WatchImages
        { get { var a = new List<WatchImage>(); a.AddRange(  Watches.SelectMany(w  => w.WatchImages)); return a; } }

        public double MaxScanRate { get { return Math.Round(Watches.Max(w => w.Frequency) * 300d) / 300d; } }

        public bool IsSynced()
        {
            return Feeds.Count > 0 && Feeds.All(x => x.GameProfile != null) && Feeds.All(x => x.Screens.Count > 0 || x.UseOCR);
        }
        
        public Image GetThumbnail(TimeSpan timestamp)
        {
            return RawFFmpeg.GetThumbnail(FilePath, new System.Windows.Size(Geometry.Width, Geometry.Height),timestamp);
        }

        public void InitProcess()
        {
            Feeds.ForEach(f => f.InitProcess());
            CalcChildAdjGeo();
            WatchZones.ForEach(wz => wz.Watches.ForEach(w => w.WatchImages
                .ForEach(i => i.SetMagickImage(wz.AdjustedGeometry))));
        }

        // Full name would be CalculateChildAdjustedGeometry, but that's a mouthful and too many characters.
        /// <summary>
        /// Cache the geometry adjustments needed to position and size images to their spot on the video.
        /// This is done so it's not crunched every single process cycle.
        /// Warning: Could break if negative offsets are used. Need to test.
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
            // Line up the crop by 16px. FFmpeg goes faster that way.
            // Disabled for now. No significant speed improvement noticed.
            //x = Math.Floor(x / 16) * 16;
            //y = Math.Floor(y / 16) * 16;
            width -= x;
            height -= y;

            ThumbnailGeometry = new Geometry(x, y, width, height);

            foreach (var f in Feeds)
            {
                f.ThumbnailGeometry = f.Geometry.RelativeToPoint(ThumbnailGeometry);

                foreach (var s in f.Screens)
                {
                    s.ThumbnailGeometry = f.ThumbnailGeometry;

                    Geometry GameGeometry = s.Geometry;
                    if (f.GameGeometry != null && f.GameGeometry.Width > 0d && f.GameGeometry.Height > 0d)
                    {
                        GameGeometry = f.GameGeometry;
                    }

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

                        newGeo.X = newGeo.X / GameGeometry.Width * f.Geometry.Width + f.ThumbnailGeometry.X;
                        newGeo.Y = newGeo.Y / GameGeometry.Height * f.Geometry.Height + f.ThumbnailGeometry.Y;
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

        public string ToFFmpegArguments()
        {

            return @"-i """ + FilePath + @""" -vf fps=" + FrameRate + @",crop=" + ThumbnailGeometry.ToFFmpegString();
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
        [XmlIgnore]
        public List<Bag> OCRBag = new List<Bag>();

        public string OCRBagCompact
        {
            get {
                string str = "";
                foreach (var b in OCRBag)
                {
                    var lb = new List<byte>();
                    lb.AddRange(BitConverter.GetBytes(b.FrameIndex));
                    lb.AddRange(BitConverter.GetBytes(b.Confidence));
                    // Testing for null termination
                    lb.AddRange(Encoding.ASCII.GetBytes(b.TimeStamp ?? ""));
                    str += Convert.ToBase64String(lb.ToArray());
                }
                return str;
            }
            set
            {

            }
        }

        public Geometry Geometry { get; set; }
        public Geometry GameGeometry { get; set; }
        public Geometry ThumbnailGeometry { get; set; }

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

        [XmlIgnore]
        private string _GameProfile;
        [XmlIgnore]
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

        [XmlIgnore]
        private List<Screen> ScreenClones;
        [XmlIgnore]
        private List<string> _Screens = new List<string>();
        [XmlIgnore]
        public List<Screen> Screens
        {
            get
            {
                if (ScreenClones == null)
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
                        return ss;
                    }
                }
                else
                {
                    return ScreenClones;
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

        public void InitProcess()
        {
            ScreenClones = Screens;
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
        public Geometry ThumbnailGeometry { get; set; }
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
        public List<WatchImage> WatchImages { get; set; } = new List<WatchImage>(); // To expand

        public ColorSpace ColorSpace { get; set; } = ColorSpace.RGB;

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

        // Todo: Add exception handling when file does not exist anymore.

        internal WatchImage() { }

        public string FilePath { get; set; }
        [XmlIgnore]
        public MagickImage MagickImage { get; internal set; }
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

        // Doesn't work as well as I would like yet...
        public double TransparencyRate()
        {
            using (MagickImage i = new MagickImage(FilePath))
            {
                i.Separate(Channels.Alpha);
                return i.ToByteArray().Cast<double>().Average();
            }
        }

        public static double TransparencyRate(Image image)
        {
            using (MagickImage i = new MagickImage((Bitmap)image))
            {
                i.Separate(Channels.Alpha);
                return i.ToByteArray().Cast<double>().Average();
            }
        }

        public static double TransparencyRate(MagickImage image)
        {
            using (MagickImage i = new MagickImage(image))
            {
                var a = i.Separate(Channels.Alpha);
                var b = Array.ConvertAll(i.ToByteArray(), item => (double)item);
                return b.Average();
            }
        }

        public void SetName(Screen screen, WatchZone watchZone, Watcher watcher)
        {
            Name = screen.Name + "/" + watchZone.Name + "/" + watcher.Name + " - " + FileName;
        }

        public void SetMagickImage(Geometry geo)
        {
            var i = new MagickImage((Bitmap)Image);/*
            var geo = new MagickGeometry((int)WZgeo.X, (int)WZgeo.Y, (int)Ggeo.Width, (int)Ggeo.Height);
            i.Extent(geo, Gravity.Northwest, MagickColor.FromRgba(0, 0, 0, 0));
            i.RePage();
            i.Scale((int)Fgeo.Width, (int)Fgeo.Height);
            i.Trim();
            MagickImage = new MagickImage(i.ToBitmap());
            Clear();*/
            var newGeo = new MagickGeometry((int)geo.Width, (int)geo.Height)
            {
                IgnoreAspectRatio = true
            };
            i.Scale(newGeo);
            MagickImage = new MagickImage(i.ToBitmap());
            Clear();
        }

        public void Clear()
        {
            image = null;
        }

        [XmlIgnore]
        public List<Bag> DeltaBag = new List<Bag>();

        public string DeltaBagCompact
        {
            get
            {
                var lb = new List<byte>();
                foreach (var b in DeltaBag)
                {
                    lb.AddRange(BitConverter.GetBytes(b.FrameIndex));
                    lb.AddRange(BitConverter.GetBytes(b.Confidence));
                }
                return Convert.ToBase64String(lb.ToArray());
            }
            set
            {

            }
        }

        override public string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Name))
                return Name;
            else
                return FileName;
        }

    }

    public class Bag
    {
        public Bag(int frameIndex, float delta, string timeStamp = null)
        {
            FrameIndex = frameIndex;
            Confidence = delta;
            TimeStamp = timeStamp;
        }
        internal Bag() { }

        [XmlAttribute("I")]
        public int FrameIndex;
        [XmlAttribute("C")]
        public float Confidence;
        [XmlAttribute("T")]
        public string TimeStamp;
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

            SubItems.Add(Video.IsSynced() ? "✔" : "");

        }

    }
}

namespace VideoImageDeltaApp.Models
{
    using Tesseract;

    public static class MyExtensions
    {
        public static ScanResults GetResults(this Tesseract.ResultIterator iter)
        {
            var llsr = new List<CharResult[]>();
            iter.Begin();


            if (iter.TryGetBoundingBox(PageIteratorLevel.Symbol, out Rect symbolBounds))
            {
                var lsr = new List<CharResult>();
                using (var choice = iter.GetChoiceIterator())
                {
                    do
                    {
                        lsr.Add(new CharResult()
                        {
                            Character = choice.GetText().First(),
                            Confidence = choice.GetConfidence()
                        });
                    } while (choice.Next());
                }
                llsr.Add(lsr.ToArray());
            }

            while (!iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Symbol))
            {
                iter.Next(PageIteratorLevel.Symbol);
                if (iter.TryGetBoundingBox(PageIteratorLevel.Symbol, out symbolBounds))
                {
                    var lsr = new List<CharResult>();
                    using (var choice = iter.GetChoiceIterator())
                    {
                        do
                        {
                            lsr.Add(new CharResult()
                            {
                                Character = choice.GetText().First(),
                                Confidence = choice.GetConfidence()
                            });
                        } while (choice.Next());
                    }
                    llsr.Add(lsr.ToArray());
                }
            };

            return new ScanResults() { Results = llsr.ToArray() };
        }

        public class ScanResults
        {
            public CharResult[][] Results;

            public string BestResult()
            {
                string str = null;
                Results.ToList().ForEach(x => str += x.First().Character);
                return str;
            }

            public string[] AllResults()
            {
                var l = new List<List<char>>();
                for (int a = 0; a < Results.Count(); a++)
                {
                    var l2 = new List<char>();
                    for (int b = 0; b < Results.ElementAt(a).Count(); b++)
                    {
                        l2.Add(Results.ElementAt(a).ElementAt(b).Character);
                    }
                    l.Add(l2);
                }
                return Utilities.Untitled2(l);
            }

        }
        public class CharResult
        {
            public char Character;
            public float Confidence;
        }
    }

}