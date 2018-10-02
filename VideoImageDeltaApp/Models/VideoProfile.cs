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

        public VideoProfile(Geometry geometry, List<Feed> feeds)
        {
            Geometry = geometry;
            Feeds = feeds;
        }

        public Geometry Geometry;
        public List<Feed> Feeds;
    }
}
