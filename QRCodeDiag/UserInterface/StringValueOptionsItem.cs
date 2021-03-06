﻿using System;
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
    internal partial class StringValueOptionsItem : UserControl
    {
        public delegate void NewValueEnteredHandler(string newValue);
        public event NewValueEnteredHandler NewValueEnteredEvent;
        public string StringValue
        {
            get { return this.valueTextBox.Text; }
            set { this.valueTextBox.Text = value; }
        }
        public string ValueName
        {
            get { return this.valueNameLabel.Text; }
            set { this.valueNameLabel.Text = value; }
        }
        public StringValueOptionsItem() : this("Name of the Value")
        {
        }
        public StringValueOptionsItem(string label)
        {
            InitializeComponent();
            this.valueNameLabel.Text = label;
            this.writeButton.Click += (s, e) => NewValueEnteredEvent?.Invoke(this.valueTextBox.Text);
        }
    }
}
