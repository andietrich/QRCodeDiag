using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace QRCodeDiagUWP
{
    public sealed partial class CodeOptions : UserControl
    {
        internal event SettingsPropertyManager.SettingsPropertyChangedEventHandler PropertyChangedEvent;
        private DrawableCodeSymbolCode drawableCodeSymbolCode;

        private bool DrawSymbolValues
        {
            get => this.drawableCodeSymbolCode.DrawSymbolValues;
            set
            {
                this.drawableCodeSymbolCode.DrawSymbolValues = value;
                this.PropertyChangedEvent?.Invoke();
            }
        }
        private bool DrawSymbolCode
        {
            get => this.drawableCodeSymbolCode.DrawSymbolCode;
            set
            {
                this.drawableCodeSymbolCode.DrawSymbolCode = value;
                this.PropertyChangedEvent?.Invoke();
            }
        }
        private bool DrawSymbolIndices
        {
            get => this.drawableCodeSymbolCode.DrawSymbolIndices;
            set
            {
                this.drawableCodeSymbolCode.DrawSymbolIndices = value;
                this.PropertyChangedEvent?.Invoke();
            }
        }
        private bool DrawBitIndices
        {
            get => this.drawableCodeSymbolCode.DrawBitIndices;
            set
            {
                this.drawableCodeSymbolCode.DrawBitIndices = value;
                this.PropertyChangedEvent?.Invoke();
            }
        }

        private string symbolValue;
        internal string CodeSymbolName { get; private set; }
        
        internal DrawableCodeSymbolCode DrawableCodeSymbolCode
        {
            get => this.drawableCodeSymbolCode;
            set
            {
                if (this.drawableCodeSymbolCode != null)
                {
                    value.DrawSymbolCode = this.DrawSymbolCode;
                    value.DrawSymbolValues = this.DrawSymbolValues;
                    value.DrawSymbolIndices = this.DrawSymbolIndices;
                    value.DrawBitIndices = this.DrawBitIndices;
                }
                this.drawableCodeSymbolCode = value;
                this.symbolValue = this.drawableCodeSymbolCode.CodeSymbolCode.ToString();
                this.PropertyChangedEvent?.Invoke();
            }
        }

        internal CodeOptions(string codeSymbolName, DrawableCodeSymbolCode setDrawableCodeSymbolCode)
        {
            this.InitializeComponent();
            this.CodeSymbolName = codeSymbolName;
            this.DrawableCodeSymbolCode = setDrawableCodeSymbolCode;
        }
    }
}
