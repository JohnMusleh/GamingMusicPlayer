using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;

namespace GamingMusicPlayer.SignalProcessing.Keyboard
{
    //class that turns keyboard input data coming from a KeyBoardListener into a time domain signal data in order to compute relevant values on it.
    public class KeyboardProcessor
    {
        private List<Key> keysDown;
        private object keysDownLock = new Object();

        private Thread mainThread;
        private short sampleValue;

        private short[] signalData;
        private int targetNumOfSamples;

        private bool working;

        public event EventHandler onDataReady; //called when data is ready

        public bool Processing { get { return this.working; } }

        public short[] Data
        {
            get
            {
                return signalData;
            }
        }

        public KeyboardProcessor()
        {
            keysDown = new List<Key>(); ;
            signalData = null;
            working = false;
            KeyboardListener.HookKeyboard();
            KeyboardListener.OnKeyPressed += OnKeyPressed;//this needs to happen only once!
        }

        //record keyboard data for secs amount of seconds and convert it into signal data
        public void record(int secs)
        {
            if (!working)
            {
                targetNumOfSamples = secs * 20;
                signalData = new short[targetNumOfSamples];

                mainThread = new Thread(new ThreadStart(process));
                mainThread.Start();
            }
            else
            {
                Console.WriteLine("keyboard processor already recording..");
            }
            
        }

        private void process()
        {
            
            int numOfSamples = 0;
            int sampleRate = 20; //20 samples per second
            lock (keysDownLock)
            {
                keysDown.Clear();
            }
            Stopwatch sw = new Stopwatch();
            sampleValue = 0;
            Stopwatch swPerSample = new Stopwatch();
            working = true;           
            sw.Start();
            swPerSample.Start();// every (1/sampleRate) seconds , add a sample

            while (numOfSamples < targetNumOfSamples && sw.Elapsed.TotalMilliseconds < (targetNumOfSamples * 100)) {
                if(swPerSample.ElapsedMilliseconds >= (1000 / sampleRate))
                {
                    signalData[numOfSamples] = sampleValue;
                    numOfSamples++;
                    swPerSample.Restart();
                }
            }
            sw.Stop();
            working = false;
            onDataReady(null,null);
        }

        private void OnKeyPressed(object sender, KeyPressedArgs e) {
            if (working)
            {
                bool tick = true;
                if (e.Down)
                {
                    //key down , check if its in list, if it is in the list do not tick
                    lock (keysDownLock)
                    {
                        foreach (Key k in keysDown)
                        {
                            if (k.Equals(e.KeyPressed))
                            {
                                tick = false;
                            }
                        }
                    }
                }
                else
                {
                    //key up, remove from list
                    lock (keysDownLock)
                    {
                        for (int i = 0; i < keysDown.Count; i++)
                        {
                            if (keysDown[i].Equals(e.KeyPressed))
                            {
                                keysDown.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                if (tick)
                {
                    if (e.Down)
                    { //add the key to the keysdown list
                        lock (keysDownLock)
                        {
                            keysDown.Add(e.KeyPressed);
                        }
                    }
                    if (sampleValue == 0)
                    {
                        //Console.WriteLine("ticked to 1"); //[TEST]
                        sampleValue = 1;
                    }
                    else {
                        //Console.WriteLine("ticked to 0"); //[TEST]
                        sampleValue = 0;
                    }
                }
                
            }
        }
    }
}
