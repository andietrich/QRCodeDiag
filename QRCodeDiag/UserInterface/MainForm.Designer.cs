namespace QRCodeDiag.UserInterface
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newCodeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rawOverlayToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.encodingToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.paddingToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.showXORedToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.mask000ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mask001ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mask010ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mask011ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mask100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mask101ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mask110ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mask111ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bgImgToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bgImgOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newCodeToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.rawOverlayToolStripButton,
            this.encodingToolStripButton,
            this.paddingToolStripButton,
            this.showXORedToolStripButton,
            this.toolStripDropDownButton1,
            this.bgImgToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(574, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newCodeToolStripButton
            // 
            this.newCodeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newCodeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newCodeToolStripButton.Image")));
            this.newCodeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newCodeToolStripButton.Name = "newCodeToolStripButton";
            this.newCodeToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newCodeToolStripButton.Text = "Create empty code";
            this.newCodeToolStripButton.Click += new System.EventHandler(this.newCodeToolStripButton_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "Open File";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "Save Data";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // rawOverlayToolStripButton
            // 
            this.rawOverlayToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rawOverlayToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("rawOverlayToolStripButton.Image")));
            this.rawOverlayToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rawOverlayToolStripButton.Name = "rawOverlayToolStripButton";
            this.rawOverlayToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.rawOverlayToolStripButton.Text = "Raw byte overlay on/off";
            this.rawOverlayToolStripButton.Click += new System.EventHandler(this.rawOverlayToolStripButton_Click);
            // 
            // encodingToolStripButton
            // 
            this.encodingToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.encodingToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("encodingToolStripButton.Image")));
            this.encodingToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.encodingToolStripButton.Name = "encodingToolStripButton";
            this.encodingToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.encodingToolStripButton.Text = "Encoded message overlay on/off";
            this.encodingToolStripButton.Click += new System.EventHandler(this.encodingToolStripButton_Click);
            // 
            // paddingToolStripButton
            // 
            this.paddingToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paddingToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("paddingToolStripButton.Image")));
            this.paddingToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paddingToolStripButton.Name = "paddingToolStripButton";
            this.paddingToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.paddingToolStripButton.Text = "Padding and Terminator overlay on/off";
            this.paddingToolStripButton.Click += new System.EventHandler(this.paddingToolStripButton_Click);
            // 
            // showXORedToolStripButton
            // 
            this.showXORedToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showXORedToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("showXORedToolStripButton.Image")));
            this.showXORedToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showXORedToolStripButton.Name = "showXORedToolStripButton";
            this.showXORedToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.showXORedToolStripButton.Text = "Show XORed code on/off";
            this.showXORedToolStripButton.Click += new System.EventHandler(this.showXORedToolStripButton_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mask000ToolStripMenuItem,
            this.mask001ToolStripMenuItem,
            this.mask010ToolStripMenuItem,
            this.mask011ToolStripMenuItem,
            this.mask100ToolStripMenuItem,
            this.mask101ToolStripMenuItem,
            this.mask110ToolStripMenuItem,
            this.mask111ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::QRCodeDiag.Properties.Resources.mask000;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "Select XOR Mask";
            // 
            // mask000ToolStripMenuItem
            // 
            this.mask000ToolStripMenuItem.Image = global::QRCodeDiag.Properties.Resources.mask000;
            this.mask000ToolStripMenuItem.Name = "mask000ToolStripMenuItem";
            this.mask000ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mask000ToolStripMenuItem.Text = "Mask 000";
            this.mask000ToolStripMenuItem.Click += new System.EventHandler(this.mask000ToolStripMenuItem_Click);
            // 
            // mask001ToolStripMenuItem
            // 
            this.mask001ToolStripMenuItem.Image = global::QRCodeDiag.Properties.Resources.mask001;
            this.mask001ToolStripMenuItem.Name = "mask001ToolStripMenuItem";
            this.mask001ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mask001ToolStripMenuItem.Text = "Mask 001";
            this.mask001ToolStripMenuItem.Click += new System.EventHandler(this.mask001ToolStripMenuItem_Click);
            // 
            // mask010ToolStripMenuItem
            // 
            this.mask010ToolStripMenuItem.Image = global::QRCodeDiag.Properties.Resources.mask010;
            this.mask010ToolStripMenuItem.Name = "mask010ToolStripMenuItem";
            this.mask010ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mask010ToolStripMenuItem.Text = "Mask 010";
            this.mask010ToolStripMenuItem.Click += new System.EventHandler(this.mask010ToolStripMenuItem_Click);
            // 
            // mask011ToolStripMenuItem
            // 
            this.mask011ToolStripMenuItem.Image = global::QRCodeDiag.Properties.Resources.mask011;
            this.mask011ToolStripMenuItem.Name = "mask011ToolStripMenuItem";
            this.mask011ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mask011ToolStripMenuItem.Text = "Mask 011";
            this.mask011ToolStripMenuItem.Click += new System.EventHandler(this.mask011ToolStripMenuItem_Click);
            // 
            // mask100ToolStripMenuItem
            // 
            this.mask100ToolStripMenuItem.Image = global::QRCodeDiag.Properties.Resources.mask100;
            this.mask100ToolStripMenuItem.Name = "mask100ToolStripMenuItem";
            this.mask100ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mask100ToolStripMenuItem.Text = "Mask 100";
            this.mask100ToolStripMenuItem.Click += new System.EventHandler(this.mask100ToolStripMenuItem_Click);
            // 
            // mask101ToolStripMenuItem
            // 
            this.mask101ToolStripMenuItem.Image = global::QRCodeDiag.Properties.Resources.mask101;
            this.mask101ToolStripMenuItem.Name = "mask101ToolStripMenuItem";
            this.mask101ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mask101ToolStripMenuItem.Text = "Mask 101";
            this.mask101ToolStripMenuItem.Click += new System.EventHandler(this.mask101ToolStripMenuItem_Click);
            // 
            // mask110ToolStripMenuItem
            // 
            this.mask110ToolStripMenuItem.Image = global::QRCodeDiag.Properties.Resources.mask110;
            this.mask110ToolStripMenuItem.Name = "mask110ToolStripMenuItem";
            this.mask110ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mask110ToolStripMenuItem.Text = "Mask 110";
            this.mask110ToolStripMenuItem.Click += new System.EventHandler(this.mask110ToolStripMenuItem_Click);
            // 
            // mask111ToolStripMenuItem
            // 
            this.mask111ToolStripMenuItem.Image = global::QRCodeDiag.Properties.Resources.mask111;
            this.mask111ToolStripMenuItem.Name = "mask111ToolStripMenuItem";
            this.mask111ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mask111ToolStripMenuItem.Text = "Mask 111";
            this.mask111ToolStripMenuItem.Click += new System.EventHandler(this.mask111ToolStripMenuItem_Click);
            // 
            // bgImgToolStripButton
            // 
            this.bgImgToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bgImgToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("bgImgToolStripButton.Image")));
            this.bgImgToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bgImgToolStripButton.Name = "bgImgToolStripButton";
            this.bgImgToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.bgImgToolStripButton.Text = "Set background image";
            this.bgImgToolStripButton.Click += new System.EventHandler(this.bgImgToolStripButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Text Files|*.txt";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 584);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(550, 116);
            this.textBox1.TabIndex = 2;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Text Files|*.txt";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(550, 550);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.Resize += new System.EventHandler(this.pictureBox1_Resize);
            // 
            // bgImgOpenFileDialog
            // 
            this.bgImgOpenFileDialog.Filter = "Portable Network Graphics (.png)|*.png|Bitmap (.bmp)|*.bmp";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 712);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.toolStrip1);
            this.MinimumSize = new System.Drawing.Size(590, 750);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton rawOverlayToolStripButton;
        private System.Windows.Forms.ToolStripButton encodingToolStripButton;
        private System.Windows.Forms.ToolStripButton paddingToolStripButton;
        private System.Windows.Forms.ToolStripButton showXORedToolStripButton;
        private System.Windows.Forms.ToolStripButton newCodeToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem mask000ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mask001ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mask010ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mask011ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mask100ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mask101ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mask110ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mask111ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton bgImgToolStripButton;
        private System.Windows.Forms.OpenFileDialog bgImgOpenFileDialog;
    }
}

