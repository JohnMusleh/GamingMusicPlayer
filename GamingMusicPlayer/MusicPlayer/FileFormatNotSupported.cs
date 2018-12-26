using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingMusicPlayer.MusicPlayer
{
    public class FileFormatNotSupported : Exception
    {
        public FileFormatNotSupported(string msg) : base(msg)
        {

        }
    }
}
