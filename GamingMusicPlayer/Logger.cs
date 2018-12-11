using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GamingMusicPlayer
{
    public partial class Logger : Form
    {
        private string txt;
        public Boolean LoggerVisible{ get; private set; }

        public Logger()
        {
            txt = "~~* Error Logger *~~";
            hide();
            InitializeComponent();
        }

        public void show()
        {
            this.Show();
            LoggerVisible = true;
            textBox1.ReadOnly = true;
            textBox1.Clear();
            textBox1.Text = txt;
        }

        public void hide()
        {
            this.Hide();
            LoggerVisible = false;
        }

        private void cmdClearLogger_Click(object sender, EventArgs e)
        {
            txt = "~~* Error Logger *~~";
            if (textBox1 != null)
            {
                textBox1.Clear();
                textBox1.Text = "~~* Error Logger *~~";
            }
            
        }

        public void log(string msg)
        {
            txt = txt + "\r\n" + msg;
            if(textBox1!=null)
            {
                textBox1.Text = textBox1.Text + "\r\n" + msg;
            }
            
        }

        private void Logger_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            //hide();
        }
    }
}
