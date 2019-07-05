/* This class is the main GUI class of the application, all features and controls are available as public methods from this class.*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

using GamingMusicPlayer.MusicPlayer;
using GamingMusicPlayer.Database;


namespace GamingMusicPlayer
{
    public partial class MainForm : Form
    {
        private const string TS3_APPNAME = "ts3client_win64";
        private const string SKYPE_APPNAME = "SkypeHost";
        private const string DISCORD_APPNAME = "Discord";

        private MusicPlayer.MusicPlayer mp;
        private bool loop;
        private bool musicTrackbarScrolling;
        private Thread updateMusicTrackbarThread;
        private Thread trackMouseOnMusicTBarThread;
        private Thread addSongsToDbThread;
        private List<Track> songsToAdd;

        //used to track mouse while on track bar
        private int lastMouseX;

        private Grapher grapherForm;
        private SongMatcher matcherForm;
        private SettingsForm settingsForm;
        private OverlayForm overlayForm;

        private VolumeMixer vm;
        private int prevVolume;
        private bool loweredVolume;

        private DatabaseAdapter dbAdapter;
        private DriveScanner driveScanner;

        private string[] dbPaths;

        //when searching -> array contains indices of the results in the dbPaths array
        private List<int> trackNameToPathIndices;
             
        public event EventHandler onSongComplete;
        public event EventHandler onPlayingChange;

        public bool Playing { get { return mp.Playing; } }

        public int CurrentlySelectedTrackIndex { get { return mp.SelectedTrackIndex; } }

        public bool AutoPick { get; set; }

        //to interrupt threads when closing the application
        public bool Running { get; private set; } 

        private void songComplete(object sender, EventArgs e) { }

        private void playingChanged(object sender, EventArgs e)
        {
            if (Playing)
            {
                if (updateMusicTrackbarThread != null)
                    updateMusicTrackbarThread.Abort();
                updateMusicTrackbarThread = new Thread(new ThreadStart(updateMusicTrackbar));
                updateMusicTrackbarThread.Start();
            }
            else
            {
                if (updateMusicTrackbarThread != null)
                {
                    updateMusicTrackbarThread.Abort();
                    updateMusicTrackbarThread = null;
                }
            }
        }

        public List<double> getKeyboardBpmHistory() { return matcherForm.keyboardBpmHistory; }
        public List<double> getMouseBpmHistory() { return matcherForm.mouseBpmHistory; }
        public List<double> getMouseZcrHistory() { return matcherForm.mouseZcrHistory; }
        public List<double> getKeyboardZcrHistory() { return matcherForm.keyboardZcrHistory; }
        public List<Double> getMouseSpectIrrHistory() { return matcherForm.mouseSpectIrrHistory; }
        public MainForm(bool devMode)
        {
            this.mp = new MusicPlayer.MusicPlayer();
            this.loop = false;
            this.musicTrackbarScrolling = false;
            this.Running = true;
            this.lastMouseX = 0;
            trackNameToPathIndices = null;
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            if (!devMode)
            {
                cmdShowGrapher.Hide();
                cmdToggleMatcher.Hide();
                cmdSaveCsv.Hide();
                cmdViewDB.Hide();
            }
            onPlayingChange += playingChanged;

            onSongComplete += songComplete;
            AutoPick = true;
            seekLabel.Text = "";
            txtMusicProgress.Text = "";
            updateMusicTrackbarThread = new Thread(new ThreadStart(updateMusicTrackbar));

            grapherForm = new Grapher(this);

            vm = new VolumeMixer();
            vm.OnPeakChanged += onPeakChanged;

            loweredVolume = false;
            dbAdapter = new DatabaseAdapter(ConfigurationManager.ConnectionStrings["GamingMusicPlayer.Properties.Settings.SongsDBConnectionString"].ConnectionString);
            matcherForm = new SongMatcher(this, dbAdapter);
            settingsForm = new SettingsForm(this);
            overlayForm = new OverlayForm(this, 200, 0);
            driveScanner = null;
            
            ToolTip vpToggleTooltip = new ToolTip();
            vpToggleTooltip.ToolTipIcon = ToolTipIcon.None;
            vpToggleTooltip.IsBalloon = true;
            vpToggleTooltip.ShowAlways = true;
            vpToggleTooltip.AutoPopDelay = 20000;
            vpToggleTooltip.SetToolTip(cmdVp, "Voice Prioritization Feature");

            ToolTip songMatchToggleTooltip = new ToolTip();
            songMatchToggleTooltip.ToolTipIcon = ToolTipIcon.None;
            songMatchToggleTooltip.IsBalloon = true;
            songMatchToggleTooltip.ShowAlways = true;
            songMatchToggleTooltip.AutoPopDelay = 20000;
            songMatchToggleTooltip.SetToolTip(cmdSongMatchToggle, "Automatic Song Matching Feature");


            dbListBox.SelectionMode = SelectionMode.MultiExtended;
            songsToAdd = new List<Track>();
            addSongsToDbThread = new Thread(delegate ()
            {
                SignalProcessing.SignalProcessor sp = new SignalProcessing.SignalProcessor();
                while (true)
                {
                    int c = 0;
                    lock (songsToAdd)
                    {
                        c = songsToAdd.Count;
                    }
                    while (c > 0)
                    {
                        Track t = songsToAdd[0];
                        sp.ComputeBPM(t.Data, (t.Length / 1000), false, false);
                        t.BPM = sp.BPM;
                        sp.computeTimbre(t.Data, t.Length / 1000, false);
                        t.ZCR = sp.ZCR;
                        t.SpectralIrregularity = sp.SpectralIrregularity;
                        lock (dbAdapter)
                        {
                            dbAdapter.addTrack(t);
                        }
                        updateSettings(true);
                        sp.clearMemory();
                        lock (songsToAdd)
                        {
                            songsToAdd.RemoveAt(0);
                            c = songsToAdd.Count;
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
            addSongsToDbThread.Start();
        }

        /* Public Control Methods*/
        //Music control

        //this function must be called every time settings are changed.
        public void updateSettings(bool updateDb)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                if (Properties.Settings.Default.vpTs3)
                    vm.subscribeApp(TS3_APPNAME);
                else
                    vm.unsubscribeApp(TS3_APPNAME);
                if (Properties.Settings.Default.vpDiscord)
                    vm.subscribeApp(DISCORD_APPNAME);
                else
                    vm.unsubscribeApp(DISCORD_APPNAME);
                if (Properties.Settings.Default.vpSkype)
                    vm.subscribeApp(SKYPE_APPNAME);
                else
                    vm.unsubscribeApp(SKYPE_APPNAME);

                volumeTrackBar.Value = Properties.Settings.Default.volume;
                mp.setVolume(Properties.Settings.Default.volume);
                volumeLabel.Text = volumeTrackBar.Value / 10 + "%";

                if (Properties.Settings.Default.vpOn)
                {
                    vm.startListening();
                    cmdVp.Image = Properties.Resources.vp_on;
                }
                else
                {
                    vm.stopListening();
                    cmdVp.Image = Properties.Resources.vp_off;
                }

                if (Properties.Settings.Default.overlayOn)
                {
                    overlayForm.Hide();
                    overlayForm.showOverlay(Properties.Settings.Default.overlayClickable);
                }
                else
                {
                    overlayForm.Hide();
                }
                if (Properties.Settings.Default.songMatchOn)
                {
                    cmdSongMatchToggle.Image = Properties.Resources.songmatch_on;
                    matcherForm.startMatching();
                }
                else
                {
                    cmdSongMatchToggle.Image = Properties.Resources.songmatch_off;
                    matcherForm.stopMatching();
                }

                if (updateDb)
                {
                    dbListBox.Items.Clear();
                    List<Track> tracks = dbAdapter.getAllTracks();
                    dbPaths = new string[tracks.Count];
                    int i = 0;
                    foreach (Track t in tracks)
                    {
                        dbPaths[i] = t.Path;
                        dbListBox.Items.Add(t.Name);
                        i++;
                    }
                }
                
            }); 
        }

        //return false is paused or couldnt pause, return true if resumed or kept playing
        public bool playPauseToggle() 
        {
            if (mp.Playing)
            {
                if (mp.pause())
                {
                    cmdPlayPause.Image = Properties.Resources.play_white;
                    cmdRemove.Enabled = true;
                    grapherForm.Track = null;
                    onPlayingChange(null, null);
                    return false;
                }
                else
                {
                    Console.WriteLine(mp.ErrorMsg);
                    onPlayingChange(null, null);
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
                        if (!File.Exists(nameListBox.SelectedItem.ToString()))
                        {
                            dbAdapter.removeTrack(nameListBox.SelectedItem.ToString());
                            updateSettings(true);
                        }
                        if (nameListBox.Items.Count > 0)
                        {
                            nameListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                            lengthListBox.Items.RemoveAt(mp.SelectedTrackIndex);
                        }
                        mp.removeTrack();
                        //[UPDATE TRACKS]
                        matcherForm.updateTrackList(mp.PlaylistTracklist);
                        onPlayingChange(null, null);
                        return false;
                    }
                    musicTrackBar.Maximum = mp.SelectedTrack.Length;
                }
                if (mp.resume())
                {
                    cmdPlayPause.Image = Properties.Resources.pause_white;
                    cmdRemove.Enabled = false;
                    grapherForm.Track = mp.SelectedTrack;
                    overlayForm.setText(mp.SelectedTrack.Name);
                    onPlayingChange(null, null);
                    return true;
                }
                else
                {
                    onPlayingChange(null, null);
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
                onPlayingChange(null, null);
                return false;
            }
            cmdRemove.Enabled = false;
            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
            lengthListBox.SelectedIndex = nameListBox.SelectedIndex;
            lengthListBox.TopIndex = nameListBox.TopIndex;

            cmdPlayPause.Image = Properties.Resources.pause_white;
            grapherForm.Track = mp.SelectedTrack;
            overlayForm.setText(mp.SelectedTrack.Name);
            onPlayingChange(null, null);
            return true;
        }

        public bool prevTrack()
        {
            if (!mp.prev())
            {
                cmdPlayPause.Image = Properties.Resources.play_white;
                onPlayingChange(null, null);
                return false;
            }
            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
            lengthListBox.SelectedIndex = nameListBox.SelectedIndex;
            lengthListBox.TopIndex = nameListBox.TopIndex;

            cmdRemove.Enabled = false;
            cmdPlayPause.Image = Properties.Resources.pause_white;
            grapherForm.Track = mp.SelectedTrack;
            overlayForm.setText(mp.SelectedTrack.Name);
            onPlayingChange(null, null);
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
                if (nameListBox.SelectedItems.Count > 0)
                {
                    if (mp.removeTrack())
                    {
                        //[UPDATE TRACKS]
                        matcherForm.updateTrackList(mp.PlaylistTracklist);
                        nameListBox.Items.RemoveAt(i);
                        lengthListBox.Items.RemoveAt(i);
                        if (nameListBox.Items.Count > 0)
                        {
                            nameListBox.SelectedIndex = mp.SelectedTrackIndex;
                        }
                        return true;
                    }
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
                    lock (songsToAdd)
                    {
                        songsToAdd.Add(t);
                    }
                }
                else
                {
                    sp.ComputeBPM(t.Data, (t.Length / 1000), false, false);
                    t.BPM = sp.BPM;
                    sp.computeTimbre(t.Data, t.Length / 1000, false);
                    t.ZCR = sp.ZCR;
                    t.SpectralIrregularity = sp.SpectralIrregularity;
                    lock (dbAdapter)
                    {
                        dbAdapter.addTrack(t);
                    }
                    updateSettings(true);
                }
            }
            matcherForm.updateTrackList(mp.LoadedPlaylist.TrackList);
        }

        public void setSongMatchingMouseKeyboardWeights(double mWeight)
        {
            matcherForm.setMkWeights(mWeight);
        }

        public double getSongMatchingMouseWeight() { return matcherForm.getMouseWeight(); }
        
        public void setOverlaySubLabel(string s)
        {
            overlayForm.setSubLabel(s);
        }

        public void clearDatabase()
        {
            dbAdapter.removeAllTracks();
            updateSettings(true);
        }
        
        /* Private internal methods */
        private void trackMouseOnMusicTBar()
        {
            while (Running && musicTrackbarScrolling)
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
            while (mp.Playing && Running)
            {
                if (!musicTrackbarScrolling)
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            musicTrackBar.Maximum = mp.getTrackLength();
                            musicTrackBar.Value = mp.getCurrentPosition();
                            txtMusicProgress.Text = msToString(musicTrackBar.Value) + " / " + msToString(musicTrackBar.Maximum);
                            if (musicTrackBar.Value == musicTrackBar.Maximum && musicTrackBar.Value != 0) //!= 0 because mp.stop sets tracklength to 0
                            {
                                if (loop)
                                {
                                    mp.setPosition(0);
                                    mp.resume();
                                }
                                else if (AutoPick)
                                {
                                    cmdNext_Click(null, null);
                                }
                                else
                                {
                                    mp.stop();
                                    cmdPlayPause.Image = Properties.Resources.play_white;
                                    cmdRemove.Enabled = true;
                                    grapherForm.Track = null;
                                }
                                onPlayingChange(null, null);
                                onSongComplete(null, null);
                            }
                        });
                    }
                    catch (System.ObjectDisposedException)
                    {
                        return;
                        //when closing app and this thread is still attemping to use objects -> throws an exception
                    }

                }
                Thread.Sleep(200);
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

        /* Events */
        // GUI and button events
        private void cmdPlayPause_Click(object sender, EventArgs e)
        {
            playPauseToggle();
        }
        private void cmdRemove_Click(object sender, EventArgs e)
        {
            removeTrack(nameListBox.SelectedIndex);
        }
        private void cmdNext_Click(object sender, EventArgs e)
        {
            nextTrack();
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
                cmdScan.Text = "Scanning..\r\n(" + Math.Round(driveScanner.CompletePercentage, 2) + "%)\r\n[Cancel]";
            }
            else
            {
                driveScanner.cancelScan();
                cmdScan.Text = "Scan";
            }
        }
        private void cmdAddSong_Click(object sender, EventArgs e)
        {
            OpenFileDialog songFileDialog = new OpenFileDialog();
            songFileDialog.Multiselect = true;
            songFileDialog.Filter = "All Files|*.*|WAV Files|*.wav|MP3 Files|*.mp*";
            songFileDialog.Title = "Select Music Files";
            if (songFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach(string fname in songFileDialog.FileNames)
                    {
                        Console.WriteLine(fname);
                        Track t = new Track(fname);
                        Console.WriteLine("calling add song from onclick..");
                        addSong(t, true, true);
                        Console.WriteLine("returned from addsong onclick..");
                    }
                }
                catch (FileFormatNotSupported ffnse)
                {
                    Console.WriteLine(ffnse.Message);
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
                    if (!File.Exists(nameListBox.SelectedItem.ToString()))
                    {
                        dbAdapter.removeTrack(nameListBox.SelectedItem.ToString());
                        updateSettings(true);
                    }
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
            Running = false;
            matcherForm.stopMatching();
            SignalProcessing.Mouse.MouseListener.UnhookMouse();
            SignalProcessing.Keyboard.KeyboardListener.UnHookKeyboard();
            
            if (Playing)
                playPauseToggle();
            if (vm.Running)
                vm.stopListening();
            
            // saving playlist and musicTrackBar value
            StringCollection savedPlaylist = new StringCollection();
            Playlist p = mp.LoadedPlaylist;
            foreach (Track t in p.TrackList)
            {
                savedPlaylist.Add(t.Path);
            }
            Properties.Settings.Default.selectedTrackIndex = mp.SelectedTrackIndex;
            Properties.Settings.Default.playlist = savedPlaylist;
            Properties.Settings.Default.musicTrackBarValue = musicTrackBar.Value;
            Properties.Settings.Default.Save();

            if (updateMusicTrackbarThread != null)
            {
                updateMusicTrackbarThread.Abort();
                updateMusicTrackbarThread = null;
            }
            addSongsToDbThread.Abort();   
        }
        private void onFocus(object sender, EventArgs e)
        {
            seekLabel.Focus();
        }

        //Other events from different components/forms
        private void onPeakChanged(object sender, VolumeMixer.PeakChangedArgs e)
        {
            if (e.app.name.Equals("ts3client_win64") || e.app.name.Equals("SkypeHost") || e.app.name.Equals("Discord"))
            {

                if (e.app.peak > 0.2 && !loweredVolume)
                {
                    prevVolume = mp.Volume;
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        mp.setVolume(130);
                    });
                    loweredVolume = true;
                }
                else if (e.app.peak < 0.05 && loweredVolume)
                {
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        mp.setVolume(prevVolume);
                    });
                    loweredVolume = false;
                }
            }
        }
        private void onScanPercChanged(object sender, EventArgs e)
        {
            cmdScan.Invoke((MethodInvoker)delegate ()
            {
                cmdScan.Text = "Scanning..\r\n(" + Math.Round(driveScanner.CompletePercentage, 2) + "%)\r\n[Cancel]";
            });
        }
        private void onScanComplete(object sender, EventArgs e)
        {
            if (Running)
            {
                cmdScan.Invoke((MethodInvoker)delegate ()
                {
                    cmdScan.Enabled = false;
                });
                List<Track> tracks = driveScanner.Tracks;
                if (tracks != null)
                {
                    foreach (Track t in tracks)
                    {
                        addSong(t, false, false);
                    }
                }
                cmdScan.Invoke((MethodInvoker)delegate ()
                {
                    cmdScan.Enabled = true;
                    cmdScan.Text = "Scan";
                });

            }
        }
        private void volumeTrackBar_Scroll(object sender, EventArgs e)
        {
            if (loweredVolume)
            {
                prevVolume = volumeTrackBar.Value;
            }
            else
            {
                mp.setVolume(volumeTrackBar.Value);
            }
            volumeLabel.Invoke((MethodInvoker)delegate ()
            {
                volumeLabel.Text = volumeTrackBar.Value / 10 + "%";
                Properties.Settings.Default.volume = volumeTrackBar.Value;
                Properties.Settings.Default.Save();
            });
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            cmdPlayPause.GotFocus += onFocus;
            cmdShuffle.GotFocus += onFocus;
            cmdLoop.GotFocus += onFocus;
            cmdNext.GotFocus += onFocus;
            cmdPrev.GotFocus += onFocus;
            cmdAddSong.GotFocus += onFocus;
            cmdRemove.GotFocus += onFocus;
            cmdScan.GotFocus += onFocus;
            cmdRemove.GotFocus += onFocus;
            cmdDbSearchGo.GotFocus += onFocus;
            cmdSettings.GotFocus += onFocus;
            cmdVp.GotFocus += onFocus;
            cmdSongMatchToggle.GotFocus += onFocus;

            musicTrackBar.GotFocus += onFocus;
            volumeTrackBar.GotFocus += onFocus;

            bottomLeftIcon.GotFocus += onFocus;
            

            StringCollection savedPaylist = Properties.Settings.Default.playlist;
            if (savedPaylist != null)
            {
                foreach (string p in savedPaylist)
                {
                    try
                    {
                        Console.WriteLine("adding song:" + p);
                        if (!File.Exists(p))
                            throw new FileNotFoundException();
                        addSong(new Track(p), true, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }
                Console.WriteLine("--DONE ADDING SONGS--");
            }

            int selectedIndex = Properties.Settings.Default.selectedTrackIndex;
            if (selectedIndex < mp.LoadedPlaylist.Count && selectedIndex<nameListBox.Items.Count)
            {
                nameListBox.SelectedIndex = selectedIndex;
                musicTrackBar.Value = Properties.Settings.Default.musicTrackBarValue;
                mp.setPosition(musicTrackBar.Value);

            }
            
            updateSettings(true);
            matcherForm.show();
            matcherForm.hide();
        }
        private void cmdDbAdd_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection selectedIndicses = dbListBox.SelectedIndices;
            if (selectedIndicses.Count > 0)
            {
                foreach(int i in selectedIndicses)
                {
                    if (trackNameToPathIndices == null)
                    {
                        addSong(new Track(dbPaths[i]), true, false);
                    }
                    else
                    {
                        addSong(new Track(dbPaths[trackNameToPathIndices[i]]), true, false);
                    }
                    
                }
            }
        }
        private void dbListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dbListBox.SelectedIndex == -1)
                cmdDbAdd.Enabled = false;
            else
                cmdDbAdd.Enabled = true;
        }
        private void nameListBox_MouseDown(object sender, MouseEventArgs e)
        {
            //for drag and drop
            //need to unlock a boolean parameter that allows onmousemove to track mouse and switch , switch also length
            int itemIndex = nameListBox.IndexFromPoint(new Point(e.X, e.Y));
            if (itemIndex >= 0 && itemIndex < nameListBox.Items.Count)
            {
                //Console.WriteLine(nameListBox.Items[itemIndex]);
            }
            else
            {
                //Console.WriteLine("no item at:"+itemIndex);
            }
        }
        private void cmdDbSearchGo_Click(object sender, EventArgs e)
        {
            string txt = dbSearchTxtBox.Text;
            if (txt.Equals(""))
            {
                trackNameToPathIndices = null;
                updateSettings(true);
                return;
            }
            List<int> tempTrackNameToPathIndices = new List<int>();
            List<string> tempResultNames = new List<string>();
            List<string> albums = new List<string>();
            int i = 0;
            foreach (Track t in dbAdapter.getAllTracks())
            {
                string tn = t.Name.ToLower();
                string artist = t.Artist;
                string album = t.Album;
                if ( tn.Contains(txt.ToLower()) || (artist!=null && artist.ToLower().Contains(txt.ToLower()) ) || (album!=null && album.ToLower().Contains(txt.ToLower()) ) )
                {
                    albums.Add(album);
                    tempResultNames.Add(tn);
                    tempTrackNameToPathIndices.Add(i);
                }
                i++;
            }


            Dictionary<string, List<int>> map = new Dictionary<string, List<int>>();
            for(i=0; i<albums.Count; i++)
            {
                if (map.ContainsKey(albums[i]))
                {
                    map[albums[i]].Add(i);
                }
                else
                {
                    List<int> list = new List<int>();
                    list.Add(i);
                    map[albums[i]] = list;
                }
            }

            List<int> groupedIndices = new List<int>(); //list of indices in the albums/names/indices , sorted and grouped by album
            foreach (string album in map.Keys)
            {
                foreach(int gi in map[album])
                {
                    groupedIndices.Add(gi);
                }
            }

            List<string> resultNames = new List<string>();
            trackNameToPathIndices = new List<int>();
            foreach(int sorted_i in groupedIndices)
            {
                resultNames.Add(tempResultNames[sorted_i]);
                trackNameToPathIndices.Add(tempTrackNameToPathIndices[sorted_i]);
            }

            //trackNameToPathIndices
            //
            dbListBox.Items.Clear();
            foreach (string tn in resultNames)
                dbListBox.Items.Add(tn);
        }

        private void cmdSettings_Click_1(object sender, EventArgs e)
        {
            settingsForm.show();
        }

        private void cmdSettings_MouseEnter(object sender, EventArgs e)
        {
            cmdSettings.Image = Properties.Resources.settings_hover;
        }

        private void cmdSettings_MouseLeave(object sender, EventArgs e)
        {
            cmdSettings.Image = Properties.Resources.settings;
        }

        private void cmdVp_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.vpOn)
            {
                vm.stopListening();
                Properties.Settings.Default.vpOn = false;
                cmdVp.Image = Properties.Resources.vp_off;
            }
            else
            {
                vm.startListening();
                Properties.Settings.Default.vpOn = true;
                cmdVp.Image = Properties.Resources.vp_on;
            }
            Properties.Settings.Default.Save();
        }

        private void cmdSongMatchToggle_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.songMatchOn)
            {
                Properties.Settings.Default.songMatchOn = false;
                cmdSongMatchToggle.Image = Properties.Resources.songmatch_off;
                matcherForm.stopMatching();
            }
            else
            {
                Properties.Settings.Default.songMatchOn = true;
                cmdSongMatchToggle.Image = Properties.Resources.songmatch_on;
                matcherForm.startMatching();
            }
            Properties.Settings.Default.Save();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            overlayForm.showCountDown("changing action in", 10, false);

            //Track t = mp.SelectedTrack;
            //Database.DatabaseAdapter.predict((float)t.BPM, (float)t.ZCR, (float)t.SpectralIrregularity);
            /*List < Track > ts = mp.LoadedPlaylist.TrackList;
            String path = "C:\\Users\\John\\Desktop\\Education\\Software Engineering B.Sc\\uni\\year_4\\grad_project\\code\\GamingMusicPlayer\\data.csv";
            File.Create(path).Close();
            String csv = "Y,BPM,ZCR,SPECTIRR,NAME,\r\n";
            foreach (Track t in ts)
            {
                csv += "0," + t.BPM + "," + t.ZCR + "," + t.SpectralIrregularity + "," + t.Name + ",\r\n";
            }
            File.WriteAllText(path, csv); */
        }
    }
}
