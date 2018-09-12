namespace VideoImageDeltaApp
{
    partial class AddVideos
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
            this.Button_Import = new System.Windows.Forms.Button();
            this.Button_Export = new System.Windows.Forms.Button();
            this.Button_Close = new System.Windows.Forms.Button();
            this.TableLayoutPanel_Window = new System.Windows.Forms.TableLayoutPanel();
            this.Panel_Core = new System.Windows.Forms.Panel();
            this.TableLayoutPanel_Single = new System.Windows.Forms.TableLayoutPanel();
            this.Panel_Input = new System.Windows.Forms.Panel();
            this.CheckBox_Perfect = new System.Windows.Forms.CheckBox();
            this.CheckBox_Timer = new System.Windows.Forms.CheckBox();
            this.Button_Add_Feed = new System.Windows.Forms.Button();
            this.Numeric_Height = new System.Windows.Forms.NumericUpDown();
            this.Numeric_Width = new System.Windows.Forms.NumericUpDown();
            this.Numeric_Y = new System.Windows.Forms.NumericUpDown();
            this.Numeric_X = new System.Windows.Forms.NumericUpDown();
            this.Label_Y = new System.Windows.Forms.Label();
            this.Label_Height = new System.Windows.Forms.Label();
            this.Label_X = new System.Windows.Forms.Label();
            this.Label_Width = new System.Windows.Forms.Label();
            this.TextBox_Name = new System.Windows.Forms.TextBox();
            this.Label_Name = new System.Windows.Forms.Label();
            this.Panel_Feeds = new System.Windows.Forms.Panel();
            this.Label_Timestamp = new System.Windows.Forms.Label();
            this.TextBox_Timestamp = new System.Windows.Forms.TextBox();
            this.ListBox_Feeds = new System.Windows.Forms.ListBox();
            this.Button_Minus_Feeds = new System.Windows.Forms.Button();
            this.Label_Feeds = new System.Windows.Forms.Label();
            this.TrackBar_Thumbnail = new System.Windows.Forms.TrackBar();
            this.Panel_Back = new System.Windows.Forms.Panel();
            this.Box_Main = new System.Windows.Forms.Panel();
            this.Box_Preview = new System.Windows.Forms.Panel();
            this.Label_Failed = new System.Windows.Forms.Label();
            this.Button_Add_Videos = new System.Windows.Forms.Button();
            this.ListView_Main = new System.Windows.Forms.ListView();
            this.VideoFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VideoFrameRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VideoWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VideoHeight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VideoDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VideoFeeds = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Panel_Top = new System.Windows.Forms.Panel();
            this.CheckBox_Advanced = new System.Windows.Forms.CheckBox();
            this.Numeric_Game_Height = new System.Windows.Forms.NumericUpDown();
            this.Numeric_Game_Width = new System.Windows.Forms.NumericUpDown();
            this.Label_Game_Height = new System.Windows.Forms.Label();
            this.Label_Game_Width = new System.Windows.Forms.Label();
            this.TableLayoutPanel_Window.SuspendLayout();
            this.Panel_Core.SuspendLayout();
            this.TableLayoutPanel_Single.SuspendLayout();
            this.Panel_Input.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_X)).BeginInit();
            this.Panel_Feeds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_Thumbnail)).BeginInit();
            this.Panel_Back.SuspendLayout();
            this.Box_Main.SuspendLayout();
            this.Panel_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Game_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Game_Width)).BeginInit();
            this.SuspendLayout();
            // 
            // Button_Help
            // 
            this.Button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Help.Location = new System.Drawing.Point(456, 447);
            this.Button_Help.Name = "Button_Help";
            this.Button_Help.Size = new System.Drawing.Size(75, 23);
            this.Button_Help.TabIndex = 203;
            this.Button_Help.Text = "Help";
            this.Button_Help.UseVisualStyleBackColor = true;
            this.Button_Help.Click += new System.EventHandler(this.Button_Help_Click);
            // 
            // Button_Import
            // 
            this.Button_Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Import.AutoSize = true;
            this.Button_Import.Location = new System.Drawing.Point(12, 447);
            this.Button_Import.Name = "Button_Import";
            this.Button_Import.Size = new System.Drawing.Size(100, 23);
            this.Button_Import.TabIndex = 201;
            this.Button_Import.Text = "Apply Profile";
            this.Button_Import.UseVisualStyleBackColor = true;
            this.Button_Import.Click += new System.EventHandler(this.Button_Import_Click);
            // 
            // Button_Export
            // 
            this.Button_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Export.AutoSize = true;
            this.Button_Export.Location = new System.Drawing.Point(118, 447);
            this.Button_Export.Name = "Button_Export";
            this.Button_Export.Size = new System.Drawing.Size(100, 23);
            this.Button_Export.TabIndex = 202;
            this.Button_Export.Text = "Export as Profile";
            this.Button_Export.UseVisualStyleBackColor = true;
            this.Button_Export.Click += new System.EventHandler(this.Button_Export_Click);
            // 
            // Button_Close
            // 
            this.Button_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Close.AutoSize = true;
            this.Button_Close.Location = new System.Drawing.Point(537, 447);
            this.Button_Close.Name = "Button_Close";
            this.Button_Close.Size = new System.Drawing.Size(75, 23);
            this.Button_Close.TabIndex = 204;
            this.Button_Close.Text = "Close";
            this.Button_Close.UseVisualStyleBackColor = true;
            this.Button_Close.Click += new System.EventHandler(this.Button_Close_Click);
            // 
            // TableLayoutPanel_Window
            // 
            this.TableLayoutPanel_Window.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel_Window.ColumnCount = 1;
            this.TableLayoutPanel_Window.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Window.Controls.Add(this.Panel_Core, 0, 2);
            this.TableLayoutPanel_Window.Controls.Add(this.ListView_Main, 0, 1);
            this.TableLayoutPanel_Window.Controls.Add(this.Panel_Top, 0, 0);
            this.TableLayoutPanel_Window.Location = new System.Drawing.Point(12, 12);
            this.TableLayoutPanel_Window.Name = "TableLayoutPanel_Window";
            this.TableLayoutPanel_Window.RowCount = 3;
            this.TableLayoutPanel_Window.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.TableLayoutPanel_Window.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.TableLayoutPanel_Window.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.TableLayoutPanel_Window.Size = new System.Drawing.Size(600, 429);
            this.TableLayoutPanel_Window.TabIndex = 209;
            // 
            // Panel_Core
            // 
            this.Panel_Core.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Core.Controls.Add(this.TableLayoutPanel_Single);
            this.Panel_Core.Location = new System.Drawing.Point(0, 162);
            this.Panel_Core.Margin = new System.Windows.Forms.Padding(0);
            this.Panel_Core.Name = "Panel_Core";
            this.Panel_Core.Size = new System.Drawing.Size(600, 267);
            this.Panel_Core.TabIndex = 210;
            // 
            // TableLayoutPanel_Single
            // 
            this.TableLayoutPanel_Single.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel_Single.ColumnCount = 3;
            this.TableLayoutPanel_Single.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.TableLayoutPanel_Single.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Single.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            this.TableLayoutPanel_Single.Controls.Add(this.Panel_Input, 2, 0);
            this.TableLayoutPanel_Single.Controls.Add(this.Panel_Feeds, 0, 0);
            this.TableLayoutPanel_Single.Controls.Add(this.Panel_Back, 1, 0);
            this.TableLayoutPanel_Single.Location = new System.Drawing.Point(3, 3);
            this.TableLayoutPanel_Single.Name = "TableLayoutPanel_Single";
            this.TableLayoutPanel_Single.RowCount = 1;
            this.TableLayoutPanel_Single.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Single.Size = new System.Drawing.Size(594, 261);
            this.TableLayoutPanel_Single.TabIndex = 214;
            // 
            // Panel_Input
            // 
            this.Panel_Input.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Input.Controls.Add(this.Numeric_Game_Height);
            this.Panel_Input.Controls.Add(this.Numeric_Game_Width);
            this.Panel_Input.Controls.Add(this.Label_Game_Height);
            this.Panel_Input.Controls.Add(this.Label_Game_Width);
            this.Panel_Input.Controls.Add(this.CheckBox_Perfect);
            this.Panel_Input.Controls.Add(this.CheckBox_Timer);
            this.Panel_Input.Controls.Add(this.Button_Add_Feed);
            this.Panel_Input.Controls.Add(this.Numeric_Height);
            this.Panel_Input.Controls.Add(this.Numeric_Width);
            this.Panel_Input.Controls.Add(this.Numeric_Y);
            this.Panel_Input.Controls.Add(this.Numeric_X);
            this.Panel_Input.Controls.Add(this.Label_Y);
            this.Panel_Input.Controls.Add(this.Label_Height);
            this.Panel_Input.Controls.Add(this.Label_X);
            this.Panel_Input.Controls.Add(this.Label_Width);
            this.Panel_Input.Controls.Add(this.TextBox_Name);
            this.Panel_Input.Controls.Add(this.Label_Name);
            this.Panel_Input.Location = new System.Drawing.Point(382, 3);
            this.Panel_Input.Name = "Panel_Input";
            this.Panel_Input.Size = new System.Drawing.Size(209, 255);
            this.Panel_Input.TabIndex = 1;
            // 
            // CheckBox_Perfect
            // 
            this.CheckBox_Perfect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CheckBox_Perfect.AutoSize = true;
            this.CheckBox_Perfect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBox_Perfect.Location = new System.Drawing.Point(44, 179);
            this.CheckBox_Perfect.Name = "CheckBox_Perfect";
            this.CheckBox_Perfect.Size = new System.Drawing.Size(106, 17);
            this.CheckBox_Perfect.TabIndex = 12;
            this.CheckBox_Perfect.Text = "Perfect Capture?";
            this.CheckBox_Perfect.UseVisualStyleBackColor = true;
            // 
            // CheckBox_Timer
            // 
            this.CheckBox_Timer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CheckBox_Timer.AutoSize = true;
            this.CheckBox_Timer.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBox_Timer.Location = new System.Drawing.Point(65, 156);
            this.CheckBox_Timer.Name = "CheckBox_Timer";
            this.CheckBox_Timer.Size = new System.Drawing.Size(85, 17);
            this.CheckBox_Timer.TabIndex = 11;
            this.CheckBox_Timer.Text = "Timer Feed?";
            this.CheckBox_Timer.UseVisualStyleBackColor = true;
            this.CheckBox_Timer.CheckedChanged += new System.EventHandler(this.CheckBox_Timer_CheckedChanged);
            // 
            // Button_Add_Feed
            // 
            this.Button_Add_Feed.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button_Add_Feed.Location = new System.Drawing.Point(75, 228);
            this.Button_Add_Feed.Name = "Button_Add_Feed";
            this.Button_Add_Feed.Size = new System.Drawing.Size(75, 23);
            this.Button_Add_Feed.TabIndex = 14;
            this.Button_Add_Feed.Text = "Add Feed";
            this.Button_Add_Feed.UseVisualStyleBackColor = true;
            this.Button_Add_Feed.Click += new System.EventHandler(this.Button_Add_Feed_Click);
            // 
            // Numeric_Height
            // 
            this.Numeric_Height.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Numeric_Height.Location = new System.Drawing.Point(120, 75);
            this.Numeric_Height.Maximum = new decimal(new int[] {
            4320,
            0,
            0,
            0});
            this.Numeric_Height.Minimum = new decimal(new int[] {
            4320,
            0,
            0,
            -2147483648});
            this.Numeric_Height.Name = "Numeric_Height";
            this.Numeric_Height.Size = new System.Drawing.Size(60, 20);
            this.Numeric_Height.TabIndex = 8;
            this.Numeric_Height.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Numeric_Height.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.Numeric_Height.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.Numeric_Height.Enter += new System.EventHandler(this.Numeric_Auto_Selector);
            // 
            // Numeric_Width
            // 
            this.Numeric_Width.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Numeric_Width.Location = new System.Drawing.Point(36, 75);
            this.Numeric_Width.Maximum = new decimal(new int[] {
            7680,
            0,
            0,
            0});
            this.Numeric_Width.Minimum = new decimal(new int[] {
            7680,
            0,
            0,
            -2147483648});
            this.Numeric_Width.Name = "Numeric_Width";
            this.Numeric_Width.Size = new System.Drawing.Size(60, 20);
            this.Numeric_Width.TabIndex = 7;
            this.Numeric_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Numeric_Width.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.Numeric_Width.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.Numeric_Width.Enter += new System.EventHandler(this.Numeric_Auto_Selector);
            // 
            // Numeric_Y
            // 
            this.Numeric_Y.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Numeric_Y.Location = new System.Drawing.Point(120, 29);
            this.Numeric_Y.Maximum = new decimal(new int[] {
            4320,
            0,
            0,
            0});
            this.Numeric_Y.Minimum = new decimal(new int[] {
            4320,
            0,
            0,
            -2147483648});
            this.Numeric_Y.Name = "Numeric_Y";
            this.Numeric_Y.Size = new System.Drawing.Size(60, 20);
            this.Numeric_Y.TabIndex = 6;
            this.Numeric_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Numeric_Y.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.Numeric_Y.Enter += new System.EventHandler(this.Numeric_Auto_Selector);
            // 
            // Numeric_X
            // 
            this.Numeric_X.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Numeric_X.Location = new System.Drawing.Point(36, 29);
            this.Numeric_X.Maximum = new decimal(new int[] {
            7680,
            0,
            0,
            0});
            this.Numeric_X.Minimum = new decimal(new int[] {
            7680,
            0,
            0,
            -2147483648});
            this.Numeric_X.Name = "Numeric_X";
            this.Numeric_X.Size = new System.Drawing.Size(60, 20);
            this.Numeric_X.TabIndex = 5;
            this.Numeric_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Numeric_X.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.Numeric_X.Enter += new System.EventHandler(this.Numeric_Auto_Selector);
            // 
            // Label_Y
            // 
            this.Label_Y.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_Y.AutoSize = true;
            this.Label_Y.Location = new System.Drawing.Point(144, 13);
            this.Label_Y.Name = "Label_Y";
            this.Label_Y.Size = new System.Drawing.Size(12, 13);
            this.Label_Y.TabIndex = 205;
            this.Label_Y.Text = "y";
            // 
            // Label_Height
            // 
            this.Label_Height.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_Height.AutoSize = true;
            this.Label_Height.Location = new System.Drawing.Point(131, 59);
            this.Label_Height.Name = "Label_Height";
            this.Label_Height.Size = new System.Drawing.Size(38, 13);
            this.Label_Height.TabIndex = 212;
            this.Label_Height.Text = "Height";
            // 
            // Label_X
            // 
            this.Label_X.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_X.AutoSize = true;
            this.Label_X.Location = new System.Drawing.Point(62, 13);
            this.Label_X.Name = "Label_X";
            this.Label_X.Size = new System.Drawing.Size(12, 13);
            this.Label_X.TabIndex = 206;
            this.Label_X.Text = "x";
            // 
            // Label_Width
            // 
            this.Label_Width.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_Width.AutoSize = true;
            this.Label_Width.Location = new System.Drawing.Point(49, 59);
            this.Label_Width.Name = "Label_Width";
            this.Label_Width.Size = new System.Drawing.Size(35, 13);
            this.Label_Width.TabIndex = 211;
            this.Label_Width.Text = "Width";
            // 
            // TextBox_Name
            // 
            this.TextBox_Name.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBox_Name.Location = new System.Drawing.Point(49, 202);
            this.TextBox_Name.MaxLength = 127;
            this.TextBox_Name.Name = "TextBox_Name";
            this.TextBox_Name.Size = new System.Drawing.Size(152, 20);
            this.TextBox_Name.TabIndex = 13;
            this.TextBox_Name.Enter += new System.EventHandler(this.TextBox_Auto_Selector);
            // 
            // Label_Name
            // 
            this.Label_Name.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_Name.AutoSize = true;
            this.Label_Name.Location = new System.Drawing.Point(8, 205);
            this.Label_Name.Name = "Label_Name";
            this.Label_Name.Size = new System.Drawing.Size(35, 13);
            this.Label_Name.TabIndex = 207;
            this.Label_Name.Text = "Name";
            // 
            // Panel_Feeds
            // 
            this.Panel_Feeds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Feeds.Controls.Add(this.Label_Timestamp);
            this.Panel_Feeds.Controls.Add(this.TextBox_Timestamp);
            this.Panel_Feeds.Controls.Add(this.ListBox_Feeds);
            this.Panel_Feeds.Controls.Add(this.Button_Minus_Feeds);
            this.Panel_Feeds.Controls.Add(this.Label_Feeds);
            this.Panel_Feeds.Controls.Add(this.TrackBar_Thumbnail);
            this.Panel_Feeds.Location = new System.Drawing.Point(3, 3);
            this.Panel_Feeds.Name = "Panel_Feeds";
            this.Panel_Feeds.Size = new System.Drawing.Size(129, 255);
            this.Panel_Feeds.TabIndex = 6;
            // 
            // Label_Timestamp
            // 
            this.Label_Timestamp.AutoSize = true;
            this.Label_Timestamp.Location = new System.Drawing.Point(9, 14);
            this.Label_Timestamp.Name = "Label_Timestamp";
            this.Label_Timestamp.Size = new System.Drawing.Size(110, 13);
            this.Label_Timestamp.TabIndex = 214;
            this.Label_Timestamp.Text = "Thumbnail Timestamp";
            // 
            // TextBox_Timestamp
            // 
            this.TextBox_Timestamp.Location = new System.Drawing.Point(30, 30);
            this.TextBox_Timestamp.Name = "TextBox_Timestamp";
            this.TextBox_Timestamp.Size = new System.Drawing.Size(69, 20);
            this.TextBox_Timestamp.TabIndex = 2;
            this.TextBox_Timestamp.Text = "01:23:45";
            this.TextBox_Timestamp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_Timestamp.TextChanged += new System.EventHandler(this.TextBox_Timestamp_TextChanged);
            // 
            // ListBox_Feeds
            // 
            this.ListBox_Feeds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ListBox_Feeds.FormattingEnabled = true;
            this.ListBox_Feeds.Location = new System.Drawing.Point(3, 75);
            this.ListBox_Feeds.Name = "ListBox_Feeds";
            this.ListBox_Feeds.Size = new System.Drawing.Size(123, 147);
            this.ListBox_Feeds.TabIndex = 3;
            this.ListBox_Feeds.SelectedIndexChanged += new System.EventHandler(this.ListBox_Feeds_SelectedIndexChanged);
            // 
            // Button_Minus_Feeds
            // 
            this.Button_Minus_Feeds.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Button_Minus_Feeds.Location = new System.Drawing.Point(74, 229);
            this.Button_Minus_Feeds.Name = "Button_Minus_Feeds";
            this.Button_Minus_Feeds.Size = new System.Drawing.Size(23, 23);
            this.Button_Minus_Feeds.TabIndex = 4;
            this.Button_Minus_Feeds.Text = "−";
            this.Button_Minus_Feeds.UseVisualStyleBackColor = true;
            this.Button_Minus_Feeds.Click += new System.EventHandler(this.Button_Minus_Feeds_Click);
            // 
            // Label_Feeds
            // 
            this.Label_Feeds.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Label_Feeds.AutoSize = true;
            this.Label_Feeds.Location = new System.Drawing.Point(32, 234);
            this.Label_Feeds.Name = "Label_Feeds";
            this.Label_Feeds.Size = new System.Drawing.Size(36, 13);
            this.Label_Feeds.TabIndex = 212;
            this.Label_Feeds.Text = "Feeds";
            this.Label_Feeds.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TrackBar_Thumbnail
            // 
            this.TrackBar_Thumbnail.Location = new System.Drawing.Point(3, 50);
            this.TrackBar_Thumbnail.Maximum = 100;
            this.TrackBar_Thumbnail.Name = "TrackBar_Thumbnail";
            this.TrackBar_Thumbnail.Size = new System.Drawing.Size(123, 45);
            this.TrackBar_Thumbnail.TabIndex = 215;
            this.TrackBar_Thumbnail.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar_Thumbnail.Value = 4;
            this.TrackBar_Thumbnail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TrackBar_Thumbnail_KeyUp);
            this.TrackBar_Thumbnail.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackBar_Thumbnail_MouseUp);
            // 
            // Panel_Back
            // 
            this.Panel_Back.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Back.AutoSize = true;
            this.Panel_Back.Controls.Add(this.Box_Main);
            this.Panel_Back.Controls.Add(this.Label_Failed);
            this.Panel_Back.Location = new System.Drawing.Point(135, 0);
            this.Panel_Back.Margin = new System.Windows.Forms.Padding(0);
            this.Panel_Back.Name = "Panel_Back";
            this.Panel_Back.Size = new System.Drawing.Size(244, 261);
            this.Panel_Back.TabIndex = 7;
            // 
            // Box_Main
            // 
            this.Box_Main.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box_Main.BackColor = System.Drawing.Color.White;
            this.Box_Main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Box_Main.Controls.Add(this.Box_Preview);
            this.Box_Main.Location = new System.Drawing.Point(27, 66);
            this.Box_Main.Name = "Box_Main";
            this.Box_Main.Size = new System.Drawing.Size(189, 120);
            this.Box_Main.TabIndex = 5;
            this.Box_Main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Box_Main_MouseDown);
            this.Box_Main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_Main_MouseMove);
            this.Box_Main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Box_Main_MouseUp);
            // 
            // Box_Preview
            // 
            this.Box_Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Box_Preview.BackColor = System.Drawing.Color.Black;
            this.Box_Preview.Location = new System.Drawing.Point(14, 25);
            this.Box_Preview.Name = "Box_Preview";
            this.Box_Preview.Size = new System.Drawing.Size(32, 32);
            this.Box_Preview.TabIndex = 1;
            // 
            // Label_Failed
            // 
            this.Label_Failed.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Label_Failed.AutoSize = true;
            this.Label_Failed.Location = new System.Drawing.Point(64, 14);
            this.Label_Failed.Name = "Label_Failed";
            this.Label_Failed.Size = new System.Drawing.Size(118, 13);
            this.Label_Failed.TabIndex = 6;
            this.Label_Failed.Text = "Failed to load thumbnail";
            this.Label_Failed.Visible = false;
            // 
            // Button_Add_Videos
            // 
            this.Button_Add_Videos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Button_Add_Videos.Location = new System.Drawing.Point(237, 3);
            this.Button_Add_Videos.Name = "Button_Add_Videos";
            this.Button_Add_Videos.Size = new System.Drawing.Size(125, 23);
            this.Button_Add_Videos.TabIndex = 0;
            this.Button_Add_Videos.Text = "Add Videos";
            this.Button_Add_Videos.UseVisualStyleBackColor = true;
            this.Button_Add_Videos.Click += new System.EventHandler(this.Button_Add_Videos_Click);
            // 
            // ListView_Main
            // 
            this.ListView_Main.AllowColumnReorder = true;
            this.ListView_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListView_Main.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.VideoFilePath,
            this.VideoFrameRate,
            this.VideoWidth,
            this.VideoHeight,
            this.VideoDuration,
            this.VideoFeeds});
            this.ListView_Main.FullRowSelect = true;
            this.ListView_Main.HideSelection = false;
            this.ListView_Main.Location = new System.Drawing.Point(3, 32);
            this.ListView_Main.Name = "ListView_Main";
            this.ListView_Main.ShowItemToolTips = true;
            this.ListView_Main.Size = new System.Drawing.Size(594, 127);
            this.ListView_Main.TabIndex = 1;
            this.ListView_Main.UseCompatibleStateImageBehavior = false;
            this.ListView_Main.View = System.Windows.Forms.View.Details;
            this.ListView_Main.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_Main_Column_Click);
            this.ListView_Main.SelectedIndexChanged += new System.EventHandler(this.ListView_Main_SelectedIndexChanged);
            this.ListView_Main.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListView_Main_KeyDown);
            // 
            // VideoFilePath
            // 
            this.VideoFilePath.Text = "File Path";
            this.VideoFilePath.Width = 255;
            // 
            // VideoFrameRate
            // 
            this.VideoFrameRate.Text = "FpS";
            this.VideoFrameRate.Width = 50;
            // 
            // VideoWidth
            // 
            this.VideoWidth.Text = "Width";
            this.VideoWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.VideoWidth.Width = 50;
            // 
            // VideoHeight
            // 
            this.VideoHeight.Text = "Height";
            this.VideoHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.VideoHeight.Width = 50;
            // 
            // VideoDuration
            // 
            this.VideoDuration.Text = "Length";
            this.VideoDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // VideoFeeds
            // 
            this.VideoFeeds.Text = "Feeds";
            this.VideoFeeds.Width = 96;
            // 
            // Panel_Top
            // 
            this.Panel_Top.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Top.Controls.Add(this.CheckBox_Advanced);
            this.Panel_Top.Controls.Add(this.Button_Add_Videos);
            this.Panel_Top.Location = new System.Drawing.Point(0, 0);
            this.Panel_Top.Margin = new System.Windows.Forms.Padding(0);
            this.Panel_Top.Name = "Panel_Top";
            this.Panel_Top.Size = new System.Drawing.Size(600, 29);
            this.Panel_Top.TabIndex = 211;
            // 
            // CheckBox_Advanced
            // 
            this.CheckBox_Advanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CheckBox_Advanced.AutoSize = true;
            this.CheckBox_Advanced.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBox_Advanced.Location = new System.Drawing.Point(3, 7);
            this.CheckBox_Advanced.Name = "CheckBox_Advanced";
            this.CheckBox_Advanced.Size = new System.Drawing.Size(144, 17);
            this.CheckBox_Advanced.TabIndex = 213;
            this.CheckBox_Advanced.Text = "Show Advanced Options";
            this.CheckBox_Advanced.UseVisualStyleBackColor = true;
            this.CheckBox_Advanced.CheckedChanged += new System.EventHandler(this.CheckBox_Advanced_CheckedChanged);
            // 
            // Numeric_Game_Height
            // 
            this.Numeric_Game_Height.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Numeric_Game_Height.Location = new System.Drawing.Point(120, 121);
            this.Numeric_Game_Height.Maximum = new decimal(new int[] {
            4320,
            0,
            0,
            0});
            this.Numeric_Game_Height.Name = "Numeric_Game_Height";
            this.Numeric_Game_Height.Size = new System.Drawing.Size(60, 20);
            this.Numeric_Game_Height.TabIndex = 10;
            this.Numeric_Game_Height.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Numeric_Game_Height.Visible = false;
            this.Numeric_Game_Height.Enter += new System.EventHandler(this.Numeric_Auto_Selector);
            // 
            // Numeric_Game_Width
            // 
            this.Numeric_Game_Width.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Numeric_Game_Width.Location = new System.Drawing.Point(36, 121);
            this.Numeric_Game_Width.Maximum = new decimal(new int[] {
            7680,
            0,
            0,
            0});
            this.Numeric_Game_Width.Name = "Numeric_Game_Width";
            this.Numeric_Game_Width.Size = new System.Drawing.Size(60, 20);
            this.Numeric_Game_Width.TabIndex = 9;
            this.Numeric_Game_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Numeric_Game_Width.Visible = false;
            this.Numeric_Game_Width.Enter += new System.EventHandler(this.Numeric_Auto_Selector);
            // 
            // Label_Game_Height
            // 
            this.Label_Game_Height.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_Game_Height.AutoSize = true;
            this.Label_Game_Height.Location = new System.Drawing.Point(116, 105);
            this.Label_Game_Height.Name = "Label_Game_Height";
            this.Label_Game_Height.Size = new System.Drawing.Size(69, 13);
            this.Label_Game_Height.TabIndex = 216;
            this.Label_Game_Height.Text = "Game Height";
            this.Label_Game_Height.Visible = false;
            // 
            // Label_Game_Width
            // 
            this.Label_Game_Width.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label_Game_Width.AutoSize = true;
            this.Label_Game_Width.Location = new System.Drawing.Point(33, 105);
            this.Label_Game_Width.Name = "Label_Game_Width";
            this.Label_Game_Width.Size = new System.Drawing.Size(66, 13);
            this.Label_Game_Width.TabIndex = 215;
            this.Label_Game_Width.Text = "Game Width";
            this.Label_Game_Width.Visible = false;
            // 
            // AddVideos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 482);
            this.Controls.Add(this.TableLayoutPanel_Window);
            this.Controls.Add(this.Button_Help);
            this.Controls.Add(this.Button_Import);
            this.Controls.Add(this.Button_Export);
            this.Controls.Add(this.Button_Close);
            this.MinimumSize = new System.Drawing.Size(408, 300);
            this.Name = "AddVideos";
            this.Text = "Add Videos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Load += new System.EventHandler(this.AddVideos_Load);
            this.ResizeEnd += new System.EventHandler(this.Resize_Box);
            this.TableLayoutPanel_Window.ResumeLayout(false);
            this.Panel_Core.ResumeLayout(false);
            this.TableLayoutPanel_Single.ResumeLayout(false);
            this.TableLayoutPanel_Single.PerformLayout();
            this.Panel_Input.ResumeLayout(false);
            this.Panel_Input.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_X)).EndInit();
            this.Panel_Feeds.ResumeLayout(false);
            this.Panel_Feeds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_Thumbnail)).EndInit();
            this.Panel_Back.ResumeLayout(false);
            this.Panel_Back.PerformLayout();
            this.Box_Main.ResumeLayout(false);
            this.Panel_Top.ResumeLayout(false);
            this.Panel_Top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Game_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_Game_Width)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Help;
        private System.Windows.Forms.Button Button_Import;
        private System.Windows.Forms.Button Button_Export;
        private System.Windows.Forms.Button Button_Close;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel_Window;
        private System.Windows.Forms.Button Button_Add_Videos;
        private System.Windows.Forms.ColumnHeader VideoFeeds;
        private System.Windows.Forms.ColumnHeader VideoFilePath;
        private System.Windows.Forms.ListView ListView_Main;
        private System.Windows.Forms.ColumnHeader VideoFrameRate;
        private System.Windows.Forms.ColumnHeader VideoWidth;
        private System.Windows.Forms.ColumnHeader VideoHeight;
        private System.Windows.Forms.ColumnHeader VideoDuration;
        private System.Windows.Forms.Panel Panel_Core;
        private System.Windows.Forms.ListBox ListBox_Feeds;
        private System.Windows.Forms.Button Button_Minus_Feeds;
        private System.Windows.Forms.Label Label_Feeds;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel_Single;
        private System.Windows.Forms.Panel Box_Main;
        private System.Windows.Forms.Panel Box_Preview;
        private System.Windows.Forms.Panel Panel_Input;
        private System.Windows.Forms.NumericUpDown Numeric_Height;
        private System.Windows.Forms.NumericUpDown Numeric_Width;
        private System.Windows.Forms.NumericUpDown Numeric_Y;
        private System.Windows.Forms.NumericUpDown Numeric_X;
        private System.Windows.Forms.Label Label_Y;
        private System.Windows.Forms.Label Label_Height;
        private System.Windows.Forms.Label Label_X;
        private System.Windows.Forms.Label Label_Width;
        private System.Windows.Forms.TextBox TextBox_Name;
        private System.Windows.Forms.Label Label_Name;
        private System.Windows.Forms.Panel Panel_Feeds;
        private System.Windows.Forms.CheckBox CheckBox_Timer;
        private System.Windows.Forms.Button Button_Add_Feed;
        private System.Windows.Forms.Panel Panel_Back;
        private System.Windows.Forms.Label Label_Failed;
        private System.Windows.Forms.CheckBox CheckBox_Perfect;
        private System.Windows.Forms.TextBox TextBox_Timestamp;
        private System.Windows.Forms.Label Label_Timestamp;
        private System.Windows.Forms.TrackBar TrackBar_Thumbnail;
        private System.Windows.Forms.Panel Panel_Top;
        private System.Windows.Forms.CheckBox CheckBox_Advanced;
        private System.Windows.Forms.NumericUpDown Numeric_Game_Height;
        private System.Windows.Forms.NumericUpDown Numeric_Game_Width;
        private System.Windows.Forms.Label Label_Game_Height;
        private System.Windows.Forms.Label Label_Game_Width;
    }
}