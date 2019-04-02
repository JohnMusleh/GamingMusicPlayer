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
        public Boolean Matching { get; private set; }
        private List<Track> tracks;

        private const int RECORDINGS_MAX_SIZE = 6; //each recording is 5 seconds
        //following queues are updated every 5 seconds when matching
        private Queue<short[]> mouseXRecordings; 
        private Queue<short[]> mouseYRecordings; 
        private Queue<short[]> keyboardRecordings;

        private const double MAX_ABPM = 30;
        private const double MAX_ASPECTIRR = 100000;

        private double keyboardBPM, mouseBPM;
        private double keyboardZCR, mouseZCR;
        private double keyboardSpectIrr, mouseSpectIrr;

        private KeyboardProcessor kp;
        private MouseProcessor mp;
        private SignalProcessor sp;

        private MainForm playerForm;
        private Database.DatabaseAdapter dbAdapter;


        private int currTrackIndex;

        public SongMatcher(MainForm main, Database.DatabaseAdapter adapter)
        {
            InitializeComponent();
            this.tracks = null;
            Matching = false;
            this.playerForm = main;
            this.dbAdapter = adapter;
            this.currTrackIndex = 0;
            sp = new SignalProcessor();

            kp = new KeyboardProcessor();
            kp.onDataReady += onKeyboardDataReady;

            mp = new MouseProcessor();
            mp.onDataReady += onMouseDataReady;
            //to get raw x/y points set mean values to 0
            mp.MeanValueX = 0;
            mp.MeanValueY = 0;

            mouseXRecordings = new Queue<short[]>();
            mouseYRecordings = new Queue<short[]>();
            keyboardRecordings = new Queue<short[]>();
            
            this.hide();
            MatcherVisible = false;
            this.TopMost = true;
        }

        /* Public Control Methods*/
        public void updateTrackList(List<Track> tracks)
        {
            this.tracks = new List<Track>();
            foreach (Track t in tracks)
            {
                Track nt = null;
                lock (dbAdapter)
                {
                    nt = dbAdapter.getTrack(t.Path);
                }
                if (nt != null)
                {
                    t.BPM = nt.BPM;
                    t.ZCR = nt.ZCR;
                    t.SpectralIrregularity = nt.SpectralIrregularity;
                    lock (tracks)
                    {
                        this.tracks.Add(t);
                    }
                }
                else
                {
                    new Thread(delegate () {
                        lock (dbAdapter)
                        {
                            playerForm.addSong(t, false, false);
                            Track at = dbAdapter.getTrack(t.Path);
                            t.BPM = at.BPM;
                            t.ZCR = at.ZCR;
                            t.SpectralIrregularity = at.SpectralIrregularity;
                            lock (tracks)
                            {
                                this.tracks.Add(t);
                            }
                        }
                    }).Start(); 
                }
                
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
            setStatus("Matching Mode:" + Matching);
        }
        public bool startMatching()
        {
            if (!Matching)
            {
                Matching = true;
                match();
                cmdToggleMatching.Invoke(new MethodInvoker(delegate {
                    cmdToggleMatching.Text = "Stop Matching";
                }));
            }
            setStatus("Matching Mode:" + Matching);
            return Matching;
        }
        public bool stopMatching()
        {
            if (Matching)
            {
                Matching = false;
                cmdToggleMatching.Invoke(new MethodInvoker(delegate {
                    cmdToggleMatching.Text = "Start Matching";
                }));
            }
            setStatus("Matching Mode:" + Matching);
            return Matching;
        }

        /* Private internal methods */
        //calculate different between track values and mouse+keyboard values (bpm/zcr/spectirr)
        private double calculateDifference(Track t)
        {
            double nMouseBPM = (mouseBPM/MAX_ABPM) * 100;
            if (mouseBPM > MAX_ABPM)
                nMouseBPM = 100;
            double nMouseSpectIrr = (mouseSpectIrr / MAX_ASPECTIRR) * 100;
            if (mouseSpectIrr > MAX_ASPECTIRR)
                nMouseSpectIrr = 100;

            double nKeyboardBPM = (keyboardBPM / MAX_ABPM) * 100;
            if (keyboardBPM > MAX_ABPM)
                nKeyboardBPM = 100;
            double nKeyboardSpectIrr = (keyboardSpectIrr / MAX_ASPECTIRR) * 100;
            if (keyboardSpectIrr > MAX_ASPECTIRR)
                nKeyboardSpectIrr = 100;

            double bpmDiff = ((0.5 * nMouseBPM) + (0.5 * nKeyboardBPM)) - normalizeTrackBPM(t.BPM);
            bpmDiff = Math.Abs(bpmDiff);

            double zcrDiff = ((0.5 * mouseZCR) + (0.5 * keyboardZCR)) - t.ZCR;
            zcrDiff = Math.Abs(zcrDiff);
            zcrDiff*=100;

            double spectIrrDiff = ((1 * nMouseSpectIrr) + (0 * nKeyboardSpectIrr)) - normalizeTrackSpectIrr(t.SpectralIrregularity);
            spectIrrDiff = Math.Abs(spectIrrDiff);
            return (bpmDiff/3) + (zcrDiff/3) + (spectIrrDiff/3);
        }
        private void pickTrack()
        {
            if (tracks != null)
            {
                double minDiff = Double.MaxValue;
                int minIndex = 0;
                for(int i=0; i<tracks.Count; i++)
                {
                    double d = calculateDifference(tracks[i]);
                    Console.WriteLine("track:" + tracks[i].Name + " D:" + d);
                    if (d < minDiff)
                    {
                        minDiff = d;
                        minIndex = i;
                    }
                }

                playerForm.playTrack(minIndex);
                
            }
        } 
        private void match()
        {
            lock (mp)
            {
                if (!mp.Processing)
                {
                    mp.record(5);
                }
            }
            lock (kp)
            {
                if (!kp.Processing)
                {
                    kp.record(5);
                }
            }
            /*lock (playerForm)
            {
                pickTrack();
            }*/
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
            List<double> tracksBPM = new List<double>();
            foreach(Track t in tracks)
            {
                tracksBPM.Add(t.BPM);
            }
            return (bpm / tracksBPM.Max())*100;
        }
        private double normalizeTrackSpectIrr(double bpm)
        {
            List<double> tracksSpectIrr = new List<double>();
            foreach (Track t in tracks)
            {
                tracksSpectIrr.Add(t.SpectralIrregularity);
            }
            return (bpm / tracksSpectIrr.Max()) * 100;
        }
        private short[] getKeyboardData()
        {
            lock (kp)
            {
                List<short> data = new List<short>();
                for (int i = 0; i < keyboardRecordings.Count; i++)
                {
                    for (int r = 0; r < keyboardRecordings.ElementAt(i).Length; r++)
                    {
                        data.Add(keyboardRecordings.ElementAt(i).ElementAt(r));
                    }
                }
                return data.ToArray();
            }
        }
        private short[] getMouseData(int xy) //xy = 0 if x , !=0 if y
        {
            lock (mp)
            {
                double avg = 0;
                List<short> data = new List<short>();
                if (xy == 0)
                {
                    for (int i = 0; i < mouseXRecordings.Count; i++)
                    {
                        for (int r = 0; r < mouseXRecordings.ElementAt(i).Length; r++)
                        {
                            data.Add(mouseXRecordings.ElementAt(i).ElementAt(r));
                            avg += mouseXRecordings.ElementAt(i).ElementAt(r);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < mouseYRecordings.Count; i++)
                    {
                        for (int r = 0; r < mouseYRecordings.ElementAt(i).Length; r++)
                        {
                            data.Add(mouseYRecordings.ElementAt(i).ElementAt(r));
                            avg += mouseYRecordings.ElementAt(i).ElementAt(r);
                        }
                    }
                }
                //substract mean value of all the data
                avg /= data.Count;
                for(int i=0; i<data.Count; i++)
                {
                    data[i] = (short)(data[i] - avg);
                }
                return data.ToArray();
            }
        }

        /* Events */
        // GUI and button events
        private void cmdPickTrack_Click_1(object sender, EventArgs e)
        {
            pickTrack();
            //playerForm.playTrack(0);
        }
        private void cmdToggleMatching_Click(object sender, EventArgs e)
        {
            if (Matching)
            {
                stopMatching();
            }
            else
            {
                startMatching();
            }
        }
        
        //Other events from different components/forms
        private void onKeyboardDataReady(object sender, EventArgs e)
        {
            if (Matching)
            {
                lock (kp)
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

                }

                lock (sp)
                {
                    //Console.WriteLine("computing values for keyboard data..");
                    sp.ComputeBPM(getKeyboardData(), 5 * keyboardRecordings.Count, true, false);
                    sp.computeTimbre(getKeyboardData(), 5 * keyboardRecordings.Count, false);
                   //Console.WriteLine("done");
                }

                //normalizing bpm
                if (sp.BPM >= MAX_ABPM)
                {
                    keyboardBPM = 100;
                }
                else
                {
                    keyboardBPM = (int)((sp.BPM / MAX_ABPM) * 100);
                }


                keyboardLabel.Invoke(new MethodInvoker(delegate {
                    keyboardLabel.Text = "keyboard BPM:" + sp.BPM;
                }));

                keyboardZCRLabel.Invoke(new MethodInvoker(delegate {
                    keyboardZCRLabel.Text = "keyboard zcr:" + sp.ZCR;
                }));

                keyboardSpectIrrLabel.Invoke(new MethodInvoker(delegate {
                    keyboardSpectIrrLabel.Text = "keyboard spect irr:" + sp.SpectralIrregularity;
                }));

                //Console.WriteLine("keyboard data added");
                match();
            }
            else
            {
                Console.WriteLine("recording stopped.");
            }
        }
        private void onMouseDataReady(object sender, EventArgs e)
        {
            if (Matching)
            {
                lock (mp)
                {
                    short[] pointerX = mp.DataX;
                    short[] dataX = new short[pointerX.Length];
                    for (int i = 0; i < pointerX.Length; i++)
                    {
                        dataX[i] = pointerX[i];
                    }

                    short[] pointerY = mp.DataY;
                    short[] dataY = new short[pointerY.Length];
                    for (int i = 0; i < pointerY.Length; i++)
                    {
                        dataY[i] = pointerY[i];
                    }

                    if (mouseXRecordings.Count < RECORDINGS_MAX_SIZE)
                    {
                        mouseXRecordings.Enqueue(dataX);
                    }
                    else
                    {
                        mouseXRecordings.Dequeue();
                        mouseXRecordings.Enqueue(dataX);
                    }

                    if (mouseYRecordings.Count < RECORDINGS_MAX_SIZE)
                    {
                        mouseYRecordings.Enqueue(dataY);
                    }
                    else
                    {
                        mouseYRecordings.Dequeue();
                        mouseYRecordings.Enqueue(dataY);
                    }
                }

                lock (sp)
                {
                    //Console.WriteLine("computing values for mouse X data..");
                    sp.ComputeBPM(getMouseData(0), 5 * mouseXRecordings.Count, true, false);
                    sp.computeTimbre(getMouseData(0), 5 * mouseXRecordings.Count, false);
                   //Console.WriteLine("done");
                }

                mouseBPM = sp.BPM;
                mouseZCR = sp.ZCR;
                mouseSpectIrr = sp.SpectralIrregularity;
                
                mouseXLabel.Invoke(new MethodInvoker(delegate {
                    //mouseXLabel.Text = "% mouseX-BPM:" + mouseBPM + "    real:"+sp.BPM+"    max:" + maxMouseBPM;
                    mouseXLabel.Text = "mouse-X BPM:" + mouseBPM;
                }));

                mouseXZCRLabel.Invoke(new MethodInvoker(delegate {
                    mouseXZCRLabel.Text = "mouse X zcr:" + mouseZCR;
                }));

                mouseXSpectIrrLabel.Invoke(new MethodInvoker(delegate {
                    mouseXSpectIrrLabel.Text = "mouse X spect Irr:" + mouseSpectIrr;
                }));

                lock (sp)
                {
                    //Console.WriteLine("computing values for mouse Y data..");
                    sp.ComputeBPM(getMouseData(1), 5 * mouseXRecordings.Count, true, false);
                    sp.computeTimbre(getMouseData(1), 5 * mouseXRecordings.Count, false);
                    //Console.WriteLine("done");
                }

                double mouseYBPM = sp.BPM;
                double mouseYZCR = sp.ZCR;
                double mouseYSpectIrr = sp.SpectralIrregularity;
                if(mouseYBPM > mouseBPM)
                    mouseBPM = mouseYBPM;

                if (mouseYZCR > mouseZCR)
                    mouseZCR = mouseYZCR;
                if (mouseYSpectIrr > mouseSpectIrr)
                    mouseSpectIrr = mouseYSpectIrr;
                mouseYLabel.Invoke(new MethodInvoker(delegate {
                    //mouseYLabel.Text = "% mouseY-BPM:" + mouseBPM + "    real:" + sp.BPM + "    max:" + maxMouseBPM;
                    mouseYLabel.Text = "mouse-Y BPM:" + mouseYBPM;
                }));

                
                mouseYZCRLabel.Invoke(new MethodInvoker(delegate {
                    mouseYZCRLabel.Text = "mouse Y zcr:" + mouseYZCR;
                }));

                mouseYSpectIrrLabel.Invoke(new MethodInvoker(delegate {
                    mouseYSpectIrrLabel.Text = "mouse Y spect Irr:" + mouseYSpectIrr;
                }));

                //Console.WriteLine("mouse data added");
                match();
            }
            else
            {
                Console.WriteLine("recording stopped.");
            }
        }
    }
}
