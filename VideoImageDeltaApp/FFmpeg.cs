using ImageMagick;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VideoImageDeltaApp
{
    public class FFmpeg
    {

        public static string probe(string videoPath, string parameters)
        {
            Process pProcess = new Process();
            pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pProcess.StartInfo.FileName = @"ffprobe";
            pProcess.StartInfo.Arguments = String.Format("-i {0} {1}", videoPath, parameters);
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.Start();

            string strOutput = pProcess.StandardOutput.ReadToEnd();

            pProcess.WaitForExit();

            return strOutput;
        }

        public static double getFrameRate(string videoPath)
        {
            string str = probe(videoPath, @"-select_streams 0 -show_entries stream=r_frame_rate -v quiet -of csv=""p = 0""");
            double frameRate = Convert.ToDouble(str.Split('/')[0]) / Convert.ToDouble(str.Split('/')[1]);

            return frameRate;
        }

        public static TimeSpan getDuration(string videoPath)
        {
            string str = probe(videoPath, @"-show_entries format=duration -v quiet -of csv=""p = 0""");
            TimeSpan duration = TimeSpan.FromSeconds(Convert.ToDouble(str));

            return duration;
        }

        public static int[] getResolution(string videoPath)
        {
            string str = probe(videoPath, @"-select_streams 0 -show_entries stream=height,width -v quiet -of csv=s=x:p=0");
            int[] wh = new int[2];
            wh[0] = Convert.ToInt32(str.Split('x')[0]);
            wh[1] = Convert.ToInt32(str.Split('x')[1]);

            return wh;
        }



    }
}
