namespace GamingMusicPlayer
{
    partial class Logger
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
            this.cmdClearLogger = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdClearLogger
            // 
            this.cmdClearLogger.Location = new System.Drawing.Point(648, 27);
            this.cmdClearLogger.Name = "cmdClearLogger";
            this.cmdClearLogger.Size = new System.Drawing.Size(140, 56);
            this.cmdClearLogger.TabIndex = 1;
            this.cmdClearLogger.Text = "Clear";
            this.cmdClearLogger.UseVisualStyleBackColor = true;
            this.cmdClearLogger.Click += new System.EventHandler(this.cmdClearLogger_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(618, 426);
            this.textBox1.TabIndex = 2;
            // 
            // Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cmdClearLogger);
            this.Name = "Logger";
            this.Text = "Logger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Logger_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdClearLogger;
        private System.Windows.Forms.TextBox textBox1;
    }
}