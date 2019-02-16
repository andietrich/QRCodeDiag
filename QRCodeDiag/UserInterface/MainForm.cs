using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QRCodeDiag.QRCode;

namespace QRCodeDiag.UserInterface
{
    public partial class MainForm : Form //ToDo: implement selecting symbol with mouse to change its value. Implement automatic generation of all elements like format info, encoding info, message, ...
    {
        //[Flags] //ToDo: hold button to draw lines
        //private enum ButtonDown
        //{
        //    None = 0x00,
        //    MouseLeft = 0x01,
        //    MouseRight = 0x02,
        //    MouseMiddle = 0x04,
        //    MouseOther = 0x08,
        //    ControlButton = 0x10,
        //    ShiftButton = 0x20,
        //    AltButton = 0x40
        //}

        private QRCode displayCode; // stores the non-xored QRCode for displaying while the backgroundCode is xored for analysis
        private QRCode backgroundCode; // qrcode that is used for decoding ToDo: Make MaskType property of QRCode, let QRCode decide which mask to use
        private MaskType maskUsed; // ToDo use better solution when mask application gets automated
        //private ButtonDown buttonDown; //ToDo: hold button to draw lines
        private bool showRawOverlay;
        private bool showEncodingOverlay;
        private bool showPaddingOverlay;
        private bool showXORed;
        
        private MaskType CurrentMaskUsed
        {
            get { return this.maskUsed; }
            set
            {
                this.maskUsed = value;
                if (this.DisplayCode != null)
                {
                    this.BackgroundCode = (value == MaskType.None) ? this.DisplayCode : XORMask.XOR(this.DisplayCode, value);
                }
            }
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
                this.CurrentMaskUsed = MaskType.None; // resetting the backgroundCode

                this.displayCode = value;
                this.UpdateTextBox();
                this.pictureBox1.Invalidate();
            }
        }
        private QRCode BackgroundCode //ToDo better solution needed
        {
            get
            {
                return this.backgroundCode;
            }
            set
            {
                this.backgroundCode = value;
                this.UpdateTextBox();
                this.pictureBox1.Invalidate();
            }
        }
        public MainForm()
        {
            InitializeComponent();
            this.showRawOverlay = false;
            this.showEncodingOverlay = true;
            this.showPaddingOverlay = true;
            this.showXORed = false;
            this.CurrentMaskUsed = MaskType.None;
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if(this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    this.DisplayCode = new QRCode(this.openFileDialog1.FileName);
                }
                catch(QRCodeFormatException ex)
                {
                    MessageBox.Show(this, ex.Message + Environment.NewLine + ex.InnerException?.Message);
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            bool drawTransparent = this.pictureBox1.BackgroundImage != null;
            if(this.showXORed)
                this.BackgroundCode?.DrawCode(e.Graphics, this.pictureBox1.Size, drawTransparent);
            else
                this.DisplayCode?.DrawCode(e.Graphics, this.pictureBox1.Size, drawTransparent);
            if (this.showRawOverlay)
                this.BackgroundCode?.DrawRawByteLocations(e.Graphics, this.pictureBox1.Size, true, true);
            if (this.showEncodingOverlay)
                this.BackgroundCode?.DrawEncodedData(e.Graphics, this.pictureBox1.Size, true, false);
            if (this.showPaddingOverlay)
            {
                this.BackgroundCode?.DrawPadding(e.Graphics, this.pictureBox1.Size, true, false);
                this.BackgroundCode?.DrawTerminator(e.Graphics, this.pictureBox1.Size, true);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.DisplayCode != null)
            {
                var edgeLength = this.DisplayCode.GetEdgeLength();
                switch(e.Button)
                {
                    case MouseButtons.Left:
                        //this.buttonDown |= ButtonDown.MouseLeft; //ToDo: hold button to draw lines
                        this.DisplayCode.SetDataCell(edgeLength * e.Location.X / pictureBox1.Size.Width, edgeLength * e.Location.Y / pictureBox1.Size.Height, '1');
                        break;
                    case MouseButtons.Right:
                        //this.buttonDown |= ButtonDown.MouseRight; //ToDo: hold button to draw lines
                        this.DisplayCode.SetDataCell(edgeLength * e.Location.X / pictureBox1.Size.Width, edgeLength * e.Location.Y / pictureBox1.Size.Height, '0');
                        break;
                    case MouseButtons.Middle:
                        //this.buttonDown |= ButtonDown.MouseMiddle; //ToDo: hold button to draw lines
                        this.DisplayCode.SetDataCell(edgeLength * e.Location.X / pictureBox1.Size.Width, edgeLength * e.Location.Y / pictureBox1.Size.Height, 'u');
                        break;
                    default:
                        //this.buttonDown |= ButtonDown.MouseOther; //ToDo: hold button to draw lines
                        this.DisplayCode.ToggleDataCell(edgeLength * e.Location.X / pictureBox1.Size.Width, edgeLength * e.Location.Y / pictureBox1.Size.Height);
                        break;
                }
                this.UpdateBackgroundCode();
                this.pictureBox1.Invalidate();
                this.UpdateTextBox();
            }
        }

        private void UpdateTextBox()
        {
            if (this.BackgroundCode != null)
            {
                var sb = new StringBuilder("Mask Used: " + this.CurrentMaskUsed.ToString());
                try
                {
                    sb.AppendLine("Message: " + this.BackgroundCode.Message);
                    sb.AppendLine("Terminator: " + this.BackgroundCode.GetTerminator());
                    sb.AppendLine("Padding bits: ");
                    foreach (var s in this.BackgroundCode.GetPaddingBits())
                    {
                        sb.AppendLine(s);
                    }
                    sb.AppendLine("RepairMessage():");
                    sb.AppendLine(this.backgroundCode.RepairMessage());
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
                if (this.CurrentMaskUsed == MaskType.None)
                {
                    this.backgroundCode = this.displayCode;
                }
                else
                {
                    this.backgroundCode = XORMask.XOR(this.DisplayCode, this.CurrentMaskUsed);
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    this.DisplayCode?.SaveToFile(this.saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not save file: " + ex.Message);
                }
            }
        }

        private void rawOverlayToolStripButton_Click(object sender, EventArgs e)
        {
            this.showRawOverlay = !this.showRawOverlay;
            this.pictureBox1.Invalidate();
        }

        private void encodingToolStripButton_Click(object sender, EventArgs e)
        {
            this.showEncodingOverlay = !this.showEncodingOverlay;
            this.pictureBox1.Invalidate();
        }

        private void paddingToolStripButton_Click(object sender, EventArgs e)
        {
            this.showPaddingOverlay = !this.showPaddingOverlay;
            this.pictureBox1.Invalidate();
        }

        private void showXORedToolStripButton_Click(object sender, EventArgs e)
        {
            this.showXORed = !this.showXORed;
            this.pictureBox1.Invalidate();
        }

        private void newCodeToolStripButton_Click(object sender, EventArgs e)
        {
            var createForm = new CreateNewCode();
            if (createForm.ShowDialog(this) == DialogResult.OK)
            {
                this.DisplayCode = new QRCode(createForm.Version); //ToDo ask to save modified codes
            }
        }

        private void mask000ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             this.CurrentMaskUsed = MaskType.Mask000;
        }

        private void mask001ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = MaskType.Mask001;
        }

        private void mask010ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = MaskType.Mask010;
        }

        private void mask011ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = MaskType.Mask011;
        }

        private void mask100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = MaskType.Mask100;
        }

        private void mask101ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = MaskType.Mask101;
        }

        private void mask110ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = MaskType.Mask110;
        }

        private void mask111ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = MaskType.Mask111;
        }

        private void bgImgToolStripButton_Click(object sender, EventArgs e)
        {
            if(this.bgImgOpenFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    this.pictureBox1.BackgroundImage = System.Drawing.Image.FromFile(this.bgImgOpenFileDialog.FileName);
                }
                catch(OutOfMemoryException ex)
                {
                    MessageBox.Show("Could not open background image: " + ex.Message);
                }
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                Control control = (Control)sender;
                if (control.Size.Height != control.Size.Width)
                {
                    control.Size = new System.Drawing.Size(control.Size.Height, control.Size.Height);
                    
                }
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            var newMinFormWidth = this.pictureBox1.Size.Width + 3 * this.pictureBox1.Location.X;
            this.Size = new System.Drawing.Size(newMinFormWidth, this.Size.Height);
        }
    }
}
