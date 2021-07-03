using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using QRCodeBaseLib;
using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace QRCodeDiagUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private enum ClickInput
        {
            Toggle,
            Black,
            White,
            Gray
        }

        private bool toggleOnTap;
        private bool showXored;
        private QRCode displayedCode;
        private readonly DrawingManager drawingManager;
        private readonly SettingsPropertyManager settingsPropertyManager;
        private XORMask.MaskType CurrentMaskUsed
        {
            get
            {
                if (this.DisplayedCode != null)
                    return this.DisplayedCode.AppliedXORMaskType;
                else
                    return XORMask.MaskType.None;
            }
            set
            {
                if (this.DisplayedCode != null)
                {
                    this.DisplayedCode.AppliedXORMaskType = value;
                    this.canvasControl.Invalidate();
                }
            }
        }
        private bool ShowXored
        {
            get => this.showXored;
            set
            {
                this.showXored = value;
                this.canvasControl.Invalidate();
            }
        }
        private QRCode DisplayedCode
        {
            get => this.displayedCode;
            set
            {
                this.displayedCode = value;
                this.settingsPropertyManager.SetQRCode(value);
                this.canvasControl.Width = this.displayedCode.GetEdgeLength() * 100;
                this.canvasControl.Height = this.canvasControl.Width;
                this.canvasControl.Invalidate();
                this.xorMaskToggleSplitButton.IsEnabled = value != null;
            }
        }
        public MainPage()
        {
            this.drawingManager = new DrawingManager();
            this.InitializeComponent();
            this.xorMaskToggleSplitButton.IsEnabled = false;
            this.canvasControl.Width = 2000;
            this.canvasControl.Height = 2000;
            this.toggleOnTap = false;
            this.showXored = false;
            this.settingsPropertyManager = new SettingsPropertyManager(this.drawingManager, this.codeOptionsStackPanel);
            this.settingsPropertyManager.PropertyChangedEvent += this.canvasControl.Invalidate;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewCodeDialog();
            var result = await dialog.ShowAsync();

            if(result == ContentDialogResult.Primary)
                this.DisplayedCode = new QRCode(new QRCodeVersion(dialog.Version), dialog.ECCLevel, dialog.MaskType);
        }


        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (this.DisplayedCode != null)
            {
                var codeElHeight = (int)Math.Min(canvasControl.Height, canvasControl.Width) / this.DisplayedCode.GetEdgeLength();
                CodeElementDrawer.DrawQRCode(this.DisplayedCode.GetBits(this.ShowXored), codeElHeight, args.DrawingSession);
                this.drawingManager.Draw(args.DrawingSession, codeElHeight);
            }
            else
            {
                args.DrawingSession.Clear(Colors.White);
            }
        }

        private void ProcessClickInput(ClickInput input, CanvasControl canvasControl, Point position)
        {
            if (this.DisplayedCode != null)
            {
                var edgeLength = this.DisplayedCode.GetEdgeLength();
                int x = (int)(edgeLength * position.X / canvasControl.ActualSize.X);
                int y = (int)(edgeLength * position.Y / canvasControl.ActualSize.Y);

                if ((x < edgeLength) && (y < edgeLength) && (x >= 0) && (y >= 0))
                {
                    switch(input)
                    {
                        case ClickInput.Black:
                            this.DisplayedCode.SetDataCell(x, y, '1');
                            break;

                        case ClickInput.White:
                            this.DisplayedCode.SetDataCell(x, y, '0');
                            break;

                        case ClickInput.Gray:
                            this.DisplayedCode.SetDataCell(x, y, 'u');
                            break;

                        case ClickInput.Toggle:
                            this.DisplayedCode.ToggleDataCell(x, y);
                            break;

                        default:
                            throw new ArgumentException(nameof(input));
                    }

                    canvasControl.Invalidate();
                }
            }
        }

        private ClickInput GetClickInput(PointerPointProperties properties)
        {
            ClickInput input;

            if (this.toggleOnTap)
            {
                input = ClickInput.Toggle;
            }
            else
            {
                if (properties.IsLeftButtonPressed)
                    input = ClickInput.Black;
                else if (properties.IsRightButtonPressed)
                    input = ClickInput.White;
                else
                    input = ClickInput.Gray;
            }

            return input;
        }

        private void CanvasControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (this.DisplayedCode != null)
            {
                var canvasCtrl = ((CanvasControl)sender);
                var point = e.GetCurrentPoint(canvasCtrl);
                ClickInput input = GetClickInput(point.Properties);

                if (!this.toggleOnTap)
                {
                    canvasCtrl.CapturePointer(e.Pointer);
                }

                this.ProcessClickInput(input, canvasCtrl, point.Position);
            }
        }

        private void CanvasControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (this.DisplayedCode != null && !this.toggleOnTap && e.Pointer.IsInContact)
            {
                var canvasCtrl = ((CanvasControl)sender);
                var point = e.GetCurrentPoint(canvasCtrl);
                ClickInput input = GetClickInput(point.Properties);

                this.ProcessClickInput(input, canvasCtrl, point.Position);
            }
        }

        private void CanvasControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ((CanvasControl)sender).ReleasePointerCapture(e.Pointer);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var itemText = ((MenuFlyoutItem)sender).Text.ToString();

            if (itemText.StartsWith("Mask"))
            {
                this.CurrentMaskUsed = (XORMask.MaskType)char.GetNumericValue(itemText, 5);
                this.ShowXored = true;
                this.xorMaskToggleSplitButton.IsChecked = true;
            }
        }

        private void XorMaskToggleSplitButton_Click(Microsoft.UI.Xaml.Controls.SplitButton sender, Microsoft.UI.Xaml.Controls.SplitButtonClickEventArgs args)
        {
            this.ShowXored = !this.ShowXored;
        }
    }
}
