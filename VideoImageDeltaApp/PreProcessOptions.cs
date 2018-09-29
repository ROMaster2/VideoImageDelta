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
            // Would make more sense to just store these staticly...
            DropBox_Hardware.Items.AddRange(RawFFmpeg.GetAvailableHardwareAccelerators());
            if (!string.IsNullOrWhiteSpace(p.UseVideoHardwareAccel))
                DropBox_Hardware.SelectedItem = p.UseVideoHardwareAccel;
            CheckBox_Stability.Checked = p.RunStabilityCheck;
            CheckBox_Errors.Checked = p.HideErrorsUntilEnd;
            CheckBox_ToRam.Checked = !p.UseHDD;
            TextBox_Cache.Text = p.TempDirectory;
            CheckBox_AutoSave.Checked = p.AutoSave;
            DropBox_Priority.SelectedItem = p.Priority;
            TextBox_Output.Text = p.OutputDirectory;
            NumericUpDown_CPU_Limit.Value = p.CPULimit;
        }

        private void Update_Values()
        {
            var p = Program.Processor;

            if (string.IsNullOrWhiteSpace((string)DropBox_Hardware.SelectedItem))
                p.UseVideoHardwareAccel = (string)DropBox_Hardware.SelectedItem;
            else
                p.UseVideoHardwareAccel = null;

            p.DebuggingEnabled = CheckBox_Debug.Checked;
            p.UseVideoHardwareAccel = (string)DropBox_Hardware.SelectedItem;
            p.RunStabilityCheck = CheckBox_Stability.Checked;
            p.HideErrorsUntilEnd = CheckBox_Errors.Checked;
            p.UseHDD = !CheckBox_ToRam.Checked;
            p.TempDirectory = TextBox_Cache.Text;
            p.AutoSave = CheckBox_AutoSave.Checked;
            p.Priority = (ProcessPriorityClass)DropBox_Priority.SelectedItem;
            p.OutputDirectory = TextBox_Output.Text;
            p.CPULimit = (int)NumericUpDown_CPU_Limit.Value;
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            Update_Values();
            Processing w = new Processing();
            w.Show();
            Close();
        }

        private void Button_Browse_Cache_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.ShowDialog();

                if (Directory.Exists(fbd.SelectedPath))
                {
                    TextBox_Cache.Text = fbd.SelectedPath;
                }
            }
        }

        private void Button_Browse_Output_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.ShowDialog();

                if (Directory.Exists(fbd.SelectedPath))
                {
                    TextBox_Output.Text = fbd.SelectedPath;
                }
            }
        }

        private void PreProcessOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            Update_Values();
        }
    }
}
