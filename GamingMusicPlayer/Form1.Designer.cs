namespace GamingMusicPlayer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cmdRemove = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.seekLabel = new System.Windows.Forms.Label();
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.musicTrackBar = new System.Windows.Forms.TrackBar();
            this.txtMusicProgress = new System.Windows.Forms.Label();
            this.bottomLeftIcon = new GamingMusicPlayer.CMButton();
            this.cmdPlayPause = new GamingMusicPlayer.CMButton();
            this.cmdPrev = new GamingMusicPlayer.CMButton();
            this.cmdNext = new GamingMusicPlayer.CMButton();
            this.cmdShuffle = new GamingMusicPlayer.CMButton();
            this.cmdLoop = new GamingMusicPlayer.CMButton();
            this.cmdAddSong = new System.Windows.Forms.Button();
            this.nameListBox = new System.Windows.Forms.ListBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.lengthLabel = new System.Windows.Forms.Label();
            this.lengthListBox = new System.Windows.Forms.ListBox();
            this.cmdLogger = new System.Windows.Forms.Button();
            this.cmdShowGrapher = new System.Windows.Forms.Button();
            this.cmdToggleMatcher = new System.Windows.Forms.Button();
            this.featuresLabel = new System.Windows.Forms.Label();
            this.cmdScan = new System.Windows.Forms.Button();
            this.cmdViewDB = new System.Windows.Forms.Button();
            this.cmdSettings = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdRemove
            // 
            this.cmdRemove.Enabled = false;
            this.cmdRemove.Location = new System.Drawing.Point(998, 81);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(125, 35);
            this.cmdRemove.TabIndex = 6;
            this.cmdRemove.Text = "Remove";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.volumeLabel);
            this.panel1.Controls.Add(this.seekLabel);
            this.panel1.Controls.Add(this.volumeTrackBar);
            this.panel1.Controls.Add(this.musicTrackBar);
            this.panel1.Controls.Add(this.txtMusicProgress);
            this.panel1.Controls.Add(this.bottomLeftIcon);
            this.panel1.Controls.Add(this.cmdPlayPause);
            this.panel1.Controls.Add(this.cmdPrev);
            this.panel1.Controls.Add(this.cmdNext);
            this.panel1.Controls.Add(this.cmdShuffle);
            this.panel1.Controls.Add(this.cmdLoop);
            this.panel1.Location = new System.Drawing.Point(0, 445);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1179, 192);
            this.panel1.TabIndex = 7;
            // 
            // volumeLabel
            // 
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.BackColor = System.Drawing.Color.Transparent;
            this.volumeLabel.Location = new System.Drawing.Point(975, 33);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(40, 17);
            this.volumeLabel.TabIndex = 22;
            this.volumeLabel.Text = "Vol%";
            // 
            // seekLabel
            // 
            this.seekLabel.AutoSize = true;
            this.seekLabel.Location = new System.Drawing.Point(211, 6);
            this.seekLabel.Name = "seekLabel";
            this.seekLabel.Size = new System.Drawing.Size(73, 17);
            this.seekLabel.TabIndex = 15;
            this.seekLabel.Text = "seekLabel";
            // 
            // volumeTrackBar
            // 
            this.volumeTrackBar.LargeChange = 10;
            this.volumeTrackBar.Location = new System.Drawing.Point(1021, 33);
            this.volumeTrackBar.Maximum = 1000;
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Size = new System.Drawing.Size(111, 56);
            this.volumeTrackBar.TabIndex = 1;
            this.volumeTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volumeTrackBar.Value = 1000;
            this.volumeTrackBar.Scroll += new System.EventHandler(this.volumeTrackBar_Scroll);
            // 
            // musicTrackBar
            // 
            this.musicTrackBar.BackColor = System.Drawing.Color.DimGray;
            this.musicTrackBar.Location = new System.Drawing.Point(316, 3);
            this.musicTrackBar.Name = "musicTrackBar";
            this.musicTrackBar.Size = new System.Drawing.Size(586, 56);
            this.musicTrackBar.TabIndex = 1;
            this.musicTrackBar.TickFrequency = 0;
            this.musicTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.musicTrackBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.musicTrackBar_MouseDown);
            this.musicTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.musicTrackBar_MouseUp);
            // 
            // txtMusicProgress
            // 
            this.txtMusicProgress.AutoSize = true;
            this.txtMusicProgress.Location = new System.Drawing.Point(900, 6);
            this.txtMusicProgress.Name = "txtMusicProgress";
            this.txtMusicProgress.Size = new System.Drawing.Size(115, 17);
            this.txtMusicProgress.TabIndex = 14;
            this.txtMusicProgress.Text = "txtMusicProgress";
            // 
            // bottomLeftIcon
            // 
            this.bottomLeftIcon.BackColor = System.Drawing.Color.Transparent;
            this.bottomLeftIcon.FlatAppearance.BorderSize = 0;
            this.bottomLeftIcon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.bottomLeftIcon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.bottomLeftIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bottomLeftIcon.Image = global::GamingMusicPlayer.Properties.Resources.controllericon2;
            this.bottomLeftIcon.Location = new System.Drawing.Point(-32, 3);
            this.bottomLeftIcon.Name = "bottomLeftIcon";
            this.bottomLeftIcon.Size = new System.Drawing.Size(360, 189);
            this.bottomLeftIcon.TabIndex = 5;
            this.bottomLeftIcon.UseVisualStyleBackColor = false;
            // 
            // cmdPlayPause
            // 
            this.cmdPlayPause.BackColor = System.Drawing.Color.Transparent;
            this.cmdPlayPause.FlatAppearance.BorderSize = 0;
            this.cmdPlayPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdPlayPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdPlayPause.Image = global::GamingMusicPlayer.Properties.Resources.play_white;
            this.cmdPlayPause.Location = new System.Drawing.Point(512, 42);
            this.cmdPlayPause.Name = "cmdPlayPause";
            this.cmdPlayPause.Size = new System.Drawing.Size(200, 147);
            this.cmdPlayPause.TabIndex = 0;
            this.cmdPlayPause.UseVisualStyleBackColor = false;
            this.cmdPlayPause.Click += new System.EventHandler(this.cmdPlayPause_Click);
            // 
            // cmdPrev
            // 
            this.cmdPrev.BackColor = System.Drawing.Color.Transparent;
            this.cmdPrev.FlatAppearance.BorderSize = 0;
            this.cmdPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdPrev.Image = global::GamingMusicPlayer.Properties.Resources.back_white;
            this.cmdPrev.Location = new System.Drawing.Point(344, 57);
            this.cmdPrev.Name = "cmdPrev";
            this.cmdPrev.Size = new System.Drawing.Size(146, 132);
            this.cmdPrev.TabIndex = 2;
            this.cmdPrev.UseVisualStyleBackColor = false;
            this.cmdPrev.Click += new System.EventHandler(this.cmdPrev_Click);
            // 
            // cmdNext
            // 
            this.cmdNext.BackColor = System.Drawing.Color.Transparent;
            this.cmdNext.FlatAppearance.BorderSize = 0;
            this.cmdNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdNext.Image = global::GamingMusicPlayer.Properties.Resources.next_white;
            this.cmdNext.Location = new System.Drawing.Point(740, 57);
            this.cmdNext.Name = "cmdNext";
            this.cmdNext.Size = new System.Drawing.Size(146, 132);
            this.cmdNext.TabIndex = 1;
            this.cmdNext.UseVisualStyleBackColor = false;
            this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
            // 
            // cmdShuffle
            // 
            this.cmdShuffle.BackColor = System.Drawing.Color.Transparent;
            this.cmdShuffle.FlatAppearance.BorderSize = 0;
            this.cmdShuffle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdShuffle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdShuffle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdShuffle.Image = global::GamingMusicPlayer.Properties.Resources.shuffle_white;
            this.cmdShuffle.Location = new System.Drawing.Point(959, 77);
            this.cmdShuffle.Name = "cmdShuffle";
            this.cmdShuffle.Size = new System.Drawing.Size(91, 92);
            this.cmdShuffle.TabIndex = 4;
            this.cmdShuffle.UseVisualStyleBackColor = false;
            this.cmdShuffle.Click += new System.EventHandler(this.cmdShuffle_Click);
            // 
            // cmdLoop
            // 
            this.cmdLoop.BackColor = System.Drawing.Color.Transparent;
            this.cmdLoop.FlatAppearance.BorderSize = 0;
            this.cmdLoop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdLoop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdLoop.Image = global::GamingMusicPlayer.Properties.Resources.loop_white;
            this.cmdLoop.Location = new System.Drawing.Point(1072, 77);
            this.cmdLoop.Name = "cmdLoop";
            this.cmdLoop.Size = new System.Drawing.Size(91, 92);
            this.cmdLoop.TabIndex = 3;
            this.cmdLoop.UseVisualStyleBackColor = false;
            this.cmdLoop.Click += new System.EventHandler(this.cmdLoop_Click);
            // 
            // cmdAddSong
            // 
            this.cmdAddSong.Location = new System.Drawing.Point(998, 40);
            this.cmdAddSong.Name = "cmdAddSong";
            this.cmdAddSong.Size = new System.Drawing.Size(125, 35);
            this.cmdAddSong.TabIndex = 8;
            this.cmdAddSong.Text = "Add Song";
            this.cmdAddSong.UseVisualStyleBackColor = true;
            this.cmdAddSong.Click += new System.EventHandler(this.cmdAddSong_Click);
            // 
            // nameListBox
            // 
            this.nameListBox.FormattingEnabled = true;
            this.nameListBox.ItemHeight = 16;
            this.nameListBox.Location = new System.Drawing.Point(393, 40);
            this.nameListBox.Name = "nameListBox";
            this.nameListBox.Size = new System.Drawing.Size(381, 372);
            this.nameListBox.TabIndex = 10;
            this.nameListBox.SelectedIndexChanged += new System.EventHandler(this.nameListBox_SelectedIndexChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(403, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(45, 17);
            this.nameLabel.TabIndex = 12;
            this.nameLabel.Text = "Name";
            // 
            // lengthLabel
            // 
            this.lengthLabel.AutoSize = true;
            this.lengthLabel.Location = new System.Drawing.Point(787, 9);
            this.lengthLabel.Name = "lengthLabel";
            this.lengthLabel.Size = new System.Drawing.Size(52, 17);
            this.lengthLabel.TabIndex = 13;
            this.lengthLabel.Text = "Length";
            // 
            // lengthListBox
            // 
            this.lengthListBox.FormattingEnabled = true;
            this.lengthListBox.ItemHeight = 16;
            this.lengthListBox.Location = new System.Drawing.Point(780, 40);
            this.lengthListBox.Name = "lengthListBox";
            this.lengthListBox.Size = new System.Drawing.Size(199, 372);
            this.lengthListBox.TabIndex = 11;
            this.lengthListBox.SelectedIndexChanged += new System.EventHandler(this.lengthListBox_SelectedIndexChanged);
            // 
            // cmdLogger
            // 
            this.cmdLogger.Location = new System.Drawing.Point(12, 341);
            this.cmdLogger.Name = "cmdLogger";
            this.cmdLogger.Size = new System.Drawing.Size(147, 41);
            this.cmdLogger.TabIndex = 14;
            this.cmdLogger.Text = "Show logger";
            this.cmdLogger.UseVisualStyleBackColor = true;
            this.cmdLogger.Click += new System.EventHandler(this.cmdLogger_Click);
            // 
            // cmdShowGrapher
            // 
            this.cmdShowGrapher.Location = new System.Drawing.Point(12, 285);
            this.cmdShowGrapher.Name = "cmdShowGrapher";
            this.cmdShowGrapher.Size = new System.Drawing.Size(147, 41);
            this.cmdShowGrapher.TabIndex = 15;
            this.cmdShowGrapher.Text = "Show Grapher";
            this.cmdShowGrapher.UseVisualStyleBackColor = true;
            this.cmdShowGrapher.Click += new System.EventHandler(this.cmdShowGrapher_Click);
            // 
            // cmdToggleMatcher
            // 
            this.cmdToggleMatcher.Location = new System.Drawing.Point(998, 327);
            this.cmdToggleMatcher.Name = "cmdToggleMatcher";
            this.cmdToggleMatcher.Size = new System.Drawing.Size(125, 35);
            this.cmdToggleMatcher.TabIndex = 16;
            this.cmdToggleMatcher.Text = "Show Matcher";
            this.cmdToggleMatcher.UseVisualStyleBackColor = true;
            this.cmdToggleMatcher.Click += new System.EventHandler(this.cmdToggleMatcher_Click);
            // 
            // featuresLabel
            // 
            this.featuresLabel.AutoSize = true;
            this.featuresLabel.Location = new System.Drawing.Point(1034, 297);
            this.featuresLabel.Name = "featuresLabel";
            this.featuresLabel.Size = new System.Drawing.Size(64, 17);
            this.featuresLabel.TabIndex = 18;
            this.featuresLabel.Text = "Features";
            // 
            // cmdScan
            // 
            this.cmdScan.Location = new System.Drawing.Point(12, 22);
            this.cmdScan.Name = "cmdScan";
            this.cmdScan.Size = new System.Drawing.Size(147, 44);
            this.cmdScan.TabIndex = 19;
            this.cmdScan.Text = "Scan";
            this.cmdScan.UseVisualStyleBackColor = true;
            this.cmdScan.Click += new System.EventHandler(this.cmdScan_Click);
            // 
            // cmdViewDB
            // 
            this.cmdViewDB.Location = new System.Drawing.Point(12, 226);
            this.cmdViewDB.Name = "cmdViewDB";
            this.cmdViewDB.Size = new System.Drawing.Size(147, 41);
            this.cmdViewDB.TabIndex = 20;
            this.cmdViewDB.Text = "View DB";
            this.cmdViewDB.UseVisualStyleBackColor = true;
            this.cmdViewDB.Click += new System.EventHandler(this.cmdViewDB_Click);
            // 
            // cmdSettings
            // 
            this.cmdSettings.Location = new System.Drawing.Point(998, 377);
            this.cmdSettings.Name = "cmdSettings";
            this.cmdSettings.Size = new System.Drawing.Size(125, 35);
            this.cmdSettings.TabIndex = 21;
            this.cmdSettings.Text = "Settings";
            this.cmdSettings.UseVisualStyleBackColor = true;
            this.cmdSettings.Click += new System.EventHandler(this.cmdSettings_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1174, 633);
            this.Controls.Add(this.cmdSettings);
            this.Controls.Add(this.cmdViewDB);
            this.Controls.Add(this.cmdScan);
            this.Controls.Add(this.featuresLabel);
            this.Controls.Add(this.cmdToggleMatcher);
            this.Controls.Add(this.cmdShowGrapher);
            this.Controls.Add(this.cmdLogger);
            this.Controls.Add(this.lengthLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.lengthListBox);
            this.Controls.Add(this.nameListBox);
            this.Controls.Add(this.cmdAddSong);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmdRemove);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "GamingMusicPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CMButton cmdPlayPause;
        private CMButton cmdNext;
        private CMButton cmdPrev;
        private CMButton cmdLoop;
        private CMButton cmdShuffle;
        private System.Windows.Forms.Button cmdRemove;
        private System.Windows.Forms.Panel panel1;
        private CMButton bottomLeftIcon;
        private System.Windows.Forms.Button cmdAddSong;
        private System.Windows.Forms.ListBox nameListBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label lengthLabel;
        private System.Windows.Forms.ListBox lengthListBox;
        private System.Windows.Forms.TrackBar musicTrackBar;
        private System.Windows.Forms.Label txtMusicProgress;
        private System.Windows.Forms.Label seekLabel;
        private System.Windows.Forms.Button cmdLogger;
        private System.Windows.Forms.Button cmdShowGrapher;
        private System.Windows.Forms.Button cmdToggleMatcher;
        private System.Windows.Forms.Label featuresLabel;
        private System.Windows.Forms.Button cmdScan;
        private System.Windows.Forms.Button cmdViewDB;
        private System.Windows.Forms.Button cmdSettings;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.Label volumeLabel;
    }
}

