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
            this.mouseLabel = new System.Windows.Forms.Label();
            this.keyboardLabel = new System.Windows.Forms.Label();
            this.cmdPickTrack = new System.Windows.Forms.Button();
            this.keyboardTrackLabel = new System.Windows.Forms.Label();
            this.mouseTrackLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 77);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(80, 17);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "status-here";
            // 
            // cmdToggleMatching
            // 
            this.cmdToggleMatching.Location = new System.Drawing.Point(12, 133);
            this.cmdToggleMatching.Name = "cmdToggleMatching";
            this.cmdToggleMatching.Size = new System.Drawing.Size(121, 25);
            this.cmdToggleMatching.TabIndex = 1;
            this.cmdToggleMatching.Text = "Start Matching";
            this.cmdToggleMatching.UseVisualStyleBackColor = true;
            this.cmdToggleMatching.Click += new System.EventHandler(this.cmdToggleMatching_Click);
            // 
            // mouseLabel
            // 
            this.mouseLabel.AutoSize = true;
            this.mouseLabel.Location = new System.Drawing.Point(12, 9);
            this.mouseLabel.Name = "mouseLabel";
            this.mouseLabel.Size = new System.Drawing.Size(83, 17);
            this.mouseLabel.TabIndex = 4;
            this.mouseLabel.Text = "mouseBPM:";
            // 
            // keyboardLabel
            // 
            this.keyboardLabel.AutoSize = true;
            this.keyboardLabel.Location = new System.Drawing.Point(12, 39);
            this.keyboardLabel.Name = "keyboardLabel";
            this.keyboardLabel.Size = new System.Drawing.Size(100, 17);
            this.keyboardLabel.TabIndex = 5;
            this.keyboardLabel.Text = "keyboardBPM:";
            // 
            // cmdPickTrack
            // 
            this.cmdPickTrack.Location = new System.Drawing.Point(172, 133);
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
            this.keyboardTrackLabel.Location = new System.Drawing.Point(169, 68);
            this.keyboardTrackLabel.Name = "keyboardTrackLabel";
            this.keyboardTrackLabel.Size = new System.Drawing.Size(80, 17);
            this.keyboardTrackLabel.TabIndex = 7;
            this.keyboardTrackLabel.Text = "status-here";
            // 
            // mouseTrackLabel
            // 
            this.mouseTrackLabel.AutoSize = true;
            this.mouseTrackLabel.Location = new System.Drawing.Point(169, 94);
            this.mouseTrackLabel.Name = "mouseTrackLabel";
            this.mouseTrackLabel.Size = new System.Drawing.Size(80, 17);
            this.mouseTrackLabel.TabIndex = 8;
            this.mouseTrackLabel.Text = "status-here";
            // 
            // SongMatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 165);
            this.Controls.Add(this.mouseTrackLabel);
            this.Controls.Add(this.keyboardTrackLabel);
            this.Controls.Add(this.cmdPickTrack);
            this.Controls.Add(this.keyboardLabel);
            this.Controls.Add(this.mouseLabel);
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
        private System.Windows.Forms.Label mouseLabel;
        private System.Windows.Forms.Label keyboardLabel;
        private System.Windows.Forms.Button cmdPickTrack;
        private System.Windows.Forms.Label keyboardTrackLabel;
        private System.Windows.Forms.Label mouseTrackLabel;
    }
}