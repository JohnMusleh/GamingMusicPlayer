using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GamingMusicPlayer
{
    class CMButton : Button
    {
        public CMButton()
        {
            this.BackColor = Color.Transparent;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.FlatStyle = FlatStyle.Flat;
            this.UseVisualStyleBackColor = false;
        }
    }
}
