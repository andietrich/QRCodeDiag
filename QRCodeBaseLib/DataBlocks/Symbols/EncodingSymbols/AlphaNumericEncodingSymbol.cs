using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols
{
    internal class AlphaNumericEncodingSymbol : CodeSymbol, IEncodingSymbol
    {
        public MessageMode.EncodingMode EncodingType { get { return MessageMode.EncodingMode.Alphanumeric; } }
        public override bool IsComplete => throw new NotImplementedException();
        public AlphaNumericEncodingSymbol() : base()
        { }
    }
}
