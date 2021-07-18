using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using QRCodeBaseLib;
using QRCodeBaseLib.DataBlocks.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace QRCodeDiagUWP
{
    internal class CodeElementDrawer
    {
        public static void DrawCodeSymbol(ICodeSymbol symbol,
                                  CanvasDrawingSession canvasDrawingSession,
                                  SymbolColors symbolColors,
                                  bool drawBitIndices,
                                  bool drawSymbolValue,
                                  int codeElHeight)
        {
            // Draw symbol edges
            int penWidth = codeElHeight / 20;
            var bitIndexFormat = new CanvasTextFormat() { FontFamily = "Lucida Console", FontSize = 0.5F * codeElHeight };
            var symbolValueFormat = new CanvasTextFormat() { FontFamily = "Lucida Console", FontSize = codeElHeight };
            foreach (var edge in symbol.GetContour())
            {
                var edgeStartX = edge.Start.X * codeElHeight;
                var edgeStartY = edge.Start.Y * codeElHeight;
                var edgeEndX = edge.End.X * codeElHeight;
                var edgeEndY = edge.End.Y * codeElHeight;
                switch (edge.GetDirection()) // Inside is on the right looking in edge direction.
                {
                    case PolygonEdge.Direction.Down:
                        edgeStartX -= penWidth / 2; // shift left
                        edgeEndX -= penWidth / 2;   // shift left
                        edgeStartY += penWidth / 2; // push down
                        edgeEndY -= penWidth / 2;  // pull up
                        break;
                    case PolygonEdge.Direction.Up:
                        edgeStartX += penWidth / 2; // shift right
                        edgeEndX += penWidth / 2;   // shift right
                        edgeStartY -= penWidth / 2; // push up
                        edgeEndY += penWidth / 2;   // pull down
                        break;
                    case PolygonEdge.Direction.Left:
                        edgeStartX -= penWidth / 2; // shift left
                        edgeEndX += penWidth / 2;   // shift right
                        edgeStartY -= penWidth / 2; // pull up
                        edgeEndY -= penWidth / 2;   // pull up
                        break;
                    case PolygonEdge.Direction.Right:
                        edgeStartX += penWidth / 2; // shift right
                        edgeEndX -= penWidth / 2;   // shift left
                        edgeStartY += penWidth / 2; // push down
                        edgeEndY += penWidth / 2;   // push down
                        break;
                    default:
                        throw new NotImplementedException("Drawing Direction type " + edge.GetDirection().ToString() + " is not implemented.");
                }
                var start = new Vector2(edgeStartX, edgeStartY);
                var end = new Vector2(edgeEndX, edgeEndY);
                canvasDrawingSession.DrawLine(start, end, symbolColors.Outline, penWidth);
            }
            // Draw bit index
            if (drawBitIndices)
            {
                for (uint i = 0; i < symbol.CurrentSymbolLength; i++)
                {
                    var pixCoord = symbol.GetBitCoordinate(i);
                    var rect = new Rect(pixCoord.X * codeElHeight, pixCoord.Y * codeElHeight, codeElHeight, codeElHeight);
                    canvasDrawingSession.DrawText(i.ToString(), rect, symbolColors.BitIndex, bitIndexFormat);
                }
            }

            if (drawSymbolValue && (symbol.CurrentSymbolLength > 0))
            {
                var symbolDrawLocation = symbol.GetBitCoordinate(Math.Min(4, symbol.CurrentSymbolLength - 1));
                var drawLocation = new Vector2(symbolDrawLocation.X * codeElHeight, symbolDrawLocation.Y * codeElHeight);
                canvasDrawingSession.DrawText(symbol.ToString(), drawLocation, symbolColors.SymbolValue, symbolValueFormat);
            }
        }

        public static void DrawCodeSymbolCode(IDrawableCodeSymbolCode drawableCode, CanvasDrawingSession canvasDrawingSession, int codeElHeight)
        {
            if (drawableCode.DrawSymbolCode)
            {
                const int preferredSymbolDrawLocation = 2;
                var symbolValueFormat = new CanvasTextFormat() { FontFamily = "Lucida Console", FontSize = codeElHeight };

                var codeSymbolList = drawableCode.CodeSymbolCode.GetCodeSymbols();

                for (int j = 0; j < codeSymbolList.Count; j++)
                {
                    var sym = codeSymbolList[j];
                    CodeElementDrawer.DrawCodeSymbol(sym,
                                                     canvasDrawingSession,
                                                     drawableCode.SymbolColoring,
                                                     drawableCode.DrawBitIndices,
                                                     drawableCode.DrawSymbolValues,
                                                     codeElHeight);

                    if (drawableCode.DrawSymbolIndices && (sym.CurrentSymbolLength > 0))
                    {
                        var symbolDrawIndexCoord = sym.GetBitCoordinate(Math.Min(preferredSymbolDrawLocation, sym.CurrentSymbolLength - 1));
                        var drawIndexCoord = new Vector2(symbolDrawIndexCoord.X * codeElHeight, symbolDrawIndexCoord.Y * codeElHeight);
                        canvasDrawingSession.DrawText(j.ToString(), drawIndexCoord, drawableCode.SymbolIndexColor, symbolValueFormat);
                    }
                }
            }
        }

        public static void DrawQRCode(char[,] bits, int codeElHeight, CanvasDrawingSession canvasDrawingSession, ICanvasImage backgroundImage)
        {
            var alpha = backgroundImage != null ? (byte)128 : (byte)255;
            var codeEdgeLength = bits.GetLength(0);
            var black = Color.FromArgb(alpha, Colors.Black.R, Colors.Black.G, Colors.Black.B);
            var white = Color.FromArgb(alpha, Colors.White.R, Colors.White.G, Colors.White.B);
            var gray = Color.FromArgb(alpha, Colors.Gray.R, Colors.Gray.G, Colors.Gray.B);

            canvasDrawingSession.Antialiasing = CanvasAntialiasing.Aliased;

            if (backgroundImage != null)
            {
                var bounds = backgroundImage.GetBounds(canvasDrawingSession);
                var sourceRect = new Rect() { Height = bounds.Height, Width = bounds.Width };
                var targetRect = new Rect() { Height = codeEdgeLength * codeElHeight, Width = codeEdgeLength * codeElHeight };
                
                canvasDrawingSession.DrawImage(backgroundImage, targetRect, sourceRect);
            }

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
                            throw new ArgumentException("Invalid codeEl value: " + bits[x, y]);
                    }

                    canvasDrawingSession.FillRectangle(x * codeElHeight, y * codeElHeight, codeElHeight, codeElHeight, color);
                }
            }
        }
    }
}
