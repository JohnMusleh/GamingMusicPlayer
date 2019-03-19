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
    public partial class DBViewer : Form
    {
        public DBViewer()
        {
            InitializeComponent();
        }

        private void songBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.songBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.songsDBDataSet);

        }

        private void DBViewer_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'songsDBDataSet.Song' table. You can move, or remove it, as needed.
            this.songTableAdapter.Fill(this.songsDBDataSet.Song);

        }
    }
}
