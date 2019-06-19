namespace GamingMusicPlayer
{
    partial class Grapher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cmdRecordKeyboard = new System.Windows.Forms.Button();
            this.cmdPlotPlayingSong = new System.Windows.Forms.Button();
            this.cmdRecordMouse = new System.Windows.Forms.Button();
            this.mouseXYComboBox = new System.Windows.Forms.ComboBox();
            this.cmdPlotMouseBpmHist = new System.Windows.Forms.Button();
            this.cmdPlotKeyBpmHist = new System.Windows.Forms.Button();
            this.cmdPlotMouseZcrHist = new System.Windows.Forms.Button();
            this.cmdPlotKeyZcrHist = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(12, 12);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(807, 320);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // cmdRecordKeyboard
            // 
            this.cmdRecordKeyboard.Location = new System.Drawing.Point(942, 12);
            this.cmdRecordKeyboard.Name = "cmdRecordKeyboard";
            this.cmdRecordKeyboard.Size = new System.Drawing.Size(174, 50);
            this.cmdRecordKeyboard.TabIndex = 2;
            this.cmdRecordKeyboard.Text = "Record Keyboard";
            this.cmdRecordKeyboard.UseVisualStyleBackColor = true;
            this.cmdRecordKeyboard.Click += new System.EventHandler(this.cmdDrawTest_Click);
            // 
            // cmdPlotPlayingSong
            // 
            this.cmdPlotPlayingSong.Location = new System.Drawing.Point(942, 124);
            this.cmdPlotPlayingSong.Name = "cmdPlotPlayingSong";
            this.cmdPlotPlayingSong.Size = new System.Drawing.Size(174, 50);
            this.cmdPlotPlayingSong.TabIndex = 3;
            this.cmdPlotPlayingSong.Text = "Plot Playing Song";
            this.cmdPlotPlayingSong.UseVisualStyleBackColor = true;
            this.cmdPlotPlayingSong.Click += new System.EventHandler(this.cmdPlotPlayingSong_Click);
            // 
            // cmdRecordMouse
            // 
            this.cmdRecordMouse.Location = new System.Drawing.Point(942, 68);
            this.cmdRecordMouse.Name = "cmdRecordMouse";
            this.cmdRecordMouse.Size = new System.Drawing.Size(174, 50);
            this.cmdRecordMouse.TabIndex = 4;
            this.cmdRecordMouse.Text = "Record Mouse";
            this.cmdRecordMouse.UseVisualStyleBackColor = true;
            this.cmdRecordMouse.Click += new System.EventHandler(this.cmdRecordMouse_Click);
            // 
            // mouseXYComboBox
            // 
            this.mouseXYComboBox.FormattingEnabled = true;
            this.mouseXYComboBox.Location = new System.Drawing.Point(871, 68);
            this.mouseXYComboBox.Name = "mouseXYComboBox";
            this.mouseXYComboBox.Size = new System.Drawing.Size(65, 24);
            this.mouseXYComboBox.TabIndex = 5;
            this.mouseXYComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // cmdPlotMouseBpmHist
            // 
            this.cmdPlotMouseBpmHist.Location = new System.Drawing.Point(924, 266);
            this.cmdPlotMouseBpmHist.Name = "cmdPlotMouseBpmHist";
            this.cmdPlotMouseBpmHist.Size = new System.Drawing.Size(191, 37);
            this.cmdPlotMouseBpmHist.TabIndex = 6;
            this.cmdPlotMouseBpmHist.Text = "plot mouse bpm hist";
            this.cmdPlotMouseBpmHist.UseVisualStyleBackColor = true;
            this.cmdPlotMouseBpmHist.Click += new System.EventHandler(this.cmdPlotMatcherHistory_Click);
            // 
            // cmdPlotKeyBpmHist
            // 
            this.cmdPlotKeyBpmHist.Location = new System.Drawing.Point(924, 309);
            this.cmdPlotKeyBpmHist.Name = "cmdPlotKeyBpmHist";
            this.cmdPlotKeyBpmHist.Size = new System.Drawing.Size(191, 37);
            this.cmdPlotKeyBpmHist.TabIndex = 7;
            this.cmdPlotKeyBpmHist.Text = "plot keyboard bpm hist";
            this.cmdPlotKeyBpmHist.UseVisualStyleBackColor = true;
            this.cmdPlotKeyBpmHist.Click += new System.EventHandler(this.cmdPlotKeyMatcherHistory_Click);
            // 
            // cmdPlotMouseZcrHist
            // 
            this.cmdPlotMouseZcrHist.Location = new System.Drawing.Point(924, 180);
            this.cmdPlotMouseZcrHist.Name = "cmdPlotMouseZcrHist";
            this.cmdPlotMouseZcrHist.Size = new System.Drawing.Size(191, 37);
            this.cmdPlotMouseZcrHist.TabIndex = 8;
            this.cmdPlotMouseZcrHist.Text = "plot mouse zcr hist";
            this.cmdPlotMouseZcrHist.UseVisualStyleBackColor = true;
            this.cmdPlotMouseZcrHist.Click += new System.EventHandler(this.cmdPlotMouseZcrHist_Click);
            // 
            // cmdPlotKeyZcrHist
            // 
            this.cmdPlotKeyZcrHist.Location = new System.Drawing.Point(924, 223);
            this.cmdPlotKeyZcrHist.Name = "cmdPlotKeyZcrHist";
            this.cmdPlotKeyZcrHist.Size = new System.Drawing.Size(191, 37);
            this.cmdPlotKeyZcrHist.TabIndex = 9;
            this.cmdPlotKeyZcrHist.Text = "plot keyboard zcr hist";
            this.cmdPlotKeyZcrHist.UseVisualStyleBackColor = true;
            this.cmdPlotKeyZcrHist.Click += new System.EventHandler(this.cmdPlotKeyZcrHist_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(696, 326);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 37);
            this.button1.TabIndex = 10;
            this.button1.Text = "plot mouse spectirr hist";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Grapher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1127, 361);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdPlotKeyZcrHist);
            this.Controls.Add(this.cmdPlotMouseZcrHist);
            this.Controls.Add(this.cmdPlotKeyBpmHist);
            this.Controls.Add(this.cmdPlotMouseBpmHist);
            this.Controls.Add(this.mouseXYComboBox);
            this.Controls.Add(this.cmdRecordMouse);
            this.Controls.Add(this.cmdPlotPlayingSong);
            this.Controls.Add(this.cmdRecordKeyboard);
            this.Controls.Add(this.chart1);
            this.Name = "Grapher";
            this.Text = "Grapher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Grapher_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NAudio.Gui.WaveViewer waveViewer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button cmdRecordKeyboard;
        private System.Windows.Forms.Button cmdPlotPlayingSong;
        private System.Windows.Forms.Button cmdRecordMouse;
        private System.Windows.Forms.ComboBox mouseXYComboBox;
        private System.Windows.Forms.Button cmdPlotMouseBpmHist;
        private System.Windows.Forms.Button cmdPlotKeyBpmHist;
        private System.Windows.Forms.Button cmdPlotMouseZcrHist;
        private System.Windows.Forms.Button cmdPlotKeyZcrHist;
        private System.Windows.Forms.Button button1;
    }
}