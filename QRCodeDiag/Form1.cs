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
        private enum MaskUsed
        {
            None = 0,
            Mask001 = 1,
            Mask100 = 4,
            Mask111 = 7
        }
        private QRCode qrcode;
        private QRCode displayCode;
        private QRCode backgroundCode; //qrcode that is used for decoding ToDo: Make MaskUsed property of QRCode, let QRCode decide which mask to use
        private MaskUsed maskUsed; // ToDo use better solution when mask application gets automated
        private MaskUsed CurrentMaskUsed
        {
            get { return this.maskUsed; }
            set { maskUsed = value; this.UpdateTextBox(); }
        }
        private QRCode DisplayCode
        {
            get
            {
                return this.displayCode;
            }
            set
            {
                this.backgroundCode = value;
                this.CurrentMaskUsed = MaskUsed.None;

                this.displayCode = value;
                this.UpdateTextBox();
                this.pictureBox1.Invalidate();
            }
        }
        public Form1()
        {
            InitializeComponent();
            this.CurrentMaskUsed = MaskUsed.None;
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
                try
                {
                    this.displayCode.PrintBlocks();
                }
                catch (QRCodeFormatException qe)
                {
                    MessageBox.Show(this, qe.Message);
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            this.displayCode?.DrawCode(e.Graphics);
        }

        private void xor001toolStripButton_Click(object sender, EventArgs e)
        {
            if (this.qrcode != null)
            {
                this.backgroundCode = QRCode.XOR(this.qrcode, QRCode.GetMask001());
                this.CurrentMaskUsed = MaskUsed.Mask001;
            }
        }

        private void xor100toolStripButton_Click(object sender, EventArgs e)
        {
            if (this.qrcode != null)
            {
                this.backgroundCode = QRCode.XOR(this.qrcode, QRCode.GetMask100());
                this.CurrentMaskUsed = MaskUsed.Mask100;
            }
        }

        private void xor111toolStripButton_Click(object sender, EventArgs e)
        {
            if (this.qrcode != null)
            {
                this.backgroundCode = QRCode.XOR(this.qrcode, QRCode.GetMask111());
                this.CurrentMaskUsed = MaskUsed.Mask111;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.DisplayCode.ToggleDataCell(QRCode.SIZE * e.Location.X / pictureBox1.Size.Width, QRCode.SIZE * e.Location.Y / pictureBox1.Size.Height);
            this.UpdateBackgroundCode();
            this.pictureBox1.Refresh();
            this.UpdateTextBox();
        }

        private void UpdateTextBox()
        {
            if (this.backgroundCode != null)
            {
                var sb = new StringBuilder("Mask Used: " + this.CurrentMaskUsed.ToString());
                try
                {
                    sb.AppendLine("Message: " + this.backgroundCode.Message);
                    sb.AppendLine("Padding bits: ");
                    foreach (var s in this.backgroundCode.GetPaddingBits())
                    {
                        sb.AppendLine(s);
                    }
                }
                catch (QRCodeFormatException qfe)
                {
                    sb.AppendLine(qfe.Message);
                }
                catch (NotImplementedException nie)
                {
                    sb.AppendLine(nie.Message);
                }
                finally
                {
                    this.textBox1.Text = sb.ToString();
                }
            }
        }

        private void UpdateBackgroundCode()
        {
            if(this.displayCode != null)
            {
                switch (this.CurrentMaskUsed)
                {
                    case MaskUsed.Mask001:
                        this.backgroundCode = QRCode.XOR(this.qrcode, QRCode.GetMask001());
                        break;
                    case MaskUsed.Mask100:
                        this.backgroundCode = QRCode.XOR(this.qrcode, QRCode.GetMask100());
                        break;
                    case MaskUsed.Mask111:
                        this.backgroundCode = QRCode.XOR(this.qrcode, QRCode.GetMask111());
                        break;
                    default:
                        this.backgroundCode = this.displayCode;
                        break;
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    this.backgroundCode?.SaveToFile(this.saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not save file: " + ex.Message);
                }
            }
        }
    }
}
