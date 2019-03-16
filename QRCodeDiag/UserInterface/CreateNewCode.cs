using QRCodeDiag.MetaInfo;
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
                switch(this.comboBox1.SelectedItem.ToString())
                {
                    case "Low":
                        return ErrorCorrectionLevel.ECCLevel.Low;
                    case "Medium":
                        return ErrorCorrectionLevel.ECCLevel.Medium;
                    case "Quartile":
                        return ErrorCorrectionLevel.ECCLevel.Quartile;
                    case "High":
                        return ErrorCorrectionLevel.ECCLevel.High;
                    default:
                        throw new InvalidOperationException("Invalid combobox item");
                }
            }
        }
        public CreateNewCode()
        {
            InitializeComponent();
        }
    }
}
