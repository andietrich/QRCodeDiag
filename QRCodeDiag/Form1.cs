using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRCodeDiag
{
    public partial class Form1 : Form //ToDo: Bytes anzeigen, Byte anklicken/auswählen und Wert ändern/anzeigen
    {
        private QRCode qrcode;
        private QRCode displayCode;
        private QRCode DisplayCode
        {
            get
            {
                return this.displayCode;
            }
            set
            {
                this.displayCode = value;
                this.panel1.Invalidate();
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if(this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    this.qrcode = new QRCode(this.openFileDialog1.FileName);
                }
                catch(QRCodeFormatException ex)
                {
                    MessageBox.Show(this, ex.Message + Environment.NewLine + ex.InnerException?.Message);
                }
                this.DisplayCode = this.qrcode;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) // Print Data
        {
            if (this.displayCode == null)
            {
                MessageBox.Show(this, "No valid code loaded.");
            }
            else
            {
                //try
                {
                    this.displayCode.PrintBlocks();
                }
                //catch (QRCodeFormatException qe)
                //{
                //    MessageBox.Show(this, qe.Message);
                //}
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            this.displayCode?.DrawCode(e.Graphics);
        }

        private void xor001toolStripButton_Click(object sender, EventArgs e)
        {
            if(this.qrcode != null)
                this.DisplayCode = QRCode.XOR(this.qrcode, QRCode.GetMask001());
        }

        private void xor100toolStripButton_Click(object sender, EventArgs e)
        {
            if (this.qrcode != null)
                this.DisplayCode = QRCode.XOR(this.qrcode, QRCode.GetMask100());
        }

        private void xor111toolStripButton_Click(object sender, EventArgs e)
        {
            if (this.qrcode != null)
                this.DisplayCode = QRCode.XOR(this.qrcode, QRCode.GetMask111());
        }
    }
}
