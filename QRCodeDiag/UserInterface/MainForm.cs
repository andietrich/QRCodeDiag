using QRCodeBaseLib;
using QRCodeBaseLib.DataBlocks.SymbolCodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private XORMask.MaskType maskUsed; // ToDo use better solution when mask application gets automated
        //private ButtonDown buttonDown; //ToDo: hold button to draw lines
        public bool ShowRawOverlay { get; set; }
        public bool ShowEncodingOverlay { get; set; }
        public bool ShowPaddingOverlay { get; set; }
        public bool ShowXORed { get; set; }

        private XORMask.MaskType CurrentMaskUsed
        {
            get { return this.maskUsed; }
            set
            {
                this.maskUsed = value;
                if (this.DisplayCode != null)
                {
                    this.BackgroundCode = (value == XORMask.MaskType.None) ? this.DisplayCode : XORMask.XOR(this.DisplayCode, value);
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
                this.CurrentMaskUsed = XORMask.MaskType.None; // resetting the backgroundCode

                this.displayCode = value;
                this.UpdateTextBox();
                this.pictureBox1.Invalidate();
            }
        }
        private QRCode BackgroundCode
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
            this.ShowRawOverlay = false;
            this.ShowEncodingOverlay = true;
            this.ShowPaddingOverlay = true;
            this.ShowXORed = false;
            this.CurrentMaskUsed = XORMask.MaskType.None;
            this.RecalculateFormMinimumSize();

            var optitem = new CodeSymbolCodeOptionsItem();
            optitem.CodeSymbolName = "Code symbol 1";
            var optitem2 = new CodeSymbolCodeOptionsItem();
            optitem2.CodeSymbolName = "Code symbol 2";
            this.optionsTableLayoutPanel.Controls.Add(optitem, 0, 0);
            //this.optionsTableLayoutPanel.Controls.Add(button_x, 0, 0);
            //this.optionsTableLayoutPanel.Controls.Add(button_x2, 0, 1);
            this.optionsTableLayoutPanel.Controls.Add(optitem2, 0, 1);
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
            if (this.BackgroundCode != null)
            {
                int     edgeLength      = this.BackgroundCode.GetEdgeLength();
                float   codeElWidth     = (float)this.pictureBox1.Size.Width / edgeLength;
                float   codeElHeight    = (float)this.pictureBox1.Size.Height / edgeLength;
                bool    drawTransparent = this.pictureBox1.BackgroundImage != null;
                var     codeDrawer      = new CodeElementDrawer(codeElWidth, codeElHeight);
                var     rawCode         = this.BackgroundCode.GetRawCode();
                var     rawDataBytes    = this.backgroundCode.GetRawDataBytes();
                var     rawECCBytes     = this.backgroundCode.GetRawECCBytes();
                var     encodedData     = this.BackgroundCode.GetEncodedSymbols();
                var     padding         = this.BackgroundCode.GetPaddingBits();
                var     terminator      = this.BackgroundCode.GetTerminator();

                if (this.ShowXORed)
                    codeDrawer.DrawQRCode(this.BackgroundCode, e.Graphics, drawTransparent);
                else
                    codeDrawer.DrawQRCode(this.DisplayCode, e.Graphics, drawTransparent);

                if (this.ShowRawOverlay && rawCode != null)
                    codeDrawer.DrawCodeSymbolCode(rawCode, e.Graphics, Color.Orange, Color.Cyan, true, true);

                if (this.ShowEncodingOverlay && encodedData != null)
                    codeDrawer.DrawCodeSymbolCode(encodedData, e.Graphics, Color.Red, Color.LightBlue, true, true);


                //////////////////////////////////////// testing only
                if (this.ShowPaddingOverlay && rawDataBytes != null)
                    codeDrawer.DrawCodeSymbolCode(rawDataBytes, e.Graphics, Color.Blue, Color.LightBlue, true, true);

                if (this.ShowPaddingOverlay && rawDataBytes != null)
                    codeDrawer.DrawCodeSymbolCode(rawECCBytes, e.Graphics, Color.Purple, Color.LightBlue, true, true);

                if (this.ShowEncodingOverlay && padding != null)
                    codeDrawer.DrawCodeSymbolCode(padding, e.Graphics, Color.Blue, Color.LightBlue, true, true);

                if (this.ShowEncodingOverlay && terminator != null)
                    codeDrawer.DrawCodeSymbol(terminator, e.Graphics, Color.Purple, true);
                //////////////////////////////////////// ^^^^^^^testing only

                //if (this.ShowPaddingOverlay && padding != null)
                //    codeDrawer.DrawCodeSymbolCode(padding, e.Graphics, Color.Blue, Color.LightBlue, true, true);

                //if (this.ShowPaddingOverlay && terminator != null)  // ToDo separate condition this.ShowPaddingOverlay for terminator
                //    codeDrawer.DrawCodeSymbol(terminator, e.Graphics, Color.Purple, true);
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
            }
        }

        private void UpdateTextBox()
        {
            if (this.BackgroundCode != null)
            {
                var sb = new StringBuilder("Mask Used: " + this.CurrentMaskUsed.ToString());
                try
                {
                    var paddingBits = this.BackgroundCode.GetPaddingBits();
                    var terminator = this.BackgroundCode.GetTerminator();
                    sb.AppendLine("Message: " + this.BackgroundCode.Message);
                    sb.AppendLine("Terminator: " + this.BackgroundCode.GetTerminator()?.BitString ?? "No terminator found");
                    sb.AppendLine("Padding bits: ");
                    if (this.BackgroundCode.GetPaddingBits() == null)
                        sb.AppendLine("Padding bits have not been initialized yet.");
                    else
                        sb.AppendLine(string.Join(Environment.NewLine, this.BackgroundCode.GetPaddingBits().GetSymbolBitStrings()));
                    
                    sb.AppendLine("RepairMessage():" + this.BackgroundCode.GetRepairMessageStatusLine());
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
            if(this.DisplayCode != null)
            {
                if (this.CurrentMaskUsed == XORMask.MaskType.None)
                {
                    this.BackgroundCode = this.DisplayCode;
                }
                else
                {
                    this.BackgroundCode = XORMask.XOR(this.DisplayCode, this.CurrentMaskUsed);
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
            this.ShowRawOverlay = !this.ShowRawOverlay;
            this.pictureBox1.Invalidate();
        }

        private void encodingToolStripButton_Click(object sender, EventArgs e)
        {
            this.ShowEncodingOverlay = !this.ShowEncodingOverlay;
            this.pictureBox1.Invalidate();
        }

        private void paddingToolStripButton_Click(object sender, EventArgs e)
        {
            this.ShowPaddingOverlay = !this.ShowPaddingOverlay;
            this.pictureBox1.Invalidate();
        }

        private void showXORedToolStripButton_Click(object sender, EventArgs e)
        {
            this.ShowXORed = !this.ShowXORed;
            this.pictureBox1.Invalidate();
        }

        private void newCodeToolStripButton_Click(object sender, EventArgs e)
        {
            var createForm = new CreateNewCode();
            if (createForm.ShowDialog(this) == DialogResult.OK)
            {
                this.DisplayCode = new QRCode((uint) createForm.Version, createForm.ECCLevel, createForm.MaskType); //ToDo ask to save modified codes
            }
        }

        private void mask000ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             this.CurrentMaskUsed = XORMask.MaskType.Mask000;
        }

        private void mask001ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = XORMask.MaskType.Mask001;
        }

        private void mask010ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = XORMask.MaskType.Mask010;
        }

        private void mask011ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = XORMask.MaskType.Mask011;
        }

        private void mask100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = XORMask.MaskType.Mask100;
        }

        private void mask101ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = XORMask.MaskType.Mask101;
        }

        private void mask110ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = XORMask.MaskType.Mask110;
        }

        private void mask111ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentMaskUsed = XORMask.MaskType.Mask111;
        }

        private void bgImgToolStripButton_Click(object sender, EventArgs e)
        {
            if(this.bgImgOpenFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    this.pictureBox1.BackgroundImage = System.Drawing.Image.FromFile(this.bgImgOpenFileDialog.FileName);
                    this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                }
                catch(OutOfMemoryException ex)
                {
                    MessageBox.Show("Could not open background image: " + ex.Message);
                }
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            //if (sender is Control)
            //{
            //    Control control = (Control)sender;
            //    if (control.Size.Height != control.Size.Width)
            //    {
            //        control.Size = new System.Drawing.Size(control.Size.Height, control.Size.Height);
            //    }
            //}
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            //var newMinFormWidth = this.pictureBox1.Size.Width + 3 * this.pictureBox1.Location.X;
            //this.Size = new System.Drawing.Size(newMinFormWidth, this.Size.Height);
        }

        private void topTableLayoutPanel_Resize(object sender, EventArgs e)
        {
            var tlPanel = sender as TableLayoutPanel;

            var width = tlPanel.GetColumnWidths()[0];
            var height = tlPanel.GetRowHeights()[0];
            var edgeLength = Math.Min(width-9, height-9);

            this.pictureBox1.Size = new Size(edgeLength, edgeLength);
        }

        private void optionsTableLayoutPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            var newMinSize = this.pictureBox1.MinimumSize;
            var optionsWidth = 0;
            var optionsHeight = 0;

            foreach (var c in this.optionsTableLayoutPanel.Controls)
            {
                Control ctrl = c as Control;
                optionsHeight += ctrl.MinimumSize.Height;
                if (ctrl.MinimumSize.Width > optionsWidth)
                    optionsWidth = ctrl.MinimumSize.Width;
            }
            newMinSize.Width += optionsWidth + 19; // 4x 4 pixels to border + 3 pixels border
            newMinSize.Height = Math.Max(newMinSize.Height, optionsHeight) + 10; // 2x 4 pixels to border + 2 pixels border
            this.topTableLayoutPanel.MinimumSize = newMinSize;

            this.RecalculateFormMinimumSize();
        }

        private void RecalculateFormMinimumSize()
        {
            var newMinimumSize = new Size();
            newMinimumSize.Height = this.textBox1.MinimumSize.Height
                                  + this.topTableLayoutPanel.MinimumSize.Height
                                  + this.toolStrip1.MinimumSize.Height
                                  + 30 + 3 + 6 + 12;
            newMinimumSize.Width = Math.Max(this.toolStrip1.MinimumSize.Width, this.topTableLayoutPanel.MinimumSize.Width + 40); // 2x 12 + 2x 8 form border

            this.MinimumSize = newMinimumSize;
        }
    }
}
