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

using Microsoft.VisualBasic.Devices;
using System.Reflection;
using System.Data.OleDb;

namespace VideoImageDeltaApp
{
    public class Processor
    {
        public const long MIN_REQUIRED_SPACE = 536870912L; // 512 Mebibytes

        public Processor() { }

        public bool DebuggingEnabled = false;
        public bool HardwareAccelForFFmpeg = false;
        public bool HardwareAccelForMagick = false;
        public bool UseHDD = true;
        public bool AutoSave = false;
        public bool AutoSaveRawOutput = false;
        public bool RunStabilityCheck = false;
        public bool HideErrorsUntilEnd = true;
        public ProcessPriorityClass Priority = ProcessPriorityClass.Normal;
        // Todo: Add option to set what order the videos are scanned based on some variables, like estimated time to finish
        // Todo: Add option to self-kill if no good matches for too long (but how?).

        private long _SpaceLimit = 0L;
        public long SpaceLimit
        {
            get
            {
                if (MIN_REQUIRED_SPACE <= 536870912L)
                {
                    long t;
                    long a;
                    // Todo: Prompt warning if user has less than 512MBs of RAM or Disk space.
                    // Note: Exception will be thrown if the user SOMEHOW has more than 8 exbibytes of RAM or Disk space.
                    // If they actually have that they have better things to do with their time than use this program.
                    if (UseHDD)
                    {
                        Utilities.GetDiskSpace(TempDirectory, out t, out a);
                    }
                    else
                    {
                        var ci = new ComputerInfo();
                        t = (long)ci.TotalVirtualMemory;
                        a = (long)ci.AvailableVirtualMemory;
                    }

                    if (t / 10L * 9L < 0L)
                    {
                        _SpaceLimit = Math.Max(MIN_REQUIRED_SPACE, a / 2L);
                    }
                    else
                    {
                        _SpaceLimit = Math.Max(MIN_REQUIRED_SPACE, Math.Max(a / 2L, t / 10L * 9L - a));
                    }
                }
                return _SpaceLimit;
            }
            set
            {
                SpaceLimit = value;
            }
        }

        private string _TempDirectory = Path.GetTempPath() + @"\" + Application.ProductName;
        public string TempDirectory
        {
            get
            {
                // Doesn't do anything if it already exists.
                try { Directory.CreateDirectory(_TempDirectory); } catch { }

                if (!Directory.Exists(_TempDirectory))
                {
                    _TempDirectory = Path.GetTempPath() + @"\" + Application.ProductName;
                }

                return _TempDirectory;
            }
            set
            {
                _TempDirectory = value;
            }
        }

        private string _OutputDirectory = Directory.GetCurrentDirectory();
        public string OutputDirectory
        {
            get
            {
                if (!Directory.Exists(_OutputDirectory))
                {
                    _OutputDirectory = Directory.GetCurrentDirectory();
                }
                return _OutputDirectory;
            }
            set
            {
                _OutputDirectory = value;
            }
        }

        private int _CPULimit = Environment.ProcessorCount;
        public int CPULimit
        {
            get
            {
                return _CPULimit;
            }
            set
            {
                _CPULimit = value;
                // Maybe put this somewhere else?
                //Utilities.SetCPULimit(_CPULimit);
            }
        }

        public void Run(List<Video> videos)
        {
            //Utilities.SetCPULimit(CPULimit);
            //Utilities.SetPriority(Priority);
            TempDirectory = @"E:\1";

            //int cacheSize = 536870912;

            // Todo: Validate the videos are processable. IsSynced isn't enough.
            foreach (Video v in videos)
            {
                v.InitProcess();

                // Raw image size + bmp overhead.
                int imageSize = (int)(v.ThumbnailGeometry.Width * v.ThumbnailGeometry.Height) * 3 + 54;

                /*var pipe = new NamedPipeServerStream(
                    "from_ffmpeg.bmp",
                    PipeDirection.In,
                    1,
                    PipeTransmissionMode.Message,
                    PipeOptions.WriteThrough,
                    cacheSize,
                    cacheSize);*/


                List<double> lwz = new List<double>();
                v.Feeds.ForEach(f => f.Screens.ForEach(s => s.WatchZones.ForEach(wz => wz.Watches.ForEach(w => lwz.Add(w.Frequency)))));
                double rate = lwz.Max();
                string rateStr = Math.Round(rate * 300d).ToString() + "/300";
                //string rateStr = 30.ToString();


                Process pProcess = new Process();
                pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                pProcess.StartInfo.FileName = "ffmpeg";
                //                pProcess.StartInfo.Arguments = string.Format(@"-threads 1000 -i ""{0}"" -vf fps={1},crop={2} -q:v 0 ""{3}"" -hide_banner",
                //                    v.FilePath, rateStr, v.ThumbnailGeometry.ToFFmpegString(), TempDirectory + @"\%08d.jpg");
                pProcess.StartInfo.Arguments = string.Format(@"-i ""{0}"" -vf fps={1},crop={2} -q:v 5 ""{3}""",
                    v.FilePath, rateStr, v.ThumbnailGeometry.ToFFmpegString(), TempDirectory + @"\%09d.bmp");
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = false;
                pProcess.StartInfo.RedirectStandardError = false;
                pProcess.StartInfo.CreateNoWindow = false;
                pProcess.Start();

                //pipe.WaitForConnection();

                if (UseHDD)
                {
                    Untitled2(pProcess, v);
                }
                else
                {
                    //Untitled(pProcess, pipe, imageSize, v);
                }

                //pipe.Dispose();
                //var a = pProcess.StandardError.ReadToEnd();
                pProcess.WaitForExit();

            }

            var a = videos[0].Feeds[0].Screens[0].WatchZones[0].Watches[0].Images[0].DeltaBag.ToArray();
            a.OrderBy(x => x.FrameIndex);
            var q = 1;
            var b = a.ToList();
            b.OrderBy(x => x.FrameIndex);
            q = 2;
            var c = b.OrderBy(x => x.FrameIndex);
            q = 3;
            var d = b.OrderByDescending(x => x.Delta);
            q = 4;

        }

        public void Untitled2(Process process, Video video)
        {
            List<WatchZone> lwz = new List<WatchZone>();
            video.Feeds.ForEach(f => f.Screens.ForEach(s => s.WatchZones.ForEach(wz => lwz.Add(wz))));
            lwz.ForEach(wz => wz.Watches.ForEach(w => w.Images.ForEach(wi => wi.DeltaBag.Clear())));

            DirectoryInfo d = new DirectoryInfo(_TempDirectory);
            FileInfo[] thumbs = d.GetFiles("*.bmp");

            while (!process.HasExited || thumbs.Count() > 0)
            {
                if (thumbs.Count() > 0)
                {
                    var bag = new List<WatchImage.Bag>();
                    //Parallel.ForEach(thumbs, (file, fileState, fileIndex) =>
                    foreach (var wz in lwz)
                    {
                        foreach (var w in wz.Watches)
                        {
                            foreach (var wi in w.Images)
                            {
                                foreach (var file in thumbs)
                                {
                                    if (file.Exists)
                                    {
                                        try
                                        {
                                            using (MagickImage fileImage = new MagickImage(file.FullName))
                                            {
                                                using (MagickImage deltaImage = (MagickImage)wi.MagickImage.Clone())
                                                {
                                                    var mg = wz.AdjustedGeometry.ToMagick();
                                                    fileImage.Crop(mg, Gravity.Northwest);
                                                    if (deltaImage.HasAlpha)
                                                    {
                                                        fileImage.Composite(deltaImage, CompositeOperator.CopyAlpha);
                                                    }
                                                    double delta = deltaImage.Compare(fileImage, ErrorMetric.PeakSignalToNoiseRatio);
                                                    int thumbID = int.Parse(file.Name.Substring(0, 9));
                                                    bag.Add(new WatchImage.Bag(wi, thumbID, delta));
                                                    if (delta > 10)
                                                    {
                                                        file.CopyTo(@"J:\2\" + file.Name);
                                                        fileImage.Write(@"J:\2\" + file.Name.Substring(0, 9) + "_.bmp");
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e) { Debug.Write(e); }
                                    }
                                }
                            }
                        }
                    }
                    foreach (var f in thumbs)
                    {
                        f.Delete();
                    }
                    //});
                    foreach (var b in bag)
                    {
                        var a = b.WatchImage.DeltaBag;
                        a.Add(new WatchImage.Bag(null, b.FrameIndex, b.Delta));
                    }
                }
                thumbs = d.GetFiles("*.bmp");
                //if (!process.HasExited)
                    //Thread.Sleep(10000 / (thumbs.Count() + 1));
            };
        }











        void Untitled(Process pProcess, NamedPipeServerStream pipe, int imageSize, Video video)
        {
            int scannedCount = 0;
            List<Image> thumbs = new List<Image>();
            while (!pProcess.HasExited || thumbs.Count() > 0)
            {
                //break;
                if (thumbs.Count() > 0)
                {
                    for(int n = 0; n < thumbs.Count() - 1; n++)
                    {
                        video.Feeds[0].Screens.ForEach(s => s.WatchZones.ForEach(wz => wz.Watches.ForEach(w => w.Images.ForEach(i =>
                        {
                            using (MagickImage diffImage = new MagickImage((Bitmap)thumbs[n]))
                            {
                                if (i.MagickImage.HasAlpha)
                                {
                                    diffImage.Composite(i.MagickImage, CompositeOperator.CopyAlpha);
                                }
                                double delta = diffImage.Compare(i.MagickImage, ErrorMetric.PeakSignalToNoiseRatio);
                                //i.AddBag(n, delta);
                            }
                        }
                        ))));
                    }


                    /*
                    Parallel.ForEach(thumbs, (a, state, index) =>
                    {
                        video.Feeds[0].Screens.ForEach(s => s.WatchZones.ForEach(wz => wz.Watches.ForEach(w => w.Images.ForEach(i =>
                            {
                                using (MagickImage diffImage = new MagickImage((Bitmap)a))
                                {
                                    if (i.MagickImage.HasAlpha)
                                    {
                                        diffImage.Composite(i.MagickImage, CompositeOperator.CopyAlpha);
                                    }
                                    double delta = diffImage.Compare(i.MagickImage, ErrorMetric.PeakSignalToNoiseRatio);
                                    i.AddBag(index, delta);
                                }
                            }
                        ))));
                    });
                    */
                }
                thumbs = GetThumbnails(pProcess, pipe, imageSize, ref scannedCount);
            }

        }

        static List<Image> GetThumbnails(Process pProcess, NamedPipeServerStream stream, int imageSize, ref int scannedCountEnd)
        {
            int scannedCountStart = scannedCountEnd;
            byte[] allImages = null;
            using (var br = new BinaryReader(stream))
            {
                int i = br.Read();
                Array.Resize(ref allImages, i);
                allImages = br.ReadBytes(i);

                //stream.CopyTo(sr);
                //stream.Read(allImages, scannedCountStart, (int)stream.Position);
                //sr.Read(allImages, scannedCountStart, (int)stream.Position);
                // If it isn't a multiple of imageSize, we're fucked.
                //scannedCountEnd = (int)ms.Length / imageSize;

            }

            var l = new List<Image>();
            for (int i = 0; i < allImages.Count(); i += imageSize)
            {
                using (var ms = new MemoryStream(imageSize))
                {
                    var start = Math.Min(0, allImages.Count());
                    var end = Math.Min(i + imageSize - 1, allImages.Count());
                    ms.Write(allImages, start, end);
                    Bitmap a = new Bitmap(1, 1);
                    try
                    {
                        a = (Bitmap)Image.FromStream(ms);
                        a.Save(@"E:\1\" + Guid.NewGuid().ToString("N"));
                    } catch (System.ArgumentException e) {
                        Debug.Write(e);
                    }
                    l.Add(a);
                }
            }
            return l;
        }



        /*
        static IEnumerable<int> GetBytePatternPositions(byte[] data, byte[] pattern)
        {
            var dataLen = data.Length;
            var patternLen = pattern.Length - 1;
            int scanData = 0;
            int scanPattern = 0;
            while (scanData < dataLen)
            {
                if (pattern[0] == data[scanData])
                {
                    scanPattern = 1;
                    scanData++;
                    while (pattern[scanPattern] == data[scanData])
                    {
                        if (scanPattern == patternLen)
                        {
                            yield return scanData - patternLen;
                            break;
                        }
                        scanPattern++;
                        scanData++;
                    }
                }
                scanData++;
            }
        }
        */
    }

}
