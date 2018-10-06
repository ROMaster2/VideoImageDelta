using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace VideoImageDeltaApp.Models
{
    public class Video : IGeometry
    {
        public static Video Create(string filePath)
        {
            if (File.Exists(filePath))
            {
                ffprobeType metaData = RawFFmpeg.GetRawMetadata(filePath);
                return new Video(filePath, metaData);
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
        public TimeSpan Duration { get; }
        private streamType RawVideoMetadata { get { return RawMetadata.streams.Where(x => x.codec_type == "video").First(); } }
        public double FrameRate { get { return (double)Utilities.DivideString(RawVideoMetadata.r_frame_rate); } }
        public int FrameCount { get { return (int)(FrameRate * Duration.TotalSeconds); } }

        public double MaxScanRate { get { return Math.Round(Watches.Max(w => w.Frequency) * 300d) / 300d; } }

        [XmlIgnore]
        public GameProfile GameProfile;

        public List<Feed> Feeds = new List<Feed>();

        [XmlIgnore]
        public List<Screen> Screens
        { get { var a = new List<Screen>();     a.AddRange(Feeds.SelectMany(f =>        f.Screens)); return a; } }
        [XmlIgnore]
        public List<WatchZone> WatchZones
        { get { var a = new List<WatchZone>();  a.AddRange(Screens.SelectMany(s =>   s.WatchZones)); return a; } }
        [XmlIgnore]
        public List<Watcher> Watches
        { get { var a = new List<Watcher>();    a.AddRange(WatchZones.SelectMany(wz => wz.Watches)); return a; } }
        [XmlIgnore]
        public List<WatchImage> WatchImages
        { get { var a = new List<WatchImage>(); a.AddRange(Watches.SelectMany(w =>  w.WatchImages)); return a; } }

        public bool IsSynced()
        {
            return Feeds.Count > 0 && Feeds.All(x => x.GameProfile != null) && Feeds.All(x => x.Screens.Count > 0 || x.UseOCR);
        }

        public Image GetThumbnail(TimeSpan timestamp)
        {
            return RawFFmpeg.GetThumbnail(FilePath, new System.Windows.Size(Geometry.Width, Geometry.Height), timestamp);
        }

        public void InitProcess()
        {
            CalcChildAdjGeo();
            WatchZones.ForEach(wz => wz.Watches.ForEach(w => w.WatchImages
                .ForEach(i => i.SetMagickImage(wz.ThumbnailGeometry))));
        }

        public Feed AddFeed(string name, bool useOCR, Geometry geometry, Geometry gameGeometry)
        {
            var feed = new Feed(this, name, useOCR, geometry, gameGeometry);
            Feeds.Add(feed);
            return feed;
        }

        public void ReSyncRelationships()
        {
            if (Feeds.Count > 0)
            {
                foreach (var f in Feeds)
                {
                    f.Parent = this;
                }
            }
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
                f.ThumbnailGeometry = f.Geometry.Clone();
                f.ThumbnailGeometry.X -= x;
                f.ThumbnailGeometry.Y -= y;

                foreach (var s in f.Screens)
                {
                    s.ThumbnailGeometry = f.ThumbnailGeometry;

                    Geometry GameGeometry = s.Geometry;
                    if (f.GameGeometry.HasSize)
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

                        var newGeo = new Geometry(_x, _y, _width, _height, wz.Geometry.Anchor);
                        newGeo.RemoveAnchor(GameGeometry);

                        newGeo.X = newGeo.X / GameGeometry.Width * f.Geometry.Width + f.ThumbnailGeometry.X;
                        newGeo.Y = newGeo.Y / GameGeometry.Height * f.Geometry.Height + f.ThumbnailGeometry.Y;
                        newGeo.Width = newGeo.Width / GameGeometry.Width * f.Geometry.Width;
                        newGeo.Height = newGeo.Height / GameGeometry.Height * f.Geometry.Height;

                        wz.ThumbnailGeometry = newGeo;
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

}
