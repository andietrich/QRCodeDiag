using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QRCodeDiag.SettingsPropertyManager;

namespace QRCodeDiag.UserInterface
{
    public partial class CodeSymbolCodeOptionsItem : UserControl
    {
        internal event SettingsPropertyChangedEventHandler PropertyChangedEvent;
        private DrawableCodeSymbolCode drawableCodeSymbolCode;
        public DrawableCodeSymbolCode DrawableCodeSymbolCode 
        {
            get => this.drawableCodeSymbolCode;
            set 
            {
                this.drawableCodeSymbolCode = value;
                this.DrawableCodeSymbolCode.DrawSymbolCode = this.checkBoxDrawElement.Checked;
                this.DrawableCodeSymbolCode.DrawSymbolValues = this.checkBoxDrawValues.Checked;
                this.DrawableCodeSymbolCode.DrawSymbolIndices = this.checkBoxSymbolIndices.Checked;
                this.DrawableCodeSymbolCode.DrawBitIndices = this.checkBoxBitIndices.Checked;

                this.stringValueOptionsItem1.StringValue = this.drawableCodeSymbolCode.CodeSymbolCode.ToString();
                this.PropertyChangedEvent?.Invoke();
            }
        }

        public string CodeSymbolName
        {
            get { return this.stringValueOptionsItem1.ValueName; }
            set { this.stringValueOptionsItem1.ValueName = value; }
        }
        public bool DrawObject
        {
            get { return this.checkBoxDrawElement.Checked; }
        }

        public bool DrawSymbolValues
        {
            get { return this.checkBoxDrawValues.Checked; }
        }

        public CodeSymbolCodeOptionsItem(string codeSymbolName, DrawableCodeSymbolCode setDrawableCodeSymbolCode)
        {
            InitializeComponent();

            this.CodeSymbolName = codeSymbolName;
            this.DrawableCodeSymbolCode = setDrawableCodeSymbolCode;
        }

        private void checkBoxDrawElement_CheckedChanged(object sender, EventArgs e)
        {
            this.DrawableCodeSymbolCode.DrawSymbolCode = this.checkBoxDrawElement.Checked;
            this.PropertyChangedEvent?.Invoke();
        }

        private void checkBoxDrawValues_CheckedChanged(object sender, EventArgs e)
        {
            this.DrawableCodeSymbolCode.DrawSymbolValues = this.checkBoxDrawValues.Checked;
            this.PropertyChangedEvent?.Invoke();
        }

        private void checkBoxSymbolIndices_CheckedChanged(object sender, EventArgs e)
        {
            this.DrawableCodeSymbolCode.DrawSymbolIndices = this.checkBoxSymbolIndices.Checked;
            this.PropertyChangedEvent?.Invoke();
        }

        private void checkBoxBitIndices_CheckedChanged(object sender, EventArgs e)
        {
            this.DrawableCodeSymbolCode.DrawBitIndices = this.checkBoxBitIndices.Checked;
            this.PropertyChangedEvent?.Invoke();
        }
    }
}
