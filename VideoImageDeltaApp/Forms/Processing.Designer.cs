namespace VideoImageDeltaApp.Forms
{
    partial class Processing
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ProgressBar_Total = new System.Windows.Forms.ProgressBar();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.Button_Pause = new System.Windows.Forms.Button();
            this.Label_Progress = new System.Windows.Forms.Label();
            this.Label_Current = new System.Windows.Forms.Label();
            this.TextBox_Current = new System.Windows.Forms.TextBox();
            this.ProgressBar_Extraction = new System.Windows.Forms.ProgressBar();
            this.Label_Extraction = new System.Windows.Forms.Label();
            this.ProgressBar_Scanning = new System.Windows.Forms.ProgressBar();
            this.Label_Extraction_Value = new System.Windows.Forms.Label();
            this.Label_Scanning = new System.Windows.Forms.Label();
            this.Label_Scanning_Value = new System.Windows.Forms.Label();
            this.Label_Speed = new System.Windows.Forms.Label();
            this.Label_Speed_Value = new System.Windows.Forms.Label();
            this.Label_Elapsed = new System.Windows.Forms.Label();
            this.Label_Remaining = new System.Windows.Forms.Label();
            this.Label_Estimated = new System.Windows.Forms.Label();
            this.TableLayoutPanel_Top = new System.Windows.Forms.TableLayoutPanel();
            this.Label_Videos_Processed = new System.Windows.Forms.Label();
            this.Label_Frames_Processed = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.Label_Frames_Processed_Value = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.Label_Elapsed_Value = new System.Windows.Forms.Label();
            this.Label_Remaining_Value = new System.Windows.Forms.Label();
            this.Label_Estimated_Value = new System.Windows.Forms.Label();
            this.Label_Videos_Processed_Value = new System.Windows.Forms.Label();
            this.Watch_Ticker = new System.Windows.Forms.Timer(this.components);
            this.FileSystemWatcher = new System.IO.FileSystemWatcher();
            this.Label_Notice = new System.Windows.Forms.Label();
            this.CheckBox_Compact = new System.Windows.Forms.CheckBox();
            this.TableLayoutPanel_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileSystemWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // ProgressBar_Total
            // 
            this.ProgressBar_Total.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar_Total.Location = new System.Drawing.Point(12, 173);
            this.ProgressBar_Total.Maximum = 1000;
            this.ProgressBar_Total.Name = "ProgressBar_Total";
            this.ProgressBar_Total.Size = new System.Drawing.Size(472, 23);
            this.ProgressBar_Total.TabIndex = 0;
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_Cancel.Location = new System.Drawing.Point(12, 311);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Button_Cancel.TabIndex = 2;
            this.Button_Cancel.Text = "Cancel";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Close_Click);
            // 
            // Button_Pause
            // 
            this.Button_Pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Pause.Location = new System.Drawing.Point(409, 311);
            this.Button_Pause.Name = "Button_Pause";
            this.Button_Pause.Size = new System.Drawing.Size(75, 23);
            this.Button_Pause.TabIndex = 3;
            this.Button_Pause.Text = "Pause";
            this.Button_Pause.UseVisualStyleBackColor = true;
            this.Button_Pause.Click += new System.EventHandler(this.Button_Pause_Click);
            // 
            // Label_Progress
            // 
            this.Label_Progress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Progress.AutoSize = true;
            this.Label_Progress.Location = new System.Drawing.Point(12, 157);
            this.Label_Progress.Name = "Label_Progress";
            this.Label_Progress.Size = new System.Drawing.Size(75, 13);
            this.Label_Progress.TabIndex = 4;
            this.Label_Progress.Text = "Total Progress";
            // 
            // Label_Current
            // 
            this.Label_Current.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Current.AutoSize = true;
            this.Label_Current.Location = new System.Drawing.Point(12, 121);
            this.Label_Current.Name = "Label_Current";
            this.Label_Current.Size = new System.Drawing.Size(71, 13);
            this.Label_Current.TabIndex = 5;
            this.Label_Current.Text = "Current Video";
            // 
            // TextBox_Current
            // 
            this.TextBox_Current.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Current.Location = new System.Drawing.Point(89, 118);
            this.TextBox_Current.Name = "TextBox_Current";
            this.TextBox_Current.ReadOnly = true;
            this.TextBox_Current.Size = new System.Drawing.Size(395, 20);
            this.TextBox_Current.TabIndex = 6;
            // 
            // ProgressBar_Extraction
            // 
            this.ProgressBar_Extraction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar_Extraction.Location = new System.Drawing.Point(12, 219);
            this.ProgressBar_Extraction.Name = "ProgressBar_Extraction";
            this.ProgressBar_Extraction.Size = new System.Drawing.Size(472, 23);
            this.ProgressBar_Extraction.TabIndex = 8;
            // 
            // Label_Extraction
            // 
            this.Label_Extraction.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Extraction.AutoSize = true;
            this.Label_Extraction.Location = new System.Drawing.Point(12, 203);
            this.Label_Extraction.Name = "Label_Extraction";
            this.Label_Extraction.Size = new System.Drawing.Size(54, 13);
            this.Label_Extraction.TabIndex = 9;
            this.Label_Extraction.Text = "Extraction";
            // 
            // ProgressBar_Scanning
            // 
            this.ProgressBar_Scanning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar_Scanning.Location = new System.Drawing.Point(12, 265);
            this.ProgressBar_Scanning.Name = "ProgressBar_Scanning";
            this.ProgressBar_Scanning.Size = new System.Drawing.Size(472, 23);
            this.ProgressBar_Scanning.TabIndex = 10;
            // 
            // Label_Extraction_Value
            // 
            this.Label_Extraction_Value.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Extraction_Value.Location = new System.Drawing.Point(384, 200);
            this.Label_Extraction_Value.Name = "Label_Extraction_Value";
            this.Label_Extraction_Value.Size = new System.Drawing.Size(100, 18);
            this.Label_Extraction_Value.TabIndex = 11;
            this.Label_Extraction_Value.Text = "0 / 0";
            this.Label_Extraction_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Scanning
            // 
            this.Label_Scanning.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Scanning.AutoSize = true;
            this.Label_Scanning.Location = new System.Drawing.Point(12, 249);
            this.Label_Scanning.Name = "Label_Scanning";
            this.Label_Scanning.Size = new System.Drawing.Size(52, 13);
            this.Label_Scanning.TabIndex = 12;
            this.Label_Scanning.Text = "Scanning";
            // 
            // Label_Scanning_Value
            // 
            this.Label_Scanning_Value.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Scanning_Value.Location = new System.Drawing.Point(384, 246);
            this.Label_Scanning_Value.Name = "Label_Scanning_Value";
            this.Label_Scanning_Value.Size = new System.Drawing.Size(100, 18);
            this.Label_Scanning_Value.TabIndex = 13;
            this.Label_Scanning_Value.Text = "0 / 0";
            this.Label_Scanning_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Speed
            // 
            this.Label_Speed.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Speed.AutoSize = true;
            this.Label_Speed.Location = new System.Drawing.Point(239, 4);
            this.Label_Speed.Name = "Label_Speed";
            this.Label_Speed.Size = new System.Drawing.Size(41, 13);
            this.Label_Speed.TabIndex = 14;
            this.Label_Speed.Text = "Speed:";
            this.Label_Speed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label_Speed_Value
            // 
            this.Label_Speed_Value.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Speed_Value.AutoSize = true;
            this.Label_Speed_Value.Location = new System.Drawing.Point(436, 4);
            this.Label_Speed_Value.Name = "Label_Speed_Value";
            this.Label_Speed_Value.Size = new System.Drawing.Size(33, 13);
            this.Label_Speed_Value.TabIndex = 15;
            this.Label_Speed_Value.Text = "1.00x";
            this.Label_Speed_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Elapsed
            // 
            this.Label_Elapsed.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Elapsed.AutoSize = true;
            this.Label_Elapsed.Location = new System.Drawing.Point(3, 4);
            this.Label_Elapsed.Name = "Label_Elapsed";
            this.Label_Elapsed.Size = new System.Drawing.Size(74, 13);
            this.Label_Elapsed.TabIndex = 16;
            this.Label_Elapsed.Text = "Time Elapsed:";
            // 
            // Label_Remaining
            // 
            this.Label_Remaining.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Remaining.AutoSize = true;
            this.Label_Remaining.Location = new System.Drawing.Point(3, 25);
            this.Label_Remaining.Name = "Label_Remaining";
            this.Label_Remaining.Size = new System.Drawing.Size(86, 13);
            this.Label_Remaining.TabIndex = 17;
            this.Label_Remaining.Text = "Time Remaining:";
            // 
            // Label_Estimated
            // 
            this.Label_Estimated.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Estimated.AutoSize = true;
            this.Label_Estimated.Location = new System.Drawing.Point(3, 46);
            this.Label_Estimated.Name = "Label_Estimated";
            this.Label_Estimated.Size = new System.Drawing.Size(82, 13);
            this.Label_Estimated.TabIndex = 18;
            this.Label_Estimated.Text = "Estimated Time:";
            // 
            // TableLayoutPanel_Top
            // 
            this.TableLayoutPanel_Top.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TableLayoutPanel_Top.ColumnCount = 4;
            this.TableLayoutPanel_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Elapsed, 0, 0);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Estimated, 0, 2);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Speed, 2, 0);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Remaining, 0, 1);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Speed_Value, 3, 0);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Videos_Processed, 0, 3);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Frames_Processed, 2, 1);
            this.TableLayoutPanel_Top.Controls.Add(this.label15, 2, 2);
            this.TableLayoutPanel_Top.Controls.Add(this.label16, 2, 3);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Frames_Processed_Value, 3, 1);
            this.TableLayoutPanel_Top.Controls.Add(this.label18, 3, 2);
            this.TableLayoutPanel_Top.Controls.Add(this.label19, 3, 3);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Elapsed_Value, 1, 0);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Remaining_Value, 1, 1);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Estimated_Value, 1, 2);
            this.TableLayoutPanel_Top.Controls.Add(this.Label_Videos_Processed_Value, 1, 3);
            this.TableLayoutPanel_Top.Location = new System.Drawing.Point(12, 12);
            this.TableLayoutPanel_Top.Name = "TableLayoutPanel_Top";
            this.TableLayoutPanel_Top.RowCount = 4;
            this.TableLayoutPanel_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel_Top.Size = new System.Drawing.Size(472, 84);
            this.TableLayoutPanel_Top.TabIndex = 19;
            // 
            // Label_Videos_Processed
            // 
            this.Label_Videos_Processed.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Videos_Processed.AutoSize = true;
            this.Label_Videos_Processed.Location = new System.Drawing.Point(3, 67);
            this.Label_Videos_Processed.Name = "Label_Videos_Processed";
            this.Label_Videos_Processed.Size = new System.Drawing.Size(95, 13);
            this.Label_Videos_Processed.TabIndex = 19;
            this.Label_Videos_Processed.Text = "Videos Processed:";
            // 
            // Label_Frames_Processed
            // 
            this.Label_Frames_Processed.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label_Frames_Processed.AutoSize = true;
            this.Label_Frames_Processed.Location = new System.Drawing.Point(239, 25);
            this.Label_Frames_Processed.Name = "Label_Frames_Processed";
            this.Label_Frames_Processed.Size = new System.Drawing.Size(97, 13);
            this.Label_Frames_Processed.TabIndex = 20;
            this.Label_Frames_Processed.Text = "Frames Processed:";
            this.Label_Frames_Processed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(239, 46);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 13);
            this.label15.TabIndex = 21;
            this.label15.Text = "Unused:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label15.Visible = false;
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(239, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 13);
            this.label16.TabIndex = 22;
            this.label16.Text = "Unused:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label16.Visible = false;
            // 
            // Label_Frames_Processed_Value
            // 
            this.Label_Frames_Processed_Value.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Frames_Processed_Value.AutoSize = true;
            this.Label_Frames_Processed_Value.Location = new System.Drawing.Point(456, 25);
            this.Label_Frames_Processed_Value.Name = "Label_Frames_Processed_Value";
            this.Label_Frames_Processed_Value.Size = new System.Drawing.Size(13, 13);
            this.Label_Frames_Processed_Value.TabIndex = 23;
            this.Label_Frames_Processed_Value.Text = "0";
            this.Label_Frames_Processed_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(425, 46);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(44, 13);
            this.label18.TabIndex = 24;
            this.label18.Text = "Unused";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label18.Visible = false;
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(425, 67);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(44, 13);
            this.label19.TabIndex = 25;
            this.label19.Text = "Unused";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label19.Visible = false;
            // 
            // Label_Elapsed_Value
            // 
            this.Label_Elapsed_Value.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Elapsed_Value.AutoSize = true;
            this.Label_Elapsed_Value.Location = new System.Drawing.Point(184, 4);
            this.Label_Elapsed_Value.Name = "Label_Elapsed_Value";
            this.Label_Elapsed_Value.Size = new System.Drawing.Size(49, 13);
            this.Label_Elapsed_Value.TabIndex = 26;
            this.Label_Elapsed_Value.Text = "00:00:00";
            this.Label_Elapsed_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Remaining_Value
            // 
            this.Label_Remaining_Value.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Remaining_Value.AutoSize = true;
            this.Label_Remaining_Value.Location = new System.Drawing.Point(184, 25);
            this.Label_Remaining_Value.Name = "Label_Remaining_Value";
            this.Label_Remaining_Value.Size = new System.Drawing.Size(49, 13);
            this.Label_Remaining_Value.TabIndex = 27;
            this.Label_Remaining_Value.Text = "00:00:00";
            this.Label_Remaining_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Estimated_Value
            // 
            this.Label_Estimated_Value.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Estimated_Value.AutoSize = true;
            this.Label_Estimated_Value.Location = new System.Drawing.Point(184, 46);
            this.Label_Estimated_Value.Name = "Label_Estimated_Value";
            this.Label_Estimated_Value.Size = new System.Drawing.Size(49, 13);
            this.Label_Estimated_Value.TabIndex = 28;
            this.Label_Estimated_Value.Text = "00:00:00";
            this.Label_Estimated_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Videos_Processed_Value
            // 
            this.Label_Videos_Processed_Value.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Videos_Processed_Value.AutoSize = true;
            this.Label_Videos_Processed_Value.Location = new System.Drawing.Point(203, 67);
            this.Label_Videos_Processed_Value.Name = "Label_Videos_Processed_Value";
            this.Label_Videos_Processed_Value.Size = new System.Drawing.Size(30, 13);
            this.Label_Videos_Processed_Value.TabIndex = 29;
            this.Label_Videos_Processed_Value.Text = "0 / 0";
            this.Label_Videos_Processed_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Watch_Ticker
            // 
            this.Watch_Ticker.Enabled = true;
            this.Watch_Ticker.Interval = 1000;
            this.Watch_Ticker.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // FileSystemWatcher
            // 
            this.FileSystemWatcher.EnableRaisingEvents = true;
            this.FileSystemWatcher.Filter = "*.bmp";
            this.FileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.FileName;
            this.FileSystemWatcher.Path = "C:\\";
            this.FileSystemWatcher.SynchronizingObject = this;
            this.FileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.FileSystemWatcher_Created);
            // 
            // Label_Notice
            // 
            this.Label_Notice.AutoSize = true;
            this.Label_Notice.Location = new System.Drawing.Point(137, 298);
            this.Label_Notice.Name = "Label_Notice";
            this.Label_Notice.Size = new System.Drawing.Size(161, 39);
            this.Label_Notice.TabIndex = 20;
            this.Label_Notice.Text = "Post-Processing features are\r\ncoming. For now, you can export\r\nthe raw output.";
            this.Label_Notice.Visible = false;
            // 
            // CheckBox_Compact
            // 
            this.CheckBox_Compact.AutoSize = true;
            this.CheckBox_Compact.Checked = true;
            this.CheckBox_Compact.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox_Compact.Location = new System.Drawing.Point(335, 307);
            this.CheckBox_Compact.Name = "CheckBox_Compact";
            this.CheckBox_Compact.Size = new System.Drawing.Size(68, 30);
            this.CheckBox_Compact.TabIndex = 21;
            this.CheckBox_Compact.Text = "Compact\r\nData";
            this.CheckBox_Compact.UseVisualStyleBackColor = true;
            this.CheckBox_Compact.Visible = false;
            // 
            // Processing
            // 
            this.AcceptButton = this.Button_Pause;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Button_Cancel;
            this.ClientSize = new System.Drawing.Size(496, 346);
            this.Controls.Add(this.CheckBox_Compact);
            this.Controls.Add(this.Label_Notice);
            this.Controls.Add(this.TableLayoutPanel_Top);
            this.Controls.Add(this.Label_Scanning_Value);
            this.Controls.Add(this.Label_Scanning);
            this.Controls.Add(this.Label_Extraction_Value);
            this.Controls.Add(this.ProgressBar_Scanning);
            this.Controls.Add(this.Label_Extraction);
            this.Controls.Add(this.ProgressBar_Extraction);
            this.Controls.Add(this.TextBox_Current);
            this.Controls.Add(this.Label_Current);
            this.Controls.Add(this.Label_Progress);
            this.Controls.Add(this.Button_Pause);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.ProgressBar_Total);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(8192, 384);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(512, 384);
            this.Name = "Processing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Load += new System.EventHandler(this.Form_Loaded);
            this.TableLayoutPanel_Top.ResumeLayout(false);
            this.TableLayoutPanel_Top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileSystemWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.Button Button_Pause;
        private System.Windows.Forms.Label Label_Progress;
        private System.Windows.Forms.Label Label_Current;
        private System.Windows.Forms.TextBox TextBox_Current;
        private System.Windows.Forms.ProgressBar ProgressBar_Extraction;
        private System.Windows.Forms.Label Label_Extraction;
        private System.Windows.Forms.ProgressBar ProgressBar_Scanning;
        private System.Windows.Forms.Label Label_Extraction_Value;
        private System.Windows.Forms.Label Label_Scanning;
        private System.Windows.Forms.Label Label_Scanning_Value;
        private System.Windows.Forms.Label Label_Speed;
        private System.Windows.Forms.Label Label_Speed_Value;
        private System.Windows.Forms.Label Label_Elapsed;
        private System.Windows.Forms.Label Label_Remaining;
        private System.Windows.Forms.Label Label_Estimated;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel_Top;
        private System.Windows.Forms.Label Label_Videos_Processed;
        private System.Windows.Forms.Label Label_Frames_Processed;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label Label_Frames_Processed_Value;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label Label_Elapsed_Value;
        private System.Windows.Forms.Label Label_Remaining_Value;
        private System.Windows.Forms.Label Label_Estimated_Value;
        private System.Windows.Forms.Label Label_Videos_Processed_Value;
        private System.Windows.Forms.ProgressBar ProgressBar_Total;
        private System.Windows.Forms.Timer Watch_Ticker;
        private System.IO.FileSystemWatcher FileSystemWatcher;
        private System.Windows.Forms.CheckBox CheckBox_Compact;
        private System.Windows.Forms.Label Label_Notice;
    }
}