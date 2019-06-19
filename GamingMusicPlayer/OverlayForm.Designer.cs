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
            this.botPanel = new System.Windows.Forms.Panel();
            this.txtCancelLbl = new System.Windows.Forms.Label();
            this.botPanel.SuspendLayout();
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
            this.cmdPlayPause.Location = new System.Drawing.Point(53, 3);
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
            this.dragPanel.Location = new System.Drawing.Point(117, 3);
            this.dragPanel.Name = "dragPanel";
            this.dragPanel.Size = new System.Drawing.Size(42, 26);
            this.dragPanel.TabIndex = 2;
            this.dragPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragPanel_MouseDown);
            this.dragPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dragPanel_MouseMove);
            this.dragPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragPanel_MouseUp);
            // 
            // botPanel
            // 
            this.botPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.botPanel.Controls.Add(this.txtCancelLbl);
            this.botPanel.Controls.Add(this.cmdPlayPause);
            this.botPanel.Controls.Add(this.dragPanel);
            this.botPanel.Location = new System.Drawing.Point(-2, 32);
            this.botPanel.Name = "botPanel";
            this.botPanel.Size = new System.Drawing.Size(185, 26);
            this.botPanel.TabIndex = 3;
            // 
            // txtCancelLbl
            // 
            this.txtCancelLbl.AutoSize = true;
            this.txtCancelLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCancelLbl.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.txtCancelLbl.Location = new System.Drawing.Point(48, -3);
            this.txtCancelLbl.Name = "txtCancelLbl";
            this.txtCancelLbl.Size = new System.Drawing.Size(79, 29);
            this.txtCancelLbl.TabIndex = 4;
            this.txtCancelLbl.Text = "label1";
            this.txtCancelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 53);
            this.Controls.Add(this.botPanel);
            this.Controls.Add(this.txtLabel);
            this.Name = "OverlayForm";
            this.Text = "OverlayTestForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayForm_FormClosing);
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.botPanel.ResumeLayout(false);
            this.botPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtLabel;
        private System.Windows.Forms.Button cmdPlayPause;
        private System.Windows.Forms.Panel dragPanel;
        private System.Windows.Forms.Panel botPanel;
        private System.Windows.Forms.Label txtCancelLbl;
    }
}