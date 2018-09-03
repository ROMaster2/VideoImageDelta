using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoImageDeltaApp.Models;

namespace VideoImageDeltaApp.Forms
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //Panel_Start.Show();
            //Panel_Video.Hide();
        }

        private void Button_Help_Click(object sender, EventArgs e)
        {

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
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "XML Files|*.xml";
            fileDialog.Title = "Select a XML File";

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

        }

        private void Unselect_Other_ListBoxes(ListBox listBox)
        {
          /*ListBox_Videos.ClearSelected();
            ListBox_Feeds.ClearSelected();
            ListBox_Watches.ClearSelected();
            ListBox_WatchZones.ClearSelected();
            ListBox_Screens.ClearSelected();
            ListBox_ImageGroups.ClearSelected();
            listBox.Show();*/
        }

        private void ListBox_Videos_SelectedIndexChanged(object sender, EventArgs e)
        {
          /*Panel_Video.Show();
            ListBox_Feeds.ClearSelected();
            ListBox_Watches.ClearSelected();
            ListBox_WatchZones.ClearSelected();
            ListBox_Screens.ClearSelected();
            ListBox_ImageGroups.ClearSelected();*/
        }

        private void ListBox_Feeds_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ListBox_Watches_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ListBox_WatchZones_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ListBox_Screens_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ListBox_ImageGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Button_Minus_Videos_Click(object sender, EventArgs e)
        {
            /*if (ListBox_Videos.SelectedItem != null)
            {
                ListBox_Videos.Items.RemoveAt(ListBox_Videos.SelectedIndex);
            }*/
        }

        private void Button_Minus_Feeds_Click(object sender, EventArgs e)
        {

        }

        private void Button_Minus_Watches_Click(object sender, EventArgs e)
        {

        }

        private void Button_Minus_WatchZones_Click(object sender, EventArgs e)
        {

        }

        private void Button_Minus_Screens_Click(object sender, EventArgs e)
        {

        }

        private void Button_Minus_ImageGroups_Click(object sender, EventArgs e)
        {

        }

        private void Button_Plus_Videos_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Video Files|*.3g2;*.3gp;*.3gp2;*.3gpp;*.amr;*.asf;*.avi;*.bik;*.d2v;*.divx;*.drc;*.dsa;*.dsm;*.dss;*.dsv;*.flc;*.fli;*.flic;*.flv;*.ifo;*.ivf;*.m1v;*.m2v;*.m4b;*.m4p;*.m4v;*.mkv;*.mp2v;*.mp4;*.mpe;*.mpeg;*.mpg;*.mpv2;*.mov;*.ogm;*.pss;*.pva;*.qt;*.ratdvd;*.rm;*.rmm;*.rmvb;*.roq;*.rpm;*.smk;*.swf;*.tp;*.tpr;*.ts;*.vob;*.vp6;*.webm;*.wm;*.wmp;*.wmv|All files|*.*";
            fileDialog.Title = "Select a Video File";

            /*if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var video = new Video(fileDialog.FileName);
                ListBox_Videos.Items.Add(video);
                ListBox_Videos.SelectedIndex = ListBox_Videos.Items.Count - 1;
            }*/


        }

        private void Button_Plus_Feeds_Click(object sender, EventArgs e)
        {

        }

        private void Button_Plus_Watches_Click(object sender, EventArgs e)
        {

        }

        private void Button_Plus_WatchZones_Click(object sender, EventArgs e)
        {

        }

        private void Button_Plus_Screens_Click(object sender, EventArgs e)
        {

        }

        private void Button_Plus_ImageGroups_Click(object sender, EventArgs e)
        {

        }

        private void Label_Video_Y_Click(object sender, EventArgs e)
        {

        }

        private void Button_Video_Add_Feed_Click(object sender, EventArgs e)
        {
            /*var x = Convert.ToInt32(TextBox_Videos_X.Text);
            var y = Convert.ToInt32(TextBox_Videos_Y.Text);
            var width = Convert.ToInt32(TextBox_Videos_Width.Text);
            var height = Convert.ToInt32(TextBox_Videos_Height.Text);
            var geo = new MagickGeometry(x, y, width, height);
            Gravity grav = (Gravity)Gravity.Parse(typeof(Gravity), DropBox_Videos_Anchor.Text, true);
            var feed = new Feed(TextBox_Videos_Name.Text, geo, grav);
            ListBox_Feeds.Items.Add(feed);
            ListBox_Feeds.SelectedIndex = ListBox_Videos.Items.Count - 1;*/
        }

        private void Button_Watch_Click(object sender, EventArgs e)
        {
            AddWatchers w = new AddWatchers();
            w.Show();
        }
    }
}
