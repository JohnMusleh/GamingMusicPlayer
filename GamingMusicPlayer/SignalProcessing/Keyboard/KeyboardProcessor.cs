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
    //class that turns keyboard input data coming from a KeyBoardListener into a time domain signal data
    public class KeyboardProcessor
    {
        private Thread mainThread;
        private Queue<KeyPressedArgs> keyInputQueue;

        private bool queueLock=false;

        private short[] signalData;
        private int targetNumOfSamples;

        private bool working;

        public event EventHandler onDataReady; //called when data is ready

        public bool Processing { get { return this.working; } }

        public short[] Data
        {
            get
            {
                short y = 0;
                List<short> finalData = new List<short>();
                for(int i = 1; i < signalData.Length; i++)
                {
                    if (signalData[i] == 0)
                    {
                        if (signalData[i - 1] == 0)
                        {
                            y--;
                        }
                        finalData.Add(y);
                    }
                    else
                    {
                        y++;
                        finalData.Add(y);
                    }
                }
                return signalData;
            }
        }

        public KeyboardProcessor()
        {
            keyInputQueue = new Queue<KeyPressedArgs>();
            KeyboardListener.HookKeyboard();
            KeyboardListener.OnKeyPressed += OnKeyPressed;//this needs to happen only once!
            signalData = null;
            working = false;
        }

        //record keyboard data for secs amount of seconds and convert it into signal data
        public void record(int secs)
        {
            keyInputQueue.Clear();
            targetNumOfSamples = secs * 20;
            signalData = new short[targetNumOfSamples];
            
            
            mainThread = new Thread(new ThreadStart(process));
            mainThread.Start();
        }

        private void process()
        {
            working = true;
            int numOfSamples = 0;
            int sampleRate = 20; //20 samples per second
           
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Stopwatch swPerSample = new Stopwatch();
            swPerSample.Start();// every (1/sampleRate) seconds , add a zero , if key was detected (up or down) restart this stopwatch

            while (numOfSamples < targetNumOfSamples && sw.Elapsed.TotalMilliseconds < (targetNumOfSamples * 100)) {
                if(keyInputQueue.Count ==0 && swPerSample.ElapsedMilliseconds >= (1000 / sampleRate))
                {
                    signalData[numOfSamples] = 0;
                    numOfSamples++;
                    swPerSample.Restart();
                }
                else if (keyInputQueue.Count > 0 && !queueLock)
                {   
                    queueLock = true;
                    KeyPressedArgs k = keyInputQueue.Dequeue();
                    //Console.WriteLine(k.ToString());
                    if (k!=null && k.Down)
                    {
                        signalData[numOfSamples] = 1;
                        numOfSamples++;
                        //key down
                    }
                    else if(k!=null)
                    {
                        signalData[numOfSamples] = 0;
                        numOfSamples++;
                        //key up
                    }
                    queueLock = false;
                    swPerSample.Restart();
                }
            }
            sw.Stop();
            working = false;
            onDataReady(null,null);
        }

        private void OnKeyPressed(object sender, KeyPressedArgs e) {
            if (working && !queueLock)
            {
                queueLock = true;
                keyInputQueue.Enqueue(e);
                queueLock = false;
            }
            
        }
    }
}
