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
    public partial class BooleanValueOptionsItem : UserControl
    {
        public delegate void CheckChangedHandler(bool newValue);
        public event CheckChangedHandler CheckChangedEvent;
        public bool Checked
        {
            get
            {
                return this.enableCheckBox.Checked;
            }
            set
            {
                this.enableCheckBox.Checked = value;
            }
        }
        public BooleanValueOptionsItem(string label = "Name of the Value", string cbDescription = "checkbox description")
        {
            InitializeComponent();
            this.valueNameLabel.Text = label;
            this.enableCheckBox.Text = cbDescription;
            this.enableCheckBox.CheckedChanged += (s, e) => this.CheckChangedEvent?.Invoke(this.enableCheckBox.Checked);
        }
    }
}
