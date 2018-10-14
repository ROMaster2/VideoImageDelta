using ImageMagick;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;

namespace VideoImageDeltaApp.Models
{
    public class WatchImage : IGeometry
    {
        internal WatchImage(Watcher watcher, string filePath)
        {
            Parent = watcher;
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
        public ConcurrentBag<Bag> DeltaBag = new ConcurrentBag<Bag>();

        public string DeltaBagCompact
        {
            get
            {
                var lb = new List<byte>();
                foreach (var b in DeltaBag)
                {
                    lb.AddRange(BitConverter.GetBytes(b.FrameIndex));
                    lb.AddRange(Utilities.FloatToBytes(b.Confidence));
                }
                return Convert.ToBase64String(lb.ToArray());
            }
            set
            {
                var lb = Convert.FromBase64String(value);
                /*if (lb.Count() % 6 != 0)
                {
                    throw new Exception("Incorrect number of bytes. Conversion not possible.");
                }*/
                DeltaBag = new ConcurrentBag<Bag>();
                for (int i = 0; i < lb.Count(); i += 6)
                {
                    var frameIndex = BitConverter.ToInt32(lb, i);
                    var confidence = Utilities.BytesToFloat(lb[i + 4], lb[i + 5]);
                    DeltaBag.Add(new Bag(frameIndex, confidence));
                }
            }
        }

        [XmlIgnore]
        public List<Bag> DeltaList
        {
            get
            {
                return DeltaBag.ToList();
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

}
