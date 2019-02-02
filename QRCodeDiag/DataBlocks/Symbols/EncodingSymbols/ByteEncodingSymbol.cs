using QRCodeDiag.MetaInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeDiag.QRCode;

namespace QRCodeDiag.DataBlocks
{
    /// <summary>
    /// Symbol for the encoding type "Byte"
    /// </summary>
    class ByteEncodingSymbol : ByteSymbol, IEncodingSymbol
    {
        public MessageMode EncodingType { get { return MessageMode.Byte; } }
        public override object Clone()
        {
            var ret = new ByteEncodingSymbol();
            for (int i = 0; i < base.bitCoordinates.Count; i++)
            {
                ret.AddBit(base.bitArray[i], base.bitCoordinates[i]);
            }
            return ret;
        }
        public override void DrawSymbol(Graphics g, Size size, Color color, bool drawBitIndices, int codeVersion)
        {
            base.DrawSymbol(g, size, color, drawBitIndices, codeVersion);
            if (this.CurrentSymbolLength > 0)
            {
                var codeElWidth = (float)size.Width / QRCodeVersion.GetEdgeSizeFromVersion(codeVersion);
                var codeElHeight = (float)size.Height / QRCodeVersion.GetEdgeSizeFromVersion(codeVersion);
                var drawLocation = this.GetBitCoordinate(Math.Min(4, this.CurrentSymbolLength - 1));
                var fontFamily = new FontFamily("Lucida Console");
                var largeFont = new Font(fontFamily, codeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
                var solidBrush = new SolidBrush(Color.Orange);
                g.DrawString(this.ToString(), largeFont, solidBrush, new Point((int)(drawLocation.X * codeElWidth), (int)(drawLocation.Y * codeElHeight)));
            }
        }
        public override string ToString()
        {
            this.GetAsByte(out var thisAsByte);
            return Encoding.GetEncoding("iso-8859-1").GetString(new byte[] { thisAsByte });
        }
    }
}
