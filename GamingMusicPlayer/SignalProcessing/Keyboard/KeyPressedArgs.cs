using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace GamingMusicPlayer.SignalProcessing.Keyboard
{
    public class KeyPressedArgs : EventArgs
    {
        public Key KeyPressed { get;}
        public Boolean Down { get;}
        public KeyPressedArgs(Key k,Boolean d)
        {
            KeyPressed = k;
            Down = d;
        }

        public override string ToString() {
            if (Down)
                return KeyPressed.ToString()+" Down";
            return KeyPressed.ToString() + " Up"; ;
        }


    }
}
