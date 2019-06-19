/* This class is the GUI class of the settings screen, through this class user changes settings, this class updates the main GUI class "MainForm" with any changes*/
using System;
using System.Windows.Forms;

using GamingMusicPlayer.SignalProcessing.Keyboard;

namespace GamingMusicPlayer
{
    public partial class SettingsForm : Form
    {
        public Boolean SettingsVisible { get; private set; }
        private ToolTip gameplayTooltip;
        private MainForm mainForm;
        public SettingsForm(MainForm mForm)
        {
            InitializeComponent();
            mainForm = mForm;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            KeyboardListener.HookKeyboard();

            cmdToggleOverlay.GotFocus += onFocus;
            cmdToggleVPFeature.GotFocus += onFocus;
            vpFeatureDiscordCB.GotFocus += onFocus;
            vpFeatureTeamspeakCB.GotFocus += onFocus;
            vpFeatureSkypeCB.GotFocus += onFocus;
            overlayClickableCB.GotFocus += onFocus;

            gameplayTooltip = new ToolTip();
            gameplayTooltip.ToolTipIcon = ToolTipIcon.Info;
            gameplayTooltip.IsBalloon = true;
            gameplayTooltip.ShowAlways = true;
            gameplayTooltip.SetToolTip(gameplayTooltipLabel, "Automatic picking works by analyzing mouse and keyboard during gameplay\nUse this to let the system know whether the game you play is based on mouse, keyboard or both in order to get good results");
            gameplayTooltip.AutoPopDelay = 20000;

            this.hide();
        }

        public void hide()
        {
            this.Hide();
            SettingsVisible = false;
        }
        public void show()
        {
            if (Properties.Settings.Default.vpOn)
                cmdToggleVPFeature.Text = "ON";
            else
                cmdToggleVPFeature.Text = "OFF";
            if (Properties.Settings.Default.vpTs3)
                vpFeatureTeamspeakCB.Checked = true;
            else
                vpFeatureTeamspeakCB.Checked = false;
            if (Properties.Settings.Default.vpDiscord)
                vpFeatureDiscordCB.Checked = true;
            else
                vpFeatureDiscordCB.Checked = false;
            if (Properties.Settings.Default.vpSkype)
                vpFeatureSkypeCB.Checked = true;
            else
                vpFeatureSkypeCB.Checked = false;
            if (Properties.Settings.Default.overlayOn)
                cmdToggleOverlay.Text = "ON";
            else
                cmdToggleOverlay.Text = "OFF";
            if (Properties.Settings.Default.overlayClickable)
                overlayClickableCB.Checked = true;
            else
                overlayClickableCB.Checked = false;
            if (Properties.Settings.Default.songMatchOn)
                cmdAutoPickToggle.Text = "ON";
            else
                cmdAutoPickToggle.Text = "OFF";

            
            gameplayTrackBar.Value = (int)(mainForm.getSongMatchingMouseWeight() * (double)(gameplayTrackBar.Maximum));
            SettingsVisible = true;
            this.ShowDialog();
            SettingsVisible = false;
        }

        private void onFocus(object sender, EventArgs e)
        {
            vpFeatureLabel.Focus();
        }

        private void cmdToggleVPFeature_Click(object sender, EventArgs e)
        {
            if (cmdToggleVPFeature.Text.Equals("OFF"))
            {
                Properties.Settings.Default.vpOn = true;
                cmdToggleVPFeature.Text = "ON";
            }
            else
            {
                Properties.Settings.Default.vpOn = false;
                cmdToggleVPFeature.Text = "OFF";
            }
            Properties.Settings.Default.Save();
            mainForm.updateSettings(false);
        }

        private void OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (e.Down)
            {
                Console.WriteLine("pressed:" + e.KeyPressed);
            }

        }

        private void vpFeatureSkypeCB_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.vpSkype = vpFeatureSkypeCB.Checked;
            Properties.Settings.Default.Save();
            mainForm.updateSettings(false);
        }

        private void vpFeatureDiscordCB_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.vpDiscord = vpFeatureDiscordCB.Checked;
            Properties.Settings.Default.Save();
            mainForm.updateSettings(false);
        }

        private void vpFeatureTeamspeakCB_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.vpTs3 = vpFeatureTeamspeakCB.Checked;
            Properties.Settings.Default.Save();
            mainForm.updateSettings(false);
        }

        private void cmdToggleOverlay_Click(object sender, EventArgs e)
        {
            if (cmdToggleOverlay.Text.Equals("OFF"))
            {
                Properties.Settings.Default.overlayOn = true;
                cmdToggleOverlay.Text = "ON";
            }
            else if (cmdToggleOverlay.Text.Equals("ON"))
            {
                Properties.Settings.Default.overlayOn = false;
                cmdToggleOverlay.Text = "OFF";
            }
            Properties.Settings.Default.Save();
            mainForm.updateSettings(false);
        }

        private void overlayClickableCB_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayClickable = overlayClickableCB.Checked;
            Properties.Settings.Default.Save();
            mainForm.updateSettings(false);
        }

        private void gameplayTrackBar_ValueChanged(object sender, EventArgs e)
        {
            double mouse = (double)gameplayTrackBar.Value / (double)gameplayTrackBar.Maximum;
            double keyboard = 1 - mouse;
            mainForm.setSongMatchingMouseKeyboardWeights(mouse);
        }

        private void cmdAutoPickToggle_Click(object sender, EventArgs e)
        {
            if (cmdAutoPickToggle.Text.Equals("OFF"))
            {
                Properties.Settings.Default.songMatchOn = true;
                cmdAutoPickToggle.Text = "ON";
            }
            else if (cmdAutoPickToggle.Text.Equals("ON"))
            {
                Properties.Settings.Default.songMatchOn = false;
                cmdAutoPickToggle.Text = "OFF";
            }
            Properties.Settings.Default.Save();
            mainForm.updateSettings(false);
        }
    }
}
