using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows;

using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using GamingMusicPlayer.MusicPlayer;

namespace GamingMusicPlayer.SignalProcessing
{
    //a class to compute values from a signal data [BPM / timbre / pitch]
    class SignalProcessor
    {
        private enum Task { BPM, Timbre, Pitch }
        private Task selectedTask;

        private Track track;
        public event EventHandler onBPMReady;

        private double bpm;
        private double beatCount;
        public double BPM { get { return bpm; } private set { bpm = value; } }
        public double BeatCount { get { return beatCount; } private set { beatCount = value; } }
        private bool artificial_signal;
        private bool threadSupport;

        private Thread mainThread;
        private bool calculating;

        public bool Processing { get { return calculating; } }

        public SignalProcessor() {
            calculating = false;
        }
        private short[] timeDomainData;
        private int lengthInSecs;


        public bool ComputeBPM(short[] timeData,int seconds, bool artificial,bool thread)
        {
            if (!calculating)
            {
                artificial_signal = artificial;
                lengthInSecs = seconds;
                timeDomainData = new short[timeData.Length];
                Array.Copy(timeData, timeDomainData, timeData.Length);
                
                if (thread)
                {
                    threadSupport = true;
                    mainThread = new Thread(new ThreadStart(process));
                    selectedTask = Task.BPM;
                    mainThread.Start();
                }
                else
                {
                    threadSupport = false;
                    process();
                } 
                return true;
            }
            Console.WriteLine("SignalProccessor: ComputerBPM(), SignalProcessor already in use.");
            return false;
        }

        private void process()
        {
            calculating = true;
            if (selectedTask == Task.BPM)
            {
                //http://archive.gamedev.net/archive/reference/programming/features/beatdetection/index.html
                //Simple sound energy algorithm #3:
                double beats = 0;
                List<short> chan1 = new List<short>();
                List<short> chan2 = new List<short>();

                for (int i = 0; i < timeDomainData.Length-1; i += 2)
                {
                    chan1.Add(timeDomainData[i]);
                    chan2.Add(timeDomainData[i + 1]);
                }
                int blockSize = 3600;     

                short[] leftChn = chan1.ToArray();
                List<double> energies = new List<double>();
                Queue<double> historyQueue = new Queue<double>();
                int historyQueueMaxSize = 43;

                if (artificial_signal)
                {
                    blockSize = timeDomainData.Length/(lengthInSecs);  //[TESTING MOUSE]
                    
                    historyQueueMaxSize = 10;//[TESTING MOUSE]
                    
                    leftChn = timeDomainData;  //[TESTING MOUSE]
                }
                /*Console.WriteLine("block size:" + blockSize);
                Console.WriteLine(historyQueueMaxSize);
                Console.WriteLine("leftChn.length" + leftChn.Length);*/
                for (int i=0; i < leftChn.Length; i+= blockSize)
                {
                    double instantEnergy = 0;
                    for (int k = i; k < i+ blockSize && k < leftChn.Length; k++)
                    {
                        instantEnergy += Math.Pow(leftChn[k], 2);
                    }
                    //Console.Write(" instant_energy:" + instantEnergy+" i:"+i+"/ ");
                    energies.Add(instantEnergy);
                    //adding to historyQueue
                    if (historyQueue.Count < historyQueueMaxSize)
                    {
                        historyQueue.Enqueue(instantEnergy);
                    }
                    else
                    {
                        historyQueue.Dequeue();
                        historyQueue.Enqueue(instantEnergy);
                    }

                    double localAverage = historyQueue.Average();

                    double variance = historyQueue.Select(val => (val - localAverage) * (val - localAverage)).Sum(); //sum squares of differences
                    variance = (variance / historyQueue.Count) / Math.Pow(10, 22);

                    double constnt = ((-0.0025714 * variance)) + 1.5142857;
                    if (instantEnergy > (constnt * localAverage))
                    {
                        if (i < 100000)
                        {
                            Console.Write("-beat at:" + i + "-");
                        }

                        //testing of weighted beat counts 
                        //[TESTING] i is in leftChan.length , need to make LAST i's heavier
                        //divide into 4 segments
                        if (i <= leftChn.Length * 0.25)
                        {
                            beats += 0.25;
                        }
                        else if (i <= leftChn.Length * 0.50)
                        {
                            beats += 0.5;
                        }
                        else if (i <= leftChn.Length * 0.75)
                        {
                            beats++;
                        }
                        else
                        {
                            beats += 2;
                        }
                        
                    }
                }
                BeatCount = beats;
                BPM = (beats*60)/lengthInSecs;
                Console.WriteLine("\nComputeBPM: beats:" + beats + " in " + lengthInSecs + " seconds");
                Console.WriteLine("ComputeBPM: BPM:" + BPM);

                timeDomainData = null;
                if (threadSupport)
                {
                    onBPMReady(null, null);
                }
            }
            else
            {
                Console.WriteLine("SignalProcessor: process(), no task set.");
            }
            calculating = false;
        }
    }
}