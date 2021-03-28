using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols
{
    internal class NumericEncodingSymbol : CodeSymbol, IEncodingSymbol
    {
        public MessageMode.EncodingMode EncodingType { get { return MessageMode.EncodingMode.Numeric; } }

        public override bool IsComplete => throw new NotImplementedException();

        public NumericEncodingSymbol() : base()
        {
            throw new NotImplementedException();
        }
    }
}
