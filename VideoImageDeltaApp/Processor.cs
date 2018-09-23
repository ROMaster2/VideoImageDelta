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
using Anchor = VideoImageDeltaApp.Models.Anchor;

namespace VideoImageDeltaApp
{
    public class Processor
    {
        private const long MIN_REQUIRED_SPACE = 536870912L; // 512 Mebibytes

        public Processor() { }

        public bool DebuggingEnabled = false;
        public bool HardwareAccelForFFmpeg = false;
        public bool HardwareAccelForMagick = false;
        public bool UseHDD = false;
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

        private string _TempDirectory = Path.GetTempPath();
        public string TempDirectory
        {
            get
            {
                // Doesn't do anything if it already exists.
                try { Directory.CreateDirectory(_TempDirectory); } catch { }

                if (!Directory.Exists(_TempDirectory))
                {
                    _TempDirectory = Path.GetTempPath();
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


    }

}
