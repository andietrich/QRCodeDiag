using QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class ByteEncodingSymbolFactory : ICodeSymbolFactory<ByteEncodingSymbol>
    {
        public ByteEncodingSymbolFactory()
        { }
        public ByteEncodingSymbol GenerateCodeSymbol()
        {
            return new ByteEncodingSymbol();
        }
    }
}
