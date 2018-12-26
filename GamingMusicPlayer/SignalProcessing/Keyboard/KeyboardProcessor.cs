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
    //the class takes 10 samples every 1000 ms (1 second)
    public class KeyboardProcessor
    {
        private Thread mainThread;
        private Queue<KeyPressedArgs> keyInputQueue;

        private bool queueLock=false;

        private float[] signalData;
        private float[][] signalDataBySamples; //an array of 10 sample arrays [ [firt 10 samples] , [2nd 10 samples], ..]
        private int targetNumOfSamples;

        public event EventHandler onDataReady; //called when data is ready

        public float[] Data
        {
            get
            {
                int s = 0;
                //transform signalDataBySamples into signalData
                for(int i=0; i < signalDataBySamples.Length; i++)
                {
                    for(int j = 0; signalDataBySamples[i]!=null && j < signalDataBySamples[i].Length;j++)
                    {
                        signalData[s] = signalDataBySamples[i][j];
                        s++;
                    }
                }
                return signalData;
            }
        }

        public KeyboardProcessor()
        {
            keyInputQueue = new Queue<KeyPressedArgs>();
            KeyboardListener.OnKeyPressed += OnKeyPressed;//this needs to happen only once!
            signalData = null;
        }

        //record keyboard data for secs amount of seconds and convert it into signal data
        public void record(int secs)
        {
            keyInputQueue.Clear();
            targetNumOfSamples = secs * 10;
            signalData = new float[targetNumOfSamples];
            signalDataBySamples = new float[secs][];
            KeyboardListener.HookKeyboard();
            
            mainThread = new Thread(new ThreadStart(process));
            mainThread.Start();
        }

        private void process()
        {
            int numOfSamples = 0;
            int sec = 0;
            int sampleInSec = 0;
           
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //while samples are less than targetNumOfSamples -> create  more samples
            //when done unhook keyboard and raise dataReady flag
            //while (sw.Elapsed.TotalMilliseconds < (targetNumOfSamples* 100)) ;
            Console.WriteLine("starting proccessing..");
            while (numOfSamples < targetNumOfSamples && sw.Elapsed.TotalMilliseconds < (targetNumOfSamples * 100)) {
                if (((targetNumOfSamples * 100) - sw.Elapsed.TotalMilliseconds) % 1000 == 0)
                {
                    Console.WriteLine((targetNumOfSamples * 100) - sw.Elapsed.TotalMilliseconds);
                   
                    Console.WriteLine(numOfSamples+"/" +targetNumOfSamples);
                    
                }
                   
                if (keyInputQueue.Count > 0 && !queueLock)
                {
                    queueLock = true;
                    if (sampleInSec == 0 && sec < signalDataBySamples.Length)
                    {
                        signalDataBySamples[sec] = new float[10];
                    }
                    KeyPressedArgs k = keyInputQueue.Dequeue();
                    if (k!=null && k.Down)
                    {
                        if(sec< signalDataBySamples.Length && sampleInSec<signalDataBySamples[sec].Length)
                            signalDataBySamples[sec][sampleInSec] = 1;
                    }
                    else
                    {
                        if (sec < signalDataBySamples.Length && sampleInSec<signalDataBySamples[sec].Length)
                            signalDataBySamples[sec][sampleInSec] = 0;

                    }
                    numOfSamples++;
                    sampleInSec++;
                    if (sampleInSec >= 10)
                    {
                        sampleInSec = 0;
                        sec++;
                    }
                    queueLock = false;
                }
            }
            Console.WriteLine("Done processing");
            KeyboardListener.UnHookKeyboard();
            sw.Stop();
            onDataReady(null,null);
        }

        private void OnKeyPressed(object sender, KeyPressedArgs e) {
            if (!queueLock)
            {
                queueLock = true;
                keyInputQueue.Enqueue(e);
                queueLock = false;
            }
            
        }

        


    }
}
