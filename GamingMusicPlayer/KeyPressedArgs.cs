using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace GamingMusicPlayer
{
    public class KeyPressedArgs : EventArgs
    {
        public Key KeyPressed { get; private set; }
        public KeyPressedArgs(Key k)
        {
            KeyPressed = k;
        }
    }
}
