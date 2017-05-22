namespace QRCodeDiag
{
    partial class DebugDrawingForm
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
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.showEncodedButton = new System.Windows.Forms.Button();
            this.showRawButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 648);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(465, 75);
            this.textBox1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(630, 630);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // showEncodedButton
            // 
            this.showEncodedButton.Location = new System.Drawing.Point(483, 648);
            this.showEncodedButton.Name = "showEncodedButton";
            this.showEncodedButton.Size = new System.Drawing.Size(159, 23);
            this.showEncodedButton.TabIndex = 3;
            this.showEncodedButton.Text = "Show Encoded";
            this.showEncodedButton.UseVisualStyleBackColor = true;
            this.showEncodedButton.Click += new System.EventHandler(this.showEncodedButton_Click);
            // 
            // showRawButton
            // 
            this.showRawButton.Location = new System.Drawing.Point(483, 677);
            this.showRawButton.Name = "showRawButton";
            this.showRawButton.Size = new System.Drawing.Size(159, 23);
            this.showRawButton.TabIndex = 4;
            this.showRawButton.Text = "Show Raw";
            this.showRawButton.UseVisualStyleBackColor = true;
            this.showRawButton.Click += new System.EventHandler(this.showRawButton_Click);
            // 
            // DebugDrawingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 735);
            this.Controls.Add(this.showRawButton);
            this.Controls.Add(this.showEncodedButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "DebugDrawingForm";
            this.Text = "DebugDrawingForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DebugDrawingForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button showEncodedButton;
        private System.Windows.Forms.Button showRawButton;
    }
}