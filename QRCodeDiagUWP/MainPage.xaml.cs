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
    }
}
