namespace VideoImageDeltaApp.Forms
{
    partial class PreviewBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ThumbnailBox = new System.Windows.Forms.PictureBox();
            this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ThumbnailBox
            // 
            this.ThumbnailBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ThumbnailBox.Location = new System.Drawing.Point(5, 5);
            this.ThumbnailBox.Margin = new System.Windows.Forms.Padding(5);
            this.ThumbnailBox.Name = "ThumbnailBox";
            this.ThumbnailBox.Size = new System.Drawing.Size(640, 480);
            this.ThumbnailBox.TabIndex = 0;
            this.ThumbnailBox.TabStop = false;
            // 
            // BackgroundWorker
            // 
            this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            // 
            // PreviewBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ThumbnailBox);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PreviewBox";
            this.Size = new System.Drawing.Size(650, 490);
            this.Load += new System.EventHandler(this.PreviewBox_Load);
            this.Click += new System.EventHandler(this.PreviewBox_Click);
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ThumbnailBox;
        private System.ComponentModel.BackgroundWorker BackgroundWorker;
    }
}
