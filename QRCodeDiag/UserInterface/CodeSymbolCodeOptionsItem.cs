using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRCodeDiag.UserInterface
{
    public partial class CodeSymbolCodeOptionsItem : UserControl
    {
        public string CodeSymbolName
        {
            get { return this.stringValueOptionsItem1.ValueName; }
            set { this.stringValueOptionsItem1.ValueName = value; }
        }
        public CodeSymbolCodeOptionsItem()
        {
            InitializeComponent();
        }
    }
}
