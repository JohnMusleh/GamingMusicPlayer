using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GamingMusicPlayer.SignalProcessing.Keyboard;
namespace GamingMusicPlayer
{
    public partial class SettingsForm : Form
    {
        public Boolean SettingsVisible { get; private set; }

        private MainForm mainForm;
        public SettingsForm(MainForm mForm)
        {
            InitializeComponent();
            mainForm = mForm;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            KeyboardListener.HookKeyboard();
            //KeyboardListener.OnKeyPressed += OnKeyPressed;
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
            SettingsVisible = true;
            this.ShowDialog();
            SettingsVisible = false;
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
            mainForm.updateSettings();
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
            mainForm.updateSettings();
        }

        private void vpFeatureDiscordCB_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.vpDiscord = vpFeatureDiscordCB.Checked;
            Properties.Settings.Default.Save();
            mainForm.updateSettings();
        }

        private void vpFeatureTeamspeakCB_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.vpTs3 = vpFeatureTeamspeakCB.Checked;
            Properties.Settings.Default.Save();
            mainForm.updateSettings();
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
            mainForm.updateSettings();
        }

        private void overlayClickableCB_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayClickable = overlayClickableCB.Checked;
            Properties.Settings.Default.Save();
            mainForm.updateSettings();
        }
    }
}
