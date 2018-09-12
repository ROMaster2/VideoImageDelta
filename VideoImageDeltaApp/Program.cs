using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Hudl.FFprobe;
using Hudl.FFprobe.Command;
using Hudl.FFprobe.Metadata;
using Hudl.FFprobe.Metadata.Models;
using ImageMagick;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
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

namespace VideoImageDeltaApp
{
    static class Program
    {
        public static List<Video> Videos = new List<Video>();
        public static List<GameProfile> GameProfiles = new List<GameProfile>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // FOR DEVELOPMENT ONLY. CHANGE BEFORE RELEASE.

            string outputPath = @"D:\";
            string FFmpegPath = @"C:\Program Files\youtube-dl\ffmpeg.exe";
            string FFprobePath = @"C:\Program Files\youtube-dl\ffprobe.exe";

            ResourceManagement.CommandConfiguration = CommandConfiguration.Create(outputPath, FFmpegPath, FFprobePath);
            string filePath = "I:\\Past Broadcasts\\Stream_2017-08-04_14;09;14.mp4";

            //RawFFmpeg.GetThumbnail(filePath);
            /*
            var tmp = Resource.From(filePath);
            tmp.LoadMetadata();
            var tmp2 = (VideoStream)tmp.Streams.First(x => x.ResourceIndicator == "v");
            var tmp3 = tmp2.Info.VideoMetadata;
            TimeSpan duration;

            if (tmp3.Duration == TimeSpan.Zero)
            {
                // God damn it now we have to do this the lowsy way.
                duration = RawFFmpeg.GetDuration(filePath);
            } else
            {
                duration = tmp3.Duration;
            }
            var tmp4 = RawFFmpeg.Hmm(filePath);
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
