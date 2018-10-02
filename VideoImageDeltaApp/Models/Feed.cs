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
    public class Feed
    {
        public Feed(string name, bool useOCR, GeometryOld geometry, GeometryOld gameGeometry)
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
                string str = null;
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

        public GeometryOld Geometry { get; set; }
        public GeometryOld GameGeometry { get; set; }
        public GeometryOld ThumbnailGeometry { get; set; }

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

}
