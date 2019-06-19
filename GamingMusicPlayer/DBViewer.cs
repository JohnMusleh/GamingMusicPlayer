/*DBViewer is a Developer mode ONLY GUI used to view and edit the database during runtime, for development and testing purposes only.*/
using System;
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
