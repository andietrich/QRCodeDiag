using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeDiag.QRCode;

namespace QRCodeDiag.DataBlocks
{
    class AlphaNumericEncodingSymbol : CodeSymbol, IEncodingSymbol
    {
        public MessageMode EncodingType { get { return MessageMode.Alphanumeric; } }
        public AlphaNumericEncodingSymbol() : base(GetSymbolLength())
        { }

        private static uint GetSymbolLength()
        {
            throw new NotImplementedException();
        }        
    }
}
