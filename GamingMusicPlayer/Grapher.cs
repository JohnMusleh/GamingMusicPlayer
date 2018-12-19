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

namespace GamingMusicPlayer
{
    public partial class Grapher : Form
    {
        private Track t;
        private KeyboardProcessor kp;
        private MouseProcessor mp;
        public Boolean GrapherVisible { get; private set; }

        private float[] readData = null;
             
        public Track Track
        {
            get { return Track; }
            set{ t = value;}
        }
        public Grapher()
        {
            t = null;
            InitializeComponent();
            chart1.Series.Add("wave");
            chart1.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series["wave"].ChartArea = "ChartArea1";
            kp = new KeyboardProcessor();
            kp.onDataReady += onKeyboardDataReady;

            mp = new MouseProcessor();
            mp.onDataReady+=onMouseDataReady;

            this.hide();
            GrapherVisible = false;
        }

        public void show()
        {
            this.Show();
            GrapherVisible = true;
        }

        public void plot(float[] signalData)//signal data in time domain
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
            kp.record(10);
            cmdRecordKeyboard.Text = "Recording..";
            cmdRecordKeyboard.Enabled = false;
            cmdPlotPlayingSong.Enabled = false;
            cmdRecordMouse.Enabled = false;

        }
        
        //invoke all gui controls
        private void onKeyboardDataReady(object sender, EventArgs e)
        {
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
            plot(mp.Data);
            cmdRecordKeyboard.Invoke(new MethodInvoker(delegate {
                cmdRecordMouse.Text = "Record Mouse";
                cmdRecordKeyboard.Enabled = true;
                cmdPlotPlayingSong.Enabled = true;
                cmdRecordMouse.Enabled = true;
            }));
        }

        

        private void cmdPlotPlayingSong_Click(object sender, EventArgs e)
        {
            if (chart1 != null && t != null)
            {
                Thread readThread = new Thread(new ThreadStart(readDataFromFile));
                readThread.Start();
            }

        }

        private void readDataFromFile()
        {
            NAudio.Wave.WaveChannel32 wave = null;
            List<float> data = new List<float>();
            if (t.Format.Equals("MP3"))
            {
                wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.Mp3FileReader(t.Path));
            }
            else if (t.Format.Equals("WAV"))
            {
                wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(t.Path));

            }
            if (wave != null)
            {
                Console.WriteLine("Reading file data");
                byte[] buffer = new byte[16384];
                int read = 0;
                while (wave.Position < wave.Length)
                {
                    read = wave.Read(buffer, 0, 16384);
                    for (int i = 0; i < read / 4; i++)
                    {
                        data.Add(BitConverter.ToSingle(buffer, i * 4));
                    }
                }
                Console.WriteLine("Done!");
            }
            plot(data.ToArray());
            data.Clear();
        }

        private void cmdRecordMouse_Click(object sender, EventArgs e)
        {
            double minDistance = 200;
            //1164,364 pos of record mouse button
            mp.record(10, (int)minDistance,new Point(1164, 364));
            cmdRecordMouse.Text = "Recording..";
            cmdRecordMouse.Enabled = false;
            cmdRecordKeyboard.Enabled = false;
            cmdPlotPlayingSong.Enabled = false;
        }
    }
}
