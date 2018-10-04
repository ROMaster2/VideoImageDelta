namespace VideoImageDeltaApp.Forms
{
    partial class PreProcessOptions
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
            this.Button_Start = new System.Windows.Forms.Button();
            this.TableLayoutPanel_Core = new System.Windows.Forms.TableLayoutPanel();
            this.TableLayoutPanel_Output = new System.Windows.Forms.TableLayoutPanel();
            this.Button_Browse_Output = new System.Windows.Forms.Button();
            this.TextBox_Output = new System.Windows.Forms.TextBox();
            this.CheckBox_Debug = new System.Windows.Forms.CheckBox();
            this.CheckBox_Stability = new System.Windows.Forms.CheckBox();
            this.CheckBox_Errors = new System.Windows.Forms.CheckBox();
            this.CheckBox_ToRam = new System.Windows.Forms.CheckBox();
            this.TableLayoutPanel_Cache = new System.Windows.Forms.TableLayoutPanel();
            this.Button_Browse_Cache = new System.Windows.Forms.Button();
            this.TextBox_Cache = new System.Windows.Forms.TextBox();
            this.CheckBox_AutoSave = new System.Windows.Forms.CheckBox();
            this.DropBox_Priority = new System.Windows.Forms.ComboBox();
            this.NumericUpDown_CPU_Limit = new System.Windows.Forms.NumericUpDown();
            this.Panel_Hardware = new System.Windows.Forms.Panel();
            this.Label_Hardware = new System.Windows.Forms.Label();
            this.DropBox_Hardware = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TableLayoutPanel_Core.SuspendLayout();
            this.TableLayoutPanel_Output.SuspendLayout();
            this.TableLayoutPanel_Cache.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_CPU_Limit)).BeginInit();
            this.Panel_Hardware.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Start
            // 
            this.Button_Start.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Button_Start.Location = new System.Drawing.Point(120, 319);
            this.Button_Start.Name = "Button_Start";
            this.Button_Start.Size = new System.Drawing.Size(80, 23);
            this.Button_Start.TabIndex = 0;
            this.Button_Start.Text = "Start";
            this.Button_Start.UseVisualStyleBackColor = true;
            this.Button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // TableLayoutPanel_Core
            // 
            this.TableLayoutPanel_Core.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel_Core.ColumnCount = 1;
            this.TableLayoutPanel_Core.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Core.Controls.Add(this.TableLayoutPanel_Output, 0, 11);
            this.TableLayoutPanel_Core.Controls.Add(this.CheckBox_Stability, 0, 3);
            this.TableLayoutPanel_Core.Controls.Add(this.CheckBox_Errors, 0, 4);
            this.TableLayoutPanel_Core.Controls.Add(this.CheckBox_ToRam, 0, 5);
            this.TableLayoutPanel_Core.Controls.Add(this.TableLayoutPanel_Cache, 0, 6);
            this.TableLayoutPanel_Core.Controls.Add(this.CheckBox_AutoSave, 0, 7);
            this.TableLayoutPanel_Core.Controls.Add(this.DropBox_Priority, 0, 9);
            this.TableLayoutPanel_Core.Controls.Add(this.NumericUpDown_CPU_Limit, 0, 12);
            this.TableLayoutPanel_Core.Controls.Add(this.Panel_Hardware, 0, 1);
            this.TableLayoutPanel_Core.Controls.Add(this.CheckBox_Debug, 0, 2);
            this.TableLayoutPanel_Core.Controls.Add(this.label1, 0, 0);
            this.TableLayoutPanel_Core.Location = new System.Drawing.Point(12, 12);
            this.TableLayoutPanel_Core.Name = "TableLayoutPanel_Core";
            this.TableLayoutPanel_Core.RowCount = 14;
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel_Core.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Core.Size = new System.Drawing.Size(280, 301);
            this.TableLayoutPanel_Core.TabIndex = 1;
            // 
            // TableLayoutPanel_Output
            // 
            this.TableLayoutPanel_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel_Output.ColumnCount = 2;
            this.TableLayoutPanel_Output.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.TableLayoutPanel_Output.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Output.Controls.Add(this.Button_Browse_Output, 0, 0);
            this.TableLayoutPanel_Output.Controls.Add(this.TextBox_Output, 1, 0);
            this.TableLayoutPanel_Output.Location = new System.Drawing.Point(0, 253);
            this.TableLayoutPanel_Output.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel_Output.Name = "TableLayoutPanel_Output";
            this.TableLayoutPanel_Output.RowCount = 1;
            this.TableLayoutPanel_Output.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Output.Size = new System.Drawing.Size(280, 23);
            this.TableLayoutPanel_Output.TabIndex = 7;
            // 
            // Button_Browse_Output
            // 
            this.Button_Browse_Output.Location = new System.Drawing.Point(0, 0);
            this.Button_Browse_Output.Margin = new System.Windows.Forms.Padding(0);
            this.Button_Browse_Output.Name = "Button_Browse_Output";
            this.Button_Browse_Output.Size = new System.Drawing.Size(75, 23);
            this.Button_Browse_Output.TabIndex = 0;
            this.Button_Browse_Output.Text = "Browse...";
            this.Button_Browse_Output.UseVisualStyleBackColor = true;
            this.Button_Browse_Output.Click += new System.EventHandler(this.Button_Browse_Output_Click);
            // 
            // TextBox_Output
            // 
            this.TextBox_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Output.Location = new System.Drawing.Point(75, 1);
            this.TextBox_Output.Margin = new System.Windows.Forms.Padding(0);
            this.TextBox_Output.MaximumSize = new System.Drawing.Size(768, 20);
            this.TextBox_Output.Name = "TextBox_Output";
            this.TextBox_Output.ReadOnly = true;
            this.TextBox_Output.Size = new System.Drawing.Size(205, 20);
            this.TextBox_Output.TabIndex = 1;
            this.TextBox_Output.Text = "Output Directory";
            // 
            // CheckBox_Debug
            // 
            this.CheckBox_Debug.AutoSize = true;
            this.CheckBox_Debug.Location = new System.Drawing.Point(3, 49);
            this.CheckBox_Debug.Name = "CheckBox_Debug";
            this.CheckBox_Debug.Size = new System.Drawing.Size(115, 17);
            this.CheckBox_Debug.TabIndex = 0;
            this.CheckBox_Debug.Text = "Display Debugging";
            this.CheckBox_Debug.UseVisualStyleBackColor = true;
            this.CheckBox_Debug.Visible = false;
            // 
            // CheckBox_Stability
            // 
            this.CheckBox_Stability.AutoSize = true;
            this.CheckBox_Stability.Location = new System.Drawing.Point(3, 72);
            this.CheckBox_Stability.Name = "CheckBox_Stability";
            this.CheckBox_Stability.Size = new System.Drawing.Size(151, 17);
            this.CheckBox_Stability.TabIndex = 3;
            this.CheckBox_Stability.Text = "Run Video stability checks";
            this.CheckBox_Stability.UseVisualStyleBackColor = true;
            this.CheckBox_Stability.Visible = false;
            // 
            // CheckBox_Errors
            // 
            this.CheckBox_Errors.AutoSize = true;
            this.CheckBox_Errors.Checked = true;
            this.CheckBox_Errors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox_Errors.Location = new System.Drawing.Point(3, 95);
            this.CheckBox_Errors.Name = "CheckBox_Errors";
            this.CheckBox_Errors.Size = new System.Drawing.Size(244, 17);
            this.CheckBox_Errors.TabIndex = 4;
            this.CheckBox_Errors.Text = "Suppress errors until everything else is finished";
            this.CheckBox_Errors.UseVisualStyleBackColor = true;
            this.CheckBox_Errors.Visible = false;
            // 
            // CheckBox_ToRam
            // 
            this.CheckBox_ToRam.AutoSize = true;
            this.CheckBox_ToRam.Checked = true;
            this.CheckBox_ToRam.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox_ToRam.Location = new System.Drawing.Point(3, 118);
            this.CheckBox_ToRam.Name = "CheckBox_ToRam";
            this.CheckBox_ToRam.Size = new System.Drawing.Size(96, 17);
            this.CheckBox_ToRam.TabIndex = 5;
            this.CheckBox_ToRam.Text = "Cache to RAM";
            this.CheckBox_ToRam.UseVisualStyleBackColor = true;
            this.CheckBox_ToRam.Visible = false;
            // 
            // TableLayoutPanel_Cache
            // 
            this.TableLayoutPanel_Cache.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel_Cache.ColumnCount = 2;
            this.TableLayoutPanel_Cache.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.TableLayoutPanel_Cache.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Cache.Controls.Add(this.Button_Browse_Cache, 0, 0);
            this.TableLayoutPanel_Cache.Controls.Add(this.TextBox_Cache, 1, 0);
            this.TableLayoutPanel_Cache.Location = new System.Drawing.Point(0, 138);
            this.TableLayoutPanel_Cache.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel_Cache.Name = "TableLayoutPanel_Cache";
            this.TableLayoutPanel_Cache.RowCount = 1;
            this.TableLayoutPanel_Cache.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel_Cache.Size = new System.Drawing.Size(280, 23);
            this.TableLayoutPanel_Cache.TabIndex = 6;
            // 
            // Button_Browse_Cache
            // 
            this.Button_Browse_Cache.Location = new System.Drawing.Point(0, 0);
            this.Button_Browse_Cache.Margin = new System.Windows.Forms.Padding(0);
            this.Button_Browse_Cache.Name = "Button_Browse_Cache";
            this.Button_Browse_Cache.Size = new System.Drawing.Size(75, 23);
            this.Button_Browse_Cache.TabIndex = 0;
            this.Button_Browse_Cache.Text = "Browse...";
            this.Button_Browse_Cache.UseVisualStyleBackColor = true;
            this.Button_Browse_Cache.Click += new System.EventHandler(this.Button_Browse_Cache_Click);
            // 
            // TextBox_Cache
            // 
            this.TextBox_Cache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Cache.Location = new System.Drawing.Point(75, 1);
            this.TextBox_Cache.Margin = new System.Windows.Forms.Padding(0);
            this.TextBox_Cache.MaximumSize = new System.Drawing.Size(768, 20);
            this.TextBox_Cache.Name = "TextBox_Cache";
            this.TextBox_Cache.ReadOnly = true;
            this.TextBox_Cache.Size = new System.Drawing.Size(205, 20);
            this.TextBox_Cache.TabIndex = 1;
            this.TextBox_Cache.Text = "Cache Directory";
            // 
            // CheckBox_AutoSave
            // 
            this.CheckBox_AutoSave.AutoSize = true;
            this.CheckBox_AutoSave.Location = new System.Drawing.Point(3, 164);
            this.CheckBox_AutoSave.Name = "CheckBox_AutoSave";
            this.CheckBox_AutoSave.Size = new System.Drawing.Size(73, 17);
            this.CheckBox_AutoSave.TabIndex = 7;
            this.CheckBox_AutoSave.Text = "auto save";
            this.CheckBox_AutoSave.UseVisualStyleBackColor = true;
            this.CheckBox_AutoSave.Visible = false;
            // 
            // DropBox_Priority
            // 
            this.DropBox_Priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DropBox_Priority.FormattingEnabled = true;
            this.DropBox_Priority.Items.AddRange(new object[] {
            System.Diagnostics.ProcessPriorityClass.Idle,
            System.Diagnostics.ProcessPriorityClass.BelowNormal,
            System.Diagnostics.ProcessPriorityClass.Normal,
            System.Diagnostics.ProcessPriorityClass.AboveNormal,
            System.Diagnostics.ProcessPriorityClass.High});
            this.DropBox_Priority.Location = new System.Drawing.Point(3, 210);
            this.DropBox_Priority.Name = "DropBox_Priority";
            this.DropBox_Priority.Size = new System.Drawing.Size(121, 21);
            this.DropBox_Priority.TabIndex = 8;
            // 
            // NumericUpDown_CPU_Limit
            // 
            this.NumericUpDown_CPU_Limit.Location = new System.Drawing.Point(3, 279);
            this.NumericUpDown_CPU_Limit.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.NumericUpDown_CPU_Limit.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            -2147483648});
            this.NumericUpDown_CPU_Limit.Name = "NumericUpDown_CPU_Limit";
            this.NumericUpDown_CPU_Limit.Size = new System.Drawing.Size(120, 20);
            this.NumericUpDown_CPU_Limit.TabIndex = 10;
            this.NumericUpDown_CPU_Limit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumericUpDown_CPU_Limit.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.NumericUpDown_CPU_Limit.Visible = false;
            // 
            // Panel_Hardware
            // 
            this.Panel_Hardware.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Hardware.Controls.Add(this.DropBox_Hardware);
            this.Panel_Hardware.Controls.Add(this.Label_Hardware);
            this.Panel_Hardware.Location = new System.Drawing.Point(0, 23);
            this.Panel_Hardware.Margin = new System.Windows.Forms.Padding(0);
            this.Panel_Hardware.Name = "Panel_Hardware";
            this.Panel_Hardware.Size = new System.Drawing.Size(280, 23);
            this.Panel_Hardware.TabIndex = 11;
            // 
            // Label_Hardware
            // 
            this.Label_Hardware.AutoSize = true;
            this.Label_Hardware.Location = new System.Drawing.Point(0, 5);
            this.Label_Hardware.Name = "Label_Hardware";
            this.Label_Hardware.Size = new System.Drawing.Size(140, 13);
            this.Label_Hardware.TabIndex = 0;
            this.Label_Hardware.Text = "Video Hardware Accelerator";
            // 
            // DropBox_Hardware
            // 
            this.DropBox_Hardware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DropBox_Hardware.FormattingEnabled = true;
            this.DropBox_Hardware.Location = new System.Drawing.Point(156, 2);
            this.DropBox_Hardware.Name = "DropBox_Hardware";
            this.DropBox_Hardware.Size = new System.Drawing.Size(121, 21);
            this.DropBox_Hardware.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Sorry for the mess, it\'s a work in progress";
            // 
            // PreProcessOptions
            // 
            this.AcceptButton = this.Button_Start;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 354);
            this.Controls.Add(this.TableLayoutPanel_Core);
            this.Controls.Add(this.Button_Start);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 38);
            this.Name = "PreProcessOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Pre-Process Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreProcessOptions_FormClosing);
            this.TableLayoutPanel_Core.ResumeLayout(false);
            this.TableLayoutPanel_Core.PerformLayout();
            this.TableLayoutPanel_Output.ResumeLayout(false);
            this.TableLayoutPanel_Output.PerformLayout();
            this.TableLayoutPanel_Cache.ResumeLayout(false);
            this.TableLayoutPanel_Cache.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_CPU_Limit)).EndInit();
            this.Panel_Hardware.ResumeLayout(false);
            this.Panel_Hardware.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Button_Start;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel_Core;
        private System.Windows.Forms.CheckBox CheckBox_Debug;
        private System.Windows.Forms.CheckBox CheckBox_Stability;
        private System.Windows.Forms.CheckBox CheckBox_Errors;
        private System.Windows.Forms.CheckBox CheckBox_ToRam;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel_Cache;
        private System.Windows.Forms.Button Button_Browse_Cache;
        private System.Windows.Forms.TextBox TextBox_Cache;
        private System.Windows.Forms.CheckBox CheckBox_AutoSave;
        private System.Windows.Forms.ComboBox DropBox_Priority;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel_Output;
        private System.Windows.Forms.Button Button_Browse_Output;
        private System.Windows.Forms.TextBox TextBox_Output;
        private System.Windows.Forms.NumericUpDown NumericUpDown_CPU_Limit;
        private System.Windows.Forms.Panel Panel_Hardware;
        private System.Windows.Forms.ComboBox DropBox_Hardware;
        private System.Windows.Forms.Label Label_Hardware;
        private System.Windows.Forms.Label label1;
    }
}