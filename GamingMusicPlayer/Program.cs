/*Main launcher of the application */
using System;
using System.Windows.Forms;

namespace GamingMusicPlayer
{
    static class Program
    {
        public const string VERSION = "1.0";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(false));
            }
            catch(Exception e)
            {
                MessageBox.Show("Error:"+e.Message, "Error Running App", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }
    }
}
