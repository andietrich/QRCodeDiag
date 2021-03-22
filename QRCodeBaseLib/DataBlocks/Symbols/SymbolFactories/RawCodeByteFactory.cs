using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class RawCodeByteFactory : ICodeSymbolFactory<RawCodeByte>
    {
        public RawCodeByteFactory()
        { }

        public RawCodeByte GenerateCodeSymbol()
        {
            return new RawCodeByte();
        }
    }
}
