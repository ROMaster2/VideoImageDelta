using Ikriv.Xml;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;
using VideoImageDeltaApp.Models;

namespace VideoImageDeltaApp.Forms
{
    public partial class Processing : Form
    {
        private int estimatedTime = 0;
        private Stopwatch Watch = new Stopwatch();
        private int videoFramesCreated = 0;
        private int totalVideoFrames = 1;
        private int allFramesCreated = 1;

        public Processing()
        {
            InitializeComponent();
            FileSystemWatcher.Path = Program.Processor.TempDirectory;
            FileSystemWatcher.Filter = Processor.FRAME_FILE_SELECTOR;
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

            Thread run = new Thread(new ThreadStart(Run));
            run.Start();

            Start_Timer();
        }

        private void Run()
        {
            Program.Processor.Run(Program.Videos.Where(x => x.IsSynced()).ToList());
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to abort the process?",
               "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Program.Processor.Abort();
                if (!Program.Processor.Finished)
                {
                    while (Program.Processor.Aborted)
                    {
                        Thread.Sleep(15);
                    }
                }
                Program.MainWindow.Show();
            }
        }

        private void Button_Pause_Click(object sender, EventArgs e)
        {
            if (Program.Processor.Finished)
            {
                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    FileName = "output",
                    DefaultExt = "xml",
                    Filter = "XML Files|*.xml",
                    ValidateNames = true,
                    Title = "Save raw output data"
                })
                {
                    sfd.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(sfd.FileName))
                    {
                        var lv = Program.Videos;

                        var overrides = new OverrideXml()
                            .Override<Feed>()
                                .Member("Screens").XmlIgnore(false);

                        if (!CheckBox_Compact.Checked)
                        {
                            overrides
                                .Override<Feed>()
                                    .Member("OCRList").XmlIgnore(false)
                                    .Member("OCRBagCompact").XmlIgnore(true)
                                .Override<WatchImage>()
                                    .Member("DeltaList").XmlIgnore(false)
                                    .Member("DeltaBagCompact").XmlIgnore(true);
                        }

                        Type t = lv.GetType();
                        XmlSerializer serializer = new XmlSerializer(t, overrides.Commit());
                        using (TextWriter writer = new StreamWriter(sfd.FileName))
                        {
                            serializer.Serialize(writer, lv);
                        }
                    }
                }
            }
            else if (Program.Processor.Paused)
            {
                Program.Processor.Resume();
                Start_Timer();
                Button_Pause.Text = "Pause";
            }
            else
            {
                Program.Processor.Pause();
                Stop_Timer();
                Button_Pause.Text = "Resume";
            }
        }

        public void Start_Timer()
        {
            Watch.Start();
            Watch_Ticker.Start();
            Update_Timers();
        }

        public void Stop_Timer()
        {
            Watch.Stop();
            Watch_Ticker.Stop();
            Update_Timers();
        }

        public void Update_Timers()
        {
            var n = Watch.ElapsedMilliseconds;
            Label_Elapsed_Value.Text = TimeSpan.FromMilliseconds(n).ToString(@"hh\:mm\:ss");
            if (estimatedTime > 0)
            {
                var a = Math.Max(0, estimatedTime - n);
                Label_Remaining_Value.Text = TimeSpan.FromMilliseconds(a).ToString(@"hh\:mm\:ss");
                Label_Estimated_Value.Text = TimeSpan.FromMilliseconds(estimatedTime).ToString(@"hh\:mm\:ss");
            }
            else
            {
                Label_Remaining_Value.Text = TimeSpan.Zero.ToString(@"hh\:mm\:ss");
                Label_Estimated_Value.Text = TimeSpan.Zero.ToString(@"hh\:mm\:ss");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Update_Timers();
            Update_Speed();
        }

        public void Update_Current_Video(string videoPath)
        {
            TextBox_Current.BeginInvoke((MethodInvoker)delegate ()
            {
                TextBox_Current.Text = videoPath;
            });
        }

        public void Update_totalVideoFrames(int n)
        {
            BeginInvoke((MethodInvoker)delegate ()
            {
                totalVideoFrames = n + 1;
            });
            Update_ProgressBar_Extraction(0);
            Update_ProgressBar_Scanning(0);
        }

        public void Finished()
        {
            Button_Pause.BeginInvoke((MethodInvoker)delegate ()
            {
                Button_Pause.Text = "Export";
                Stop_Timer();
                CheckBox_Compact.Show();
                Label_Notice.Show();
            });
        }

        public void Update_Speed()
        {
            // Todo: Make it rolling average instead.
            Label_Speed_Value.Text = (allFramesCreated * 1000 / (double)Watch.ElapsedMilliseconds).ToString(@"0.##\x");
        }

        public void Update_Video_Count(int finishedVideoCount, int totalVideoCount)
        {
            Update_Counter(Label_Videos_Processed_Value, finishedVideoCount, totalVideoCount);
        }

        private void Update_Frames_Processed()
        {
            // Todo: Scanned, not extracted.
            Label_Frames_Processed_Value.Text = allFramesCreated.ToString();
        }

        public void Update_ProgressBar_Total(int finishedFrameCount)
        {
            // Todo: not use this.totalVideoFrames
            Update_ProgressBar(ProgressBar_Total, finishedFrameCount, this.totalVideoFrames);
        }

        public void Update_ProgressBar_Extraction(int finishedFrameCount)
        {
            Update_ProgressBar(ProgressBar_Extraction, finishedFrameCount, this.totalVideoFrames);
            Update_Counter(Label_Extraction_Value, finishedFrameCount, this.totalVideoFrames);
        }

        public void Update_ProgressBar_Scanning(int finishedFrameCount)
        {
            Update_ProgressBar(ProgressBar_Scanning, finishedFrameCount, this.totalVideoFrames);
            Update_Counter(Label_Scanning_Value, finishedFrameCount, this.totalVideoFrames);
        }

        private void Update_Counter(Label label, int numerator, int denominator)
        {
            try
            {
                label.BeginInvoke((MethodInvoker)delegate ()
                {
                    label.Text = numerator.ToString() + " / " + denominator.ToString();
                });
            }
            catch (System.InvalidOperationException e)
            {
                Debug.Write(e.ToString());
            }
        }

        private void Update_ProgressBar(ProgressBar progressBar, int finishedFrameCount, int totalFrameCount)
        {
            try
            {
                progressBar.BeginInvoke((MethodInvoker)delegate ()
                {
                    int i = finishedFrameCount * progressBar.Maximum / totalFrameCount;
                    progressBar.Value = Math.Min(progressBar.Maximum, i);
                });
            }
            catch (System.InvalidOperationException e)
            {
                Debug.Write(e.ToString());
            }
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            videoFramesCreated++;
            allFramesCreated++;
            Update_ProgressBar_Extraction(videoFramesCreated);
            Update_Frames_Processed();
        }
    }
}
