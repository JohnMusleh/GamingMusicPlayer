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
        public double BPM { get { return bpm; } private set { bpm = value; } }
        private bool artificial_signal;

        private Thread mainThread;
        private bool mainThreadRunning;

        public SignalProcessor() {
            mainThreadRunning = false;
        }
        private short[] timeDomainData;
        private int lengthInSecs;

        public bool ComputeBPM(short[] timeData,int seconds, bool artificial)
        {
            if (!mainThreadRunning)
            {
                artificial_signal = artificial;
                lengthInSecs = seconds;
                timeDomainData = new short[timeData.Length];
                Array.Copy(timeData, timeDomainData, timeData.Length);
                mainThread = new Thread(new ThreadStart(process));
                selectedTask = Task.BPM;
                mainThread.Start();
                return true;
            }
            Console.WriteLine("SignalProccessor: ComputerBPM(), SignalProcessor already in use.");
            return false;
        }

        private void process()
        {
            mainThreadRunning = true;
            if (selectedTask == Task.BPM)
            {
                /*//converting float to short
                byte[] bytes = new byte[timeDomainData.Length * sizeof(float)];
                Buffer.BlockCopy(timeDomainData, 0, bytes, 0, bytes.Length); //floats to bytes

                short[] shortSampleBuffer = new short[bytes.Length / sizeof(short)];
                Buffer.BlockCopy(bytes, 0, shortSampleBuffer, 0, bytes.Length); //bytes to shorts*/

                //http://archive.gamedev.net/archive/reference/programming/features/beatdetection/index.html
                //Simple sound energy algorithm #3:
                int beats = 0;
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
                    blockSize = timeDomainData.Length/lengthInSecs;  //[TESTING MOUSE]
                    
                    historyQueueMaxSize = (lengthInSecs*3)/10;//[TESTING MOUSE]
                    
                    leftChn = timeDomainData;  //[TESTING MOUSE]
                }
                Console.WriteLine("block size:" + blockSize);
                Console.WriteLine(historyQueueMaxSize);
                Console.WriteLine("leftChn.length" + leftChn.Length);
                for (int i=0; i < leftChn.Length; i+= blockSize)
                {
                    double instantEnergy = 0;
                    for (int k = i; k < i+ blockSize && k < leftChn.Length; k++)
                    {
                        instantEnergy += Math.Pow(leftChn[k], 2);
                    }
                    Console.Write(" instant_energy:" + instantEnergy+" i:"+i+"/ ");
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
                        
                        beats++;
                    }
                }
                BPM = (beats*60)/lengthInSecs;
                Console.WriteLine("\nComputeBPM: beats:" + beats + " in " + lengthInSecs + " seconds");
                Console.WriteLine("ComputeBPM: BPM:" + BPM);

                timeDomainData = null;
                onBPMReady(null, null);
            }
            else
            {
                Console.WriteLine("SignalProcessor: process(), no task set.");
            }
            mainThreadRunning = false;
        }
    }
}