using System.Collections.Generic;

namespace VideoImageDeltaApp.Models
{
    public struct VideoProfile
    {
        public VideoProfile(Video video)
        {
            Geometry = video.Geometry;
            Feeds = video.Feeds;
        }

        public VideoProfile(GeometryOld geometry, List<Feed> feeds)
        {
            Geometry = geometry;
            Feeds = feeds;
        }

        public GeometryOld Geometry;
        public List<Feed> Feeds;
    }
}
