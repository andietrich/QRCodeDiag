using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols
{
    /// <summary>
    /// Symbol for the encoding type "Byte"
    /// </summary>
    internal class ByteEncodingSymbol : ByteSymbol, IEncodingSymbol
    {
        public MessageMode.EncodingMode EncodingType { get { return MessageMode.EncodingMode.Byte; } }
        public override object Clone()
        {
            var ret = new ByteEncodingSymbol();
            for (int i = 0; i < base.bitCoordinates.Count; i++)
            {
                ret.AddBit(base.bitValues[i], base.bitCoordinates[i]);
            }
            return ret;
        }
        public override string ToString()
        {
            this.TryGetAsByte(out var thisAsByte);
            return Encoding.GetEncoding("iso-8859-1").GetString(new byte[] { thisAsByte });
        }
    }
}
