using ImageMagick;
using System.Collections.Generic;

namespace VideoImageDeltaApp.Models
{
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

}
