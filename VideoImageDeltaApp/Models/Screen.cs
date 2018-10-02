using System.Collections.Generic;

namespace VideoImageDeltaApp.Models
{
    public class Screen
    {
        public Screen(string name, GeometryOld geometry)
        {
            Name = name;
            UseAdvanced = false;
            Geometry = geometry;
        }

        public Screen(string name, bool useAdvanced, GeometryOld geometry)
        {
            Name = name;
            UseAdvanced = useAdvanced;
            Geometry = geometry;
        }

        internal Screen() { }

        public string Name { get; set; }
        public bool UseAdvanced { get; set; }
        public GeometryOld Geometry { get; set; }
        public GeometryOld ThumbnailGeometry { get; set; }
        public List<WatchZone> WatchZones { get; set; } = new List<WatchZone>();

        override public string ToString()
        {
            return Name;
        }

    }

}
