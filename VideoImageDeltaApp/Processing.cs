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

namespace VideoImageDeltaApp
{
    public partial class Processing : Form
    {
        public Processing()
        {
            InitializeComponent();
            Program.Processor.Run(Program.Videos.Where(x => x.IsSynced()).ToList());
        }

        private void Form_Loaded(object sender, EventArgs e)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i] != this)
                {
                    if (Application.OpenForms[i].Name != "MainWindow")
                        Application.OpenForms[i].Close();
                    else
                        Application.OpenForms[i].Hide();
                }
            }
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to cancel the process?",
               "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                // Kill processes
                Program.MainWindow.Show();
            }
        }

        private void Button_Pause_Click(object sender, EventArgs e)
        {

        }
    }
}
