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
    public partial class PreProcessOptions : Form
    {
        public PreProcessOptions()
        {
            InitializeComponent();
            var p = Program.Processor;
            CheckBox_Debug.Checked = p.DebuggingEnabled;
            CheckBox_Accel_Video.Checked = p.HardwareAccelForFFmpeg;
            CheckBox_Accel_Image.Checked = p.HardwareAccelForMagick;
            CheckBox_Stability.Checked = p.RunStabilityCheck;
            CheckBox_Errors.Checked = p.HideErrorsUntilEnd;
            CheckBox_ToRam.Checked = !p.UseHDD;
            TextBox_Cache.Text = p.TempDirectory.ToString();
            CheckBox_AutoSave.Checked = p.AutoSave;
            DropBox_Priority.SelectedItem = p.Priority;
            //Numeric_Space_Limit.Value = p.SpaceLimit;
            TextBox_Output.Text = p.OutputDirectory.ToString();
            NumericUpDown_CPU_Limit.Value = p.CPULimit;

            DropBox_Priority.SelectedIndex = 2;
        }
    }
}
