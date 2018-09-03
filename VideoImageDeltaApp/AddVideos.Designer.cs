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
            this.Button_Quit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Button_Help
            // 
            this.Button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Help.Location = new System.Drawing.Point(296, 407);
            this.Button_Help.Name = "Button_Help";
            this.Button_Help.Size = new System.Drawing.Size(75, 23);
            this.Button_Help.TabIndex = 103;
            this.Button_Help.Text = "Help";
            this.Button_Help.UseVisualStyleBackColor = true;
            // 
            // Button_Quit
            // 
            this.Button_Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Quit.AutoSize = true;
            this.Button_Quit.Location = new System.Drawing.Point(377, 407);
            this.Button_Quit.Name = "Button_Quit";
            this.Button_Quit.Size = new System.Drawing.Size(75, 23);
            this.Button_Quit.TabIndex = 104;
            this.Button_Quit.Text = "Close";
            this.Button_Quit.UseVisualStyleBackColor = true;
            // 
            // AddVideos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 442);
            this.Controls.Add(this.Button_Help);
            this.Controls.Add(this.Button_Quit);
            this.Name = "AddVideos";
            this.Text = "AddVideos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Help;
        private System.Windows.Forms.Button Button_Quit;
    }
}