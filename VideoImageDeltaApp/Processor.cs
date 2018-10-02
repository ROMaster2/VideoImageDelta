using ImageMagick;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoImageDeltaApp.Models;

namespace VideoImageDeltaApp
{
    public class Processor
    {
        public const long MIN_REQUIRED_SPACE = 536870912L; // 512 Mebibytes
        public const string FRAME_FILE_TYPE = "bmp";
        public const string FRAME_FILE_SELECTOR = "*." + FRAME_FILE_TYPE;
        public const int BMP_OVERHEAD = 54;

        public Processor() { }

        public bool Finished { get; internal set; } = false;
        public bool Paused { get; internal set; } = false;
        public bool Aborted { get; internal set; } = false;
        public Processing ProcessingWindow;
        public Process Process;

        public bool DebuggingEnabled = false;
        public string UseVideoHardwareAccel = null;
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

        //private string _TempDirectory = Path.GetTempPath() + Application.ProductName;
        private string _TempDirectory = @"E:\1";
        public string TempDirectory
        {
            get
            {
                // Doesn't do anything if it already exists.
                try { Directory.CreateDirectory(_TempDirectory); } catch { }

                if (!Directory.Exists(_TempDirectory))
                {
                    _TempDirectory = Path.GetTempPath() + Application.ProductName;
                    _TempDirectory = @"E:\1";
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

        public void Abort()
        {
            if (Process != null && !Process.HasExited) Process.Kill();
            Aborted = true;
        }

        public void Pause()
        {
            Paused = true;
            if (Process != null && !Process.HasExited) Process.Suspend();
        }

        public void Resume()
        {
            Paused = false;
            if (Process != null && !Process.HasExited) Process.Resume();
        }

        public void Run(IEnumerable<Video> videos)
        {
            Finished = false;
            Paused = false;


            //Utilities.SetCPULimit(CPULimit);
            Utilities.SetPriority(Priority);

            var processingForms = Application.OpenForms.OfType<Processing>();
            if (processingForms.Count() <= 0)
            {
                throw new MissingMemberException("Processing window does not exist. I don't know how this could happen.");
            }
            else
            {
                ProcessingWindow = processingForms.First();
            }

            int videosFinished = 0;
            ProcessingWindow.Update_Video_Count(videosFinished, videos.Count());
            // Todo: Validate the videos are processable. IsSynced isn't enough.
            foreach (Video video in videos)
            {
                if (!Aborted)
                {
                    ProcessingWindow.Update_Current_Video(video.FilePath);
                    video.InitProcess();

                    //int imageSize = (int)(v.ThumbnailGeometry.Width * v.ThumbnailGeometry.Height) * 3 + BMP_OVERHEAD;

                    /*var pipe = new NamedPipeServerStream(
                        "from_ffmpeg.bmp",
                        PipeDirection.In,
                        1,
                        PipeTransmissionMode.Message,
                        PipeOptions.WriteThrough,
                        cacheSize,
                        cacheSize);*/


                    string rateStr = Math.Round(video.MaxScanRate * 300d).ToString() + "/300";

                    string hwAccel = null;
                    if (!string.IsNullOrWhiteSpace(UseVideoHardwareAccel))
                        hwAccel = "-hwaccel " + UseVideoHardwareAccel;


                    Process = new Process();
                    Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.StartInfo.FileName = "ffmpeg";
                    Process.StartInfo.Arguments = string.Format(@"{0} -i ""{1}"" -vf fps={2},crop={3} -q:v 5 ""{4}""",
                        hwAccel, video.FilePath, rateStr, video.ThumbnailGeometry.ToFFmpegString(), TempDirectory + @"\%08d." + FRAME_FILE_TYPE);
                    Process.StartInfo.UseShellExecute = false;
                    Process.StartInfo.RedirectStandardOutput = false;
                    Process.StartInfo.RedirectStandardError = false;
                    Process.StartInfo.CreateNoWindow = false;
                    Process.Start();

                    //pipe.WaitForConnection();

                    if (UseHDD)
                    {
                        ScanVideo(video);
                    }
                    else
                    {
                        //Untitled(pProcess, pipe, imageSize, v);
                    }

                    //pipe.Dispose();
                    //var a = pProcess.StandardError.ReadToEnd();
                    Process.WaitForExit();
                    videosFinished++;
                    ProcessingWindow.Update_Video_Count(videosFinished, videos.Count());
                    //video.Feeds.ForEach(f => f.OCRBag = f.OCRBag.OrderBy(b => b.FrameIndex ?? 0).ToList());
                    //video.WatchImages.ForEach(wi => wi.DeltaBag = wi.DeltaBag.OrderBy(b => b.FrameIndex).ToList());

                    foreach(var f in video.Feeds)
                    {
                        if (f.OCRBag.Count > 0)
                        {
                            f.OCRBag = new ConcurrentBag<Bag>(f.OCRBag.OrderBy(b => b.FrameIndex));
                        }
                    }
                    foreach (var wi in video.WatchImages)
                    {
                        if (wi.DeltaBag.Count > 0)
                        {
                            wi.DeltaBag = new ConcurrentBag<Bag>(wi.DeltaBag.OrderBy(b => b.FrameIndex));
                        }
                    }
                }
                if (Aborted)
                {
                    DirectoryInfo directory = new DirectoryInfo(TempDirectory);
                    FileInfo[] files = directory.GetFiles(FRAME_FILE_SELECTOR);
                    Parallel.ForEach(files, (file) =>
                    {
                        file.Delete();
                    });
                }
            }
            /*
            var d = 0;
            var a1 = videos[0].Feeds.Where(f => f.UseOCR).First().OCRBag;
            var b1 = videos[0].Feeds[0].Screens[0].WatchZones[0].Watches[0].Images[0].DeltaBag;
            var b2 = e.OrderBy(f => f.Confidence);
            var b3 = e.OrderBy(f => f.FrameIndex);
            var c1 = videos[0].Feeds[0].Screens[0].WatchZones[1].Watches[0].Images[0].DeltaBag;
            var c2 = u.OrderBy(o => o.FrameIndex);
            d += 1;
            */

            Aborted = false;
            Finished = true;
            ProcessingWindow.Finished();
        }

        public void ScanVideo(Video video)
        {
            int scanCount = (int)Math.Floor(video.FrameCount / video.MaxScanRate / video.FrameRate);
            int finishCount = 0;

            ProcessingWindow.Update_totalVideoFrames(scanCount);

            // Todo: Make method in Video for this instead.
            video.Feeds.ForEach(f => f.OCRBag = new ConcurrentBag<Bag>());
            video.WatchImages.ForEach(wi => wi.DeltaBag = new ConcurrentBag<Bag>());

            DirectoryInfo directory = new DirectoryInfo(TempDirectory);
            FileInfo[] files = directory.GetFiles(FRAME_FILE_SELECTOR);
            Parallel.ForEach(files, (file) =>
            {
                file.Delete();
            });

            while (!Aborted && (!Process.HasExited || files.Count() > 0))
            {
                files = directory.GetFiles(FRAME_FILE_SELECTOR);
                if (files.Count() > 0 && !Paused && !Aborted)
                {
                    Parallel.ForEach(files, (file) =>
                    {
                        int thumbID = int.Parse(file.Name.Substring(0, 8));
                        if (file.Exists && !Paused && !Aborted)
                        {
                            try
                            {
                                using (MagickImage fileImageBase = new MagickImage(file.FullName))
                                {
                                    if (fileImageBase.Width <= 1 && fileImageBase.Height <= 1)
                                    { // Sometimes happens. Dunno how. Probably because it's parallel.
                                        goto Skip;
                                    }
                                    fileImageBase.RePage();
                                    Parallel.ForEach(video.Feeds, (f) =>
                                    {
                                        if (!f.UseOCR)
                                        {
                                            Parallel.ForEach(f.Screens, (s) =>
                                            {
                                                Parallel.ForEach(s.WatchZones, (wz) =>
                                                {
                                                    var mg = wz.AdjustedGeometry.ToMagick();
                                                // Doesn't crop perfectly. Investigate later.
                                                using (var fileImageCrop = (MagickImage)fileImageBase.Clone())
                                                    {
                                                        fileImageCrop.Crop(mg, Gravity.Northwest);
                                                        Parallel.ForEach(wz.Watches, (w) =>
                                                        {
                                                            Parallel.ForEach(w.WatchImages, (wi) =>
                                                            {
                                                                using (MagickImage deltaImage = (MagickImage)wi.MagickImage.Clone())
                                                                {
                                                                    using (var fileImageCompare = (MagickImage)fileImageCrop.Clone())
                                                                    {
                                                                        if (deltaImage.HasAlpha)
                                                                        {
                                                                            fileImageCompare.Composite(deltaImage, CompositeOperator.CopyAlpha);
                                                                        }
                                                                        float delta = (float)deltaImage.Compare(fileImageCompare,
                                                                            ErrorMetric.PeakSignalToNoiseRatio);
                                                                        wi.DeltaBag.Add(new Bag(thumbID, delta));
                                                                    }
                                                                }
                                                            });
                                                        });
                                                    }
                                                });
                                            });
                                        }
                                        else
                                        {
                                            using (var b = fileImageBase.ToBitmap())
                                            {
                                                Utilities.ReadImage(b, f.ThumbnailGeometry, out string str, out float confidence);
                                                f.OCRBag.Add(new Bag(thumbID, confidence, str));
                                            }
                                        }
                                    });
                                }
                            } // Sometimes Magick can't read an image correctly. No idea why.
                            catch (MagickCorruptImageErrorException e)
                            {
                                Debug.Write(e);
                                goto Skip;
                            }
                            file.Delete();
                            finishCount++;
                            ProcessingWindow.Update_ProgressBar_Scanning(finishCount);
                            Skip:;
                        }
                    });
                }

                // Don't hog the CPU if it doesn't have many files to check.
                if (!Process.HasExited)
                {
                    Thread.Sleep(Math.Min(1000, 10000 / (files.Count() + 1)));
                }
                while (Paused && !Aborted)
                {
                    Thread.Sleep(250);
                }
            };
        }
        /*
        void Untitled(Process pProcess, NamedPipeServerStream pipe, int imageSize, Video video)
        {
            int scannedCount = 0;
            List<Image> thumbs = new List<Image>();
            while (!pProcess.HasExited || thumbs.Count() > 0)
            {
                //break;
                if (thumbs.Count() > 0)
                {
                    for (int n = 0; n < thumbs.Count() - 1; n++)
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
        /*
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
        }
        catch (System.ArgumentException e)
        {
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
