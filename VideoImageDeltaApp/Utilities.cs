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

using Tesseract;
using Tesseract.Interop;
using System.Runtime.InteropServices;

namespace VideoImageDeltaApp
{
    public static class Utilities
    {
        public static void Error(int id)
        {
            DialogResult dr = MessageBox.Show(
                "This shouldn't happen. Error code: " +
                    id.ToString() +
                    "\r\n" +
                    "If you could, report this to [Link] with details on how you got this, it'd really help.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.None
                );
        }

        public static void Flicker(Control form, int milliseconds, Color color)
        {
            var origColor = SystemColors.Window;
            form.BackColor = color;
            var t = new System.Windows.Forms.Timer()
            {
                Interval = milliseconds
            };
            t.Tick += delegate (object a, EventArgs b) {
                form.BackColor = origColor;
            };
            t.Start();
        }

        public static string PrefixNumber(decimal number, int precision = 2, string specifier = "G")
        {
            number = Math.Round(number, precision);
            var str = number.ToString(specifier, CultureInfo.CurrentCulture);
            if (number >= 0)
                str = "+" + str;
            else
                str = "-" + str;
            return str;
        }

        public static bool GetDiskSpace(string directory, out long totalSpace, out long freeSpace)
        {
            var directoryRoot = Directory.GetDirectoryRoot(directory);
            bool isAvailable = false;
            totalSpace = -1L;
            freeSpace = -1L;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if(drive.Name == directoryRoot)
                {
                    totalSpace = drive.TotalSize;

                    if (drive.IsReady)
                    {
                        freeSpace = drive.AvailableFreeSpace;
                        isAvailable = true;
                    }
                    break;
                }
            }
            return isAvailable;
        }

        /// There's no need to worry about specific CPUs.
        /// <summary>
        /// Set the number of CPUs the program is allowed to use.
        /// </summary>
        /// <param name="processorLimit"></param>
        public static void SetCPULimit(int processorLimit)
        {
            if (processorLimit < 0)
                processorLimit = Environment.ProcessorCount + processorLimit;

            if (processorLimit <= 0 || processorLimit > Environment.ProcessorCount)
                processorLimit = Environment.ProcessorCount;

            processorLimit = (int)Math.Pow(processorLimit, 2);

            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)processorLimit;
        }

        /// <summary>
        /// Set the priority of the program.
        /// </summary>
        /// <param name="processorLimit"></param>
        public static void SetPriority(ProcessPriorityClass priority)
        {
            Process.GetCurrentProcess().PriorityClass = priority;
        }

        public static double CalculateStdDev(IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //Compute the Average      
                double avg = values.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }

        public static decimal DivideString(string str)
        {
            var s = str.Split('/');
            decimal i = decimal.Parse(s[0]);
            for (var n = 1; n < s.Count(); n++)
            {
                i /= decimal.Parse(s[n]);
            }
            return i;
        }

        public static void ReadImage(Image image, Geometry geo, out string text, out float confidence)
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", @"./tessdata/digits"))
                {
                    using (var page = engine.Process((Bitmap)image, geo.ToTesseract()))
                    {
                        text = page.GetText().Split(new[] { '\r', '\n' }).FirstOrDefault().Trim();
                        confidence = page.GetIterator().GetConfidence(PageIteratorLevel.TextLine);
                        if (!Utilities.ValidateTimeOCR(text, true))
                        {
                            confidence = 0.01f;
                            var r = page.GetIterator().GetResults().AllResults().OrderByDescending(x => x.Length).ToList();
                            foreach (var s in r)
                            {
                                if (Utilities.ValidateTimeOCR(s.Replace(" ",""), false))
                                {
                                    text = s.Replace(" ", "");
                                    break;
                                }
                            }
                        }
                        if (!Utilities.ValidateTimeOCR(text))
                        {
                            confidence = 0;
                            text = "";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                text = "Error";
                confidence = 0f;
            }
        }

        public static bool ValidateTimeOCR(string text, bool tryHard = false)
        {
            // Maybe try seeing if replacing the space(s) with : or . possibly validates things.
            var a = text.Replace(" ", "");
            if (tryHard)
            {
                var b = text.Replace(" ", ":");
                var c = text.Replace(" ", ".");
                return Regex.IsMatch(a, @"^-?(?:(?:(\d?\d):)?([0-5]\d):)?([0-5]\d)(\.\d?(\d))?$") ||
                    Regex.IsMatch(b, @"^-?(?:(?:(\d?\d):)?([0-5]\d):)?([0-5]\d)(\.\d?(\d))?$") ||
                    Regex.IsMatch(c, @"^-?(?:(?:(\d?\d):)?([0-5]\d):)?([0-5]\d)(\.\d?(\d))?$");
            }
            else
            {
                return Regex.IsMatch(a, @"^-?(?:(?:(\d?\d):)?([0-5]\d):)?([0-5]\d)(\.\d?(\d))?$");
            }
       }

        public static ConcurrentBag<string> Untitled1(int a, List<List<char?>> x)
        {
            ConcurrentBag<string> retval = new ConcurrentBag<string>();
            if (a == x.Count || a > 11)
            {
                retval.Add("");
                return retval;
            }
            Parallel.ForEach<char?>(x[a], (y) =>
            {
                Parallel.ForEach<string>(Untitled1(a + 1, x), (x2) =>
                {
                    retval.Add(y.ToString() + x2.ToString());
                });
            });
            return retval;
        }
        public static string[] Untitled2(List<List<char?>> myList)
        {
            var l = new List<string>();
            foreach (string x in Untitled1(0, myList))
            {
                l.Add(x);
            }
            return l.ToArray();
        }


    }

    [Flags]
    public enum ThreadAccess : int
    {
        TERMINATE = (0x0001),
        SUSPEND_RESUME = (0x0002),
        GET_CONTEXT = (0x0008),
        SET_CONTEXT = (0x0010),
        SET_INFORMATION = (0x0020),
        QUERY_INFORMATION = (0x0040),
        SET_THREAD_TOKEN = (0x0080),
        IMPERSONATE = (0x0100),
        DIRECT_IMPERSONATION = (0x0200)
    }


    public static class ProcessExtension
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);

        public static void Suspend(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }
                SuspendThread(pOpenThread);
            }
        }
        public static void Resume(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }
                ResumeThread(pOpenThread);
            }
        }
    }
}
