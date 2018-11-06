using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GamingMusicPlayer
{
    public class GlobalMouseEventArgs : EventArgs
    {
        public Point Position { get; private set; }
        public GlobalMouseEventArgs(Point p)
        {
            Position = new Point(p.X, p.Y);
        }
        
    }
}
