using QRCodeBaseLib;
using QRCodeBaseLib.MetaInfo;
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
    internal partial class CreateNewCode : Form
    {
        public int Version { get { return (int)this.numericUpDown1.Value; } }
        public ErrorCorrectionLevel.ECCLevel ECCLevel
        {
            get
            {
                return (ErrorCorrectionLevel.ECCLevel)comboBoxECCLevel.SelectedItem;
            }
        }
        public XORMask.MaskType MaskType
        {
            get
            {
                return (XORMask.MaskType)comboBoxXORMask.SelectedItem;
            }
        }
        public CreateNewCode()
        {
            InitializeComponent();
            this.comboBoxECCLevel.DataSource = Enum.GetValues(typeof(ErrorCorrectionLevel.ECCLevel));
            this.comboBoxECCLevel.SelectedItem = ErrorCorrectionLevel.ECCLevel.Low;
            this.comboBoxXORMask.DataSource = Enum.GetValues(typeof(XORMask.MaskType));
            this.comboBoxXORMask.SelectedItem = XORMask.MaskType.None;
        }
    }
}
