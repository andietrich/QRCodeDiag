using QRCodeDiag.MetaInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    internal abstract class DrawableCode<T> where T : CodeSymbol
    {
        protected abstract List<T> GetByteSymbols();
        public void DrawCode(Graphics g, Size size, Color bitColor, Color symbolColor, bool drawBitIndices, bool drawSymbolIndices, int codeVersion)
        {
            var preferredSymbolDrawLocation = 2;

            var codeElWidth = (float)size.Width / QRCodeVersion.GetEdgeSizeFromVersion(codeVersion);
            var codeElHeight = (float)size.Height / QRCodeVersion.GetEdgeSizeFromVersion(codeVersion);

            var fontFamily = new FontFamily("Lucida Console");
            var largeFont = new Font(fontFamily, codeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var symbolIndexBrush = new SolidBrush(symbolColor);

            var byteSymbolList = this.GetByteSymbols();

            for (int j = 0; j < byteSymbolList.Count; j++)
            {
                var wd = byteSymbolList[j];
                wd.DrawSymbol(g, size, bitColor, drawBitIndices, codeVersion);
                if (drawSymbolIndices && wd.CurrentSymbolLength > 0)
                {
                    var drawIndexCoord = wd.GetBitCoordinate(Math.Min(preferredSymbolDrawLocation, wd.CurrentSymbolLength));
                    g.DrawString(j.ToString(), largeFont, symbolIndexBrush, new Point((int)(drawIndexCoord.X * codeElWidth), (int)(drawIndexCoord.Y * codeElHeight)));
                }
            }
        }
    }
}
