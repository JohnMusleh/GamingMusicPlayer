using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GamingMusicPlayer.MusicPlayer;
using GamingMusicPlayer.SignalProcessing;
using GamingMusicPlayer.SignalProcessing.Keyboard;
using GamingMusicPlayer.SignalProcessing.Mouse;
using System.Diagnostics;
using System.Threading;

namespace GamingMusicPlayer
{
    public partial class SongMatcher : Form
    {
        public Boolean MatcherVisible { get; private set; }

        private bool matching;
        private List<Track> tracks;
        private List<double> tracksBPM;

        private const int BEATS_QUEUE_SIZE = 12;
        private Queue<int> mouseBeats;
        private Queue<int> keyboardBeats;

        private double keyboardBPM;
        private double mouseBPM;
        private KeyboardProcessor kp;
        private MouseProcessor mp;
        private SignalProcessor sp;

        private bool mouseLocked; //to lock match() while reading mouse data
        private bool keyboardLocked; //to lock match() while reading keyboard data
        private int currTrackIndex;

        private Thread worker;
        private bool workerAlive;


        public SongMatcher()
        {
            InitializeComponent();
            this.tracksBPM = null;
            this.tracks = null;
            this.matching = false;

            this.currTrackIndex = 0;
            sp = new SignalProcessor();
            sp.onBPMReady += onBPMReady;

            kp = new KeyboardProcessor();
            kp.onDataReady += onKeyboardDataReady;

            mp = new MouseProcessor();
            mp.onDataReady += onMouseDataReady;

            mouseBeats = new Queue<int>();
            keyboardBeats = new Queue<int>();

            txtLogs.ReadOnly = true;
            
            this.hide();
            MatcherVisible = false;
            workerAlive = false;
            this.TopMost = true;

            mouseLocked = false;
            keyboardLocked = false;
        }

        public void setTracks(List<Track> tracks)
        {
            this.tracks = new List<Track>();
            foreach (Track t in tracks)
            {
                this.tracks.Add(t);
            }
        }

        private void onKeyboardDataReady(object sender, EventArgs e)
        {
            Console.WriteLine("keyboard data length:" + kp.Data.Length);
            if (matching)
            {
                keyboardLocked = true;
                short[] pointer = kp.Data;
                short[] data = new short[pointer.Length];
                for(int i = 0; i < pointer.Length; i++)
                {
                    data[i] = pointer[i];
                }
                keyboardLocked = false;
                sp.ComputeBPM(data, 5, true, false);
                if (keyboardBeats.Count < BEATS_QUEUE_SIZE)
                {
                    keyboardBeats.Enqueue(sp.BeatCount);
                    keyboardBPM = -1;
                }
                else
                {
                    keyboardBeats.Dequeue();
                    keyboardBeats.Enqueue(sp.BeatCount);
                    keyboardBPM = keyboardBeats.Sum();
                }
                //log("keyboard bpm:" + keyboardBPM);
                keyboardLabel.Invoke(new MethodInvoker(delegate {
                    keyboardLabel.Text = "keyboardBPM:" + keyboardBPM;
                }));
               
                match();
            }
            else
            {
                Console.WriteLine("recording stopped.");
            }
        }

        private void onBPMReady(object sender, EventArgs e)
        {
            
        }
        private void onMouseDataReady(object sender, EventArgs e)
        {
            Console.WriteLine("mouse data length:" + mp.Data.Length);
            if (matching)
            {
                mouseLocked = true;
                short[] pointer = mp.Data;
                short[] data = new short[pointer.Length];  
                for (int i = 0; i < pointer.Length; i++)
                {
                    data[i] = pointer[i];
                }
                mouseLocked = false;
                sp.ComputeBPM(data, 5, true, false);
                if (mouseBeats.Count < BEATS_QUEUE_SIZE)
                {
                    mouseBeats.Enqueue(sp.BeatCount);
                    mouseBPM = -1;
                }
                else
                {
                    mouseBeats.Dequeue();
                    mouseBeats.Enqueue(sp.BeatCount);
                    mouseBPM = mouseBeats.Sum();
                }
                
                //log("mouse bpm:" + mouseBPM);
                mouseLabel.Invoke(new MethodInvoker(delegate {
                    mouseLabel.Text = "mouseBPM:" + mouseBPM;
                }));
                
                match();
            }
            else
            {
                Console.WriteLine("recording stopped.");
            }
        }

        public void hide()
        {
            this.Hide();
            MatcherVisible = false;
        }

        public void show()
        {
            this.Show();
            MatcherVisible = true;
            worker = new Thread(new ThreadStart(worker_calcTracksBPM));
            worker.Start();
        }


        private void cmdToggleMatching_Click(object sender, EventArgs e)
        {
            if (matching)
            {
                matching = false;
                cmdToggleMatching.Text = "Start Matching";
            }
            else
            {
                matching = true;
                match();
                cmdToggleMatching.Text = "Stop Matching";
            }
            setStatus("Matching Mode:" + matching);
        }

        private void match()
        {
            while (mouseLocked || keyboardLocked);
            if (!mp.Processing)
            {
                mp.record(5, 200, new System.Windows.Point(1164, 364));
            }
            if (!kp.Processing)
            {
                kp.record(5);
            }
            
        }

        private void worker_calcTracksBPM()
        {
            if (tracks == null)
                return;
            if (!workerAlive)
            {
                workerAlive = true;
                cmdToggleMatching.Invoke(new MethodInvoker(delegate {
                    cmdToggleMatching.Enabled = false;
                }));
                setStatus("Calculating BPM for every track..");
                this.tracksBPM = new List<double>();
                currTrackIndex = 0;
                foreach (Track t in tracks)
                {
                    sp.ComputeBPM(t.Data, t.Length / 1000, false, false);
                    tracksBPM.Add(sp.BPM);
                }
                setStatus("Matching Mode:" + matching);
                cmdToggleMatching.Invoke(new MethodInvoker(delegate {
                    cmdToggleMatching.Enabled = true;
                }));
                workerAlive = false;
            }
            else
            {
                Console.WriteLine("worker_calcTracksBPM failed: signal processor already in use.");
            }
            
        }

        private void setStatus(String str)
        {
            if (MatcherVisible)
            {
                statusLabel.Invoke(new MethodInvoker(delegate {
                    statusLabel.Text = str;
                }));
            }
            
        }

        private void log(String str)
        {
            if (MatcherVisible)
            {
                txtLogs.Invoke(new MethodInvoker(delegate {
                    txtLogs.Text = txtLogs.Text + "\r\n" + str;
                }));
            }

        }

        private void cmdClearLogs_Click(object sender, EventArgs e)
        {
            txtLogs.Text = "";
        }
    }
}
