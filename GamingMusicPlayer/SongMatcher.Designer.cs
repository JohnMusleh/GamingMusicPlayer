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
            this.txtLogs = new System.Windows.Forms.TextBox();
            this.cmdClearLogs = new System.Windows.Forms.Button();
            this.mouseLabel = new System.Windows.Forms.Label();
            this.keyboardLabel = new System.Windows.Forms.Label();
            this.teamspeakPeakLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(236, 103);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(80, 17);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "status-here";
            // 
            // cmdToggleMatching
            // 
            this.cmdToggleMatching.Location = new System.Drawing.Point(84, 83);
            this.cmdToggleMatching.Name = "cmdToggleMatching";
            this.cmdToggleMatching.Size = new System.Drawing.Size(125, 56);
            this.cmdToggleMatching.TabIndex = 1;
            this.cmdToggleMatching.Text = "Start Matching";
            this.cmdToggleMatching.UseVisualStyleBackColor = true;
            this.cmdToggleMatching.Click += new System.EventHandler(this.cmdToggleMatching_Click);
            // 
            // txtLogs
            // 
            this.txtLogs.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtLogs.Location = new System.Drawing.Point(334, 83);
            this.txtLogs.Multiline = true;
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.Size = new System.Drawing.Size(387, 279);
            this.txtLogs.TabIndex = 2;
            // 
            // cmdClearLogs
            // 
            this.cmdClearLogs.Location = new System.Drawing.Point(612, 382);
            this.cmdClearLogs.Name = "cmdClearLogs";
            this.cmdClearLogs.Size = new System.Drawing.Size(98, 56);
            this.cmdClearLogs.TabIndex = 3;
            this.cmdClearLogs.Text = "Clear Logs";
            this.cmdClearLogs.UseVisualStyleBackColor = true;
            this.cmdClearLogs.Click += new System.EventHandler(this.cmdClearLogs_Click);
            // 
            // mouseLabel
            // 
            this.mouseLabel.AutoSize = true;
            this.mouseLabel.Location = new System.Drawing.Point(81, 9);
            this.mouseLabel.Name = "mouseLabel";
            this.mouseLabel.Size = new System.Drawing.Size(83, 17);
            this.mouseLabel.TabIndex = 4;
            this.mouseLabel.Text = "mouseBPM:";
            // 
            // keyboardLabel
            // 
            this.keyboardLabel.AutoSize = true;
            this.keyboardLabel.Location = new System.Drawing.Point(81, 38);
            this.keyboardLabel.Name = "keyboardLabel";
            this.keyboardLabel.Size = new System.Drawing.Size(100, 17);
            this.keyboardLabel.TabIndex = 5;
            this.keyboardLabel.Text = "keyboardBPM:";
            // 
            // teamspeakPeakLabel
            // 
            this.teamspeakPeakLabel.AutoSize = true;
            this.teamspeakPeakLabel.Location = new System.Drawing.Point(97, 277);
            this.teamspeakPeakLabel.Name = "teamspeakPeakLabel";
            this.teamspeakPeakLabel.Size = new System.Drawing.Size(112, 17);
            this.teamspeakPeakLabel.TabIndex = 6;
            this.teamspeakPeakLabel.Text = "teamspeak peak";
            // 
            // SongMatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.teamspeakPeakLabel);
            this.Controls.Add(this.keyboardLabel);
            this.Controls.Add(this.mouseLabel);
            this.Controls.Add(this.cmdClearLogs);
            this.Controls.Add(this.txtLogs);
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
        private System.Windows.Forms.TextBox txtLogs;
        private System.Windows.Forms.Button cmdClearLogs;
        private System.Windows.Forms.Label mouseLabel;
        private System.Windows.Forms.Label keyboardLabel;
        private System.Windows.Forms.Label teamspeakPeakLabel;
    }
}