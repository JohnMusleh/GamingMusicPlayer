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
using System.IO;

using System.Configuration;
using System.Data.SqlClient;

using GamingMusicPlayer.MusicPlayer;
using GamingMusicPlayer.Database;


namespace GamingMusicPlayer
{
    /* This class is the main GUI class of the application, the user is able to use all the features from this Form.*/
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
        private SongMatcher matcherForm;

        private VolumeMixer vm;
        private int prevVolume;
        private bool loweredVolume;

        private DatabaseAdapter dbAdapter;
        private DriveScanner driveScanner;

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
            

            vm = new VolumeMixer();
            vm.OnPeakChanged += onPeakChanged;
            vm.subscribeApp("ts3client_win64");
            vm.subscribeApp("SkypeHost");
            vm.subscribeApp("Discord");
            prevVolume = mp.Volume;
            loweredVolume = false;
            //for developer mode
            //cmdShowGrapher.Visible = false;
            //cmdLogger.Visible = false;
            //ConfigurationManager.ConnectionStrings["GamingMusicPlayer.Properties.Settings.SongsDBConnectionString"].ConnectionString
            dbAdapter = new DatabaseAdapter(ConfigurationManager.ConnectionStrings["GamingMusicPlayer.Properties.Settings.SongsDBConnectionString"].ConnectionString);
            matcherForm = new SongMatcher(this, dbAdapter);
            driveScanner = null;
        }

        /* Public Control Methods*/
        //Music control
        //to add.. volume(), skipTo()..
        public bool playPauseToggle() //return false is paused or couldnt pause, return true if resumed or kept playing
        {
            if (mp.Playing)
            {
                if (mp.pause())
                {
                    cmdPlayPause.Image = Properties.Resources.play_white;
                    cmdRemove.Enabled = true;
                    grapherForm.Track = null;
                    return false;
                }
                else
                {
                    log(mp.ErrorMsg);
                    return true;
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
                        //[UPDATE TRACKS]
                        matcherForm.updateTrackList(mp.PlaylistTracklist);
                        return false;
                    }
                    musicTrackBar.Maximum = mp.SelectedTrack.Length;
                }
                if (mp.resume())
                {
                    cmdPlayPause.Image = Properties.Resources.pause_white;
                    cmdRemove.Enabled = false;
                    grapherForm.Track = mp.SelectedTrack;
                    return true;
                }
                else
                {
                    log(mp.ErrorMsg);
                    return false;
                }
            }
        }

        public void playTrack(int trackIndex) 
        {
            if (mp.selectTrack(trackIndex))
            {
                nameListBox.Invoke((MethodInvoker)delegate ()
                {
                    nameListBox.SelectedIndex = trackIndex;
                    playPauseToggle();
                });
                
                
            }
        }

        public bool nextTrack()
        {
            if (!mp.next())
            {
                cmdPlayPause.Image = Properties.Resources.play_white;
                log(mp.ErrorMsg);
                return false;
            }
            cmdRemove.Enabled = false;
            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
            lengthListBox.SelectedIndex = nameListBox.SelectedIndex;
            lengthListBox.TopIndex = nameListBox.TopIndex;

            cmdPlayPause.Image = Properties.Resources.pause_white;
            grapherForm.Track = mp.SelectedTrack;
            return true;
        }

        public bool prevTrack()
        {
            if (!mp.prev())
            {
                cmdPlayPause.Image = Properties.Resources.play_white;
                log(mp.ErrorMsg);
                return false;
            }
            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
            lengthListBox.SelectedIndex = nameListBox.SelectedIndex;
            lengthListBox.TopIndex = nameListBox.TopIndex;

            cmdRemove.Enabled = false;
            cmdPlayPause.Image = Properties.Resources.pause_white;
            grapherForm.Track = mp.SelectedTrack;
            return true;
        }

        public bool removeTrack(int i)
        {
            if (!mp.Playing)
            {
                if (i != mp.SelectedTrackIndex)
                {
                    mp.stop();
                    grapherForm.Track = null;
                    if (!mp.selectTrack(i))
                    {
                        log(mp.ErrorMsg);
                        return false;
                    }
                    if (nameListBox.Items.Count > 0)
                    {
                        nameListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                        lengthListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                    }
                    mp.removeTrack();
                    //[UPDATE TRACKS]
                    matcherForm.updateTrackList(mp.PlaylistTracklist);
                    return true;
                }
                log("mp before selected index=" + mp.SelectedTrackIndex);
                if (nameListBox.SelectedItems.Count > 0)
                {
                    if (mp.removeTrack())
                    {
                        //[UPDATE TRACKS]
                        matcherForm.updateTrackList(mp.PlaylistTracklist);
                        nameListBox.Items.RemoveAt(i);
                        lengthListBox.Items.RemoveAt(i);
                        log("mp after selected index=" + mp.SelectedTrackIndex);
                        if (nameListBox.Items.Count > 0)
                        {
                            log("attemtping to select the index:" + mp.SelectedTrackIndex);
                            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
                        }
                        log(mp.ErrorMsg);
                        return true;
                    }
                    log(mp.ErrorMsg);
                    return false;
                }
            }
            return false;
        }

        public void addSong(Track t, bool addToActivePlaylist, bool threadWrap)
        {
            if (addToActivePlaylist)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    mp.addTrack(t);
                    //[UPDATE TRACKS]
                    matcherForm.updateTrackList(mp.PlaylistTracklist);

                    nameListBox.Items.Add(t.Name);
                    lengthListBox.Items.Add(msToString(t.Length));
                    nameListBox.SelectedIndex = mp.SelectedTrackIndex;
                    if (mp.deshuffle())
                    {
                        cmdShuffle.Image = Properties.Resources.shuffle_white;
                    }
                });
            }

            if (dbAdapter.getTrack(t.Path) == null)
            {
                SignalProcessing.SignalProcessor sp = new SignalProcessing.SignalProcessor();
                if (threadWrap)
                {
                    new Thread(delegate () {
                        sp.ComputeBPM(t.Data, (t.Length / 1000), false, false);
                        t.BPM = sp.BPM;
                        sp.computeTimbre(t.Data, t.Length / 1000, false);
                        t.ZCR = sp.ZCR;
                        t.SpectralIrregularity = sp.SpectralIrregularity;
                        Console.WriteLine("adding tack: " + t.Path);
                        dbAdapter.addTrack(t);
                    }).Start();
                }
                else
                {
                    sp.ComputeBPM(t.Data, (t.Length / 1000), false, false);
                    t.BPM = sp.BPM;
                    sp.computeTimbre(t.Data, t.Length / 1000, false);
                    t.ZCR = sp.ZCR;
                    t.SpectralIrregularity = sp.SpectralIrregularity;
                    Console.WriteLine("adding tack: " + t.Path);
                    dbAdapter.addTrack(t);
                }
            }
        }

        /* Private internal methods */
        private void trackMouseOnMusicTBar()
        {
            while (running && musicTrackbarScrolling)
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
                        seekLabel.Text = "Skip to:" + msToString(newValue);

                    });

                }
            }
            this.Invoke((MethodInvoker)delegate ()
            {
                seekLabel.Text = "";
            });
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
                    SpinWait.SpinUntil(() => (mp.Playing || !running));
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

       

        /* Events */
        // GUI and button events
        private void cmdPlayPause_Click(object sender, EventArgs e)
        {
            playPauseToggle();
            //Console.WriteLine("cmdPlayPause:" + playPauseToggle());
        }
        private void cmdRemove_Click(object sender, EventArgs e)
        {
            removeTrack(nameListBox.SelectedIndex);
            //[BUG] removing song can select an unexisting index, and therefore when pressing cmdPlayPause it will remove each song 1 by 1
        }
        private void cmdNext_Click(object sender, EventArgs e)
        {
            nextTrack();
            //Console.WriteLine("cmdNext:" + next());
        }
        private void cmdPrev_Click(object sender, EventArgs e)
        {
            prevTrack();
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
        private void cmdToggleMatcher_Click(object sender, EventArgs e)
        {
            if (matcherForm.MatcherVisible)
            {
                matcherForm.hide();
                cmdToggleMatcher.Text = "Show Matcher";
            }
            else
            {
                matcherForm.show();
                cmdToggleMatcher.Text = "Hide Matcher";
            }
        }
        private void cmdPriorTs3_Click(object sender, EventArgs e)
        {
            if (vm.Running)
            {
                vm.stopListening();
                cmdPriorTs3.Text = "Prioritize voice communication:OFF";
            }
            else
            {
                vm.startListening();
                cmdPriorTs3.Text = "Prioritize voice communication:ON";
            }
        }
        private void cmdScan_Click(object sender, EventArgs e)
        {
            if (driveScanner == null || !driveScanner.Scanning)
            {
                string dir = "";
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        dir = fbd.SelectedPath;
                    }
                }
                driveScanner = new DriveScanner(dir);
                driveScanner.onPercChanged += onScanPercChanged;
                driveScanner.onScanComplete += onScanComplete;
                driveScanner.scan(true);
                cmdScan.Text = "Scanning.. \r\n(" + Math.Round(driveScanner.CompletePercentage, 2) + "%)\r\nClick to Cancel";
            }
            else
            {
                driveScanner.cancelScan();
                cmdScan.Text = "Scan Computer";
            }


            /*DialogResult dupFilesRes = MessageBox.Show("After scanning 10 duplicate files were found, would you like for them to get deleted?", "Duplicate Music Files", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dupFilesRes == System.Windows.Forms.DialogResult.Yes)
            {
                Console.WriteLine("to delete");
            }
            else
            {
                Console.WriteLine("not to delete");
            }*/
        }
        private void cmdAddSong_Click(object sender, EventArgs e)
        {
            OpenFileDialog songFileDialog = new OpenFileDialog();
            songFileDialog.Filter = "All Files|*.*|WAV Files|*.wav|MP3 Files|*.mp*";
            songFileDialog.Title = "Select a Music File";
            if (songFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Track t = new Track(songFileDialog.FileName);
                    addSong(t,true,true);

                }
                catch (FileFormatNotSupported ffnse)
                {
                    log(ffnse.Message);
                }
            }
        }
        private void cmdViewDB_Click(object sender, EventArgs e)
        {
            new DBViewer().Show();
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
                    //[UPDATE TRACKS]
                    matcherForm.updateTrackList(mp.PlaylistTracklist);
                }
                else
                {
                    musicTrackBar.Maximum = mp.SelectedTrack.Length;
                }
            }
            lengthListBox.SelectedIndex = nameListBox.SelectedIndex;
            lengthListBox.TopIndex = nameListBox.TopIndex;
            cmdRemove.Enabled = true;

        }
        private void lengthListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lengthListBox.SelectedIndex = nameListBox.SelectedIndex;
            lengthListBox.TopIndex = nameListBox.TopIndex;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            vm.stopListening();
            mp.stop();
            running = false;

            loggerForm.Dispose();
            grapherForm.Dispose();
            matcherForm.Dispose();
            Application.Exit();
        }

        //Other events from different components/forms
        private void onPeakChanged(object sender, VolumeMixer.PeakChangedArgs e)
        {
            if (e.app.name.Equals("ts3client_win64") || e.app.name.Equals("SkypeHost") || e.app.name.Equals("Discord"))
            {

                if (e.app.peak > 0.2 && !loweredVolume)
                {
                    prevVolume = mp.Volume;
                    Console.WriteLine(e.app.name + ":peak:" + e.app.peak);
                    Console.WriteLine("prev volume:" + mp.Volume);
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        mp.setVolume(130);
                    });
                    Console.WriteLine("new volume:" + mp.Volume);
                    loweredVolume = true;
                }
                else if (e.app.peak < 0.05 && loweredVolume)
                {
                    Console.WriteLine(e.app.name + "peak:" + e.app.peak);
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        mp.setVolume(prevVolume);
                    });
                    Console.WriteLine("new volume:" + mp.Volume);
                    loweredVolume = false;
                }
            }
            //Console.WriteLine(e.app.name+" -volume peak:"+e.app.peak);
        }
        private void onScanPercChanged(object sender, EventArgs e)
        {
            cmdScan.Invoke((MethodInvoker)delegate ()
            {
                cmdScan.Text = "Scanning.. \r\n(" + Math.Round(driveScanner.CompletePercentage, 2) + "%)\r\nClick to Cancel";
            });
        }
        private void onScanComplete(object sender, EventArgs e)
        {

            cmdScan.Invoke((MethodInvoker)delegate ()
            {
                cmdScan.Enabled = false;
            });
            List<Track> tracks = driveScanner.Tracks;
            if (tracks != null)
            {
                Console.WriteLine(" MAIN FORM --- : scan complete-- #ofTracks:" + driveScanner.Tracks.Count);
                foreach (Track t in tracks)
                {
                    addSong(t, false, false);
                }
            }
            cmdScan.Invoke((MethodInvoker)delegate ()
            {
                cmdScan.Enabled = true;
                cmdScan.Text = "Scan Computer";
            });

        }

        
    }
}
