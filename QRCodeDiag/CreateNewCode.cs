﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRCodeDiag
{
    public partial class CreateNewCode : Form
    {
        public int Version { get { return (int)this.numericUpDown1.Value; } }
        public CreateNewCode()
        {
            InitializeComponent();
        }
    }
}
