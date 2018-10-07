namespace VideoImageDeltaApp.Forms
{
    partial class TestForm
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
            this.previewBox1 = new VideoImageDeltaApp.Forms.PreviewBox();
            this.SuspendLayout();
            // 
            // previewBox1
            // 
            this.previewBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewBox1.AutoSize = true;
            this.previewBox1.Location = new System.Drawing.Point(9, 9);
            this.previewBox1.Margin = new System.Windows.Forms.Padding(0);
            this.previewBox1.Name = "previewBox1";
            this.previewBox1.Size = new System.Drawing.Size(1034, 778);
            this.previewBox1.TabIndex = 0;
            // 
            // TestForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1052, 796);
            this.Controls.Add(this.previewBox1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PreviewBox previewBox1;
    }
}