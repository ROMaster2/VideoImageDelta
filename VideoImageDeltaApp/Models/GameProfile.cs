using System.Collections.Generic;

namespace VideoImageDeltaApp.Models
{
    public class GameProfile : IGeometry
    {
        public GameProfile(string name)
        {
            Name = name;
        }

        internal GameProfile() { }

        public List<Screen> Screens { get; set; } = new List<Screen>();

        public Screen AddScreen(string name, bool useAdvanced, Geometry geometry)
        {
            var screen = new Screen(this, name, useAdvanced, geometry);
            Screens.Add(screen);
            return screen;
        }

        public void ReSyncRelationships()
        {
            if (Screens.Count > 0) {
                foreach (var s in Screens)
                {
                    s.Parent = this;
                    s.ReSyncRelationships();
                }
            }
        }

        override public string ToString()
        {
            return Name;
        }

    }

}
