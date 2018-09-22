using System.Drawing;
using System.Windows.Forms;

namespace VideoImageDeltaApp.Forms
{
    partial class MainWindow
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
            this.Button_Help = new System.Windows.Forms.Button();
            this.Button_Quit = new System.Windows.Forms.Button();
            this.Button_Export = new System.Windows.Forms.Button();
            this.Button_Import = new System.Windows.Forms.Button();
            this.Button_Process = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Button_Videos = new System.Windows.Forms.Button();
            this.Button_Watch = new System.Windows.Forms.Button();
            this.ListView_Main = new System.Windows.Forms.ListView();
            this.ColumnVideo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnGameProfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Help
            // 
            this.Button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Help.Location = new System.Drawing.Point(456, 407);
            this.Button_Help.Name = "Button_Help";
            this.Button_Help.Size = new System.Drawing.Size(75, 23);
            this.Button_Help.TabIndex = 101;
            this.Button_Help.Text = "Help";
            this.Button_Help.UseVisualStyleBackColor = true;
            this.Button_Help.Click += new System.EventHandler(this.Button_Help_Click);
            // 
            // Button_Quit
            // 
            this.Button_Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Quit.AutoSize = true;
            this.Button_Quit.Location = new System.Drawing.Point(537, 407);
            this.Button_Quit.Name = "Button_Quit";
            this.Button_Quit.Size = new System.Drawing.Size(75, 23);
            this.Button_Quit.TabIndex = 102;
            this.Button_Quit.Text = "Quit";
            this.Button_Quit.UseVisualStyleBackColor = true;
            this.Button_Quit.Click += new System.EventHandler(this.Button_Quit_Click);
            // 
            // Button_Export
            // 
            this.Button_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Export.AutoSize = true;
            this.Button_Export.Location = new System.Drawing.Point(118, 407);
            this.Button_Export.Name = "Button_Export";
            this.Button_Export.Size = new System.Drawing.Size(100, 23);
            this.Button_Export.TabIndex = 99;
            this.Button_Export.Text = "Export Settings";
            this.Button_Export.UseVisualStyleBackColor = true;
            this.Button_Export.Click += new System.EventHandler(this.Button_Export_Click);
            // 
            // Button_Import
            // 
            this.Button_Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Import.AutoSize = true;
            this.Button_Import.Location = new System.Drawing.Point(12, 407);
            this.Button_Import.Name = "Button_Import";
            this.Button_Import.Size = new System.Drawing.Size(100, 23);
            this.Button_Import.TabIndex = 98;
            this.Button_Import.Text = "Import Settings";
            this.Button_Import.UseVisualStyleBackColor = true;
            this.Button_Import.Click += new System.EventHandler(this.Button_Import_Click);
            // 
            // Button_Process
            // 
            this.Button_Process.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Button_Process.AutoSize = true;
            this.Button_Process.Location = new System.Drawing.Point(250, 407);
            this.Button_Process.Name = "Button_Process";
            this.Button_Process.Size = new System.Drawing.Size(125, 23);
            this.Button_Process.TabIndex = 100;
            this.Button_Process.Text = "Process";
            this.Button_Process.UseVisualStyleBackColor = true;
            this.Button_Process.Click += new System.EventHandler(this.Button_Process_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.Button_Videos, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Button_Watch, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(600, 29);
            this.tableLayoutPanel1.TabIndex = 103;
            // 
            // Button_Videos
            // 
            this.Button_Videos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Videos.Location = new System.Drawing.Point(303, 3);
            this.Button_Videos.Name = "Button_Videos";
            this.Button_Videos.Size = new System.Drawing.Size(294, 23);
            this.Button_Videos.TabIndex = 0;
            this.Button_Videos.Text = "Add Videos";
            this.Button_Videos.UseVisualStyleBackColor = true;
            this.Button_Videos.Click += new System.EventHandler(this.Button_Videos_Click);
            // 
            // Button_Watch
            // 
            this.Button_Watch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Watch.Location = new System.Drawing.Point(3, 3);
            this.Button_Watch.Name = "Button_Watch";
            this.Button_Watch.Size = new System.Drawing.Size(294, 23);
            this.Button_Watch.TabIndex = 2;
            this.Button_Watch.Text = "Add Watchers";
            this.Button_Watch.UseVisualStyleBackColor = true;
            this.Button_Watch.Click += new System.EventHandler(this.Button_Watch_Click);
            // 
            // ListView_Main
            // 
            this.ListView_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListView_Main.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnVideo,
            this.ColumnGameProfile});
            this.ListView_Main.FullRowSelect = true;
            this.ListView_Main.GridLines = true;
            this.ListView_Main.Location = new System.Drawing.Point(12, 44);
            this.ListView_Main.Name = "ListView_Main";
            this.ListView_Main.Size = new System.Drawing.Size(600, 354);
            this.ListView_Main.TabIndex = 3;
            this.ListView_Main.UseCompatibleStateImageBehavior = false;
            this.ListView_Main.View = System.Windows.Forms.View.Details;
            // 
            // ColumnVideo
            // 
            this.ColumnVideo.Width = 360;
            // 
            // ColumnGameProfile
            // 
            this.ColumnGameProfile.Text = "Game Profile";
            this.ColumnGameProfile.Width = 120;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.ListView_Main);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.Button_Help);
            this.Controls.Add(this.Button_Process);
            this.Controls.Add(this.Button_Import);
            this.Controls.Add(this.Button_Export);
            this.Controls.Add(this.Button_Quit);
            this.MinimumSize = new System.Drawing.Size(600, 200);
            this.Name = "MainWindow";
            this.Text = "Video Image Delta";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button Button_Process;
        private Button Button_Import;
        private Button Button_Export;
        private Button Button_Quit;
        private Button Button_Help;
        private TableLayoutPanel tableLayoutPanel1;
        private Button Button_Videos;
        private Button Button_Watch;
        private ListView ListView_Main;
        private ColumnHeader ColumnVideo;
        private ColumnHeader ColumnGameProfile;
    }
}

