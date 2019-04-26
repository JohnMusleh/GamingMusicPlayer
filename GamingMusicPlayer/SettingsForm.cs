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
            this.ShowDialog();
            SettingsVisible = true;
            
        }

        private void cmdToggleVPFeature_Click(object sender, EventArgs e)
        {
            if (cmdToggleVPFeature.Text.Equals("OFF"))
            {
                mainForm.toggleVPFeature(true);
                cmdToggleVPFeature.Text = "ON";
            }
            else
            {
                mainForm.toggleVPFeature(false);
                cmdToggleVPFeature.Text = "OFF";
            }
        }

        private void OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (e.Down)
            {
                Console.WriteLine("pressed:" + e.KeyPressed);
            }

        }

    }
}
