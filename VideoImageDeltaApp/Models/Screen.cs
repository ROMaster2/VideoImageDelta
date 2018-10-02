using System.Collections.Generic;

namespace VideoImageDeltaApp.Models
{
    public class Screen
    {
        public Screen(string name, Geometry geometry)
        {
            Name = name;
            UseAdvanced = false;
            Geometry = geometry;
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

}
