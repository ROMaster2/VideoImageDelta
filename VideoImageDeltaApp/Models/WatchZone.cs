using System;
using System.Collections.Generic;

namespace VideoImageDeltaApp.Models
{
    public class WatchZone
    {
        public WatchZone(string name, ScaleType scaleType, GeometryOld geometry)
        {
            Name = name;
            ScaleType = scaleType;
            Geometry = geometry;
        }

        internal WatchZone() { }

        public string Name { get; set; }
        public ScaleType ScaleType { get; set; }
        public GeometryOld Geometry { get; set; }
        public List<Watcher> Watches { get; set; } = new List<Watcher>();

        public GeometryOld AdjustedGeometry { get; set; }

        public GeometryOld WithoutScale(GeometryOld screenGeo, GeometryOld feedGeo, GeometryOld gameGeo)
        {
            GeometryOld GameGeometry = screenGeo;
            if (gameGeo.HasSize())
            {
                GameGeometry = gameGeo;
            }

            double xScale = feedGeo.Width / GameGeometry.Width;
            double yScale = feedGeo.Height / GameGeometry.Height;

            double _x = Geometry.X;
            double _y = Geometry.Y;
            double _width = Geometry.Width;
            double _height = Geometry.Height;

            if (ScaleType == ScaleType.KeepRatio)
            {
                double scale = Math.Min(xScale, yScale); // Min or Max?
                xScale = scale;
                yScale = scale;
            }
            if (ScaleType != ScaleType.NoScale)
            {
                _x *= xScale;
                _y *= yScale;
                _width *= xScale;
                _height *= yScale;
            }

            return new GeometryOld(_x, _y, _width, _height, Geometry.Anchor)/*.WithoutAnchor(GameGeometry)*/;
        }

        override public string ToString()
        {
            return Name;
        }

    }

    public enum ScaleType
    {
        Undefined = -1,
        NoScale = 0,
        KeepRatio = 1,
        Scale = 2,
    }

}
