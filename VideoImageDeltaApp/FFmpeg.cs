/**
 * Very spagehtti. Will clean up later.
 */

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

using System.Reflection;
using System.Data.OleDb;

namespace VideoImageDeltaApp
{

    public class RawFFmpeg
    {
        private static string ffmpegPath;
        private static string ffprobePath;

        /**
         * Placing the array here stops the memory leaking. Not really a problem since only one thumbnail is shown at a time.
         * But if the user makes multiple windows, things could get weird. A better solution would be preferred.
         * 
         * Will cover up to 1920x1080. More than that will crash or return incomplete/corrupted.
         * The odds of this being used for anything larger is very unlikely. The better solution should fix that.
         */
        public const int MAX_IMAGE_SIZE = 6291456; // 6MiBs flat
        private static readonly byte[] imageCache = new byte[MAX_IMAGE_SIZE];

        public static bool FindFFExecutables()
        {
            retry:
            // Check if executables exist in the same directory as the program.
            string srcDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (File.Exists(srcDir + @"\" + "ffmpeg.exe"))
                ffmpegPath = srcDir + @"\" + "ffmpeg.exe";
            if (File.Exists(srcDir + @"\" + "ffprobe.exe"))
                ffprobePath = srcDir + @"\" + "ffprobe.exe";

            // Check if executables exist in system paths.
            string[] dirs = Environment.GetEnvironmentVariable("PATH").Split(';');
            if (String.IsNullOrWhiteSpace(ffmpegPath))
            {
                foreach (string dir in dirs)
                {
                    if (File.Exists(dir + @"\" + "ffmpeg.exe"))
                    {
                        ffmpegPath = dir + @"\" + "ffmpeg.exe";
                        break;
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(ffprobePath))
            {
                foreach (string dir in dirs)
                {
                    if (File.Exists(dir + @"\" + "ffprobe.exe"))
                    {
                        ffprobePath = dir + @"\" + "ffprobe.exe";
                        break;
                    }
                }
            }

            // Windows search. Can't test on my PC since I have it disabled.
            /*
            try
            {
                var connection = new OleDbConnection(@"Provider=Search.CollatorDSO;Extended Properties=""Application=Windows""");
                var query = @"SELECT System.ItemName FROM SystemIndex WHERE scope ='file:C:/' AND System.ItemName = 'ffmpeg.exe'";
                connection.Open();
                var command = new OleDbCommand(query, connection);
                using (var r = command.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Console.WriteLine(r[0]);
                    }
                }
                connection.Close();
            }
            catch (InvalidOperationException e) { Debug.Write(e); }
            */

            if (!File.Exists(ffmpegPath) || !File.Exists(ffprobePath))
            {
                string str = "FFmpeg and FFprobe are ";
                if (File.Exists(ffmpegPath))
                {
                    str = "FFprobe is ";
                } else if (File.Exists(ffprobePath))
                {
                    str = "FFmpeg is ";
                }
                DialogResult dr = MessageBox.Show(
                    str + "required for this program to run properly and cannot be found." +
                    " The latest build for FFmpeg can be downloaded at" +
                    "\r\nhttps://ffmpeg.zeranoe.com/builds/" + 
                    "\r\nThis program will now close.",
                    "Fatal Error",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Error
                );
                if (dr == DialogResult.Retry)
                {
                    goto retry;
                } else
                {
                    return false;
                }
            } else
            {
                ResourceManagement.CommandConfiguration = CommandConfiguration.Create(Path.GetTempPath(), ffmpegPath, ffprobePath);
                return true;
            }
        }

        public static string FFCommand(string process, string videoPath, string parameters)
        {
            Process pProcess = new Process();
            pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pProcess.StartInfo.FileName = process;
            pProcess.StartInfo.Arguments = String.Format(@"-i ""{0}"" {1}", videoPath, parameters);
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.Start();

            string strOutput = pProcess.StandardOutput.ReadToEnd();

            pProcess.WaitForExit();

            return strOutput;
        }

        public static double GetFrameRate(string videoPath)
        {
            string str = FFCommand(@"ffprobe", videoPath, @"-select_streams 0 -show_entries stream=r_frame_rate -v quiet -of csv=""p = 0""");
            double frameRate = Convert.ToDouble(str.Split('/')[0]) / Convert.ToDouble(str.Split('/')[1]);

            return frameRate;
        }

        public static TimeSpan GetDuration(string videoPath)
        {
            string str = FFCommand(@"ffprobe", videoPath, @"-show_entries format=duration -v quiet -of csv=""p = 0""");
            TimeSpan duration = TimeSpan.FromSeconds(Convert.ToDouble(str));

            return duration;
        }

        public static int[] GetResolution(string videoPath)
        {
            string str = FFCommand(@"ffprobe", videoPath, @"-select_streams 0 -show_entries stream=height,width -v quiet -of csv=s=x:p=0");
            int[] wh = new int[2];
            wh[0] = Convert.ToInt32(str.Split('x')[0]);
            wh[1] = Convert.ToInt32(str.Split('x')[1]);

            return wh;
        }

        /**
         * Extra Spaghetti.
         */

        public static Image FFCommand2(string process, TimeSpan timestamp, string videoPath, System.Windows.Size size, string parameters)
        {
            Array.Clear(imageCache, 0, MAX_IMAGE_SIZE);
            NamedPipeServerStream p_from_ffmpeg;
            p_from_ffmpeg = new NamedPipeServerStream(
                "from_ffmpeg",
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.WriteThrough,
                MAX_IMAGE_SIZE,
                MAX_IMAGE_SIZE);

            string start = timestamp.ToString().Substring(0, 8);
            string args = String.Format(@"-ss {0} -i ""{1}"" {2}", start, videoPath, parameters);

            Process pProcess = new Process();
            pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pProcess.StartInfo.FileName = process;
            pProcess.StartInfo.Arguments = args;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.RedirectStandardError = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();

            p_from_ffmpeg.WaitForConnection();

            pProcess.WaitForExit();

            int imageSize = (int)(size.Width * size.Height) * 3;
            p_from_ffmpeg.Read(imageCache, 0, imageSize + 128); // 128 is added for bmp overhead
            object t;

            try
            {
                t = Image.FromStream(new MemoryStream(imageCache));
            }
            catch (ArgumentException e)
            { // Don't stop the program because it failed to load the thumbnail. Just return blank.
                Debug.Write(e);
                t = null;
            }

            pProcess.Dispose();
            p_from_ffmpeg.Dispose();

            return (Image)t;
        }

        public static Image GetThumbnail(string videoPath, System.Windows.Size size, TimeSpan timestamp)
        {
            Image i = FFCommand2(@"ffmpeg", timestamp, videoPath, size,
                @"-y -f image2pipe -vframes 1 ""\\.\pipe\from_ffmpeg""");

            return i;
        }

        // Forcibly calculate the Framerate. Will most likely not keep but it's still here so I don't forget how it's done.
        public static string Hmm(string videoPath)
        {
            string str = FFCommand(@"ffprobe", videoPath,
                @"-loglevel error -skip_frame nokey -select_streams v:0 -show_entries frame=pkt_pts_time -of csv=print_section=0");

            List<string> list = new List<string>(str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            List<double> a = list.Select(double.Parse).ToList();
            List<double> b = new List<double>();
            for (int d = 1; d < a.Count; d++)
            {
                b.Add(a[d] - a[d - 1]);
            }

            double min = b.Min();
            double max = b.Max();
            double avg = b.Average();
            double sum2 = b.Sum(d => Math.Pow(d - avg, 2));
            double stdev = Math.Sqrt((sum2) / (b.Count() - 1));

            return "";
        }

    }
}
