using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace VideoImageDeltaApp.Models
{
    public class Feed : IGeometry
    {
        public Feed(Video video, string name, bool useOCR, Geometry geometry, Geometry gameGeometry)
        {
            Parent = video;
            Name = name;
            UseOCR = useOCR;
            Geometry = geometry;
            GameGeometry = gameGeometry;
        }

        internal Feed() { }

        [XmlIgnore]
        public new Video Parent;

        [XmlElement("IsTimer")]
        public bool UseOCR { get; set; }

        [XmlIgnore]
        public ConcurrentBag<Bag> OCRBag = new ConcurrentBag<Bag>();

        [XmlIgnore]
        public List<Bag> OCRList
        {
            get
            {
                return OCRBag.ToList();
            }
        }

        public string OCRBagCompact
        {
            get
            {
                string str = "";
                foreach (var b in OCRBag)
                {
                    var lb = new List<byte>();
                    lb.AddRange(BitConverter.GetBytes(b.FrameIndex));
                    lb.AddRange(BitConverter.GetBytes(b.Confidence));
                    // Testing for null termination
                    lb.AddRange(Encoding.ASCII.GetBytes(b.TimeStamp ?? ""));
                    str += Convert.ToBase64String(lb.ToArray()) + "=";
                }
                return str;
            }
            set
            {
                var lb = Convert.FromBase64String(value);
                OCRBag = new ConcurrentBag<Bag>();
                for (int i = 0; i < lb.Count(); i += 8)
                {
                    var frameIndex = BitConverter.ToInt32(lb, i);
                    var confidence = BitConverter.ToSingle(lb, i + 4);
                    OCRBag.Add(new Bag(frameIndex, confidence));
                }
            }
        }

        public Geometry GameGeometry { get; set; }

        public string FullName
        {
            get
            {
                string start = Geometry.Width.ToString() + "x" + Geometry.Height.ToString();
                string middle = null;
                if (Geometry.HasPoint || Geometry.HasAnchor)
                    middle = Utilities.PrefixNumber((decimal)Geometry.X) + Utilities.PrefixNumber((decimal)Geometry.Y);
                return Name + " - " + start + middle;
            }
        }

        [XmlIgnore]
        public GameProfile GameProfile;

        [XmlIgnore]
        public List<Screen> Screens = new List<Screen>();

        override public string ToString()
        {
            return FullName;
        }

    }

}
