using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GamingMusicPlayer.MusicPlayer;
using GamingMusicPlayer.SignalProcessing;
using GamingMusicPlayer.SignalProcessing.Keyboard;
using GamingMusicPlayer.SignalProcessing.Mouse;
using System.Threading;

namespace GamingMusicPlayer
{
    /*SongMatcher is an always-on-top GUI that automatically picks songs according to mouse+keyboard inputs */
    public partial class SongMatcher : Form
    {
        public Boolean MatcherVisible { get; private set; }

        private bool matching;
        private List<Track> tracks;
        private List<double> tracksBPM;

        private const int RECORDINGS_MAX_SIZE = 6; //each recording is 5 seconds
        private Queue<short[]> mouseRecordings; 
        private Queue<short[]> keyboardRecordings;

        private double keyboardBPM;
        private double mouseBPM;
        private KeyboardProcessor kp;
        private MouseProcessor mp;
        private SignalProcessor sp;

        private MainForm playerForm;

        //used for normalization
        private double maxMouseBPM;
        private double maxKeyboardBPM;

        private Object mouseLock=new Object(); //to lock match() while reading mouse data
        private Object keyboardLock=new Object(); //to lock match() while reading keyboard data
        private int currTrackIndex;

        private Thread worker;
        private bool workerAlive;

        public SongMatcher(MainForm main)
        {
            InitializeComponent();
            this.tracksBPM = null;
            this.tracks = null;
            this.matching = false;
            playerForm = main;
            this.currTrackIndex = 0;
            sp = new SignalProcessor();
            sp.onBPMReady += onBPMReady;

            kp = new KeyboardProcessor();
            kp.onDataReady += onKeyboardDataReady;

            mp = new MouseProcessor();
            mp.onDataReady += onMouseDataReady;

            mouseRecordings = new Queue<short[]>();
            keyboardRecordings = new Queue<short[]>();
            
            this.hide();
            MatcherVisible = false;
            workerAlive = false;
            this.TopMost = true;;

            maxMouseBPM = 30;
            maxKeyboardBPM = 30;
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
            if (matching)
            {
                lock (keyboardLock)
                {
                    short[] pointer = kp.Data;
                    short[] data = new short[pointer.Length];
                    for (int i = 0; i < pointer.Length; i++)
                    {
                        data[i] = pointer[i];
                    }
                    if (keyboardRecordings.Count < RECORDINGS_MAX_SIZE)
                    {
                        keyboardRecordings.Enqueue(data);
                    }
                    else
                    {
                        keyboardRecordings.Dequeue();
                        keyboardRecordings.Enqueue(data);
                    }
                    //Console.WriteLine("computing bpm of keyboard data, duration:" + (5 * keyboardRecordings.Count));
                    sp.ComputeBPM(getKeyboardData(), 5 * keyboardRecordings.Count, true, false);
                }
                
                
                

                //normalizing bpm
                if (sp.BPM >=maxKeyboardBPM)
                {
                    keyboardBPM = 100;
                }
                else
                {
                    keyboardBPM = (int)((sp.BPM / maxKeyboardBPM) * 100);
                }
                
                
                keyboardLabel.Invoke(new MethodInvoker(delegate {
                    keyboardLabel.Text = "% keyBPM:" + keyboardBPM + "    real:"+sp.BPM+"    max:" + maxKeyboardBPM;
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
            if (matching)
            {
                lock (mouseLock)
                {
                    short[] pointer = mp.Data;
                    short[] data = new short[pointer.Length];
                    for (int i = 0; i < pointer.Length; i++)
                    {
                        data[i] = pointer[i];
                    }
                    if (mouseRecordings.Count < RECORDINGS_MAX_SIZE)
                    {
                        mouseRecordings.Enqueue(data);
                    }
                    else
                    {
                        mouseRecordings.Dequeue();
                        mouseRecordings.Enqueue(data);
                    }
                    sp.ComputeBPM(getMouseData(), 5 * mouseRecordings.Count, true, false);
                }
                
                
                //normalizing bpm
                if (sp.BPM >= maxMouseBPM)
                {
                    mouseBPM = 100;
                }
                else
                {
                    mouseBPM = (int)((sp.BPM / maxMouseBPM) * 100);
                }

               
                mouseLabel.Invoke(new MethodInvoker(delegate {
                    mouseLabel.Text = "% mouseBPM:" + mouseBPM + "    real:"+sp.BPM+"    max:" + maxMouseBPM;
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

        private void pickTrack()
        {
            if (tracksBPM == null)
            {
                return;
            }
            
            double minDiff = 100;
            int trackIndex = 0;
            for (int i = 0; i < tracksBPM.Count; i++)
            {
                double d = (1*mouseBPM+0*keyboardBPM)-normalizeTrackBPM(tracksBPM[i]);
                if (d < 0)
                {
                    d *= -1;
                }
                if (d < minDiff)
                {
                    minDiff = d;
                    trackIndex = i;
                }
            }
            mouseTrackLabel.Invoke(new MethodInvoker(delegate {
                keyboardTrackLabel.Text = tracks[trackIndex].Name;
                mouseTrackLabel.Text = " with bpm:" + tracksBPM[trackIndex] + "   %:" +(int)normalizeTrackBPM(tracksBPM[trackIndex]);
            }));
            if (currTrackIndex != trackIndex)
            {
                playerForm.playTrack(trackIndex);
                currTrackIndex = trackIndex;
            }
        }

        private void match()
        {
            lock (mouseLock)
            {
                if (!mp.Processing)
                {
                    mp.record(5, 200, new System.Windows.Point(1164, 364));
                }
            }
            lock (keyboardLock)
            {
                if (!kp.Processing)
                {
                    kp.record(5);
                }
            }
            lock (playerForm)
            {
                pickTrack();
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

        private double normalizeTrackBPM(double bpm)
        {
            return (bpm / tracksBPM.Max())*100;
        }

        private short[] getKeyboardData()
        {
            List<short> data = new List<short>();
            for(int i = 0; i < keyboardRecordings.Count; i++)
            {
                for(int r = 0; r < keyboardRecordings.ElementAt(i).Length; r++)
                {
                    data.Add(keyboardRecordings.ElementAt(i).ElementAt(r));
                }
            }

            return data.ToArray();
        }

        private short[] getMouseData()
        {
            List<short> data = new List<short>();
            for (int i = 0; i < mouseRecordings.Count; i++)
            {
                for (int r = 0; r < mouseRecordings.ElementAt(i).Length; r++)
                {
                    data.Add(mouseRecordings.ElementAt(i).ElementAt(r));
                }
            }

            return data.ToArray();
        }


        private void cmdPickTrack_Click_1(object sender, EventArgs e)
        {
            pickTrack();
            //playerForm.playTrack(0);
        }
    }
}
