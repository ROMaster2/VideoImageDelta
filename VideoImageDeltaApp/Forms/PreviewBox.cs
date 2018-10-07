using Ikriv.Xml;
using ImageMagick;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using VideoImageDeltaApp;
using VideoImageDeltaApp.Forms;
using VideoImageDeltaApp.Models;
using Anchor = VideoImageDeltaApp.Models.Anchor;

namespace VideoImageDeltaApp.Forms
{
    public partial class PreviewBox : UserControl
    {
        private Video Video;

        private const double DEFAULT_TIMESTAMP_POSITION = 2d / 3d; // Needs better name.

        private TimeSpan DefaultTimestamp
        {
            get
            {
                if (Video != null && Video.Duration != null)
                {
                    long ticks = (long)Math.Floor(Video.Duration.Ticks * DEFAULT_TIMESTAMP_POSITION);
                    return new TimeSpan(ticks);
                }
                else
                {
                    return TimeSpan.Zero;
                }
            }
        }

        private Bitmap FullThumbnail;
        private Bitmap CroppedThumbnail;
        private Bitmap DeltaThumbnail;
        private Bitmap ScaledThumbnail
        {
            get
            {
                return (Bitmap)ThumbnailBox.Image;
            }
            set
            {
                ThumbnailBox.Image = value;
            }
        }

        /*
        private Geometry ThumbnailBoxGeometry
        {
            get
            {
                var x = ThumbnailBox.Location.X;
                var y = ThumbnailBox.Location.Y;
                var width = ThumbnailBox.Width;
                var height = ThumbnailBox.Height;
                return new Geometry(x, y, width, height);
            }
            set
            {
                Point point = new Point((int)value.X, (int)value.Y);
                ThumbnailBox.Location = point;
                ThumbnailBox.Width = (int)value.Width;
                ThumbnailBox.Height = (int)value.Height;
            }
        }
        */

        private double ScaledRatio
        {
            get
            {
                return Math.Min(
                    Math.Min(
                        1,
                        (this.Width - ThumbnailBox.Margin.Left - ThumbnailBox.Margin.Right) / Video.Geometry.Width
                    ),
                    (this.Height - ThumbnailBox.Margin.Top - ThumbnailBox.Margin.Bottom) / Video.Geometry.Height
                );
            }
        }

        private double ScaledWidth
        {
            get
            {
                return Math.Max(Video.Geometry.Width * ScaledRatio, 1);
            }
        }

        private double ScaledHeight
        {
            get
            {
                return Video.Geometry.Height * ScaledRatio;
            }
        }

        public PreviewBox()
        {
            InitializeComponent();
        }

        private void PreviewBox_Load(object sender, EventArgs e)
        {
            SetVideo(Video.Create(@"I:\Past Broadcasts\holyfuck.mp4"));
            RefreshThumbnail();
        }

        public void SetVideo(Video video)
        {
            Video = video;
        }

        private void RefreshThumbnail()
        {
            var width = (int)Math.Round(ScaledWidth);
            var height = (int)Math.Round(ScaledHeight);
            ThumbnailBox.Size = new Size(width, height);
            ThumbnailBox.Location = new Point(ThumbnailBox.Margin.Left, ThumbnailBox.Margin.Top);

            using (var mi = new MagickImage(Video.GetThumbnail(DefaultTimestamp)))
            {
                var mGeo = new MagickGeometry(width, height) { IgnoreAspectRatio = true };
                mi.FilterType = FilterType.Lanczos;
                //mi.Resize((Percentage)(Math.Pow(2, -0.5) * 100d));
                mi.Resize(mGeo);
                var i = mi.ToBitmap();
                i.SetResolution((float)ScaledWidth, (float)ScaledHeight);
                FullThumbnail = i;
            }
            ThumbnailBox.Image = FullThumbnail;
        }

        public void SetGeometry(Geometry geometry)
        {

        }

        public void SetGeometry(double x, double y, double width, double height)
        {
            SetGeometry(new Geometry(x, y, width, height));
        }

        private void PreviewBox_Click(object sender, EventArgs e)
        {

        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
