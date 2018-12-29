using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using GamingMusicPlayer.MusicPlayer;

namespace GamingMusicPlayer
{
    public partial class MainForm : Form
    {
        private MusicPlayer.MusicPlayer mp;
        private bool loop;
        private bool musicTrackbarScrolling;
        private bool running; //for threads
        private Thread updateMusicTrackbarThread;
        private Thread trackMouseOnMusicTBarThread;
        private int lastMouseX;//used to track mouse while on track bar

        private Logger loggerForm;
        private Grapher grapherForm;
        

        public MainForm()
        {
            mp = new MusicPlayer.MusicPlayer();
            this.loop = false;
            this.musicTrackbarScrolling = false;
            this.running = true;
            this.lastMouseX = 0;
            InitializeComponent();
            seekLabel.Text = "";
            txtMusicProgress.Text = "";
            updateMusicTrackbarThread = new Thread(new ThreadStart(updateMusicTrackbar));
            updateMusicTrackbarThread.Start();

            loggerForm = new Logger();
            grapherForm = new Grapher();
        }

        

        private void updateMusicTrackbar()
        {
            while (running) //needs to be while app is still alive!
            {
                if (mp.Playing && !musicTrackbarScrolling)
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            musicTrackBar.Maximum = mp.getTrackLength();
                            musicTrackBar.Value = mp.getCurrentPosition();
                            txtMusicProgress.Text = msToString(musicTrackBar.Value)+" / "+msToString(musicTrackBar.Maximum);
                            if (musicTrackBar.Value == musicTrackBar.Maximum && musicTrackBar.Value != 0) //!= 0 because mp.stop sets tracklength to 0
                            {
                                if (loop)
                                {
                                    mp.setPosition(0);
                                    mp.resume();
                                }
                                else
                                    cmdNext_Click(null, null);
                            }
                        });
                    }
                    catch (System.ObjectDisposedException)
                    {
                        return;
                        //when closing app and this thread is still attemping to use objects -> throws an exception
                    }

                }
                else
                {
                    System.Threading.SpinWait.SpinUntil(() => (mp.Playing || !running));
                }
            }

        }

        private string msToString(int ms)
        {
            int minutes = 0, hours = 0;
            int seconds = ms / 1000;
            if (seconds >= 60)
            {
                int tmp = seconds;
                seconds = tmp % 60;
                minutes = tmp / 60;
            }
            if (minutes >= 60)
            {
                int tmp = minutes;
                minutes = tmp % 60;
                hours = tmp / 60;
            }
            string rtrn = "";
            if (hours > 0)
            {
                if (hours < 10)
                    rtrn += "0";
                rtrn += hours + ":";
            }
            if (minutes < 10)
                rtrn += "0";
            rtrn += minutes + ":";
            if (seconds < 10)
                rtrn += "0";
            rtrn += seconds;
            return rtrn;
        }

        private void log(string msg)
        {
            loggerForm.log(msg);
        }

        private void cmdAddSong_Click(object sender, EventArgs e)
        {
            OpenFileDialog songFileDialog = new OpenFileDialog();
            songFileDialog.Filter = "WAV Files|*.wav|MP3 Files|*.mp*|All Files|*.*";
            songFileDialog.Title = "Select a Music File";
            if (songFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Track t = new Track(songFileDialog.FileName);
                    mp.addTrack(t);
                    nameListBox.Items.Add(t.Name);
                    lengthListBox.Items.Add(msToString(t.Length));
                    nameListBox.SelectedIndex = mp.SelectedTrackIndex;
                    if (mp.deshuffle())
                    {
                        cmdShuffle.Image = Properties.Resources.shuffle_white;
                    }

                }catch (FileFormatNotSupported ffnse)
                {
                    log(ffnse.Message);
                }
            }
        }

        private void cmdPlayPause_Click(object sender, EventArgs e)
        {
            if (mp.Playing)
            {
                if (mp.pause())
                {
                    cmdPlayPause.Image = Properties.Resources.play_white;
                    cmdRemove.Enabled = true;
                    grapherForm.Track = null;
                }
                else
                {
                    log(mp.ErrorMsg);
                }
            }
            else
            {
                if (nameListBox.SelectedIndex != mp.SelectedTrackIndex)
                {
                    mp.stop();
                    grapherForm.Track = null;
                    if (!mp.selectTrack(nameListBox.SelectedIndex))
                    {
                        log(mp.ErrorMsg);
                        if (nameListBox.Items.Count > 0)
                        {
                            nameListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                            lengthListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                        }
                        mp.removeTrack();
                        return;
                    }
                    musicTrackBar.Maximum = mp.SelectedTrack.Length;
                }
                if (mp.resume())
                {
                    cmdPlayPause.Image = Properties.Resources.pause_white;
                    cmdRemove.Enabled = false;
                    grapherForm.Track = mp.SelectedTrack;
                }
                else
                {
                    log(mp.ErrorMsg);
                }
            }
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (!mp.Playing)
            {
                if(nameListBox.SelectedIndex != mp.SelectedTrackIndex)
                {
                    mp.stop();
                    grapherForm.Track = null;
                    if (!mp.selectTrack(nameListBox.SelectedIndex))
                    {
                        log(mp.ErrorMsg);
                        if (nameListBox.Items.Count > 0)
                        {
                            nameListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                            lengthListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                        }
                        mp.removeTrack();
                        return;
                    }
                }
                log("mp before selected index=" + mp.SelectedTrackIndex);
                if (nameListBox.SelectedItems.Count > 0)
                {
                    if (mp.removeTrack())
                    {
                        lengthListBox.Items.RemoveAt(nameListBox.SelectedIndex);
                        nameListBox.Items.RemoveAt(nameListBox.SelectedIndex);
                        log("mp after selected index=" + mp.SelectedTrackIndex);
                        if (nameListBox.Items.Count > 0)
                        {
                            log("attemtping to select the index:" + mp.SelectedTrackIndex);
                            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
                        }
                        log(mp.ErrorMsg);
                        return;
                    }
                    log(mp.ErrorMsg);
                }
            }
        }

        private void nameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nameListBox.SelectedItems.Count == 0)
            {
                cmdRemove.Enabled = false;
                return;
            }
            if (mp.Playing)
            {
                nameListBox.SelectedIndex = mp.SelectedTrackIndex;
                return;
            }
            if (!mp.Paused)
            {
                if (!mp.selectTrack(nameListBox.SelectedIndex))
                {
                    log(mp.ErrorMsg);
                    nameListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                    nameListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                    mp.removeTrack();
                }
                else
                {
                    musicTrackBar.Maximum = mp.SelectedTrack.Length;
                }
            }

            cmdRemove.Enabled = true;

        }

        private void lengthListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lengthListBox.ClearSelected();
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            if (!mp.next())
            {
                cmdPlayPause.Image = Properties.Resources.play_white;
                log(mp.ErrorMsg);
                return;
            }
            cmdRemove.Enabled = false;
            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
            cmdPlayPause.Image = Properties.Resources.pause_white;
            grapherForm.Track = mp.SelectedTrack;
        }

        private void cmdPrev_Click(object sender, EventArgs e)
        {
            if (!mp.prev())
            {
                cmdPlayPause.Image = Properties.Resources.play_white;
                log(mp.ErrorMsg);
                return;
            }
            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
            cmdRemove.Enabled = false;
            cmdPlayPause.Image = Properties.Resources.pause_white;
            grapherForm.Track = mp.SelectedTrack;
        }

        private void cmdShuffle_Click(object sender, EventArgs e)
        {
            if (mp.Shuffled)
            {
                if (mp.deshuffle())
                {
                    cmdShuffle.Image = Properties.Resources.shuffle_white;
                }    
            }
            else
            {
                if (mp.shuffle())
                {
                    cmdShuffle.Image = Properties.Resources.shuffle_red;
                } 
            }
        }

        private void musicTrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            musicTrackbarScrolling = true;
            trackMouseOnMusicTBarThread = new Thread(new ThreadStart(trackMouseOnMusicTBar));
            trackMouseOnMusicTBarThread.Start();
        }

        private void musicTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            int mouseX = (Cursor.Position.X - this.Location.X) - musicTrackBar.Location.X;
            //log("mouseup:mouseX=" + mouseX);
            int musicTrackBarCapacity = musicTrackBar.Maximum - musicTrackBar.Minimum;
            int musicTrackBarWidth = musicTrackBar.Size.Width;
            if (mouseX > musicTrackBarWidth)
                mouseX = musicTrackBarWidth;
            if (mouseX < 0)
                mouseX = 0;
            double ratio = (double)mouseX / (double)musicTrackBarWidth;
            int newValue = (int)(ratio * (double)musicTrackBarCapacity);

            musicTrackBar.Value = newValue;
            mp.setPosition(newValue);
            musicTrackbarScrolling = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mp.stop();
            running = false;

            loggerForm.Dispose();
            grapherForm.Dispose();
            Application.Exit();
        }

        private void cmdLoop_Click(object sender, EventArgs e)
        {
            if (loop)
            {
                cmdLoop.Image = Properties.Resources.loop_white;
                loop = false;
            }
            else
            {
                cmdLoop.Image = Properties.Resources.loop_red;
                loop = true;
            }
        }

        private void trackMouseOnMusicTBar()
        {
            while(running && musicTrackbarScrolling)
            {
                int mouseX = (Cursor.Position.X - this.Location.X) - musicTrackBar.Location.X;
                if (lastMouseX != mouseX)
                {
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        seekLabel.Text = "";
                    });   
                    lastMouseX = mouseX;
                    int musicTrackBarCapacity = musicTrackBar.Maximum - musicTrackBar.Minimum;
                    int musicTrackBarWidth = musicTrackBar.Size.Width;
                    if (mouseX > musicTrackBarWidth)
                        mouseX = musicTrackBarWidth;
                    if (mouseX < 0)
                        mouseX = 0;
                    double ratio = (double)mouseX / (double)musicTrackBarWidth;
                    int newValue = (int)(ratio * (double)musicTrackBarCapacity);
                    
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        seekLabel.Text = "Skip to:"+msToString(newValue);
                        
                    });
                    
                }
            }
            this.Invoke((MethodInvoker)delegate ()
            {
                seekLabel.Text = "";
            });
        }

        private void cmdLogger_Click(object sender, EventArgs e)
        {
            if (loggerForm.LoggerVisible)
            {
                loggerForm.hide();
                cmdLogger.Text = "Show Logger";
            }
            else
            {
                loggerForm.show();
                cmdLogger.Text = "Hide Logger";
            }
            
        }

        private void cmdShowGrapher_Click(object sender, EventArgs e)
        {
            if (grapherForm.GrapherVisible)
            {
                grapherForm.hide();
                cmdShowGrapher.Text = "Show Grapher";
            }
            else
            {
                grapherForm.show();
                cmdShowGrapher.Text = "Hide Grapher";
            }
        }
    }
}
