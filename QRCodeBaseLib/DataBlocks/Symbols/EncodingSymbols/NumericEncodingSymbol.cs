using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols
{
    class NumericEncodingSymbol : CodeSymbol, IEncodingSymbol
    {
        public MessageMode EncodingType { get { return MessageMode.Numeric; } }
        public NumericEncodingSymbol() : base(GetSymbolLength())
        { }

        private static uint GetSymbolLength()
        {
            throw new NotImplementedException();
        }
    }
}
