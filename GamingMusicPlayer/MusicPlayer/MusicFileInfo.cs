using System;
using System.Text;
using System.Runtime.InteropServices;

namespace GamingMusicPlayer.MusicPlayer
{
    //static class for the purpose of using mci device functions to get metadata
    public static class MusicFileInfo
    {
        public static string error_msg = "no error";
        [DllImport("winmm.dll")]
        public static extern int mciSendString(string strCommand,
      StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        [DllImport("winmm.dll")]
        public static extern int mciGetErrorString(int errCode,
                      StringBuilder errMsg, int buflen);
        public static int getLength(string path) //return length of music file in ms , returns -1 on errors
        {
            string mci_command;
            StringBuilder mci_return = new StringBuilder(128);
            int rflag;//return flag
            mci_command = "open \"" + path +
                          "\" type mpegvideo alias MusicFileInfo";
            rflag = mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
            if (rflag != 0)
            {
                // Let MCI decide which file type the song is
                mci_command = "open \"" + path +
                               "\" alias MusicFileInfo";
                rflag = mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
                if(rflag!=0)
                {
                    mciGetErrorString(rflag, mci_return, mci_return.Capacity);
                    error_msg = "getLength(): mci open command failed -> " + mci_return.ToString();
                    close();
                    return -1;
                }
            }

            mci_command = "status MusicFileInfo length";
            rflag = mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
            if (rflag == 0)
            {
                if (mci_return.Length == 0)
                {
                    close();
                    return 0;
                } 
                try
                {
                    int l = int.Parse(mci_return.ToString());
                    close();
                    return l;
                }
                catch (Exception e)
                {
                    error_msg = "getLength(): return string:" + mci_return.ToString() + "  exception:" + e.Message;
                    close();
                    return -1;
                }
            }
            mciGetErrorString(rflag, mci_return, mci_return.Capacity);
            error_msg = "getLength(): mci status command failed -> " + mci_return.ToString();
            close();
            return -1;
        }

        private static void close()
        {
            string mci_command = "close MusicFileInfo";
            mciSendString(mci_command, null, 0, IntPtr.Zero);
        }

        public static bool isCorrectFormat(string path)
        {
            string[] tokens = path.Split('.');
            string fileType = "NA";
            if (tokens.Length > 0)
            {
                fileType = tokens[tokens.Length - 1];
            }
            fileType = fileType.ToUpper();
            if (!fileType.Equals("MP3") && !fileType.Equals("WAV"))
            {
                error_msg = "music file format not supported! path:" + path;
                return false;
            }
            return true;
        }

    }
}
