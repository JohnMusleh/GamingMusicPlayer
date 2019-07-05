/* This class is in charge of song matching feature, it does that by 3 main things:
 *     1.classifying the current playlist of songs using the DatabaseAdapter,
 *     2.real time recording of mouse and keyboard data using KeyboardProcessor and MouseProcessor classes,
 *     computing ZCR on mouse+keyboard signals using the SignalProcessor class and picking a song from the pull that matches the value 
 *     
 *  It is also be used as a GUI window in developer mode to view the real time recording and processing values.*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using GamingMusicPlayer.MusicPlayer;
using GamingMusicPlayer.SignalProcessing;
using GamingMusicPlayer.SignalProcessing.Keyboard;
using GamingMusicPlayer.SignalProcessing.Mouse;

namespace GamingMusicPlayer
{
    public partial class SongMatcher : Form
    {
        public Boolean MatcherVisible { get; private set; }
        public Boolean Matching { get; private set; }
        private List<Track> tracks;
        private List<Track> lowGroup;
        private List<Track> medGroup;
        private List<Track> highGroup;
        private List<int> lowGroupIndices, medGroupIndices, highGroupIndices;
        private int pickedGroupLength;
        private Queue<int> historyQueue; //a queue to hold all the track indeces picked from song matching
        private bool[] historyMap; //historyMap[i] = true if track i is inside history queue
        private const int HISTORY_MAX_SIZE = 10;
        private const int MIN_DIFF = 5;

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

        private double mouseWeight, keyboardWeight;

        private KeyboardProcessor kp;
        private MouseProcessor mp;
        private SignalProcessor sp;

        private MainForm playerForm;
        private Database.DatabaseAdapter dbAdapter;

        private int currTrackIndex;

        private string prevAction;
        private Stopwatch actionChangedStopWatch;

        private Random rnd;

        private const int secondsToWaitForAction = 30;

        //to track bpm in games, every 5 seconds add 5 samples of the computed bpm
        public List<double> keyboardBpmHistory;
        public List<double> mouseBpmHistory;
        public List<double> mouseZcrHistory;
        public List<double> keyboardZcrHistory;
        public List<double> mouseSpectIrrHistory;

        public SongMatcher(MainForm main, Database.DatabaseAdapter adapter)
        {
            InitializeComponent();
            tracks = null;
            lowGroup = medGroup = highGroup = null;
            lowGroupIndices = medGroupIndices = highGroupIndices = null;
            historyMap = null;
            historyQueue = new Queue<int>();
            Matching = false;
            this.playerForm = main;
            this.dbAdapter = adapter;
            this.currTrackIndex = -1;
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

            mkTrackBar.ValueChanged += trackBarValuesChanged;
            this.hide();
            MatcherVisible = false;
            this.TopMost = true;

            main.onSongComplete += onSongComplete;
            prevAction = "";
            rnd = new Random();
            actionChangedStopWatch = new Stopwatch();
        }

        /* Public Control Methods*/
        public void updateTrackList(List<Track> tracks)
        {
            this.tracks = new List<Track>();
            double maxBpm, maxZcr,minBpm,minZcr;
            maxBpm = maxZcr = Double.MinValue;
            minBpm = minZcr = Double.MaxValue;
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
                    if (t.BPM > maxBpm)
                        maxBpm = t.BPM;
                    if (t.BPM < minBpm)
                        minBpm = t.BPM;
                    if (t.ZCR > maxZcr)
                        maxZcr = t.ZCR;
                    if (t.ZCR < minZcr)
                        minZcr = t.ZCR;
                    t.SpectralIrregularity = nt.SpectralIrregularity;
                    lock (tracks)
                    {
                        this.tracks.Add(t);
                    }
                }
            }

            this.historyMap = new bool[tracks.Count];
            for (int i = 0; i < historyMap.Length; i++)
            {
                historyMap[i] = false;
            }

            int sizeOfGroup = this.tracks.Count / 3;
            int remainder = this.tracks.Count % 3;
            if (sizeOfGroup == 0)
                return;
            List<Track> tempTracks = new List<Track>();
            List<int> tempIndices = new List<int>();
            for(int i=0; i<this.tracks.Count; i++)
            {
                tempTracks.Add(this.tracks[i]);
                tempIndices.Add(i);
            }

            int lowGroupSize = sizeOfGroup;
            if (remainder > 0)
                lowGroupSize++;
            lowGroup = new List<Track>();
            lowGroupIndices = new List<int>();

            double minD = Double.MaxValue;
            while (lowGroup.Count != lowGroupSize)
            {
                int minIndex = 0;
                for (int i = 0; i < tempTracks.Count; i++)
                {
                    // was--> (0.5 * (tempTracks[i].BPM / maxBpm)) + (0.5 * (tempTracks[i].ZCR / maxZcr));
                    double currentD = Database.DatabaseAdapter.predicts((float)tempTracks[i].BPM, (float)tempTracks[i].ZCR, (float)tempTracks[i].SpectralIrregularity); 
                    if (currentD < minD)
                    {
                        minD = currentD;
                        minIndex = i;
                    }
                }
                minD = Double.MaxValue;
                lowGroup.Add(tempTracks[minIndex]);
                lowGroupIndices.Add(tempIndices[minIndex]);
                tempTracks.RemoveAt(minIndex);
                tempIndices.RemoveAt(minIndex);
            }

            int medGroupSize = sizeOfGroup;
            if (remainder == 2)
                medGroupSize++;
            medGroup = new List<Track>();
            medGroupIndices = new List<int>();
            minD = Double.MaxValue;
            while (medGroup.Count != medGroupSize)
            {
                int minIndex = 0;
                for (int i = 0; i < tempTracks.Count; i++)
                {
                    double currentD = Database.DatabaseAdapter.predicts((float)tempTracks[i].BPM, (float)tempTracks[i].ZCR, (float)tempTracks[i].SpectralIrregularity);
                    if (currentD < minD)
                    {
                        minD = currentD;
                        minIndex = i;
                    }
                }
                minD = Double.MaxValue;
                medGroup.Add(tempTracks[minIndex]);
                medGroupIndices.Add(tempIndices[minIndex]);
                tempTracks.RemoveAt(minIndex);
                tempIndices.RemoveAt(minIndex);
            }


            highGroup = new List<Track>();
            highGroupIndices = new List<int>();
            minD = Double.MaxValue;
            while (highGroup.Count != sizeOfGroup)
            {
                int minIndex = 0;
                for (int i = 0; i < tempTracks.Count; i++)
                {
                    double currentD = Database.DatabaseAdapter.predicts((float)tempTracks[i].BPM, (float)tempTracks[i].ZCR, (float)tempTracks[i].SpectralIrregularity);
                    if (currentD < minD)
                    {
                        minD = currentD;
                        minIndex = i;
                    }
                }
                minD = Double.MaxValue;
                highGroup.Add(tempTracks[minIndex]);
                highGroupIndices.Add(tempIndices[minIndex]);
                tempTracks.RemoveAt(minIndex);
                tempIndices.RemoveAt(minIndex);
            }

            pickedGroupLength = 0;

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
            classifySongs();
            
        }
        public bool startMatching()
        {
            if (!Matching)
            {
                classifySongs();
                Matching = true;
                match();
                if (this.Visible)
                {
                    cmdToggleMatching.Invoke(new MethodInvoker(delegate
                    {
                        cmdToggleMatching.Text = "Stop Matching";
                    }));
                }
                playerForm.setOverlaySubLabel(prevAction+" action");
            }
            setStatus("Matching Mode:" + Matching);
            playerForm.AutoPick = false;
            keyboardBpmHistory = new List<double>();
            mouseBpmHistory = new List<double>();
            keyboardZcrHistory = new List<double>();
            mouseZcrHistory = new List<double>();
            mouseSpectIrrHistory = new List<double>();
            return Matching;
        }
        public bool stopMatching()
        {
            if (Matching)
            {
                Matching = false;
                cmdToggleMatching.Invoke(new MethodInvoker(delegate
                {
                    cmdToggleMatching.Text = "Start Matching";
                }));
            }
            playerForm.setOverlaySubLabel("");
            setStatus("Matching Mode:" + Matching);
            playerForm.AutoPick = true;
            return Matching;
        }
        public void setMkWeights(double mWeight)
        {
            if (mWeight > 1)
                mWeight = 1;
            int v = (int)(mWeight * (double)mkTrackBar.Maximum);
            mkTrackBar.Value = (int)(mWeight * (double)mkTrackBar.Maximum);
        }
        public double getMouseWeight() { return mouseWeight; }

        /* Private internal methods */
        private void classifySongs()
        {
            Console.WriteLine("CLASSIFIYING..");
            double maxBpm, maxZcr;
            maxBpm = maxZcr = Double.MinValue;
            if (tracks == null || lowGroup == null || medGroup == null || highGroup == null)
            {
                return;
            }
            foreach (Track t in tracks)
            {
                Console.Write(t.Name + "  :");
                Console.WriteLine(Database.DatabaseAdapter.predicts((float)t.BPM, (float)t.ZCR, (float)t.SpectralIrregularity));
                if (t.BPM > maxBpm)
                    maxBpm = t.BPM;
                if (t.ZCR > maxZcr)
                    maxZcr = t.ZCR;
            }

            Console.WriteLine("\nLOW GROUP --");
            foreach (Track t in lowGroup)
                Console.WriteLine(t.Name);
            foreach (int i in lowGroupIndices)
                Console.Write(i + "   ");

            Console.WriteLine("\n\nMED GROUP --");
            foreach (Track t in medGroup)
                Console.WriteLine(t.Name);
            foreach (int i in medGroupIndices)
                Console.Write(i + "   ");

            Console.WriteLine("\n\nHIGH GROUP --");
            foreach (Track t in highGroup)
                Console.WriteLine(t.Name);
            foreach (int i in highGroupIndices)
                Console.Write(i + "   ");
            Console.WriteLine();
        }
        //calculate different between track values and mouse+keyboard values (bpm/zcr/spectirr)
        private double calculateDifference(Track t)
        {
            double nMouseBPM = (mouseBPM / MAX_ABPM) * 100;
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
            double bpmDiff = ((mouseWeight * nMouseBPM) + (keyboardWeight * nKeyboardBPM)) - normalizeTrackBPM(t.BPM);
            bpmDiff = Math.Abs(bpmDiff);

            double zcrDiff = ((mouseWeight * mouseZCR) + (keyboardWeight * keyboardZCR)) - t.ZCR;
            zcrDiff = Math.Abs(zcrDiff);
            zcrDiff *= 100;

            double spectIrrDiff = ((1 * nMouseSpectIrr) + (0 * nKeyboardSpectIrr)) - normalizeTrackSpectIrr(t.SpectralIrregularity);
            spectIrrDiff = Math.Abs(spectIrrDiff);
            return (bpmDiff * 0.2) + (zcrDiff * 0.6) + (spectIrrDiff * 0.2);
        }
        private void pickTrack(bool force)
        {
            if (tracks != null)
            {
                if (force)
                {
                    Console.WriteLine("\n--FORCE PICKING NEXT SONG--");
                }
                bool actionChanged = false;
                List<Track> group = null;//the group picked to pick tracks from
                List<int> indices = null; //real index of the track in the whole list of tracks
                if (mouseZCR + keyboardZCR == 0)
                {
                    mouseWeight = 1;
                    keyboardWeight = 0;
                }
                else
                {
                    mouseWeight = (mouseZCR / (mouseZCR + keyboardZCR));
                    keyboardWeight = 1 - mouseWeight;
                }
                if (this.Visible)
                {
                    mkWeightLabel.Invoke(new MethodInvoker(delegate
                    {
                        mkWeightLabel.Text = " Mouse ratio:" + mouseWeight;
                    }));
                }
                
                mouseWeight = 0.5;
                keyboardWeight = 0.5;
                double overallZcr = (mouseWeight * mouseZCR) + (keyboardWeight * keyboardZCR);
                currTrackIndex = playerForm.CurrentlySelectedTrackIndex;
                if (overallZcr < 0.02)
                {
                    group = lowGroup;
                    indices = lowGroupIndices;
                    if (!prevAction.Equals("low"))
                    {   
                        if(actionChangedStopWatch.Elapsed.TotalSeconds> secondsToWaitForAction || !actionChangedStopWatch.IsRunning)
                            actionChanged = true;
                        historyQueue.Clear();
                        for (int i = 0; i < historyMap.Length; i++)
                        {
                            historyMap[i] = false;
                        }
                    }
                    if (actionChanged)
                    {
                        prevAction = "low";
                        playerForm.setOverlaySubLabel("low action");
                    }
                    if (this.Visible)
                    {
                        txtGroupLbl.Invoke(new MethodInvoker(delegate
                        {
                            txtGroupLbl.Text = prevAction + " ACTION:" + overallZcr;
                        }));
                    }
                    
                }
                else if (overallZcr < 0.06)
                {
                    group = medGroup;
                    indices = medGroupIndices;
                    if (!prevAction.Equals("medium"))
                    {
                        if (actionChangedStopWatch.Elapsed.TotalSeconds > secondsToWaitForAction || !actionChangedStopWatch.IsRunning)
                            actionChanged = true;
                        historyQueue.Clear();
                        for (int i = 0; i < historyMap.Length; i++)
                        {
                            historyMap[i] = false;
                        }
                    }
                    if (actionChanged)
                    {
                        prevAction = "medium";
                        playerForm.setOverlaySubLabel("medium action");
                    }
                    if (this.Visible)
                    {
                        txtGroupLbl.Invoke(new MethodInvoker(delegate
                        {
                            txtGroupLbl.Text = prevAction + " ACTION:" + overallZcr;
                        }));
                    }
                    
                }
                else
                {
                    group = highGroup;
                    indices = highGroupIndices;
                    if (!prevAction.Equals("high"))
                    {
                        if (actionChangedStopWatch.Elapsed.TotalSeconds > secondsToWaitForAction || !actionChangedStopWatch.IsRunning)
                            actionChanged = true;
                        historyQueue.Clear();
                        for (int i = 0; i < historyMap.Length; i++)
                        {
                            historyMap[i] = false;
                        }
                    }
                    if (actionChanged)
                    {
                        prevAction = "high";
                        playerForm.setOverlaySubLabel("high action");
                    }

                    if (this.Visible)
                    {
                        txtGroupLbl.Invoke(new MethodInvoker(delegate
                        {
                            txtGroupLbl.Text = prevAction + " ACTION:" + overallZcr;
                        }));
                    }
                    

                }
                if (group == null)
                {
                    Console.WriteLine("ERROR NO GROUP PICKED");
                    return;
                }
                pickedGroupLength = group.Count;
                int pickedTrackIndex = -1;
                List<int> consideredTracksIndices = new List<int>();
                for (int i = 0; i < group.Count; i++)
                {
                    if (!historyMap[indices[i]] || indices[i] == currTrackIndex)
                    {
                        double d = calculateDifference(group[i]);
                        if (indices[i] == currTrackIndex)
                            continue;
                        consideredTracksIndices.Add(indices[i]);
                    }
                }

                //pick random index from 0 to consideredTracksIndices.length-1
                if (consideredTracksIndices.Count > 0)
                {
                    pickedTrackIndex = consideredTracksIndices[rnd.Next(0, consideredTracksIndices.Count)];
                }

                if (pickedTrackIndex < 0)
                {
                    Console.WriteLine("ERROR no song found");
                    return;
                }

                if (actionChanged || force)
                {
                    if (actionChanged)
                    {
                        actionChangedStopWatch.Reset();
                        actionChangedStopWatch.Start();
                    }
                    if (force)
                    {
                        Console.WriteLine("-- forced --minIndex: " + pickedTrackIndex + " currTrackI:" + currTrackIndex);
                    }
                    if (pickedTrackIndex != currTrackIndex && (playerForm.Playing || force) && mouseXRecordings.Count >= RECORDINGS_MAX_SIZE)
                    {
                        Console.WriteLine("inside force if");
                        playerForm.playTrack(pickedTrackIndex);
                        currTrackIndex = pickedTrackIndex;
                        if (historyQueue.Count >= HISTORY_MAX_SIZE || historyQueue.Count >= group.Count)
                        {
                            Console.WriteLine("h queue full , clear it then add");
                            historyQueue.Clear();
                            for (int i = 0; i < historyMap.Length; i++)
                            {
                                historyMap[i] = false;
                            }
                            historyQueue.Enqueue(currTrackIndex);
                            historyMap[currTrackIndex] = true;
                        }
                        else
                        {
                            Console.WriteLine("h queue not full , just add");
                            historyQueue.Enqueue(currTrackIndex);
                            historyMap[currTrackIndex] = true;
                        }
                    }
                }


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
            lock (playerForm)
            {
                pickTrack(false);
            }
        }
        private void setStatus(String str)
        {
            if (MatcherVisible)
            {
                statusLabel.Invoke(new MethodInvoker(delegate
                {
                    statusLabel.Text = str;
                }));
            }

        }
        private double normalizeTrackBPM(double bpm)
        {
            List<double> tracksBPM = new List<double>();
            foreach (Track t in tracks)
            {
                tracksBPM.Add(t.BPM);
            }
            return (bpm / tracksBPM.Max()) * 100;
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

        private short[] getMouseData(int xy) //xy==0 if x , xy!=0 if y
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
                for (int i = 0; i < data.Count; i++)
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
            pickTrack(false);
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
                for (int i = 0; i < historyMap.Length; i++)
                    historyMap[i] = false;
                historyQueue.Clear();
                startMatching();
            }
        }

        private void trackBarValuesChanged(object sender, EventArgs e)
        {
            mouseWeight = (double)mkTrackBar.Value / (double)mkTrackBar.Maximum;
            keyboardWeight = 1 - mouseWeight;

            mouseWeightLbl.Text = "mouse:" + mouseWeight;
            keyboardWeightLbl.Text = "keyboard:" + keyboardWeight;


        }

        private void onSongComplete(object sender, EventArgs e)
        {
            if (Matching)
            {
                Console.WriteLine("song is done");
                if (historyQueue.Count >= pickedGroupLength || historyQueue.Count >= HISTORY_MAX_SIZE)
                {
                    historyQueue.Clear();
                    for (int i = 0; i < historyMap.Length; i++)
                        historyMap[i] = false;
                }
                lock (playerForm)
                {
                    Console.WriteLine("calling force pick..");
                    pickTrack(true);
                }
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
                keyboardZCR = sp.ZCR;
                if (sp.BPM >= MAX_ABPM)
                {
                    keyboardBPM = 100;
                }
                else
                {
                    keyboardBPM = (int)((sp.BPM / MAX_ABPM) * 100);
                }
                for (int i = 0; i < 5; i++)
                {
                    keyboardBpmHistory.Add(sp.BPM);
                    keyboardZcrHistory.Add(sp.ZCR);
                }
                keyboardLabel.Invoke(new MethodInvoker(delegate
                {
                    keyboardLabel.Text = "keyboard BPM:" + sp.BPM;
                }));

                keyboardZCRLabel.Invoke(new MethodInvoker(delegate
                {
                    keyboardZCRLabel.Text = "keyboard zcr:" + sp.ZCR;
                }));

                keyboardSpectIrrLabel.Invoke(new MethodInvoker(delegate
                {
                    keyboardSpectIrrLabel.Text = "keyboard spect irr:" + sp.SpectralIrregularity;
                }));

                keyboardSpectIrr = sp.SpectralIrregularity;



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
                    mouseBPM = sp.BPM;
                    mouseZCR = sp.ZCR;
                    mouseSpectIrr = sp.SpectralIrregularity;
                }


                
                mouseXLabel.Invoke(new MethodInvoker(delegate
                {
                    //mouseXLabel.Text = "% mouseX-BPM:" + mouseBPM + "    real:"+sp.BPM+"    max:" + maxMouseBPM;
                    mouseXLabel.Text = "mouse-X BPM:" + mouseBPM;
                }));

                mouseXZCRLabel.Invoke(new MethodInvoker(delegate
                {
                    mouseXZCRLabel.Text = "mouse X zcr:" + mouseZCR;
                }));

                mouseXSpectIrrLabel.Invoke(new MethodInvoker(delegate
                {
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
                if (mouseYBPM > mouseBPM)
                    mouseBPM = mouseYBPM;

                
                if (mouseYZCR > mouseZCR)
                    mouseZCR = mouseYZCR;
                if (mouseYSpectIrr > mouseSpectIrr)
                    mouseSpectIrr = mouseYSpectIrr;

                for (int i = 0; i < 5; i++)
                {
                    mouseBpmHistory.Add(mouseBPM);
                    mouseZcrHistory.Add(mouseZCR);
                    mouseSpectIrrHistory.Add(mouseSpectIrr);
                }

                mouseYLabel.Invoke(new MethodInvoker(delegate
                {
                    //mouseYLabel.Text = "% mouseY-BPM:" + mouseBPM + "    real:" + sp.BPM + "    max:" + maxMouseBPM;
                    mouseYLabel.Text = "mouse-Y BPM:" + mouseYBPM;
                }));


                mouseYZCRLabel.Invoke(new MethodInvoker(delegate
                {
                    mouseYZCRLabel.Text = "mouse Y zcr:" + mouseYZCR;
                }));

                mouseYSpectIrrLabel.Invoke(new MethodInvoker(delegate
                {
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

        private void button1_Click(object sender, EventArgs e)
        {
            setMkWeights(0.4);
        }
    }
}
