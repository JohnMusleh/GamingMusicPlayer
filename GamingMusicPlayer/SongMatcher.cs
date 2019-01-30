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
using NAudio.CoreAudioApi;

namespace GamingMusicPlayer
{
    public partial class SongMatcher : Form
    {
        public Boolean MatcherVisible { get; private set; }

        private bool matching;
        private List<Track> tracks;
        private List<double> tracksBPM;

        private const int RECORDINGS_MAX_SIZE = 12; //each recording is 5 seconds
        private Queue<short[]> mouseRecordings; 
        private Queue<short[]> keyboardRecordings;

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

        //ts3client_win64
        private VolumeMixer vm;


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

            mouseRecordings = new Queue<short[]>();
            keyboardRecordings = new Queue<short[]>();

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
                
                if (keyboardRecordings.Count < RECORDINGS_MAX_SIZE)
                {
                    keyboardRecordings.Enqueue(data);
                }
                else
                {
                    keyboardRecordings.Dequeue();
                    keyboardRecordings.Enqueue(data);
                }
                Console.WriteLine("computijng bpm of keyboard data, duration:" + (5 * keyboardRecordings.Count));
                sp.ComputeBPM(getKeyboardData(), 5*keyboardRecordings.Count, true, false);
                keyboardBPM = sp.BPM;
                
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
                mouseBPM = sp.BPM;
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
            //txtLogs.Text = "";
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Communications);
            var sessions = device.AudioSessionManager.Sessions;
            Console.WriteLine("device:"+device+"  sessions:"+sessions.Count);
            for (int i = 0; i < sessions.Count; i++)
            {
                var session = sessions[i];
                if (ProcessExists(session.GetProcessID))
                {
                    try
                    {
                        var process = Process.GetProcessById((int)session.GetProcessID);
                        Console.WriteLine((i + 1) + ":" + process.ProcessName + " peak:" + session.AudioMeterInformation.MasterPeakValue);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }


            vm = new VolumeMixer();
            vm.OnPeakChanged += onPeakChanged;
            vm.subscribeApp("ts3client_win64");
            vm.subscribeApp("chrome");
            vm.startListening();
            

        }

        private void onPeakChanged(object sender, VolumeMixer.PeakChangedArgs e)
        {
            if (e.app.name.Equals("ts3client_win64"))
            {
                teamspeakPeakLabel.Invoke(new MethodInvoker(delegate {
                    if(e.app.peak>0.2 || e.app.peak == 0)
                    {
                        teamspeakPeakLabel.Text = "ts3 peak:" + e.app.peak;
                    }
                    
                }));
            }
            //Console.WriteLine(e.app.name+" -volume peak:"+e.app.peak);
        }

        bool ProcessExists(uint processId)
        {
            try
            {
                var process = Process.GetProcessById((int)processId);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
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
    }
}
