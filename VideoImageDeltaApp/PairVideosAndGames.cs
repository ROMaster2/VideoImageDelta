using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
//using Hudl.FFmpeg.Settings;
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

namespace VideoImageDeltaApp
{
    public partial class PairVideosAndGames : Form
    {
        public PairVideosAndGames()
        {
            InitializeComponent();
            
            foreach (var gp in Program.GameProfiles)
            {
                ComboBox_GameProfile.Items.Add(gp);
            }
            if (ComboBox_GameProfile.Items.Count > 0)
            {
                ComboBox_GameProfile.SelectedIndex = 0;
                ComboBox_GameProfile.Enabled = true;
            }
            else
            {
                ComboBox_GameProfile.Enabled = false;
            }

            foreach (var p in Program.Pairs)
            {
                ListView_Main.Items.Add(new ListPair(p));
            }
            foreach (var v in Program.Videos)
            {
                ListView_Main.Items.Add(new ListPair(new Pair(v, null)));
            }
            ListView_Main.Enabled = (ListView_Main.Items.Count > 0);

        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            Form_Closing(null, null);
            Close();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Program.Pairs.Clear();
            foreach (ListVideo lv in ListView_Main.Items)
                Program.Videos.Add(lv.Video);
        }

        private void Button_Help_Click(object sender, EventArgs e)
        {
            // Todo
        }

        private void Button_Pair_VandG_Click(object sender, EventArgs e)
        {
            if (ListView_Main.SelectedItems.Count > 0 && ComboBox_GameProfile.SelectedIndex > -1)
            {
                foreach(ListPair lp in ListView_Main.SelectedItems)
                {
                    lp.Pair.GameProfile = (GameProfile)ComboBox_GameProfile.SelectedItem;
                    ListView_Main.Items[lp.Index] = lp;
                }
            }
        }

        private void ListView_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enable_VandG_Pair();
        }

        private void ComboBox_GameProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enable_VandG_Pair();
        }

        private void Enable_VandG_Pair()
        {
            Button_Pair_VandG.Enabled = (ListView_Main.SelectedItems.Count > 0 && ComboBox_GameProfile.SelectedIndex > -1);
        }
    }
}
