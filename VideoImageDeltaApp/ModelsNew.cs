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
//using System.Drawing;
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
//using VideoImageDeltaApp.Models;

//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Shapes;
using Microsoft.VisualBasic.Devices;
using System.Windows;

using Rect = VideoImageDeltaApp.Rect;
using Size = System.Windows.Size;
using MessageBox = System.Windows.Forms.MessageBox;


namespace VideoImageDeltaApp.ModelsNew
{

    class Processor
    {
        protected const uint  MINIMUM_THREADS_REQUIRED =         1;
        protected const ulong MINIMUM_MEMORY_REQUIRED  = 134217728; // 128MBs
        protected const ulong MINIMUM_SPACE_REQUIRED   = 134217728; // 128MBs

        // Will likely change in the future.
        private uint  maxThreads = 0;
        public  uint  MaxThreads {
            get
            {
                if (maxThreads <= 0)
                {
                    maxThreads = (uint)Environment.ProcessorCount;
                }
                return maxThreads;
            }
            set
            {
                maxThreads = Math.Max(MINIMUM_THREADS_REQUIRED, value);
            }
        }

        private ulong maxMemory = 0;
        public  ulong MaxMemory
        {
            get
            {
                if (maxMemory == 0)
                {
                    maxMemory = new ComputerInfo().TotalVirtualMemory;
                }
                return maxMemory;
            }
            set
            {
                maxMemory = Math.Max(MINIMUM_MEMORY_REQUIRED, value);
            }
        }

        private ulong maxStorage = 0;
        public  ulong MaxStorage
        {
            get
            {
                if (maxStorage == 0)
                {
                    maxStorage = (ulong)new DriveInfo(Path.GetPathRoot(TempPath)).AvailableFreeSpace;
                }
                return maxStorage;
            }
            set
            {
                maxStorage = Math.Max(MINIMUM_SPACE_REQUIRED, value);
            }
        }

        public string TempPath = Path.GetTempPath();
        public string OutputPath;
        public bool isSynchronized = false;

        public Video[] Videos;
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

                Size = new Size(RawMetadata.Width, RawMetadata.Height);
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
        public Size Size { get; }
        public Rect AdjustedGeometry { get; set; }
        public double FrameRate
        {
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

        public System.Drawing.Image GetThumbnail(TimeSpan timestamp)
        {
            // Would use Hudl but figuring out the syntax is a pain. Documentation kinda sucks.
            return RawFFmpeg.GetThumbnail(FilePath, Size, timestamp);
        }

        override public string ToString()
        {
            return FilePath;
        }


        public class Feed
        {
            public Feed(string name, bool useOCR, bool accurateCapture, Rect geometry, Rect gameGeometry)
            {
                Name = name;
                UseOCR = useOCR;
                AccurateCapture = accurateCapture;
                Geometry = geometry;
                GameGeometry = gameGeometry;
            }

            internal Feed() { }

            public int ID
            {
                get
                {
                    return ((Video)base.MemberwiseClone()).Feeds.IndexOf(this);
                }
            }
            public string Name { get; set; }
            [XmlElement("IsTimer")]
            public bool UseOCR { get; set; }
            [XmlElement("PerfectCapture")]
            public bool AccurateCapture { get; set; }
            public Rect Geometry { get; set; }
            public Rect GameGeometry { get; set; }

            public Screen Screen { get; set; } // Experimenting

            override public string ToString()
            { // Figure out how to get + and - to appear based on value.
                return Geometry.Width.ToString() + "x" + Geometry.Height.ToString() + " " + Name;
            }

        }


    }



}
