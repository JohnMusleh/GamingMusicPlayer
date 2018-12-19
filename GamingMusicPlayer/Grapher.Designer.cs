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
            this.cmdRecordKeyboard.Location = new System.Drawing.Point(942, 32);
            this.cmdRecordKeyboard.Name = "cmdRecordKeyboard";
            this.cmdRecordKeyboard.Size = new System.Drawing.Size(174, 50);
            this.cmdRecordKeyboard.TabIndex = 2;
            this.cmdRecordKeyboard.Text = "Record Keyboard";
            this.cmdRecordKeyboard.UseVisualStyleBackColor = true;
            this.cmdRecordKeyboard.Click += new System.EventHandler(this.cmdDrawTest_Click);
            // 
            // cmdPlotPlayingSong
            // 
            this.cmdPlotPlayingSong.Location = new System.Drawing.Point(942, 191);
            this.cmdPlotPlayingSong.Name = "cmdPlotPlayingSong";
            this.cmdPlotPlayingSong.Size = new System.Drawing.Size(174, 50);
            this.cmdPlotPlayingSong.TabIndex = 3;
            this.cmdPlotPlayingSong.Text = "Plot Playing Song";
            this.cmdPlotPlayingSong.UseVisualStyleBackColor = true;
            this.cmdPlotPlayingSong.Click += new System.EventHandler(this.cmdPlotPlayingSong_Click);
            // 
            // cmdRecordMouse
            // 
            this.cmdRecordMouse.Location = new System.Drawing.Point(942, 108);
            this.cmdRecordMouse.Name = "cmdRecordMouse";
            this.cmdRecordMouse.Size = new System.Drawing.Size(174, 50);
            this.cmdRecordMouse.TabIndex = 4;
            this.cmdRecordMouse.Text = "Record Mouse";
            this.cmdRecordMouse.UseVisualStyleBackColor = true;
            this.cmdRecordMouse.Click += new System.EventHandler(this.cmdRecordMouse_Click);
            // 
            // Grapher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1127, 344);
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
    }
}