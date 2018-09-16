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
    class Test
    {
        static public void RunTest(List<Video> videos, List<GameProfile> gameProfiles)
        {
            // Single video and profile for testing
            var v = videos[0];
            var gp = gameProfiles[0];

            string inputVideoPath = v.FilePath;
            string deltaImagePath = gp.Screens[0].WatchZones[0].Watches[0].Images[0].FilePath;
            string dumpFolder = @"J:\Dump";
            int width = (int)gp.Screens[0].WatchZones[0].Geometry.Width;
            int height = (int)gp.Screens[0].WatchZones[0].Geometry.Height;
            int x = (int)(gp.Screens[0].WatchZones[0].Geometry.X + v.Feeds[0].Geometry.X);
            int y = (int)(gp.Screens[0].WatchZones[0].Geometry.Y + v.Feeds[0].Geometry.Y);

            double frameRate = v.FrameRate;
            TimeSpan duration = v.Duration;
            int frameCount = v.FrameCount;

            MagickImage deltaImage = new MagickImage(deltaImagePath);

            if (deltaImage.BaseWidth != width || deltaImage.BaseHeight != height)
                deltaImage.Resize(width, height);

            string cropString = width.ToString() + ':' + height.ToString() + ':' + x.ToString() + ':' + y.ToString();

            Process ffmpegProcess = new Process();
            ProcessStartInfo ffmpegProcessInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = @"C:\Program Files\youtube-dl\ffmpeg.exe",
                Arguments = @"-i """ + inputVideoPath + @""" -vf fps=" + frameRate.ToString() + @",crop=" + cropString + @" """ + dumpFolder + @"\%08d.bmp"" -hide_banner"
            };
            ffmpegProcess.StartInfo = ffmpegProcessInfo;
            ffmpegProcess.Start();
            var hitFrames = ScanThumbnails(ffmpegProcess, dumpFolder, deltaImage);
            ffmpegProcess.WaitForExit();

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }

        public static string GetCommandOutput(string process, string parameters)
        {
            Process pProcess = new Process();
            pProcess.StartInfo.FileName = process;
            pProcess.StartInfo.Arguments = parameters;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.Start();

            string strOutput = pProcess.StandardOutput.ReadToEnd();

            pProcess.WaitForExit();

            return strOutput;
        }

        public static ConcurrentBag<int> ScanThumbnails(Process ffmpegProcess, string dumpPath, MagickImage deltaImage)
        {
            DirectoryInfo d = new DirectoryInfo(dumpPath);
            FileInfo[] thumbs = d.GetFiles("*.bmp");
            ConcurrentBag<int> hitFramesTemp2 = new ConcurrentBag<int>();
            //Async can cause...problems
            while (!ffmpegProcess.HasExited || thumbs.Count() > 0)
            {
                thumbs = d.GetFiles("*.bmp");
                ConcurrentBag<int> hitFramesTemp = new ConcurrentBag<int>();
                if (thumbs.Count() > 1)
                {
                    Parallel.ForEach(thumbs, (i) =>
                    {
                        string thumbPath = i.ToString();
                        string fullThumbPath = dumpPath + @"\" + thumbPath;
                        int thumbID = Convert.ToInt32(i.Name.Substring(0, 8));
                        if (File.Exists(fullThumbPath))
                        {
                            MagickImage diffImage = new MagickImage(fullThumbPath);
                            //diffImage.Composite(deltaImage, CompositeOperator.CopyAlpha);
                            diffImage.Composite(deltaImage, CompositeOperator.Difference);
                            Double.TryParse(diffImage.FormatExpression("%[fx:mean]"), out double delta);
                            if (delta < 0.25)
                            {
                                File.Copy(fullThumbPath, @"J:\Dump2" + @"\" + i.Name, true);
                                //hitFramesTemp.Add(thumbID);
                            }
                            i.Delete();
                        }
                    });
                    foreach (int n in hitFramesTemp)
                    {
                        hitFramesTemp2.Add(n);
                    }
                }
            };
            return hitFramesTemp2;
        }
    }
}
