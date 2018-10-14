using ImageMagick;
using System.Collections.Generic;

namespace VideoImageDeltaApp.Models
{
    public class Watcher : IGeometry
    {
        internal Watcher(WatchZone watchZone, string name, double frequency = 1d, ColorSpace colorSpace = ColorSpace.RGB)
        {
            Parent = watchZone;
            Name = name;
            ColorSpace = colorSpace;
            Frequency = frequency;
        }

        internal Watcher() { }

        public double Frequency { get; set; }
        public ColorSpace ColorSpace { get; set; } // To expand
        public List<WatchImage> WatchImages { get; set; } = new List<WatchImage>();

        public WatchImage AddWatchImage(string filePath)
        {
            var watchImage = new WatchImage(this, filePath);
            WatchImages.Add(watchImage);
            return watchImage;
        }

        public void ReSyncRelationships()
        {
            if (WatchImages.Count > 0)
            {
                foreach (var wi in WatchImages)
                {
                    wi.Parent = this;
                }
            }
        }

        override public string ToString()
        {
            return Name;
        }

    }

}
