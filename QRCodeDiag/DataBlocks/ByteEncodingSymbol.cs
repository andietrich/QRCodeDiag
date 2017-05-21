using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeDiag.QRCode;

namespace QRCodeDiag.DataBlocks
{
    class ByteEncodingSymbol : RawCodeByte, IEncodingSymbol
    {
        public MessageMode EncodingType { get { return MessageMode.Byte; } }

        public static string DecodeSymbols(IList<ByteEncodingSymbol> symbols, char unknownSymbol = '_')
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var unknownSymbolByte = encoding.GetBytes(new char[] { unknownSymbol })[0];
            var symbolsAsBytes = new byte[symbols.Count];
            for (int i = 0; i < symbols.Count; i++)
            {
                symbolsAsBytes[i] = symbols[i].GetAsByte(out var value) ? value : unknownSymbolByte;
            }
            return encoding.GetString(symbolsAsBytes);
        }
    }
}
