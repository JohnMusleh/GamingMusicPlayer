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
            this.FeaturesLabel = new System.Windows.Forms.Label();
            this.hotkeysLabel = new System.Windows.Forms.Label();
            this.vpFeatureLabel = new System.Windows.Forms.Label();
            this.cmdToggleVPFeature = new System.Windows.Forms.Button();
            this.vpFeatureSkypeCB = new System.Windows.Forms.CheckBox();
            this.vpFeatureTeamspeakCB = new System.Windows.Forms.CheckBox();
            this.vpFeatureDiscordCB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // FeaturesLabel
            // 
            this.FeaturesLabel.AutoSize = true;
            this.FeaturesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FeaturesLabel.Location = new System.Drawing.Point(25, 28);
            this.FeaturesLabel.Name = "FeaturesLabel";
            this.FeaturesLabel.Size = new System.Drawing.Size(127, 32);
            this.FeaturesLabel.TabIndex = 0;
            this.FeaturesLabel.Text = "Features";
            // 
            // hotkeysLabel
            // 
            this.hotkeysLabel.AutoSize = true;
            this.hotkeysLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hotkeysLabel.Location = new System.Drawing.Point(25, 238);
            this.hotkeysLabel.Name = "hotkeysLabel";
            this.hotkeysLabel.Size = new System.Drawing.Size(117, 32);
            this.hotkeysLabel.TabIndex = 1;
            this.hotkeysLabel.Text = "Hotkeys";
            // 
            // vpFeatureLabel
            // 
            this.vpFeatureLabel.AutoSize = true;
            this.vpFeatureLabel.Location = new System.Drawing.Point(219, 28);
            this.vpFeatureLabel.Name = "vpFeatureLabel";
            this.vpFeatureLabel.Size = new System.Drawing.Size(125, 17);
            this.vpFeatureLabel.TabIndex = 2;
            this.vpFeatureLabel.Text = "Voice Prioritization";
            // 
            // cmdToggleVPFeature
            // 
            this.cmdToggleVPFeature.Location = new System.Drawing.Point(355, 25);
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
            this.vpFeatureSkypeCB.Location = new System.Drawing.Point(355, 63);
            this.vpFeatureSkypeCB.Name = "vpFeatureSkypeCB";
            this.vpFeatureSkypeCB.Size = new System.Drawing.Size(69, 21);
            this.vpFeatureSkypeCB.TabIndex = 7;
            this.vpFeatureSkypeCB.Text = "Skype";
            this.vpFeatureSkypeCB.UseVisualStyleBackColor = true;
            // 
            // vpFeatureTeamspeakCB
            // 
            this.vpFeatureTeamspeakCB.AutoSize = true;
            this.vpFeatureTeamspeakCB.Location = new System.Drawing.Point(355, 90);
            this.vpFeatureTeamspeakCB.Name = "vpFeatureTeamspeakCB";
            this.vpFeatureTeamspeakCB.Size = new System.Drawing.Size(56, 21);
            this.vpFeatureTeamspeakCB.TabIndex = 8;
            this.vpFeatureTeamspeakCB.Text = "TS3";
            this.vpFeatureTeamspeakCB.UseVisualStyleBackColor = true;
            // 
            // vpFeatureDiscordCB
            // 
            this.vpFeatureDiscordCB.AutoSize = true;
            this.vpFeatureDiscordCB.Location = new System.Drawing.Point(355, 117);
            this.vpFeatureDiscordCB.Name = "vpFeatureDiscordCB";
            this.vpFeatureDiscordCB.Size = new System.Drawing.Size(78, 21);
            this.vpFeatureDiscordCB.TabIndex = 9;
            this.vpFeatureDiscordCB.Text = "Discord";
            this.vpFeatureDiscordCB.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.vpFeatureDiscordCB);
            this.Controls.Add(this.vpFeatureTeamspeakCB);
            this.Controls.Add(this.vpFeatureSkypeCB);
            this.Controls.Add(this.cmdToggleVPFeature);
            this.Controls.Add(this.vpFeatureLabel);
            this.Controls.Add(this.hotkeysLabel);
            this.Controls.Add(this.FeaturesLabel);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FeaturesLabel;
        private System.Windows.Forms.Label hotkeysLabel;
        private System.Windows.Forms.Label vpFeatureLabel;
        private System.Windows.Forms.Button cmdToggleVPFeature;
        private System.Windows.Forms.CheckBox vpFeatureSkypeCB;
        private System.Windows.Forms.CheckBox vpFeatureTeamspeakCB;
        private System.Windows.Forms.CheckBox vpFeatureDiscordCB;
    }
}