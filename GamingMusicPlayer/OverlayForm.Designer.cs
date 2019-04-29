namespace GamingMusicPlayer
{
    partial class OverlayForm
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
            this.txtLabel = new System.Windows.Forms.Label();
            this.cmdPlayPause = new System.Windows.Forms.Button();
            this.dragPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // txtLabel
            // 
            this.txtLabel.AutoSize = true;
            this.txtLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.txtLabel.Location = new System.Drawing.Point(0, 0);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(79, 29);
            this.txtLabel.TabIndex = 0;
            this.txtLabel.Text = "label1";
            this.txtLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdPlayPause
            // 
            this.cmdPlayPause.Location = new System.Drawing.Point(68, 32);
            this.cmdPlayPause.Name = "cmdPlayPause";
            this.cmdPlayPause.Size = new System.Drawing.Size(58, 26);
            this.cmdPlayPause.TabIndex = 1;
            this.cmdPlayPause.Text = "play";
            this.cmdPlayPause.UseVisualStyleBackColor = true;
            this.cmdPlayPause.Click += new System.EventHandler(this.cmdPlayPause_Click);
            // 
            // dragPanel
            // 
            this.dragPanel.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.dragPanel.Location = new System.Drawing.Point(132, 32);
            this.dragPanel.Name = "dragPanel";
            this.dragPanel.Size = new System.Drawing.Size(42, 26);
            this.dragPanel.TabIndex = 2;
            this.dragPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragPanel_MouseDown);
            this.dragPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dragPanel_MouseMove);
            this.dragPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragPanel_MouseUp);
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 53);
            this.Controls.Add(this.dragPanel);
            this.Controls.Add(this.cmdPlayPause);
            this.Controls.Add(this.txtLabel);
            this.Name = "OverlayForm";
            this.Text = "OverlayTestForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayForm_FormClosing);
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtLabel;
        private System.Windows.Forms.Button cmdPlayPause;
        private System.Windows.Forms.Panel dragPanel;
    }
}