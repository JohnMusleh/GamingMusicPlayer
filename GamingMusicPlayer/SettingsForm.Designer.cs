namespace GamingMusicPlayer
{
    partial class SettingsForm
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
            this.overlayLabel = new System.Windows.Forms.Label();
            this.autoPickLabel = new System.Windows.Forms.Label();
            this.vpFeatureLabel = new System.Windows.Forms.Label();
            this.cmdToggleVPFeature = new System.Windows.Forms.Button();
            this.vpFeatureSkypeCB = new System.Windows.Forms.CheckBox();
            this.vpFeatureTeamspeakCB = new System.Windows.Forms.CheckBox();
            this.vpFeatureDiscordCB = new System.Windows.Forms.CheckBox();
            this.cmdToggleOverlay = new System.Windows.Forms.Button();
            this.overlayClickableCB = new System.Windows.Forms.CheckBox();
            this.cmdAutoPickToggle = new System.Windows.Forms.Button();
            this.autoPickGameplayLabel = new System.Windows.Forms.Label();
            this.gameplayTooltipLabel = new System.Windows.Forms.Label();
            this.gameplayTrackBar = new System.Windows.Forms.TrackBar();
            this.gameplayMouseLabel = new System.Windows.Forms.Label();
            this.gameplayKeyboardLabel = new System.Windows.Forms.Label();
            this.txtAboutLbl = new System.Windows.Forms.Label();
            this.txtAboutCntxt = new System.Windows.Forms.RichTextBox();
            this.txtVersion = new System.Windows.Forms.Label();
            this.txtDbLbl = new System.Windows.Forms.Label();
            this.cmdClearDb = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gameplayTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // overlayLabel
            // 
            this.overlayLabel.AutoSize = true;
            this.overlayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayLabel.Location = new System.Drawing.Point(374, 24);
            this.overlayLabel.Name = "overlayLabel";
            this.overlayLabel.Size = new System.Drawing.Size(73, 20);
            this.overlayLabel.TabIndex = 0;
            this.overlayLabel.Text = "Overlay";
            // 
            // autoPickLabel
            // 
            this.autoPickLabel.AutoSize = true;
            this.autoPickLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoPickLabel.Location = new System.Drawing.Point(6, 247);
            this.autoPickLabel.Name = "autoPickLabel";
            this.autoPickLabel.Size = new System.Drawing.Size(183, 20);
            this.autoPickLabel.TabIndex = 1;
            this.autoPickLabel.Text = "Automatic Song Pick";
            // 
            // vpFeatureLabel
            // 
            this.vpFeatureLabel.AutoSize = true;
            this.vpFeatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vpFeatureLabel.Location = new System.Drawing.Point(8, 24);
            this.vpFeatureLabel.Name = "vpFeatureLabel";
            this.vpFeatureLabel.Size = new System.Drawing.Size(170, 20);
            this.vpFeatureLabel.TabIndex = 2;
            this.vpFeatureLabel.Text = "Voice Prioritization";
            // 
            // cmdToggleVPFeature
            // 
            this.cmdToggleVPFeature.Location = new System.Drawing.Point(67, 47);
            this.cmdToggleVPFeature.Name = "cmdToggleVPFeature";
            this.cmdToggleVPFeature.Size = new System.Drawing.Size(75, 23);
            this.cmdToggleVPFeature.TabIndex = 6;
            this.cmdToggleVPFeature.Text = "OFF";
            this.cmdToggleVPFeature.UseVisualStyleBackColor = true;
            this.cmdToggleVPFeature.Click += new System.EventHandler(this.cmdToggleVPFeature_Click);
            // 
            // vpFeatureSkypeCB
            // 
            this.vpFeatureSkypeCB.AutoSize = true;
            this.vpFeatureSkypeCB.Location = new System.Drawing.Point(67, 83);
            this.vpFeatureSkypeCB.Name = "vpFeatureSkypeCB";
            this.vpFeatureSkypeCB.Size = new System.Drawing.Size(69, 21);
            this.vpFeatureSkypeCB.TabIndex = 7;
            this.vpFeatureSkypeCB.Text = "Skype";
            this.vpFeatureSkypeCB.UseVisualStyleBackColor = true;
            this.vpFeatureSkypeCB.CheckedChanged += new System.EventHandler(this.vpFeatureSkypeCB_CheckedChanged);
            // 
            // vpFeatureTeamspeakCB
            // 
            this.vpFeatureTeamspeakCB.AutoSize = true;
            this.vpFeatureTeamspeakCB.Location = new System.Drawing.Point(67, 110);
            this.vpFeatureTeamspeakCB.Name = "vpFeatureTeamspeakCB";
            this.vpFeatureTeamspeakCB.Size = new System.Drawing.Size(56, 21);
            this.vpFeatureTeamspeakCB.TabIndex = 8;
            this.vpFeatureTeamspeakCB.Text = "TS3";
            this.vpFeatureTeamspeakCB.UseVisualStyleBackColor = true;
            this.vpFeatureTeamspeakCB.CheckedChanged += new System.EventHandler(this.vpFeatureTeamspeakCB_CheckedChanged);
            // 
            // vpFeatureDiscordCB
            // 
            this.vpFeatureDiscordCB.AutoSize = true;
            this.vpFeatureDiscordCB.Location = new System.Drawing.Point(67, 137);
            this.vpFeatureDiscordCB.Name = "vpFeatureDiscordCB";
            this.vpFeatureDiscordCB.Size = new System.Drawing.Size(78, 21);
            this.vpFeatureDiscordCB.TabIndex = 9;
            this.vpFeatureDiscordCB.Text = "Discord";
            this.vpFeatureDiscordCB.UseVisualStyleBackColor = true;
            this.vpFeatureDiscordCB.CheckedChanged += new System.EventHandler(this.vpFeatureDiscordCB_CheckedChanged);
            // 
            // cmdToggleOverlay
            // 
            this.cmdToggleOverlay.Location = new System.Drawing.Point(399, 47);
            this.cmdToggleOverlay.Name = "cmdToggleOverlay";
            this.cmdToggleOverlay.Size = new System.Drawing.Size(75, 23);
            this.cmdToggleOverlay.TabIndex = 10;
            this.cmdToggleOverlay.Text = "OFF";
            this.cmdToggleOverlay.UseVisualStyleBackColor = true;
            this.cmdToggleOverlay.Click += new System.EventHandler(this.cmdToggleOverlay_Click);
            // 
            // overlayClickableCB
            // 
            this.overlayClickableCB.AutoSize = true;
            this.overlayClickableCB.Location = new System.Drawing.Point(399, 83);
            this.overlayClickableCB.Name = "overlayClickableCB";
            this.overlayClickableCB.Size = new System.Drawing.Size(194, 21);
            this.overlayClickableCB.TabIndex = 11;
            this.overlayClickableCB.Text = "Clickable (use to relocate)";
            this.overlayClickableCB.UseVisualStyleBackColor = true;
            this.overlayClickableCB.CheckedChanged += new System.EventHandler(this.overlayClickableCB_CheckedChanged);
            // 
            // cmdAutoPickToggle
            // 
            this.cmdAutoPickToggle.Location = new System.Drawing.Point(67, 280);
            this.cmdAutoPickToggle.Name = "cmdAutoPickToggle";
            this.cmdAutoPickToggle.Size = new System.Drawing.Size(75, 23);
            this.cmdAutoPickToggle.TabIndex = 12;
            this.cmdAutoPickToggle.Text = "OFF";
            this.cmdAutoPickToggle.UseVisualStyleBackColor = true;
            this.cmdAutoPickToggle.Click += new System.EventHandler(this.cmdAutoPickToggle_Click);
            // 
            // autoPickGameplayLabel
            // 
            this.autoPickGameplayLabel.AutoSize = true;
            this.autoPickGameplayLabel.Location = new System.Drawing.Point(69, 332);
            this.autoPickGameplayLabel.Name = "autoPickGameplayLabel";
            this.autoPickGameplayLabel.Size = new System.Drawing.Size(72, 17);
            this.autoPickGameplayLabel.TabIndex = 13;
            this.autoPickGameplayLabel.Text = "Gameplay";
            this.autoPickGameplayLabel.Visible = false;
            // 
            // gameplayTooltipLabel
            // 
            this.gameplayTooltipLabel.AutoSize = true;
            this.gameplayTooltipLabel.Cursor = System.Windows.Forms.Cursors.Help;
            this.gameplayTooltipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameplayTooltipLabel.Location = new System.Drawing.Point(147, 329);
            this.gameplayTooltipLabel.Name = "gameplayTooltipLabel";
            this.gameplayTooltipLabel.Size = new System.Drawing.Size(19, 20);
            this.gameplayTooltipLabel.TabIndex = 14;
            this.gameplayTooltipLabel.Text = "?";
            this.gameplayTooltipLabel.Visible = false;
            // 
            // gameplayTrackBar
            // 
            this.gameplayTrackBar.Location = new System.Drawing.Point(172, 352);
            this.gameplayTrackBar.Name = "gameplayTrackBar";
            this.gameplayTrackBar.Size = new System.Drawing.Size(104, 56);
            this.gameplayTrackBar.TabIndex = 15;
            this.gameplayTrackBar.Visible = false;
            this.gameplayTrackBar.ValueChanged += new System.EventHandler(this.gameplayTrackBar_ValueChanged);
            // 
            // gameplayMouseLabel
            // 
            this.gameplayMouseLabel.AutoSize = true;
            this.gameplayMouseLabel.Location = new System.Drawing.Point(282, 362);
            this.gameplayMouseLabel.Name = "gameplayMouseLabel";
            this.gameplayMouseLabel.Size = new System.Drawing.Size(50, 17);
            this.gameplayMouseLabel.TabIndex = 16;
            this.gameplayMouseLabel.Text = "mouse";
            this.gameplayMouseLabel.Visible = false;
            // 
            // gameplayKeyboardLabel
            // 
            this.gameplayKeyboardLabel.AutoSize = true;
            this.gameplayKeyboardLabel.Location = new System.Drawing.Point(99, 362);
            this.gameplayKeyboardLabel.Name = "gameplayKeyboardLabel";
            this.gameplayKeyboardLabel.Size = new System.Drawing.Size(67, 17);
            this.gameplayKeyboardLabel.TabIndex = 17;
            this.gameplayKeyboardLabel.Text = "keyboard";
            this.gameplayKeyboardLabel.Visible = false;
            // 
            // txtAboutLbl
            // 
            this.txtAboutLbl.AutoSize = true;
            this.txtAboutLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAboutLbl.Location = new System.Drawing.Point(374, 247);
            this.txtAboutLbl.Name = "txtAboutLbl";
            this.txtAboutLbl.Size = new System.Drawing.Size(57, 20);
            this.txtAboutLbl.TabIndex = 18;
            this.txtAboutLbl.Text = "About";
            // 
            // txtAboutCntxt
            // 
            this.txtAboutCntxt.Location = new System.Drawing.Point(399, 303);
            this.txtAboutCntxt.Name = "txtAboutCntxt";
            this.txtAboutCntxt.Size = new System.Drawing.Size(319, 169);
            this.txtAboutCntxt.TabIndex = 19;
            this.txtAboutCntxt.Text = "";
            this.txtAboutCntxt.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.txtAboutCntxt_LinkClicked);
            // 
            // txtVersion
            // 
            this.txtVersion.AutoSize = true;
            this.txtVersion.Location = new System.Drawing.Point(396, 274);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(69, 17);
            this.txtVersion.TabIndex = 20;
            this.txtVersion.Text = "VERSION";
            // 
            // txtDbLbl
            // 
            this.txtDbLbl.AutoSize = true;
            this.txtDbLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDbLbl.Location = new System.Drawing.Point(4, 362);
            this.txtDbLbl.Name = "txtDbLbl";
            this.txtDbLbl.Size = new System.Drawing.Size(89, 20);
            this.txtDbLbl.TabIndex = 21;
            this.txtDbLbl.Text = "Database";
            // 
            // cmdClearDb
            // 
            this.cmdClearDb.Location = new System.Drawing.Point(65, 385);
            this.cmdClearDb.Name = "cmdClearDb";
            this.cmdClearDb.Size = new System.Drawing.Size(99, 36);
            this.cmdClearDb.TabIndex = 22;
            this.cmdClearDb.Text = "Clear Data";
            this.cmdClearDb.UseVisualStyleBackColor = true;
            this.cmdClearDb.Click += new System.EventHandler(this.cmdClearDb_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 544);
            this.Controls.Add(this.cmdClearDb);
            this.Controls.Add(this.txtDbLbl);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.txtAboutCntxt);
            this.Controls.Add(this.txtAboutLbl);
            this.Controls.Add(this.gameplayKeyboardLabel);
            this.Controls.Add(this.gameplayMouseLabel);
            this.Controls.Add(this.gameplayTrackBar);
            this.Controls.Add(this.gameplayTooltipLabel);
            this.Controls.Add(this.autoPickGameplayLabel);
            this.Controls.Add(this.cmdAutoPickToggle);
            this.Controls.Add(this.overlayClickableCB);
            this.Controls.Add(this.cmdToggleOverlay);
            this.Controls.Add(this.vpFeatureDiscordCB);
            this.Controls.Add(this.vpFeatureTeamspeakCB);
            this.Controls.Add(this.vpFeatureSkypeCB);
            this.Controls.Add(this.cmdToggleVPFeature);
            this.Controls.Add(this.vpFeatureLabel);
            this.Controls.Add(this.autoPickLabel);
            this.Controls.Add(this.overlayLabel);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.gameplayTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label overlayLabel;
        private System.Windows.Forms.Label autoPickLabel;
        private System.Windows.Forms.Label vpFeatureLabel;
        private System.Windows.Forms.Button cmdToggleVPFeature;
        private System.Windows.Forms.CheckBox vpFeatureSkypeCB;
        private System.Windows.Forms.CheckBox vpFeatureTeamspeakCB;
        private System.Windows.Forms.CheckBox vpFeatureDiscordCB;
        private System.Windows.Forms.Button cmdToggleOverlay;
        private System.Windows.Forms.CheckBox overlayClickableCB;
        private System.Windows.Forms.Button cmdAutoPickToggle;
        private System.Windows.Forms.Label autoPickGameplayLabel;
        private System.Windows.Forms.Label gameplayTooltipLabel;
        private System.Windows.Forms.TrackBar gameplayTrackBar;
        private System.Windows.Forms.Label gameplayMouseLabel;
        private System.Windows.Forms.Label gameplayKeyboardLabel;
        private System.Windows.Forms.Label txtAboutLbl;
        private System.Windows.Forms.RichTextBox txtAboutCntxt;
        private System.Windows.Forms.Label txtVersion;
        private System.Windows.Forms.Label txtDbLbl;
        private System.Windows.Forms.Button cmdClearDb;
    }
}