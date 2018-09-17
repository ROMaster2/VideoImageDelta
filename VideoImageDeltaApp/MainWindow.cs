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

namespace VideoImageDeltaApp.Forms
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Help_Click(object sender, EventArgs e)
        {
            // Todo
        }

        private void Button_Quit_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Are you sure you want to quit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            //if (dr == DialogResult.Yes)
            //{
                Application.Exit();
            //}
        }

        private void Button_Import_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "XML Files|*.xml",
                Title = "Select a XML File"
            };

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Process XML
            }

        }

        private void Button_Export_Click(object sender, EventArgs e)
        {

        }

        private void Button_Process_Click(object sender, EventArgs e)
        {
            //Test.RunTest(Program.Videos, Program.GameProfiles);
        }

        private void Button_Minus_Videos_Click(object sender, EventArgs e)
        {
            /*if (ListBox_Videos.SelectedItem != null)
            {
                ListBox_Videos.Items.RemoveAt(ListBox_Videos.SelectedIndex);
            }*/
        }

        private void Button_Watch_Click(object sender, EventArgs e)
        {
            AddWatchers w = new AddWatchers();
            w.Show();
        }

        private void Button_Videos_Click(object sender, EventArgs e)
        {
            AddVideos w = new AddVideos();
            w.Show();
        }
    }
}
