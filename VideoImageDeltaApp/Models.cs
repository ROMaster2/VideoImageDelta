using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VideoImageDeltaApp.Models
{
    public class Video
    {
        public Video(string filePath)
        {
            FilePath = filePath;
            // Add error handling for when the file doesn't exist, isn't valid, or ffmpeg throws an exception.
            Duration = FFmpeg.getDuration(filePath);
            FrameRate = FFmpeg.getFrameRate(filePath);
            int[] wh = FFmpeg.getResolution(filePath);
            Geometry = new MagickGeometry(wh[0], wh[1]);
        }

        public string FilePath { get; }
        public TimeSpan Duration { get; }
        public double FrameRate { get; }
        public MagickGeometry Geometry { get; }

        public int FrameCount
        {
            get
            {
                return Convert.ToInt32(FrameRate * Duration.TotalSeconds);
            }
        }

        public Feed[] Feeds { get; set; } // Change to list

        override public string ToString()
        {
            var str = FilePath.Split('\\');
            return str[str.Count() - 1];
        }

    }

    public class Feed
    {
        // Require Width and Height to be greater than 0.
        public Feed(string name, MagickGeometry geometry, Gravity anchor)
        {
            Name = name;
            Geometry = geometry;
            Anchor = anchor;
        }

        public string Name { get; set; }
        public MagickGeometry Geometry { get; set; }
        public Gravity Anchor { get; set; }

        override public string ToString()
        {
            return Name;
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

        public VideoProfile(string name, MagickGeometry geometry, params Feed[] feeds)
        {
            Name = name;
            Geometry = geometry;
            Feeds = feeds;
        }

        public string Name { get; set; }
        public MagickGeometry Geometry { get; }
        public Feed[] Feeds { get; set; } // Change to list

        override public string ToString()
        {
            return Name;
        }

    }



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
        public Watcher(WatchZone watchZone, int frequency = 60, ScanType rescanType = ScanType.Both, TimeSpan? rescanRange = null)
        {
            WatchZone = watchZone;
            Frequency = frequency;
            RescanType = rescanType;
            RescanRange = rescanRange ?? TimeSpan.FromSeconds(1);
        }
        public Watcher(WatchZone watchZone, ScanType rescanType = ScanType.Both, TimeSpan? rescanRange = null)
        {
            WatchZone = watchZone;
            Frequency = 60;
            RescanType = rescanType;
            RescanRange = rescanRange ?? TimeSpan.FromSeconds(1);
        }
        public Watcher(WatchZone watchZone, int frequency = 60, TimeSpan? rescanRange = null)
        {
            WatchZone = watchZone;
            Frequency = frequency;
            RescanType = ScanType.Both;
            RescanRange = rescanRange ?? TimeSpan.FromSeconds(1);
        }
        public Watcher(WatchZone watchZone, TimeSpan? rescanRange = null)
        {
            WatchZone = watchZone;
            Frequency = 60;
            RescanType = ScanType.Both;
            RescanRange = rescanRange ?? TimeSpan.FromSeconds(1);
        }
        public Watcher(WatchZone watchZone, int frequency = 60, ScanType rescanType = ScanType.Both)
        {
            WatchZone = watchZone;
            Frequency = frequency;
            RescanType = rescanType;
            RescanRange = TimeSpan.FromSeconds(1);
        }
        public Watcher(WatchZone watchZone, ScanType rescanType = ScanType.Both)
        {
            WatchZone = watchZone;
            Frequency = 60;
            RescanType = rescanType;
            RescanRange = TimeSpan.FromSeconds(1);
        }
        public Watcher(WatchZone watchZone, int frequency = 60)
        {
            WatchZone = watchZone;
            Frequency = frequency;
            RescanType = ScanType.Both;
            RescanRange = TimeSpan.FromSeconds(1);
        }
        public Watcher(WatchZone watchZone)
        {
            WatchZone = watchZone;
            Frequency = 60;
            RescanType = ScanType.Both;
            RescanRange = TimeSpan.FromSeconds(1);
        }

        public string Name { get; set; }
        public WatchZone WatchZone { get; set; }
        public int Frequency { get; set; }
        public ScanType RescanType { get; set; }
        public TimeSpan RescanRange { get; set; }
        public MagickImageCollection Images { get; set; } // Still don't know which class this should go into.

    }

    public class WatchZone
    {
        // Require Width and Height to be greater than 0.
        public WatchZone(string name, Screen screen, MagickGeometry geometry, Gravity anchor)
        {
            Name = name;
            Screen = screen;
            Geometry = geometry;
            Anchor = anchor;
        }

        public string Name { get; set; }
        public Screen Screen { get; set; }
        public Gravity Anchor { get; set; }
        public MagickGeometry Geometry { get; set; }

        override public string ToString()
        {
            return Name;
        }

    }

    public class Screen
    {
        // Require Width and Height to be greater than 0.
        public Screen(string name, MagickGeometry geometry)
        {
            Name = name;
            Geometry = geometry;
        }

        public string Name { get; set; }
        public MagickGeometry Geometry { get; set; }

        override public string ToString()
        {
            return Name;
        }

    }

    public class GameProfile
    {
        public GameProfile() { }

        public GameProfile(string name)
        {
            Name = name;
            Watches = new List<Watcher>();
            WatchZones = new List<WatchZone>();
            Screens = new List<Screen>();
        }

        public string Name { get; set; }
        public List<Watcher> Watches { get; set; }
        public List<WatchZone> WatchZones { get; set; }
        public List<Screen> Screens { get; set; }

        override public string ToString()
        {
            return Name;
        }

    }

}
