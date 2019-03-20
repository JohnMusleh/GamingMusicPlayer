using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows;

namespace GamingMusicPlayer.SignalProcessing.Mouse
{
    //class that turns mouse input data coming from a MouseListener into a time domain signal data in order to compute relevant values on it.
    public class MouseProcessor
    {
        private Point currPosition;

        private Object sigDataLock = new Object();
        private List<int> sigDataListX;
        private List<int> sigDataListY;
        private int targetNumOfSamples;

        private Thread mainThread;

        private bool working;

        public event EventHandler onDataReady;

        public bool Processing { get { return this.working; } }

        public short[] DataX
        {
            get
            {
                lock (sigDataListX)
                {
                    short[] data = new short[sigDataListX.Count];
                    double avg = sigDataListX.Average();
                    for (int i=0; i<sigDataListX.Count; i++)
                    {
                        data[i] = (short)(sigDataListX[i] - avg);
                    }
                    return data;
                }
            }
        }

        public short[] DataY
        {
            get
            {
                lock (sigDataListY)
                {
                    short[] data = new short[sigDataListY.Count];
                    double avg = sigDataListY.Average();
                    for (int i = 0; i < sigDataListY.Count; i++)
                    {
                        data[i] = (short)(sigDataListY[i] - avg);
                    }
                    return data;
                }
            }
        }

        public MouseProcessor()
        {
            MouseListener.HookMouse();
            MouseListener.OnMouseMoved  += OnMouseMoved;//this needs to happen only once!

            sigDataListX = new List<int>();
            sigDataListY = new List<int>();
            
            working = false;
            onDataReady = done;
        }

        private void done(object sender, EventArgs e)
        {

        }

        public void record(int secs)
        {
            if (!working)
            {
                targetNumOfSamples = secs * 20;
                mainThread = new Thread(new ThreadStart(process));
                mainThread.Start();
            }
            else
            {
                Console.WriteLine("mouse processor already recording..");
            }
        }

        private void OnMouseMoved(object sender, GlobalMouseEventArgs e)
        {

            if (working)
            {
                currPosition = e.Position;
            }

        }

        private void process()
        {
            int numOfSamples = 0;
            int sampleRate = 20; //20 samples per second

            lock (sigDataLock)
            {
                sigDataListX.Clear();
                sigDataListY.Clear();
            }
            

            Stopwatch sw = new Stopwatch();
            Stopwatch swPerSample = new Stopwatch();
            working = true;
            sw.Start();
            swPerSample.Start();// every (1/sampleRate) seconds , add a sample

            while (numOfSamples < targetNumOfSamples && sw.Elapsed.TotalMilliseconds < (targetNumOfSamples * 100))
            {
                if (swPerSample.ElapsedMilliseconds >= (1000 / sampleRate))
                {
                    lock (sigDataLock)
                    {
                        sigDataListX.Add((short)currPosition.X);
                        sigDataListY.Add((short)currPosition.Y);
                    }
                    numOfSamples++;
                    swPerSample.Restart();
                }
            }
            sw.Stop();
            working = false;
            onDataReady(null, null);

        }

    }
}
