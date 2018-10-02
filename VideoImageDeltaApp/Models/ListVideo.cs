using System;
using System.Windows.Forms;

namespace VideoImageDeltaApp.Models
{
    public class ListVideo : ListViewItem
    {
        public ListVideo(Video video) : base()
        {
            Video = video;
            Text = Video.FilePath;
            SubItems.Add((Math.Round(Video.FrameRate * 1000d) / 1000d).ToString());
            SubItems.Add(Video.Geometry.Width.ToString());
            SubItems.Add(Video.Geometry.Height.ToString());
            SubItems.Add(Video.Duration.ToString().Substring(0, 8));
            SubItems.Add("");
            SubItems.Add("");
            SubItems.Add("");
            RefreshValues();
        }

        public Video Video { get; }

        public void RefreshValues()
        {
            // What's a better method? lol
            SubItems.RemoveAt(SubItems.Count - 1);
            SubItems.RemoveAt(SubItems.Count - 1);
            SubItems.RemoveAt(SubItems.Count - 1);

            if (Video.Feeds.Count > 1)
            {
                string s = null;
                foreach (Feed f in Video.Feeds)
                {
                    if (s != null)
                    {
                        s = s + ", " + f.Name;
                    }
                    else
                    {
                        s = f.Name;
                    }
                }
                SubItems.Add(s);
            }
            else if (Video.Feeds.Count == 1)
            {
                SubItems.Add(Video.Feeds[0].ToString());
            }
            else
            {
                SubItems.Add("None Set");
            }

            if (Video.GameProfile != null)
                SubItems.Add(Video.GameProfile.Name);
            else
                SubItems.Add("Not Set");

            SubItems.Add(Video.IsSynced() ? "✔" : "");
        }
    }
}
