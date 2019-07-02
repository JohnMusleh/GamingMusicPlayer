/*KeyboardProcessor class turns keyboard input data coming from a KeyBoardListener into a time domain signal data in order to compute relevant values on it.*/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;

namespace GamingMusicPlayer.SignalProcessing.Keyboard
{
    public class KeyboardProcessor
    {
        public const int SAMPLE_RATE = 20;//samples per second
        private List<Key> keysDown;

        private Thread mainThread;
        private short sampleValue;
        private int targetNumOfSamples;

        public event EventHandler onDataReady; //called when data is ready

        public bool Processing { get; private set; }

        public short[] Data { get; private set; }

        public KeyboardProcessor()
        {
            keysDown = new List<Key>(); ;
            Data = null;
            Processing = false;
            KeyboardListener.HookKeyboard();
            KeyboardListener.OnKeyPressed += OnKeyPressed;//this needs to happen only once!
        }

        //record keyboard data for secs amount of seconds and convert it into signal data
        public void record(int secs)
        {
            if (!Processing)
            {
                targetNumOfSamples = secs * SAMPLE_RATE;
                Data = new short[targetNumOfSamples];

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
            lock (keysDown)
            {
                keysDown.Clear();
            }
            Stopwatch sw = new Stopwatch();
            sampleValue = 0;
            Stopwatch swPerSample = new Stopwatch();
            Processing = true;           
            sw.Start();
            swPerSample.Start();// every (1/sampleRate) seconds , add a sample

            while (numOfSamples < targetNumOfSamples && sw.Elapsed.TotalMilliseconds < (targetNumOfSamples * 100)) {
                if(swPerSample.ElapsedMilliseconds >= (1000 / SAMPLE_RATE))
                {
                    Data[numOfSamples] = sampleValue;
                    numOfSamples++;
                    swPerSample.Restart();
                }
                else
                {
                    //avoid busy waiting
                    Thread.Sleep(TimeSpan.FromMilliseconds(1000 / SAMPLE_RATE));
                }
            }
            sw.Stop();
            Processing = false;
            onDataReady(null,null);
        }

        private void OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (Processing)
            {
                bool tick = true;
                if (e.Down)
                {
                    //key down , check if its in list, if it is in the list do not tick
                    lock (keysDown)
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
                    lock (keysDown)
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
                        lock (keysDown)
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
