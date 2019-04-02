namespace GamingMusicPlayer
{
    partial class SongMatcher
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
            this.statusLabel = new System.Windows.Forms.Label();
            this.cmdToggleMatching = new System.Windows.Forms.Button();
            this.mouseXLabel = new System.Windows.Forms.Label();
            this.keyboardLabel = new System.Windows.Forms.Label();
            this.cmdPickTrack = new System.Windows.Forms.Button();
            this.keyboardTrackLabel = new System.Windows.Forms.Label();
            this.mouseTrackLabel = new System.Windows.Forms.Label();
            this.mouseYLabel = new System.Windows.Forms.Label();
            this.mouseXZCRLabel = new System.Windows.Forms.Label();
            this.mouseYZCRLabel = new System.Windows.Forms.Label();
            this.keyboardZCRLabel = new System.Windows.Forms.Label();
            this.keyboardSpectIrrLabel = new System.Windows.Forms.Label();
            this.mouseYSpectIrrLabel = new System.Windows.Forms.Label();
            this.mouseXSpectIrrLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(418, 190);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(80, 17);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "status-here";
            // 
            // cmdToggleMatching
            // 
            this.cmdToggleMatching.Location = new System.Drawing.Point(425, 232);
            this.cmdToggleMatching.Name = "cmdToggleMatching";
            this.cmdToggleMatching.Size = new System.Drawing.Size(121, 25);
            this.cmdToggleMatching.TabIndex = 1;
            this.cmdToggleMatching.Text = "Start Matching";
            this.cmdToggleMatching.UseVisualStyleBackColor = true;
            this.cmdToggleMatching.Click += new System.EventHandler(this.cmdToggleMatching_Click);
            // 
            // mouseXLabel
            // 
            this.mouseXLabel.AutoSize = true;
            this.mouseXLabel.Location = new System.Drawing.Point(300, 9);
            this.mouseXLabel.Name = "mouseXLabel";
            this.mouseXLabel.Size = new System.Drawing.Size(92, 17);
            this.mouseXLabel.TabIndex = 4;
            this.mouseXLabel.Text = "mouseXBPM:";
            // 
            // keyboardLabel
            // 
            this.keyboardLabel.AutoSize = true;
            this.keyboardLabel.Location = new System.Drawing.Point(20, 9);
            this.keyboardLabel.Name = "keyboardLabel";
            this.keyboardLabel.Size = new System.Drawing.Size(100, 17);
            this.keyboardLabel.TabIndex = 5;
            this.keyboardLabel.Text = "keyboardBPM:";
            // 
            // cmdPickTrack
            // 
            this.cmdPickTrack.Location = new System.Drawing.Point(681, 232);
            this.cmdPickTrack.Name = "cmdPickTrack";
            this.cmdPickTrack.Size = new System.Drawing.Size(115, 25);
            this.cmdPickTrack.TabIndex = 6;
            this.cmdPickTrack.Text = "Pick Track";
            this.cmdPickTrack.UseVisualStyleBackColor = true;
            this.cmdPickTrack.Click += new System.EventHandler(this.cmdPickTrack_Click_1);
            // 
            // keyboardTrackLabel
            // 
            this.keyboardTrackLabel.AutoSize = true;
            this.keyboardTrackLabel.Location = new System.Drawing.Point(574, 207);
            this.keyboardTrackLabel.Name = "keyboardTrackLabel";
            this.keyboardTrackLabel.Size = new System.Drawing.Size(80, 17);
            this.keyboardTrackLabel.TabIndex = 7;
            this.keyboardTrackLabel.Text = "status-here";
            // 
            // mouseTrackLabel
            // 
            this.mouseTrackLabel.AutoSize = true;
            this.mouseTrackLabel.Location = new System.Drawing.Point(574, 190);
            this.mouseTrackLabel.Name = "mouseTrackLabel";
            this.mouseTrackLabel.Size = new System.Drawing.Size(80, 17);
            this.mouseTrackLabel.TabIndex = 8;
            this.mouseTrackLabel.Text = "status-here";
            // 
            // mouseYLabel
            // 
            this.mouseYLabel.AutoSize = true;
            this.mouseYLabel.Location = new System.Drawing.Point(515, 9);
            this.mouseYLabel.Name = "mouseYLabel";
            this.mouseYLabel.Size = new System.Drawing.Size(92, 17);
            this.mouseYLabel.TabIndex = 9;
            this.mouseYLabel.Text = "mouseYBPM:";
            // 
            // mouseXZCRLabel
            // 
            this.mouseXZCRLabel.AutoSize = true;
            this.mouseXZCRLabel.Location = new System.Drawing.Point(300, 41);
            this.mouseXZCRLabel.Name = "mouseXZCRLabel";
            this.mouseXZCRLabel.Size = new System.Drawing.Size(87, 17);
            this.mouseXZCRLabel.TabIndex = 10;
            this.mouseXZCRLabel.Text = "mouseXZCR";
            // 
            // mouseYZCRLabel
            // 
            this.mouseYZCRLabel.AutoSize = true;
            this.mouseYZCRLabel.Location = new System.Drawing.Point(515, 41);
            this.mouseYZCRLabel.Name = "mouseYZCRLabel";
            this.mouseYZCRLabel.Size = new System.Drawing.Size(87, 17);
            this.mouseYZCRLabel.TabIndex = 11;
            this.mouseYZCRLabel.Text = "mouseYZCR";
            // 
            // keyboardZCRLabel
            // 
            this.keyboardZCRLabel.AutoSize = true;
            this.keyboardZCRLabel.Location = new System.Drawing.Point(20, 41);
            this.keyboardZCRLabel.Name = "keyboardZCRLabel";
            this.keyboardZCRLabel.Size = new System.Drawing.Size(99, 17);
            this.keyboardZCRLabel.TabIndex = 12;
            this.keyboardZCRLabel.Text = "keyboardZCR:";
            // 
            // keyboardSpectIrrLabel
            // 
            this.keyboardSpectIrrLabel.AutoSize = true;
            this.keyboardSpectIrrLabel.Location = new System.Drawing.Point(20, 76);
            this.keyboardSpectIrrLabel.Name = "keyboardSpectIrrLabel";
            this.keyboardSpectIrrLabel.Size = new System.Drawing.Size(116, 17);
            this.keyboardSpectIrrLabel.TabIndex = 13;
            this.keyboardSpectIrrLabel.Text = "keyboardSpectIrr";
            // 
            // mouseYSpectIrrLabel
            // 
            this.mouseYSpectIrrLabel.AutoSize = true;
            this.mouseYSpectIrrLabel.Location = new System.Drawing.Point(515, 76);
            this.mouseYSpectIrrLabel.Name = "mouseYSpectIrrLabel";
            this.mouseYSpectIrrLabel.Size = new System.Drawing.Size(108, 17);
            this.mouseYSpectIrrLabel.TabIndex = 14;
            this.mouseYSpectIrrLabel.Text = "mouseYSpectIrr";
            // 
            // mouseXSpectIrrLabel
            // 
            this.mouseXSpectIrrLabel.AutoSize = true;
            this.mouseXSpectIrrLabel.Location = new System.Drawing.Point(300, 76);
            this.mouseXSpectIrrLabel.Name = "mouseXSpectIrrLabel";
            this.mouseXSpectIrrLabel.Size = new System.Drawing.Size(108, 17);
            this.mouseXSpectIrrLabel.TabIndex = 15;
            this.mouseXSpectIrrLabel.Text = "mouseXSpectIrr";
            // 
            // SongMatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 269);
            this.Controls.Add(this.mouseXSpectIrrLabel);
            this.Controls.Add(this.mouseYSpectIrrLabel);
            this.Controls.Add(this.keyboardSpectIrrLabel);
            this.Controls.Add(this.keyboardZCRLabel);
            this.Controls.Add(this.mouseYZCRLabel);
            this.Controls.Add(this.mouseXZCRLabel);
            this.Controls.Add(this.mouseYLabel);
            this.Controls.Add(this.mouseTrackLabel);
            this.Controls.Add(this.keyboardTrackLabel);
            this.Controls.Add(this.cmdPickTrack);
            this.Controls.Add(this.keyboardLabel);
            this.Controls.Add(this.mouseXLabel);
            this.Controls.Add(this.cmdToggleMatching);
            this.Controls.Add(this.statusLabel);
            this.Name = "SongMatcher";
            this.Text = "SongMatcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button cmdToggleMatching;
        private System.Windows.Forms.Label mouseXLabel;
        private System.Windows.Forms.Label keyboardLabel;
        private System.Windows.Forms.Button cmdPickTrack;
        private System.Windows.Forms.Label keyboardTrackLabel;
        private System.Windows.Forms.Label mouseTrackLabel;
        private System.Windows.Forms.Label mouseYLabel;
        private System.Windows.Forms.Label mouseXZCRLabel;
        private System.Windows.Forms.Label mouseYZCRLabel;
        private System.Windows.Forms.Label keyboardZCRLabel;
        private System.Windows.Forms.Label keyboardSpectIrrLabel;
        private System.Windows.Forms.Label mouseYSpectIrrLabel;
        private System.Windows.Forms.Label mouseXSpectIrrLabel;
    }
}