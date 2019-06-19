using System;
using System.Windows.Input;

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
