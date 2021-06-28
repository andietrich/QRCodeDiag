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
using QRCodeBaseLib.MetaInfo;
using QRCodeBaseLib;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace QRCodeDiagUWP
{
    public sealed partial class NewCodeDialog : ContentDialog
    {
        public uint Version { get; private set; }
        public ErrorCorrectionLevel.ECCLevel ECCLevel => (ErrorCorrectionLevel.ECCLevel)this.comboBoxEccLevel.SelectedItem;
        public XORMask.MaskType MaskType => (XORMask.MaskType)this.comboBoxXorMask.SelectedItem;
        public NewCodeDialog()
        {
            this.InitializeComponent();

            this.Version = 1;

            var ecclevels = Enum.GetValues(typeof(ErrorCorrectionLevel.ECCLevel)).Cast<ErrorCorrectionLevel.ECCLevel>();
            this.comboBoxEccLevel.ItemsSource = ecclevels.ToList();
            this.comboBoxEccLevel.SelectedIndex = 0;

            var xorMasks = Enum.GetValues(typeof(XORMask.MaskType)).Cast<XORMask.MaskType>();
            this.comboBoxXorMask.ItemsSource = xorMasks.ToList();
            this.comboBoxXorMask.SelectedIndex = (int)XORMask.MaskType.None;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
