/*MouseProcessor class turns mouse input data coming from a MouseListener into a time domain signal data in order to compute relevant values on it.*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows;

namespace GamingMusicPlayer.SignalProcessing.Mouse
{
    public class MouseProcessor
    {
        public const int SAMPLE_RATE = 20;
        private Point currPosition;

        private List<short> sigDataListX;
        private List<short> sigDataListY;
        private int targetNumOfSamples;

        //mean value to substract from each sample, if this value is less than 0: mean value dynamically the average of the samples
        public double MeanValueX { get; set; }

        public double MeanValueY { get; set; }

        public event EventHandler onDataReady;

        public bool Processing { get; private set; }

        public short[] DataX
        {
            get
            {
                lock (sigDataListX)
                {
                    double avg = 0;
                    for (int i = 0; i < sigDataListX.Count; i++)
                        avg += sigDataListX.ElementAt(i);
                    avg /= sigDataListX.Count;
                    if (MeanValueX >= 0)
                    {
                        avg = MeanValueX;
                    }
                    if (avg == 0)
                        return sigDataListX.ToArray();
                    short[] data = new short[sigDataListX.Count];
                    for (int i = 0; i < sigDataListX.Count; i++)
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
                    double avg = 0;
                    for (int i = 0; i < sigDataListY.Count; i++)
                        avg += sigDataListY.ElementAt(i);
                    avg /= sigDataListY.Count;
                    if (MeanValueX >= 0)
                    {
                        avg = MeanValueX;
                    }
                    if (avg == 0)
                        return sigDataListY.ToArray();
                    short[] data = new short[sigDataListY.Count];
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
            MouseListener.OnMouseMoved += OnMouseMoved;//this needs to happen only once!

            sigDataListX = new List<short>();
            sigDataListY = new List<short>();

            Processing = false;
            onDataReady = done;

            MeanValueX = -1;
            MeanValueY = -1;
        }

        private void done(object sender, EventArgs e)
        {
            Console.WriteLine("MOUSE DATA DONE");
        }

        public void record(int secs)
        {
            if (!Processing)
            {
                targetNumOfSamples = secs * SAMPLE_RATE;
                Thread thread = new Thread(new ThreadStart(process));
                thread.Start();
            }
            else
            {
                Console.WriteLine("mouse processor already recording..");
            }
        }

        private void OnMouseMoved(object sender, GlobalMouseEventArgs e)
        {
            if (Processing)
            {
                currPosition = e.Position;
            }

        }

        private void process()
        {
            int numOfSamples = 0;

            lock (sigDataListX)
            {
                sigDataListX.Clear();
            }
            lock (sigDataListY)
            {
                sigDataListY.Clear();
            }


            Stopwatch sw = new Stopwatch();
            Stopwatch swPerSample = new Stopwatch();
            Processing = true;
            sw.Start();
            swPerSample.Start();// every (1/sampleRate) seconds , add a sample

            while (numOfSamples < targetNumOfSamples && sw.Elapsed.TotalMilliseconds < (targetNumOfSamples / SAMPLE_RATE) * 1000)
            {
                if (swPerSample.ElapsedMilliseconds >= (1000 / SAMPLE_RATE))
                {
                    lock (sigDataListX)
                    {
                        sigDataListX.Add((short)currPosition.X);

                    }
                    lock (sigDataListY)
                    {
                        sigDataListY.Add((short)currPosition.Y);
                    }
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
            onDataReady(null, null);
        }

    }
}
