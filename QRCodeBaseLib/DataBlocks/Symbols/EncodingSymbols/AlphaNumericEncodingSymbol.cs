using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols
{
    public class AlphaNumericEncodingSymbol : CodeSymbol, IEncodingSymbol
    {
        public MessageMode.EncodingMode EncodingType { get { return MessageMode.EncodingMode.Alphanumeric; } }
        public AlphaNumericEncodingSymbol() : base(GetSymbolLength())
        { }

        private static uint GetSymbolLength()
        {
            throw new NotImplementedException();
        }        
    }
}
