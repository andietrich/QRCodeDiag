using QRCodeBaseLib;
using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRCodeDiag.UserInterface
{
    public partial class MainForm : Form //ToDo: implement selecting symbol with mouse to change its value. Implement automatic generation of all elements like format info, encoding info, message, ...
    {
        private QRCode displayCode; // stores the non-xored QRCode for displaying while the backgroundCode is xored for analysis
        private readonly SettingsPropertyManager settingsPropertyManager;
        private readonly CodeElementDrawer codeDrawer;
        private readonly DrawingManager drawingManager;

        public bool ShowXORed { get; set; }

        private XORMask.MaskType CurrentMaskUsed
        {
            get
            {
                if (this.DisplayCode != null)
                    return this.DisplayCode.AppliedXORMaskType;
                else
                    return XORMask.MaskType.None;
            }
            set
            {
                if (this.DisplayCode != null)
                {
                    this.DisplayCode.AppliedXORMaskType = value;
                    this.UpdateTextBox();
                    this.pictureBox1.Invalidate();
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
                this.displayCode = value;
                this.settingsPropertyManager.SetQRCode(value);
                this.UpdateTextBox();
                this.pictureBox1.Invalidate();
            }
        }

        public MainForm()
        {
            InitializeComponent();
            this.ShowXORed = false;
            this.CurrentMaskUsed = XORMask.MaskType.None;
            this.codeDrawer = new CodeElementDrawer(new FontFamily("Lucida Console"));
            this.drawingManager = new DrawingManager(this.codeDrawer);
            this.settingsPropertyManager = new SettingsPropertyManager(this.drawingManager, this.optionsTableLayoutPanel.Controls);
            this.RecalculateFormMinimumSize();
            this.settingsPropertyManager.PropertyChangedEvent += this.pictureBox1.Invalidate;
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if(this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var fileContent = File.ReadAllLines(this.openFileDialog1.FileName);
                    this.DisplayCode = new QRCode(fileContent);
                }
                catch(QRCodeFormatException ex)
                {
                    MessageBox.Show(this, ex.Message + Environment.NewLine + ex.InnerException?.Message);
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (this.DisplayCode != null)
            {
                int     edgeLength      = this.DisplayCode.GetEdgeLength();
                bool    drawTransparent = this.pictureBox1.BackgroundImage != null;
                this.codeDrawer.CodeElWidth = (float)this.pictureBox1.Size.Width / edgeLength;
                this.codeDrawer.CodeElHeight = (float)this.pictureBox1.Size.Height / edgeLength;
                this.codeDrawer.DrawQRCode(this.DisplayCode.GetBits(this.ShowXORed), e.Graphics, drawTransparent);
                this.drawingManager.Draw(e.Graphics);
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
                        this.DisplayCode.SetDataCell(edgeLength * e.Location.X / pictureBox1.Size.Width, edgeLength * e.Location.Y / pictureBox1.Size.Height, '1');
                        break;
                    case MouseButtons.Right:
                        this.DisplayCode.SetDataCell(edgeLength * e.Location.X / pictureBox1.Size.Width, edgeLength * e.Location.Y / pictureBox1.Size.Height, '0');
                        break;
                    case MouseButtons.Middle:
                        this.DisplayCode.SetDataCell(edgeLength * e.Location.X / pictureBox1.Size.Width, edgeLength * e.Location.Y / pictureBox1.Size.Height, 'u');
                        break;
                    default:
                        this.DisplayCode.ToggleDataCell(edgeLength * e.Location.X / pictureBox1.Size.Width, edgeLength * e.Location.Y / pictureBox1.Size.Height);
                        break;
                }

                this.UpdateTextBox();
                this.pictureBox1.Invalidate();
            }
        }

        private void UpdateTextBox()
        {
            if (this.DisplayCode != null)
            {
                var sb = new StringBuilder();
                
                sb.AppendLine("Mask Used: " + this.CurrentMaskUsed.ToString());
                try
                {   
                    sb.AppendLine($"RepairMessage(): {Environment.NewLine}{this.DisplayCode.GetRepairMessageStatusLine()}");
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

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    if(this.DisplayCode != null)
                    {
                        var content = this.displayCode.GetSaveFileContent();
                        File.WriteAllText(this.saveFileDialog1.FileName, content);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not save file: " + ex.Message);
                }
            }
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
                this.DisplayCode = new QRCode(new QRCodeVersion((uint) createForm.Version), createForm.ECCLevel, createForm.MaskType); //ToDo ask to save modified codes
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
