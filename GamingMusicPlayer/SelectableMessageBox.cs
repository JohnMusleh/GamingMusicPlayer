using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace GamingMusicPlayer
{
    class SelectableMessageBox : Form
    {
        private Button cmdOK;
        private Label textLabel;
        private ListBox itemsListBox;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        public SelectableMessageBox(string text,string[] items)
        {
            InitializeComponent();
            textLabel.Text = text;
            foreach(string item in items)
            {
                itemsListBox.Items.Add(item);
            }
            itemsListBox.SelectedIndex = 0;
        }
        

        public string getSelectedItem()
        {
            return itemsListBox.SelectedItem.ToString();
        }

        private void InitializeComponent()
        {
            this.cmdOK = new System.Windows.Forms.Button();
            this.textLabel = new System.Windows.Forms.Label();
            this.itemsListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(12, 119);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(142, 25);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // textLabel
            // 
            this.textLabel.AutoSize = true;
            this.textLabel.Location = new System.Drawing.Point(43, 9);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(65, 17);
            this.textLabel.TabIndex = 1;
            this.textLabel.Text = "titleLabel";
            // 
            // itemsListBox
            // 
            this.itemsListBox.FormattingEnabled = true;
            this.itemsListBox.ItemHeight = 16;
            this.itemsListBox.Location = new System.Drawing.Point(24, 29);
            this.itemsListBox.Name = "itemsListBox";
            this.itemsListBox.Size = new System.Drawing.Size(120, 84);
            this.itemsListBox.TabIndex = 2;
            // 
            // SelectableMessageBox
            // 
            this.ClientSize = new System.Drawing.Size(175, 156);
            this.ControlBox = false;
            this.Controls.Add(this.itemsListBox);
            this.Controls.Add(this.textLabel);
            this.Controls.Add(this.cmdOK);
            this.Name = "SelectableMessageBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
