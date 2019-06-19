/* SignalProcessor class is used to compute values/features from a signal data*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace GamingMusicPlayer.SignalProcessing
{
    class SignalProcessor
    {
        private enum Task { BPM, Timbre, Pitch }
        private Task selectedTask;

        public event EventHandler onBPMReady; //invoked when computeBPM is done and it was called with thread=true
        public event EventHandler onTimbreReady;//invoked when computeTimbre is done and it was called with thread=true

        public double BPM { get; private set; }
        public double BeatCount { get; private set; }
        public double ZCR { get; private set; }
        public double SpectralIrregularity { get; private set; }
        private bool artificial_signal;
        private bool threadSupport;

        private Thread mainThread;

        public bool Processing { get; private set; }

        public SignalProcessor() {
            Processing = false;
        }
        private short[] timeDomainData;
        private int lengthInSecs;

        public bool computeTimbre(short[] timeData, int seconds, bool thread)
        {
            if (!Processing)
            {
                lengthInSecs = seconds;
                timeDomainData = new short[timeData.Length];
                Array.Copy(timeData, timeDomainData, timeData.Length);
                selectedTask = Task.Timbre;

                if (thread)
                {
                    threadSupport = true;
                    mainThread = new Thread(new ThreadStart(process));
                    mainThread.Start();
                }
                else
                {
                    threadSupport = false;
                    process();
                }
                return true;
            }
            Console.WriteLine("SignalProccessor: ComputerTimbre(), SignalProcessor already in use.");
            return false;
        }

        public bool ComputeBPM(short[] timeData,int seconds, bool artificial,bool thread)
        {
            if (!Processing)
            {
                artificial_signal = artificial;
                lengthInSecs = seconds;
                timeDomainData = new short[timeData.Length];
                Array.Copy(timeData, timeDomainData, timeData.Length);
                selectedTask = Task.BPM;
                if (thread)
                {
                    threadSupport = true;
                    mainThread = new Thread(new ThreadStart(process));
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

        public void clearMemory()
        {
            timeDomainData = null;
            GC.Collect();
        }

        private void process()
        {
            Processing = true;
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
                int blockSize = 3600;  //3600   

                short[] leftChn = chan1.ToArray();
                List<double> energies = new List<double>();
                Queue<double> historyQueue = new Queue<double>();
                int historyQueueMaxSize = 43;

                if (artificial_signal)
                {
                    blockSize = timeDomainData.Length/(lengthInSecs);  //[TESTING MOUSE]
                    if (blockSize < 20)
                    {
                        blockSize = 20;
                    }
                    historyQueueMaxSize = 10;//[TESTING MOUSE]
                    
                    leftChn = timeDomainData;  //[TESTING MOUSE]
                }
                //[DEBUG]
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
                        /* [DEBUG]
                        if (i < 100000)
                        {
                            Console.WriteLine("-beat at:" + i + "-");
                        }*/

                        
                        if (artificial_signal)
                        {
                            //testing of weighted beat counts 
                            //[TESTING] i is in leftChan.length , need to make LAST i's 'heavier'
                            //divide into 5 segments
                            beats++;
                            /*if (i <= leftChn.Length * 0.2)
                            {
                                beats += 0.25;
                            }
                            else if (i <= leftChn.Length * 0.4)
                            {
                                beats += 0.5;
                            }
                            else if (i <= leftChn.Length * 0.6)
                            {
                                beats++;
                            }
                            else if (i <= leftChn.Length * 0.8)
                            {
                                beats += 2;
                            }
                            else
                            {
                                beats += 4;
                            }*/
                            //beats++;
                        }
                        else
                        {
                            beats++;
                        }
                    }
                }
                BeatCount = beats;
                BPM = (beats*60)/lengthInSecs;

                timeDomainData = null;
                GC.Collect();
                if (threadSupport)
                {
                    onBPMReady?.Invoke(null, null);
                }
            }
            else if (selectedTask == Task.Timbre)
            {
                bool positive = true;
                if (timeDomainData[0] <= 0)
                {
                    positive = false;
                }
                
                int zcCount = 0;
                int localMax = 0; //amplitude peak (can be a positive or a negative peak)
                List<int> peaks = new List<int>();
                for(int i=0; i<timeDomainData.Length; i++)
                {
                    if (timeDomainData[i]>0 )
                    {
                        if(positive == false)
                        {
                            //add peak , add zero-cross count
                            peaks.Add(Math.Abs(localMax));
                            localMax = 0;
                            zcCount++;
                            positive = true;
                        }
                        else
                        {
                            if (timeDomainData[i] > localMax) //positive peak
                            {
                                localMax = timeDomainData[i];
                            }
                        }
                    }
                    else if (timeDomainData[i] <= 0)
                    {
                        if (positive == false)
                        {
                            if(timeDomainData[i] < localMax) //negative peak
                            {
                                localMax = timeDomainData[i];
                            }
                        }
                        else
                        {
                            //add peak
                            peaks.Add(Math.Abs(localMax));
                            localMax = 0;
                            positive = false;
                        }
                        
                    }
                }
                double spectralIrregularity = 0;
                for (int i = 0; i < peaks.Count - 1; i++)
                {
                    spectralIrregularity += (Math.Pow((peaks[i + 1] - peaks[i]), 2));
                }
                //for future optimzation-> zcCount = peaks.Count/2
                if(timeDomainData == null)
                {
                    Console.WriteLine("Error in signal processing: timeDomainData is null, ending processing");
                    return;
                }
                double zcRate = (double)zcCount / (double)timeDomainData.Length;
                ZCR = zcRate;
                SpectralIrregularity = (spectralIrregularity / (double)timeDomainData.Length);

                timeDomainData = null;
                GC.Collect();
                if (threadSupport)
                {
                    onTimbreReady?.Invoke(null, null);
                }
            }
            else
            {
                Console.WriteLine("SignalProcessor: process(), no task set.");
            }
            Processing = false;
        }
    }
}