/* This class is the GUI class of the overlay, it has public methods to control the overlay*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace GamingMusicPlayer
{
    public partial class OverlayForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);
        
        private int initialStyle;
        private MainForm playerForm;

        private bool dragPanelMouseDown;
        private Point lastLocation;

        private Thread animateSongNameThread;

        private bool countingDown;
        public bool LastCountDownCancelled { get; private set; }
        public event EventHandler onCountDownComplete; //invoked when done counting down to notify

        public OverlayForm(MainForm main, int x, int y)
        {
            InitializeComponent();
            this.Width = 200;
            this.Height = 100;
            this.initialStyle = GetWindowLong(this.Handle, -20);
            this.playerForm = main;
            this.SetDesktopLocation(x, y);
            this.ShowInTaskbar = false;
            this.countingDown = false;
            LastCountDownCancelled = false;

            
            txtCancelLbl.Hide();
            txtLabel.TextChanged += fitTextLabel;
            txtLabel.Text = "Enjoy the music";
            animateSongNameThread = null;

            botPanel.Width = this.Width;
            botPanel.Height = this.Height / 2;
            botPanel.Location = new Point((Width - botPanel.Width) / 2, (Height/4));
            cmdPlayPause.Location = new Point((Width - cmdPlayPause.Width) / 2, cmdPlayPause.Location.Y);
            dragPanel.Location = new Point(cmdPlayPause.Location.X + cmdPlayPause.Width, dragPanel.Location.Y);
            cmdPlayPause.GotFocus += onFocus;

            playerForm.onPlayingChange += playingChanged;
            dragPanelMouseDown = false;
            this.Location = new Point(Properties.Settings.Default.overlayX, Properties.Settings.Default.overlayY);
            lastLocation = this.Location;
            Label lbl = new Label();
            lbl.Size = dragPanel.Size;
            lbl.Enabled = false;
            lbl.Font = new Font(lbl.Font.Name, 15);
            lbl.Text = "|||";
            dragPanel.Controls.Add(lbl);
        }
    
        public void showOverlay(bool clickable)
        {
            if (!Visible)
            {
                Show();

                if (clickable)
                {
                    showButtons();
                    SetWindowLong(this.Handle, -20, initialStyle);
                }
                else
                {
                    hideButtons();
                    SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
                }
            }
        }
        
        public void showCountDown(string msg, int seconds, bool clickable)
        {
            if (!countingDown)
            {
                countingDown = true;
                string prevText = txtLabel.Text;
                bool prevVisible = Visible;
                bool prevClickable = cmdPlayPause.Visible;
                if (!Visible)
                {
                    Show();
                }

                if (clickable)
                {
                    showButtons();
                    SetWindowLong(this.Handle, -20, initialStyle);
                }
                else
                {
                    hideButtons();
                    SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
                }
                txtCancelLbl.Text = "Press to cancel";
                txtCancelLbl.Location = new Point((Width - txtCancelLbl.Width) / 2, txtCancelLbl.Location.Y);
                txtCancelLbl.Show();
               
                new Thread(delegate ()
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    while (sw.Elapsed.TotalSeconds < seconds)
                    {
                        txtLabel.Invoke((MethodInvoker)delegate ()
                        {
                            txtLabel.Text = msg + " " + (int)(seconds - sw.Elapsed.TotalSeconds) + "secs";
                        });
                        Thread.Sleep(1000);
                    }
                    sw.Stop();
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        txtLabel.Text = prevText;
                        txtCancelLbl.Hide();
                        if (!prevVisible)
                            Hide();
                        if (prevClickable)
                        {
                            showButtons();
                            SetWindowLong(this.Handle, -20, initialStyle);
                        }
                        else
                        {
                            hideButtons();
                            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
                        }
                        countingDown = false;
                        onCountDownComplete?.Invoke(null, null);
                    });
                }).Start();
            }
            
        }

        public void setSubLabel(string s)
        {
            txtCancelLbl.Invoke((MethodInvoker)delegate ()
            {
                txtCancelLbl.Show();
                txtCancelLbl.Text = s;
                if (!s.Equals(""))
                    txtCancelLbl.Visible = true;
                else
                    txtCancelLbl.Visible = false;
            });
        }

        //used to change song name
        public void setText(string txt)
        {
            txtLabel.Text = txt;
        }

        public void resetLocation()
        {
            this.SetDesktopLocation(Screen.PrimaryScreen.Bounds.Size.Width - Width, 0);
            Properties.Settings.Default.overlayX = this.Location.X;
            Properties.Settings.Default.overlayY = this.Location.Y;
            Properties.Settings.Default.Save();

        }

        private void hideButtons()
        {
            cmdPlayPause.Hide();
            dragPanel.Hide();
        }

        private void showButtons()
        {
            if (playerForm.Playing)
                cmdPlayPause.Text = "pause";
            else
                cmdPlayPause.Text = "play";
            cmdPlayPause.Show();

            dragPanel.Show();
        }

        private void playingChanged(object sender, EventArgs e)
        {
            if (playerForm.Playing)
            {
                cmdPlayPause.Text = "pause";
            }
            else
            {
                cmdPlayPause.Text = "play";
            }
        }

        private void onFocus(object sender, EventArgs e)
        {
            txtLabel.Focus();
        }

        private void fitTextLabel(object sender, EventArgs e)
        {
            //this only works for width = 200 pixels, height = 100 pixels
            if (txtLabel.Text.Length > 30)
            {
                txtLabel.Font = new Font(txtLabel.Font.Name, 9);
                txtLabel.Left = 0;
                if (animateSongNameThread != null)
                    animateSongNameThread.Abort();
                animateSongNameThread = new Thread(delegate ()
                {
                    while (playerForm.Running)
                    {
                        txtLabel.Invoke((MethodInvoker)delegate ()
                        {
                            txtLabel.Left = txtLabel.Left - 5;
                            if (txtLabel.Left < txtLabel.Text.Length * -4)
                            {
                                txtLabel.Left = txtLabel.Text.Length ;
                            }
                        });
                        Thread.Sleep(400);
                    }
                });
                animateSongNameThread.Start();
            }
            else
            {
                if (animateSongNameThread != null)
                    animateSongNameThread.Abort();
                float s = (float)((float)(248.4) / (float)txtLabel.Text.Length);
                if (s > 20)
                    s = 20;
                txtLabel.Font = new Font(txtLabel.Font.Name, s);
                txtLabel.Width = txtLabel.Text.Length * 10;
                txtLabel.Location = new Point((Width - txtLabel.Width) / 2, txtLabel.Location.Y);
            }


        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.DarkKhaki;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 10, 10));
            txtLabel.Focus();

        }

        private void cmdPlayPause_Click(object sender, EventArgs e)
        {
            if (playerForm.playPauseToggle())
            {
                cmdPlayPause.Text = "pause";
            }
            else
                cmdPlayPause.Text = "play";
        }

        private void dragPanel_MouseDown(object sender, MouseEventArgs e)
        {
            dragPanelMouseDown = true;
            lastLocation = e.Location;
        }

        private void dragPanel_MouseUp(object sender, MouseEventArgs e)
        {
            dragPanelMouseDown = false;
            Properties.Settings.Default.overlayX = this.Location.X;
            Properties.Settings.Default.overlayY = this.Location.Y;
            Properties.Settings.Default.Save();
        }

        private void dragPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragPanelMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.ShowInTaskbar = false;
                this.Update();
            }
        }

        private void OverlayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
