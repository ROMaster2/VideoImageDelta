/**
 * Very spagehtti. Will clean up later.
 */

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
        public static string ffmpegPath;
        public static string ffprobePath;

        /**
         * Placing the array here stops the memory leaking. Not really a problem since only one thumbnail is shown at a time.
         * But if the user makes multiple windows, things could get weird. A better solution would be preferred.
         * 
         * Will cover up to 1920x1080. More than that will crash or return incomplete/corrupted.
         * The odds of this being used for anything larger is very unlikely. The better solution should fix that.
         */
        public const int MAX_IMAGE_SIZE = 6291456; // 6MiBs flat
        //private static readonly byte[] imageCache = new byte[MAX_IMAGE_SIZE];

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
                return true;
            }
        }

        public static string FFCommand(string process, string videoPath, string parameters, string preparam = "")
        {
            Process pProcess = new Process();
            pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pProcess.StartInfo.FileName = process;
            pProcess.StartInfo.Arguments = String.Format(@"{2} -i ""{0}"" {1}", videoPath, parameters, preparam);
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.RedirectStandardError = true;
            pProcess.StartInfo.CreateNoWindow = true;
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

        public static Bitmap FFCommand2(string process, TimeSpan timestamp, string videoPath, System.Windows.Size size, string parameters)
        {
            Bitmap t = null;
            using (NamedPipeServerStream p_from_ffmpeg = new NamedPipeServerStream(
                    "from_ffmpeg.bmp",
                    PipeDirection.In,
                    1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.None,
                    MAX_IMAGE_SIZE,
                    MAX_IMAGE_SIZE)
                )
            {
                string start = timestamp.ToString().Substring(0, 8);
                string args = String.Format(@"-ss {0} -i ""{1}"" {2}", start, videoPath, parameters);

                using (Process pProcess = new Process())
                {
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

                    // Raw image size + bmp overhead.
                    int imageSize = (int)(size.Width * size.Height) * 3 + 2048;
                    byte[] imageCache = new byte[imageSize];

                    p_from_ffmpeg.Read(imageCache, 0, imageSize);

                    try
                    {
                        using (var ms = new MemoryStream(imageCache))
                        {
                            t = new MagickImage((Bitmap)Image.FromStream(ms)).ToBitmap();
                        }
                    }
                    catch (ArgumentException e)
                    { // Don't stop the program because it failed to load the thumbnail. Just return blank.
                        Debug.Write(e);
                        t = null;
                    }
                }

            }

            return t;
        }

        public static Bitmap GetThumbnail(string videoPath, System.Windows.Size size, TimeSpan timestamp)
        {
            Bitmap i = FFCommand2(@"ffmpeg", timestamp, videoPath, size,
                @"-y -vframes 1 ""\\.\pipe\from_ffmpeg.bmp""");

            return i;
        }

        // Unuased I know what accelerators ffmpeg supports (specifically their command names).
        public enum HardwareAccelerator
        {

        }

        public static string[] GetAvailableHardwareAccelerators()
        {
            string str = FFCommand(@"ffmpeg", null, @"-hwaccels");
            string[] lines = str.Split( new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            lines[0] = "";
            return lines;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoPath"></param>
        /// <returns></returns>
        public static void CheckStability(string videoPath)
        {
            string str = FFCommand(@"ffprobe", videoPath,
                @"-loglevel error -skip_frame nokey -select_streams v:0 -show_entries frame=pkt_pts_time -of csv=print_section=0");

            List<string> list = new List<string>(str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            List<double> a = list.Select(double.Parse).ToList();
            List<float> b = new List<float>();
            for (int d = 1; d < a.Count; d++)
            {
                b.Add((float)Math.Round(a[d] - a[d - 1], 3));
            }

            int count = b.Count();
            float min = b.Min();
            float max = b.Max();
            float mode = b.GroupBy(n => n).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();
            int modeCount = b.Where(n => n == mode).Count();
            double integrity = (double)modeCount / (double)count;
            double avg = b.Average();
            double variance = b.Sum(d => Math.Pow(d - avg, 2));
            double stdev = Math.Sqrt(variance / (b.Count() - 1));

            List<ushort> test = new List<ushort>() { (ushort)Math.Round(a[0] * 1000d) };
            List<byte> test2 = new List<byte>() { (byte)(((ushort)Math.Round(a[0] * 1000d)) & 0xF), (byte)(((ushort)Math.Round(a[0] * 1000d)) >> 4) };
            for (int d = 1; d < a.Count; d++)
            {
                ushort n = (ushort)Math.Round((a[d] - a[d - 1]) * 1000);
                test.Add(n);
                test2.Add((byte)(n & 0xF));
                test2.Add((byte)(n >> 4));
            }

            string ayy = Convert.ToBase64String(test2.ToArray());
        }

        public static ffprobeType GetRawMetadata(string video)
        {
            var str = FFCommand("ffprobe", video, "", "-v quiet -print_format xml -show_format -show_streams");
            ffprobeType result = null;
            XmlSerializer xs = new XmlSerializer(typeof(ffprobeType), new XmlRootAttribute("ffprobe"));
            using (StringReader reader = new StringReader(str))
            {
                result = (ffprobeType)xs.Deserialize(reader);
            }
            return result;

        }

    }
}
