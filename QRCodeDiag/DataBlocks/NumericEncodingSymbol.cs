using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeDiag.QRCode;

namespace QRCodeDiag.DataBlocks
{
    class NumericEncodingSymbol : CodeSymbol, IEncodingSymbol
    {
        public override uint SymbolLength => throw new NotImplementedException();

        public MessageMode EncodingType { get { return MessageMode.Numeric; } }
    }
}
