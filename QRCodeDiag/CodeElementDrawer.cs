using QRCodeBaseLib;
using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.DataBlocks.Symbols;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    internal class CodeElementDrawer
    {
        private readonly FontFamily fontFamily;
        public float CodeElWidth { get; set; }
        public float CodeElHeight { get; set; }
        public CodeElementDrawer(FontFamily setFontFamily)
        {
            this.fontFamily = new FontFamily("Lucida Console");
        }

        public void DrawCodeSymbol(DrawableCodeSymbol drawableSymbol, Graphics g)
        {
            this.DrawCodeSymbol(drawableSymbol.CodeSymbol,
                                g,
                                drawableSymbol.SymbolColoring,
                                drawableSymbol.DrawBitIndices,
                                drawableSymbol.DrawSymbolValue);
        }
        public void DrawCodeSymbol(ICodeSymbol symbol,
                                  Graphics g,
                                  SymbolColors symbolColors,
                                  bool drawBitIndices,
                                  bool drawSymbolValue)
        {
            // Draw symbol edges
            float penWidth = 2;
            var p = new Pen(symbolColors.Outline, penWidth);
            var smallFont = new Font(this.fontFamily, 0.5F * CodeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var largeFont = new Font(this.fontFamily, CodeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var solidbrush = new SolidBrush(symbolColors.BitIndex);
            foreach (var edge in symbol.GetContour())
            {
                var edgeStartX = edge.Start.X * this.CodeElWidth;
                var edgeStartY = edge.Start.Y * this.CodeElHeight;
                var edgeEndX = edge.End.X * this.CodeElWidth;
                var edgeEndY = edge.End.Y * this.CodeElHeight;
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
                g.DrawLine(p, edgeStartX, edgeStartY, edgeEndX, edgeEndY);
            }
            // Draw bit index
            if (drawBitIndices)
            {
                for (uint i = 0; i < symbol.CurrentSymbolLength; i++)
                {
                    var pixCoord = symbol.GetBitCoordinate(i);
                    g.DrawString(i.ToString(),
                                 smallFont,
                                 solidbrush,
                                 new Point((int)((pixCoord.X + 0.4F) * CodeElWidth), (int)((pixCoord.Y + 0.4F) * CodeElHeight)));
                }
            }

            if (drawSymbolValue && (symbol.CurrentSymbolLength > 0))
            {
                var drawLocation = symbol.GetBitCoordinate(Math.Min(4, symbol.CurrentSymbolLength - 1));
                var solidBrush = new SolidBrush(symbolColors.SymbolValue);

                g.DrawString(symbol.ToString(),
                                largeFont,
                                solidBrush,
                                new Point((int)(drawLocation.X * CodeElWidth), (int)(drawLocation.Y * CodeElHeight)));
            }
        }

        public void DrawCodeSymbolCode(IDrawableCodeSymbolCode drawableCode, Graphics g)
        {
            if (drawableCode.DrawSymbolCode)
            {
                const int preferredSymbolDrawLocation = 2;
                var largeFont = new Font(this.fontFamily, this.CodeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
                var symbolIndexBrush = new SolidBrush(drawableCode.SymbolIndexColor);
                var codeSymbolList = drawableCode.CodeSymbolCode.GetCodeSymbols();

                for (int j = 0; j < codeSymbolList.Count; j++)
                {
                    var sym = codeSymbolList[j];
                    this.DrawCodeSymbol(sym,
                                        g,
                                        drawableCode.SymbolColoring,
                                        drawableCode.DrawBitIndices, 
                                        drawableCode.DrawSymbolValues);

                    if (drawableCode.DrawSymbolIndices && (sym.CurrentSymbolLength > 0))
                    {
                        var drawIndexCoord = sym.GetBitCoordinate(Math.Min(preferredSymbolDrawLocation, sym.CurrentSymbolLength));
                        g.DrawString(j.ToString(),
                                     largeFont,
                                     symbolIndexBrush,
                                     new Point((int)(drawIndexCoord.X * this.CodeElWidth), (int)(drawIndexCoord.Y * this.CodeElHeight)));
                    }
                }
            }
        }

        public void DrawQRCode(char[,] bits, Graphics g, bool transparent = false)
        {
            byte alpha      = transparent ? (byte)128 : (byte)255;
            var edgeLength  = bits.GetLength(0);
            var blackBrush  = new SolidBrush(Color.FromArgb(alpha, Color.Black.R, Color.Black.G, Color.Black.B));
            var whiteBrush  = new SolidBrush(Color.FromArgb(alpha, Color.White.R, Color.White.G, Color.White.B));
            var grayBrush   = new SolidBrush(Color.FromArgb(alpha, Color.Gray.R,  Color.Gray.G,  Color.Gray.B));

            for (int y = 0; y < edgeLength; y++)
            {
                for (int x = 0; x < edgeLength; x++)
                {
                    SolidBrush b;
                    switch (bits[x, y])
                    {
                        case '0':
                        case 'w':
                        case 's':
                            b = whiteBrush;
                            break;
                        case '1':
                        case 'b':
                            b = blackBrush;
                            break;
                        case 'u':
                            b = grayBrush;
                            break;
                        default:
                            throw new QRCodeFormatException("Invalid codeEl value: " + bits[x, y]);
                    }
                    g.FillRectangle(b, x * CodeElWidth, y * CodeElHeight, CodeElWidth, CodeElHeight);
                }
            }
        }
    }
}
