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
    class CodeElementDrawer
    {
        private readonly FontFamily fontFamily;
        private readonly float codeElWidth;
        private readonly float codeElHeight;
        public CodeElementDrawer(float codeElementWidth, float codeElementHeight)
        {
            this.codeElWidth        = codeElementWidth;
            this.codeElHeight       = codeElementHeight;
            this.fontFamily         = new FontFamily("Lucida Console");
        }

        public void DrawCodeSymbol(CodeSymbol symbol, Graphics g, Color color, bool drawBitIndices)
        {
            // Draw symbol edges
            float penWidth = 2;
            var p = new Pen(color, penWidth);
            var smallFont = new Font(this.fontFamily, 0.5F * codeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var largeFont = new Font(this.fontFamily, codeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var solidrush = new SolidBrush(color);
            foreach (var edge in symbol.GetContour())
            {
                var edgeStartX = edge.Start.X * this.codeElWidth;
                var edgeStartY = edge.Start.Y * this.codeElHeight;
                var edgeEndX = edge.End.X * this.codeElWidth;
                var edgeEndY = edge.End.Y * this.codeElHeight;
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
                for (int i = 0; i < symbol.CurrentSymbolLength; i++)
                {
                    var pixCoord = symbol.GetBitCoordinate(i);
                    g.DrawString(i.ToString(),
                                 smallFont,
                                 solidrush,
                                 new Point((int)((pixCoord.X + 0.4F) * codeElWidth), (int)((pixCoord.Y + 0.4F) * codeElHeight)));
                }
            }

            if (symbol.CurrentSymbolLength > 0)
            {
                var drawLocation = symbol.GetBitCoordinate(Math.Min(4, symbol.CurrentSymbolLength - 1));
                var solidBrush = new SolidBrush(Color.Orange);

                g.DrawString(symbol.ToString(),
                             largeFont,
                             solidBrush,
                             new Point((int)(drawLocation.X * codeElWidth), (int)(drawLocation.Y * codeElHeight)));
            }
        }

        public void DrawCodeSymbolCode<T>(CodeSymbolCode<T> codeToDraw, Graphics g, Color bitColor, Color symbolColor, bool drawBitIndices, bool drawSymbolIndices) where T : CodeSymbol, new()
        {
            const int preferredSymbolDrawLocation = 2;
            var largeFont = new Font(this.fontFamily, this.codeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var symbolIndexBrush = new SolidBrush(symbolColor);
            var codeSymbolList = codeToDraw.GetCodeSymbols();

            for (int j = 0; j < codeSymbolList.Count; j++)
            {
                var sym = codeSymbolList[j];
                this.DrawCodeSymbol(sym, g, bitColor, drawBitIndices);
                if (drawSymbolIndices && sym.CurrentSymbolLength > 0)
                {
                    var drawIndexCoord = sym.GetBitCoordinate(Math.Min(preferredSymbolDrawLocation, sym.CurrentSymbolLength));
                    g.DrawString(j.ToString(),
                                 largeFont,
                                 symbolIndexBrush,
                                 new Point((int)(drawIndexCoord.X * this.codeElWidth), (int)(drawIndexCoord.Y * this.codeElHeight)));
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
                    g.FillRectangle(b, x * codeElWidth, y * codeElHeight, codeElWidth, codeElHeight);
                }
            }
        }
    }
}
