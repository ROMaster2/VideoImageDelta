using System.Collections.Generic;

namespace VideoImageDeltaApp.Models
{
    public class Screen : IGeometry
    {
        internal Screen(GameProfile gameProfile, string name, bool useAdvanced, Geometry geometry)
        {
            Parent = gameProfile;
            Name = name;
            UseAdvanced = useAdvanced;
            Geometry = geometry;
        }

        internal Screen() { }

        public bool UseAdvanced { get; set; }
        public List<WatchZone> WatchZones { get; set; } = new List<WatchZone>();

        public WatchZone AddWatchZone(string name, ScaleType scaleType, Geometry geometry)
        {
            var watchZone = new WatchZone(this, name, scaleType, geometry);
            WatchZones.Add(watchZone);
            return watchZone;
        }

        public void ReSyncRelationships()
        {
            if (WatchZones.Count > 0)
            {
                foreach (var wz in WatchZones)
                {
                    wz.Parent = this;
                    wz.ReSyncRelationships();
                }
            }
        }

        override public string ToString()
        {
            return Name;
        }

    }

}
