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
            this.hotkeysLabel = new System.Windows.Forms.Label();
            this.vpFeatureLabel = new System.Windows.Forms.Label();
            this.cmdToggleVPFeature = new System.Windows.Forms.Button();
            this.vpFeatureSkypeCB = new System.Windows.Forms.CheckBox();
            this.vpFeatureTeamspeakCB = new System.Windows.Forms.CheckBox();
            this.vpFeatureDiscordCB = new System.Windows.Forms.CheckBox();
            this.cmdToggleOverlay = new System.Windows.Forms.Button();
            this.overlayClickableCB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // overlayLabel
            // 
            this.overlayLabel.AutoSize = true;
            this.overlayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayLabel.Location = new System.Drawing.Point(392, 24);
            this.overlayLabel.Name = "overlayLabel";
            this.overlayLabel.Size = new System.Drawing.Size(73, 20);
            this.overlayLabel.TabIndex = 0;
            this.overlayLabel.Text = "Overlay";
            // 
            // hotkeysLabel
            // 
            this.hotkeysLabel.AutoSize = true;
            this.hotkeysLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hotkeysLabel.Location = new System.Drawing.Point(6, 247);
            this.hotkeysLabel.Name = "hotkeysLabel";
            this.hotkeysLabel.Size = new System.Drawing.Size(117, 32);
            this.hotkeysLabel.TabIndex = 1;
            this.hotkeysLabel.Text = "Hotkeys";
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
            this.cmdToggleOverlay.Location = new System.Drawing.Point(437, 47);
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
            this.overlayClickableCB.Location = new System.Drawing.Point(437, 83);
            this.overlayClickableCB.Name = "overlayClickableCB";
            this.overlayClickableCB.Size = new System.Drawing.Size(194, 21);
            this.overlayClickableCB.TabIndex = 11;
            this.overlayClickableCB.Text = "Clickable (use to relocate)";
            this.overlayClickableCB.UseVisualStyleBackColor = true;
            this.overlayClickableCB.CheckedChanged += new System.EventHandler(this.overlayClickableCB_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.overlayClickableCB);
            this.Controls.Add(this.cmdToggleOverlay);
            this.Controls.Add(this.vpFeatureDiscordCB);
            this.Controls.Add(this.vpFeatureTeamspeakCB);
            this.Controls.Add(this.vpFeatureSkypeCB);
            this.Controls.Add(this.cmdToggleVPFeature);
            this.Controls.Add(this.vpFeatureLabel);
            this.Controls.Add(this.hotkeysLabel);
            this.Controls.Add(this.overlayLabel);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label overlayLabel;
        private System.Windows.Forms.Label hotkeysLabel;
        private System.Windows.Forms.Label vpFeatureLabel;
        private System.Windows.Forms.Button cmdToggleVPFeature;
        private System.Windows.Forms.CheckBox vpFeatureSkypeCB;
        private System.Windows.Forms.CheckBox vpFeatureTeamspeakCB;
        private System.Windows.Forms.CheckBox vpFeatureDiscordCB;
        private System.Windows.Forms.Button cmdToggleOverlay;
        private System.Windows.Forms.CheckBox overlayClickableCB;
    }
}