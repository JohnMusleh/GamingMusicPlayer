using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows;

namespace GamingMusicPlayer
{
    public class MouseProcessor
    {
        private bool queueLock = false;
        private List<float> sigDataList;
        private Thread mainThread;

        private int secsToProcess;
        private int minDistance;//minimum distance between last pos and current pos to capture mouse position
        private Point center;
        private Point lastPosition;

        private Queue<double> mouseInputQueue; //queue of distances from the center point

        public event EventHandler onDataReady;

        public float[] Data
        {
            get
            {
                //normalize to [-1,1] -> for each x -> x = ((x-2)*2/(max-min))-1
                float max = sigDataList.Max();
                float min = sigDataList.Min();
                Console.WriteLine("max:" + max+" min:"+min);
                for(int i=0; i < sigDataList.Count(); i++)
                {
                    sigDataList[i] = (((sigDataList[i] - min) * 2) / (max - min)) - 1;
                }
                return sigDataList.ToArray();
            }
        }

        public MouseProcessor()
        {
            mouseInputQueue = new Queue<double>();
            MouseListener.OnMouseMoved  += OnMouseMoved;//this needs to happen only once!
            sigDataList = new List<float>();
            secsToProcess = -1;
            minDistance = -1;
            center = new Point(0, 0);
            lastPosition = new Point(-1, -1);
        }

        public void record(int secs,int minDist, Point c)
        {
            secsToProcess = secs;
            minDistance = minDist;
            center = c;
            MouseListener.HookMouse();
            mainThread = new Thread(new ThreadStart(process));
            mainThread.Start();
        }

        private void OnMouseMoved(object sender, GlobalMouseEventArgs e)
        {
            if (!queueLock)
            {
                queueLock = true;
                double distance = Point.Subtract(e.Position, lastPosition).Length;
                if (distance >= minDistance)
                {
                    mouseInputQueue.Enqueue(Point.Subtract(e.Position, center).Length);
                    //Console.WriteLine(e.Position.ToString());
                    lastPosition = e.Position;
                }
                
                queueLock = false;
            }

        }

        private void process()
        {
            sigDataList.Clear();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed.TotalMilliseconds<secsToProcess*1000)
            {
                if (!queueLock)
                {
                    if (mouseInputQueue.Count > 0)
                    {
                        queueLock = true;
                        double d = mouseInputQueue.Dequeue();
                        sigDataList.Add((float)d);
                        queueLock = false;
                    }
                    
                }
            }
            Console.WriteLine("Done processing");
            sw.Stop();
            MouseListener.UnhookMouse();
            onDataReady(null, null);
        }

    }
}
