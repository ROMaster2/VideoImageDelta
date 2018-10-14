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
using VideoImageDeltaApp.Forms;

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
        private string _TempDirectory = @"J:\1";
        public string TempDirectory
        {
            get
            {
                // Doesn't do anything if it already exists.
                try { Directory.CreateDirectory(_TempDirectory); } catch { }

                if (!Directory.Exists(_TempDirectory))
                {
                    _TempDirectory = Path.GetTempPath() + Application.ProductName;
                    _TempDirectory = @"J:\1";
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
            Aborted = false;
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

                    string hardwareAccelStr = null;
                    if (!string.IsNullOrWhiteSpace(UseVideoHardwareAccel))
                        hardwareAccelStr = "-hwaccel " + UseVideoHardwareAccel;

                    Process = new Process();
                    Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.StartInfo.FileName = RawFFmpeg.ffmpegPath;
                    Process.StartInfo.Arguments = string.Format(
                        @"{0} -i ""{1}"" -vf fps={2},crop={3} -q:v 5 ""{4}""",
                        hardwareAccelStr,
                        video.FilePath,
                        video.ScanRateFraction,
                        video.ThumbnailGeometry.ToFFmpegString(),
                        TempDirectory + @"\%08d." + FRAME_FILE_TYPE
                        );
                    Process.StartInfo.UseShellExecute = false;
                    Process.StartInfo.RedirectStandardOutput = false;
                    Process.StartInfo.RedirectStandardError = false;
                    Process.StartInfo.CreateNoWindow = false;
                    Process.Start();

                    if (UseHDD)
                    {
                        ScanVideo(video);
                    }
                    Process.WaitForExit();
                    videosFinished++;
                    ProcessingWindow.Update_Video_Count(videosFinished, videos.Count());

                    foreach (var f in video.Feeds)
                    {
                        if (f.OCRBag.Count > 0)
                        {
                            f.OCRBag = new ConcurrentBag<Bag>(f.OCRBag.OrderBy(b => -b.FrameIndex));
                        }
                    }
                    foreach (var wi in video.WatchImages)
                    {
                        if (wi.DeltaBag.Count > 0)
                        {
                            wi.DeltaBag = new ConcurrentBag<Bag>(wi.DeltaBag.OrderBy(b => -b.FrameIndex));
                        }
                    }
                }
            }

            DirectoryInfo directory = new DirectoryInfo(TempDirectory);
            FileInfo[] files = directory.GetFiles(FRAME_FILE_SELECTOR);
            Parallel.ForEach(files, (file) =>
            {
                file.Delete();
            });

            Aborted = false;
            Finished = true;
            Paused = false;
            ProcessingWindow.Finished();
        }

        public void ScanVideo(Video video)
        {
            int scanCount = (int)Math.Ceiling(video.Duration.TotalSeconds * video.ScanRate);
            int finishCount = 0;
            ProcessingWindow.Update_totalVideoFrames(scanCount);

            double timeIndexMultiplier = 1000d / video.ScanRate;

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
                                using (var fileImageBase = new MagickImage(file.FullName))
                                {
                                    if (fileImageBase.Width <= 1 && fileImageBase.Height <= 1)
                                    { // Sometimes happens. Dunno how. Probably because it's parallel.
                                        goto Skip;
                                    }
                                    fileImageBase.RePage();
                                    foreach (var f in video.Feeds)
                                    {
                                        if (!f.UseOCR)
                                        {
                                            foreach (var s in f.Screens)
                                            {
                                                foreach (var wz in s.WatchZones)
                                                {
                                                    var mg = wz.ThumbnailGeometry.ToMagick();
                                                    // Doesn't crop perfectly. Investigate later.
                                                    using (var fileImageCropped = (MagickImage)fileImageBase.Clone())
                                                    {
                                                        fileImageCropped.Crop(mg, Gravity.Northwest);
                                                        foreach (var w in wz.Watches)
                                                        {
                                                            using (var fileImageComposed = (MagickImage)fileImageCropped.Clone())
                                                            {
                                                                fileImageComposed.ColorSpace = w.ColorSpace;
                                                                foreach (var wi in w.WatchImages)
                                                                {
                                                                    var deltaImage = wi.MagickImage;
                                                                    using (var fileImageCompare = (MagickImage)fileImageComposed.Clone())
                                                                    {
                                                                        if (deltaImage.HasAlpha)
                                                                        {
                                                                            fileImageCompare.Composite(deltaImage, CompositeOperator.CopyAlpha);
                                                                        }

                                                                        var delta = (float)deltaImage.Compare(fileImageCompare,
                                                                            ErrorMetric.NormalizedCrossCorrelation);

                                                                        int id = (int)Math.Round(thumbID * timeIndexMultiplier);
                                                                        wi.DeltaBag.Add(new Bag(id, delta));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (var b = fileImageBase.ToBitmap())
                                            {
                                                Utilities.ReadImage(b, f.ThumbnailGeometry, out string str, out float confidence);
                                                f.OCRBag.Add(new Bag(thumbID, confidence, str));
                                            }
                                        }
                                    }
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
    }
}
