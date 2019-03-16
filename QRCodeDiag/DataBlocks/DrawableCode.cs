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
        public bool Show { get; set; }
        public void DrawCode(Graphics g, float codeElWidth, float codeElHeight, Color bitColor, Color symbolColor, bool drawBitIndices, bool drawSymbolIndices)
        {
            var preferredSymbolDrawLocation = 2;

            var fontFamily = new FontFamily("Lucida Console");
            var largeFont = new Font(fontFamily, codeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var symbolIndexBrush = new SolidBrush(symbolColor);

            var byteSymbolList = this.GetByteSymbols();

            for (int j = 0; j < byteSymbolList.Count; j++)
            {
                var wd = byteSymbolList[j];
                wd.DrawSymbol(g, codeElWidth, codeElHeight, bitColor, drawBitIndices);
                if (drawSymbolIndices && wd.CurrentSymbolLength > 0)
                {
                    var drawIndexCoord = wd.GetBitCoordinate(Math.Min(preferredSymbolDrawLocation, wd.CurrentSymbolLength));
                    g.DrawString(j.ToString(), largeFont, symbolIndexBrush, new Point((int)(drawIndexCoord.X * codeElWidth), (int)(drawIndexCoord.Y * codeElHeight)));
                }
            }
        }
    }
}
