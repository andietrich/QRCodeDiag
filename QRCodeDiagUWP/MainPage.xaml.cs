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
        private QRCode displayedCode;
        private QRCode DisplayedCode
        {
            get => this.displayedCode;
            set
            {
                this.displayedCode = value;
                this.canvasControl.Width = this.displayedCode.GetEdgeLength() * 10;
                this.canvasControl.Height = this.canvasControl.Width;
            }
        }
        public MainPage()
        {
            this.InitializeComponent();
            this.canvasControl.Width = 2000;
            this.canvasControl.Height = 2000;
            this.toggleOnTap = false;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewCodeDialog();
            var result = await dialog.ShowAsync();

            if(result == ContentDialogResult.Primary)
                this.DisplayedCode = new QRCode(new QRCodeVersion(dialog.Version), dialog.ECCLevel, dialog.MaskType);
        }



        public void DrawQRCode(char[,] bits, CanvasDrawingSession canvasDrawingSession, bool transparent = false)
        {
            var alpha = transparent ? (byte)128 : (byte)255;
            var codeEdgeLength = bits.GetLength(0);
            var black = Color.FromArgb(alpha, Colors.Black.R, Colors.Black.G, Colors.Black.B);
            var white = Color.FromArgb(alpha, Colors.White.R, Colors.White.G, Colors.White.B);
            var gray = Color.FromArgb(alpha, Colors.Gray.R, Colors.Gray.G, Colors.Gray.B);
            var codeElHeight = (int)Math.Min(canvasControl.Height, canvasControl.Width) / codeEdgeLength;

            canvasDrawingSession.Antialiasing = CanvasAntialiasing.Aliased;

            for (int y = 0; y < codeEdgeLength; y++)
            {
                for (int x = 0; x < codeEdgeLength; x++)
                {
                    Color color;
                    switch (bits[x, y])
                    {
                        case '0':
                        case 'w':
                        case 's':
                            color = white;
                            break;
                        case '1':
                        case 'b':
                            color = black;
                            break;
                        case 'u':
                            color = gray;
                            break;
                        default:
                            throw new QRCodeFormatException("Invalid codeEl value: " + bits[x, y]);
                    }

                    canvasDrawingSession.FillRectangle(x * codeElHeight, y * codeElHeight, codeElHeight, codeElHeight, color);
                }
            }
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (this.DisplayedCode != null)
                this.DrawQRCode(this.DisplayedCode.GetBits(true), args.DrawingSession);
            else
                args.DrawingSession.Clear(Colors.White);
        }

        private void ProcessClickInput(ClickInput input, CanvasControl canvasControl, Point position)
        {
            if (this.displayedCode != null)
            {
                var edgeLength = this.displayedCode.GetEdgeLength();
                int x = (int)(edgeLength * position.X / canvasControl.ActualSize.X);
                int y = (int)(edgeLength * position.Y / canvasControl.ActualSize.Y);

                if ((x < edgeLength) && (y < edgeLength) && (x >= 0) && (y >= 0))
                {
                    switch(input)
                    {
                        case ClickInput.Black:
                            this.displayedCode.SetDataCell(x, y, '1');
                            break;

                        case ClickInput.White:
                            this.displayedCode.SetDataCell(x, y, '0');
                            break;

                        case ClickInput.Gray:
                            this.displayedCode.SetDataCell(x, y, 'u');
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
            if (this.displayedCode != null)
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
            if (this.displayedCode != null && !this.toggleOnTap && e.Pointer.IsInContact)
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
    }
}
