using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Windows;

using GamingMusicPlayer.MusicPlayer;
using GamingMusicPlayer.SignalProcessing.Keyboard;
using GamingMusicPlayer.SignalProcessing.Mouse;
using GamingMusicPlayer.SignalProcessing;

namespace GamingMusicPlayer
{
    /*This class is used to test signal processing algorithms, it has the ability to draw a signal on the screen. */
    public partial class Grapher : Form
    {
        private Track t;
        private KeyboardProcessor kp;
        private MouseProcessor mp;
        SignalProcessor sp;
        public Boolean GrapherVisible { get; private set; }

        private Thread drawThread;
        private float[] sigData = null;
             
        public Track Track
        {
            get { return Track; }
            set{ t = value;}
        }
        public Grapher()
        {
            InitializeComponent();
            t = null;
            chart1.Series.Add("wave");
            chart1.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series["wave"].ChartArea = "ChartArea1";
            kp = new KeyboardProcessor();
            kp.onDataReady += onKeyboardDataReady;

            mp = new MouseProcessor();
            mp.onDataReady+=onMouseDataReady;

            sp = new SignalProcessor();
            sp.onBPMReady += onBPMReady;
            drawThread = null;
            this.hide();
            GrapherVisible = false;
        }

        public void show()
        {
            this.Show();
            GrapherVisible = true;
        }

        public void plot(short[] signalData)//signal data in time domain
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            chart1.Invoke(new MethodInvoker(delegate {
                chart1.Series.Remove(chart1.Series["wave"]);
                chart1.Series.Add("wave");
                chart1.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Series["wave"].ChartArea = "ChartArea1";
            }));
            
            for (int i=0; i< signalData.Length; i++)
            {
                chart1.Invoke(new MethodInvoker(delegate {
                    chart1.Series["wave"].Points.Add(signalData[i]);
                }));
                
            }
            sw.Stop();
            Console.WriteLine("plotted in:" + sw.Elapsed.TotalMilliseconds+" ms");
            
            

        }

        public void hide()
        {
            this.Hide();
            GrapherVisible = false;
        }

        private void Grapher_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void cmdDrawTest_Click(object sender, EventArgs e)
        {
            kp.record(30);
            cmdRecordKeyboard.Text = "Recording..";
            cmdRecordKeyboard.Enabled = false;
            cmdPlotPlayingSong.Enabled = false;
            cmdRecordMouse.Enabled = false;

        }
        
        //invoke all gui controls
        private void onKeyboardDataReady(object sender, EventArgs e)
        {
            if (drawThread != null)
            {
                drawThread.Suspend();
            }
            sp.ComputeBPM(kp.Data, 30,true,true);
            plot(kp.Data);
            cmdRecordKeyboard.Invoke(new MethodInvoker(delegate {
                cmdRecordKeyboard.Text = "Record Keyboard";
                cmdRecordKeyboard.Enabled = true;
                cmdPlotPlayingSong.Enabled = true;
                cmdRecordMouse.Enabled = true;
            }));
           
        }

        private void onMouseDataReady(object sender, EventArgs e)
        {
            sp.ComputeBPM(mp.Data, 30,true,true);
            if (drawThread != null)
            {
                drawThread.Suspend();
            }
            plot(mp.Data);
            cmdRecordKeyboard.Invoke(new MethodInvoker(delegate {
                cmdRecordMouse.Text = "Record Mouse";
                cmdRecordKeyboard.Enabled = true;
                cmdPlotPlayingSong.Enabled = true;
                cmdRecordMouse.Enabled = true;
            }));
        }

        private void onBPMReady(object sender, EventArgs e)
        {
            Console.WriteLine("BPM computed:"+sp.BPM);
        }

        private void cmdPlotPlayingSong_Click(object sender, EventArgs e)
        {

            if (chart1 != null && t != null)
            {
                if (drawThread != null)
                {
                    drawThread.Suspend();
                }
                short[] trackData = readDataFromFile();
                drawThread = new Thread(new ThreadStart(readDataAndDraw));
                drawThread.Start();
                sp.ComputeBPM(trackData, (t.Length / 1000),false,true);
            }
            
        }

        private void readDataAndDraw()
        {
            short[] data = null;
            NAudio.Wave.WaveStream reader = null;
            if (t.Format.Equals("MP3"))
            {
                reader = new NAudio.Wave.Mp3FileReader(t.Path);
            }
            else if (t.Format.Equals("WAV"))
            {
                reader = new NAudio.Wave.WaveFileReader(t.Path);

            }
            if (reader != null)
            {
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 0, buffer.Length);
                data = new short[read / sizeof(short)];
                Buffer.BlockCopy(buffer, 0, data, 0, read);
            }
            List<short> chan1 = new List<short>();
            List<short> chan2 = new List<short>();

            for (int i = 0; i < data.Length - 1; i += 2)
            {
                chan1.Add(data[i]);
                chan2.Add(data[i + 1]);
            }
            plot(chan1.ToArray());
        }

        private short[] readDataFromFile()
        {
            short[] data = null;
            NAudio.Wave.WaveStream reader = null;
            if (t.Format.Equals("MP3"))
            {
                reader = new NAudio.Wave.Mp3FileReader(t.Path);
            }
            else if (t.Format.Equals("WAV"))
            {
                reader = new NAudio.Wave.WaveFileReader(t.Path);

            }
            
            if (reader != null)
            {
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 0, buffer.Length);
                data = new short[read / sizeof(short)];
                Buffer.BlockCopy(buffer, 0, data, 0, read);
            }
            return data;
        }

        private void cmdRecordMouse_Click(object sender, EventArgs e)
        {
            double minDistance = 200;
            //1164,364 pos of record mouse button
            mp.record(30, (int)minDistance,new Point(1164, 364));
            cmdRecordMouse.Text = "Recording..";
            cmdRecordMouse.Enabled = false;
            cmdRecordKeyboard.Enabled = false;
            cmdPlotPlayingSong.Enabled = false;
        }
    }
}
