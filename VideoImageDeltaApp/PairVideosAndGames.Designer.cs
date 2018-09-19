namespace VideoImageDeltaApp
{
    partial class PairVideosAndGames
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
            this.Button_Close = new System.Windows.Forms.Button();
            this.ListView_Main = new System.Windows.Forms.ListView();
            this.ColumnFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnFeeds = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnGameProfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnIsSynced = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this.Panel_GameProfile = new System.Windows.Forms.Panel();
            this.CheckBox_Auto = new System.Windows.Forms.CheckBox();
            this.Button_Pair_VandG = new System.Windows.Forms.Button();
            this.Label_GameProfile = new System.Windows.Forms.Label();
            this.ComboBox_GameProfile = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Panel_FeednScreen = new System.Windows.Forms.Panel();
            this.Button_AutoAlign = new System.Windows.Forms.Button();
            this.Button_Pair_FandS = new System.Windows.Forms.Button();
            this.Label_Feed = new System.Windows.Forms.Label();
            this.Label_Screen = new System.Windows.Forms.Label();
            this.ComboBox_Screen = new System.Windows.Forms.ComboBox();
            this.ListBox_Feeds = new System.Windows.Forms.ListBox();
            this.TableLayoutPanel_Main.SuspendLayout();
            this.Panel_GameProfile.SuspendLayout();
            this.Panel_FeednScreen.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Help
            // 
            this.Button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Help.Location = new System.Drawing.Point(616, 527);
            this.Button_Help.Name = "Button_Help";
            this.Button_Help.Size = new System.Drawing.Size(75, 23);
            this.Button_Help.TabIndex = 205;
            this.Button_Help.Text = "Help";
            this.Button_Help.UseVisualStyleBackColor = true;
            // 
            // Button_Close
            // 
            this.Button_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Close.AutoSize = true;
            this.Button_Close.Location = new System.Drawing.Point(697, 527);
            this.Button_Close.Name = "Button_Close";
            this.Button_Close.Size = new System.Drawing.Size(75, 23);
            this.Button_Close.TabIndex = 206;
            this.Button_Close.Text = "Close";
            this.Button_Close.UseVisualStyleBackColor = true;
            this.Button_Close.Click += new System.EventHandler(this.Button_Close_Click);
            // 
            // ListView_Main
            // 
            this.ListView_Main.AllowColumnReorder = true;
            this.ListView_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListView_Main.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnFilePath,
            this.ColumnFeeds,
            this.ColumnGameProfile,
            this.ColumnIsSynced});
            this.ListView_Main.FullRowSelect = true;
            this.ListView_Main.HideSelection = false;
            this.ListView_Main.Location = new System.Drawing.Point(3, 3);
            this.ListView_Main.Name = "ListView_Main";
            this.ListView_Main.ShowItemToolTips = true;
            this.ListView_Main.Size = new System.Drawing.Size(604, 146);
            this.ListView_Main.TabIndex = 2;
            this.ListView_Main.UseCompatibleStateImageBehavior = false;
            this.ListView_Main.View = System.Windows.Forms.View.Details;
            this.ListView_Main.SelectedIndexChanged += new System.EventHandler(this.ListView_Main_SelectedIndexChanged);
            // 
            // ColumnFilePath
            // 
            this.ColumnFilePath.Text = "File Path";
            this.ColumnFilePath.Width = 255;
            // 
            // ColumnFeeds
            // 
            this.ColumnFeeds.Text = "Feeds";
            this.ColumnFeeds.Width = 128;
            // 
            // ColumnGameProfile
            // 
            this.ColumnGameProfile.Text = "Game Profile";
            this.ColumnGameProfile.Width = 96;
            // 
            // ColumnIsSynced
            // 
            this.ColumnIsSynced.Text = "Is Synced";
            this.ColumnIsSynced.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TableLayoutPanel_Main
            // 
            this.TableLayoutPanel_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel_Main.ColumnCount = 2;
            this.TableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.TableLayoutPanel_Main.Controls.Add(this.ListView_Main, 0, 0);
            this.TableLayoutPanel_Main.Controls.Add(this.Panel_GameProfile, 1, 0);
            this.TableLayoutPanel_Main.Controls.Add(this.panel2, 0, 1);
            this.TableLayoutPanel_Main.Controls.Add(this.Panel_FeednScreen, 1, 1);
            this.TableLayoutPanel_Main.Location = new System.Drawing.Point(12, 12);
            this.TableLayoutPanel_Main.Name = "TableLayoutPanel_Main";
            this.TableLayoutPanel_Main.RowCount = 2;
            this.TableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.TableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.TableLayoutPanel_Main.Size = new System.Drawing.Size(760, 509);
            this.TableLayoutPanel_Main.TabIndex = 3;
            // 
            // Panel_GameProfile
            // 
            this.Panel_GameProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_GameProfile.Controls.Add(this.CheckBox_Auto);
            this.Panel_GameProfile.Controls.Add(this.Button_Pair_VandG);
            this.Panel_GameProfile.Controls.Add(this.Label_GameProfile);
            this.Panel_GameProfile.Controls.Add(this.ComboBox_GameProfile);
            this.Panel_GameProfile.Location = new System.Drawing.Point(613, 3);
            this.Panel_GameProfile.Name = "Panel_GameProfile";
            this.Panel_GameProfile.Size = new System.Drawing.Size(144, 146);
            this.Panel_GameProfile.TabIndex = 4;
            // 
            // CheckBox_Auto
            // 
            this.CheckBox_Auto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CheckBox_Auto.AutoSize = true;
            this.CheckBox_Auto.Checked = true;
            this.CheckBox_Auto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox_Auto.Location = new System.Drawing.Point(9, 12);
            this.CheckBox_Auto.Name = "CheckBox_Auto";
            this.CheckBox_Auto.Size = new System.Drawing.Size(128, 30);
            this.CheckBox_Auto.TabIndex = 3;
            this.CheckBox_Auto.Text = "Auto-match Feed and\r\nScreen by name";
            this.CheckBox_Auto.UseVisualStyleBackColor = true;
            // 
            // Button_Pair_VandG
            // 
            this.Button_Pair_VandG.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button_Pair_VandG.Enabled = false;
            this.Button_Pair_VandG.Location = new System.Drawing.Point(34, 94);
            this.Button_Pair_VandG.Name = "Button_Pair_VandG";
            this.Button_Pair_VandG.Size = new System.Drawing.Size(75, 23);
            this.Button_Pair_VandG.TabIndex = 2;
            this.Button_Pair_VandG.Text = "Pair";
            this.Button_Pair_VandG.UseVisualStyleBackColor = true;
            this.Button_Pair_VandG.Click += new System.EventHandler(this.Button_Pair_VandG_Click);
            // 
            // Label_GameProfile
            // 
            this.Label_GameProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_GameProfile.AutoSize = true;
            this.Label_GameProfile.Location = new System.Drawing.Point(38, 51);
            this.Label_GameProfile.Name = "Label_GameProfile";
            this.Label_GameProfile.Size = new System.Drawing.Size(67, 13);
            this.Label_GameProfile.TabIndex = 1;
            this.Label_GameProfile.Text = "Game Profile";
            // 
            // ComboBox_GameProfile
            // 
            this.ComboBox_GameProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComboBox_GameProfile.FormattingEnabled = true;
            this.ComboBox_GameProfile.Location = new System.Drawing.Point(12, 67);
            this.ComboBox_GameProfile.Name = "ComboBox_GameProfile";
            this.ComboBox_GameProfile.Size = new System.Drawing.Size(120, 21);
            this.ComboBox_GameProfile.TabIndex = 0;
            this.ComboBox_GameProfile.SelectedIndexChanged += new System.EventHandler(this.ComboBox_GameProfile_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Location = new System.Drawing.Point(3, 155);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(604, 351);
            this.panel2.TabIndex = 5;
            // 
            // Panel_FeednScreen
            // 
            this.Panel_FeednScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_FeednScreen.Controls.Add(this.Button_AutoAlign);
            this.Panel_FeednScreen.Controls.Add(this.Button_Pair_FandS);
            this.Panel_FeednScreen.Controls.Add(this.Label_Feed);
            this.Panel_FeednScreen.Controls.Add(this.Label_Screen);
            this.Panel_FeednScreen.Controls.Add(this.ComboBox_Screen);
            this.Panel_FeednScreen.Controls.Add(this.ListBox_Feeds);
            this.Panel_FeednScreen.Location = new System.Drawing.Point(613, 155);
            this.Panel_FeednScreen.Name = "Panel_FeednScreen";
            this.Panel_FeednScreen.Size = new System.Drawing.Size(144, 351);
            this.Panel_FeednScreen.TabIndex = 6;
            // 
            // Button_AutoAlign
            // 
            this.Button_AutoAlign.Location = new System.Drawing.Point(34, 258);
            this.Button_AutoAlign.Name = "Button_AutoAlign";
            this.Button_AutoAlign.Size = new System.Drawing.Size(75, 23);
            this.Button_AutoAlign.TabIndex = 5;
            this.Button_AutoAlign.Text = "Auto-Align";
            this.Button_AutoAlign.UseVisualStyleBackColor = true;
            // 
            // Button_Pair_FandS
            // 
            this.Button_Pair_FandS.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button_Pair_FandS.Location = new System.Drawing.Point(34, 173);
            this.Button_Pair_FandS.Name = "Button_Pair_FandS";
            this.Button_Pair_FandS.Size = new System.Drawing.Size(75, 23);
            this.Button_Pair_FandS.TabIndex = 4;
            this.Button_Pair_FandS.Text = "Pair";
            this.Button_Pair_FandS.UseVisualStyleBackColor = true;
            // 
            // Label_Feed
            // 
            this.Label_Feed.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_Feed.AutoSize = true;
            this.Label_Feed.Location = new System.Drawing.Point(56, 6);
            this.Label_Feed.Name = "Label_Feed";
            this.Label_Feed.Size = new System.Drawing.Size(31, 13);
            this.Label_Feed.TabIndex = 4;
            this.Label_Feed.Text = "Feed";
            // 
            // Label_Screen
            // 
            this.Label_Screen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_Screen.AutoSize = true;
            this.Label_Screen.Location = new System.Drawing.Point(51, 130);
            this.Label_Screen.Name = "Label_Screen";
            this.Label_Screen.Size = new System.Drawing.Size(41, 13);
            this.Label_Screen.TabIndex = 3;
            this.Label_Screen.Text = "Screen";
            // 
            // ComboBox_Screen
            // 
            this.ComboBox_Screen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComboBox_Screen.FormattingEnabled = true;
            this.ComboBox_Screen.Location = new System.Drawing.Point(12, 146);
            this.ComboBox_Screen.Name = "ComboBox_Screen";
            this.ComboBox_Screen.Size = new System.Drawing.Size(120, 21);
            this.ComboBox_Screen.TabIndex = 2;
            // 
            // ListBox_Feeds
            // 
            this.ListBox_Feeds.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ListBox_Feeds.FormattingEnabled = true;
            this.ListBox_Feeds.Location = new System.Drawing.Point(3, 23);
            this.ListBox_Feeds.Name = "ListBox_Feeds";
            this.ListBox_Feeds.Size = new System.Drawing.Size(138, 95);
            this.ListBox_Feeds.TabIndex = 0;
            // 
            // PairVideosAndGames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.TableLayoutPanel_Main);
            this.Controls.Add(this.Button_Help);
            this.Controls.Add(this.Button_Close);
            this.Name = "PairVideosAndGames";
            this.Text = "Pair Videos and Game Profiles";
            this.TableLayoutPanel_Main.ResumeLayout(false);
            this.Panel_GameProfile.ResumeLayout(false);
            this.Panel_GameProfile.PerformLayout();
            this.Panel_FeednScreen.ResumeLayout(false);
            this.Panel_FeednScreen.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Button_Help;
        private System.Windows.Forms.Button Button_Close;
        private System.Windows.Forms.ListView ListView_Main;
        private System.Windows.Forms.ColumnHeader ColumnFilePath;
        private System.Windows.Forms.ColumnHeader ColumnFeeds;
        private System.Windows.Forms.ColumnHeader ColumnGameProfile;
        private System.Windows.Forms.ColumnHeader ColumnIsSynced;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel_Main;
        private System.Windows.Forms.Panel Panel_GameProfile;
        private System.Windows.Forms.Button Button_Pair_VandG;
        private System.Windows.Forms.Label Label_GameProfile;
        private System.Windows.Forms.ComboBox ComboBox_GameProfile;
        private System.Windows.Forms.CheckBox CheckBox_Auto;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel Panel_FeednScreen;
        private System.Windows.Forms.Button Button_Pair_FandS;
        private System.Windows.Forms.Label Label_Feed;
        private System.Windows.Forms.Label Label_Screen;
        private System.Windows.Forms.ComboBox ComboBox_Screen;
        private System.Windows.Forms.ListBox ListBox_Feeds;
        private System.Windows.Forms.Button Button_AutoAlign;
    }
}