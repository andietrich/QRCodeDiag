using QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class ByteEncodingSymbolFactory : ICodeSymbolFactory<ByteEncodingSymbol>
    {
        private int symbolsCreated = 0;
        private readonly uint maxSymbols;
        public ByteEncodingSymbolFactory(uint setMaxSymbols)
        {
            this.maxSymbols = setMaxSymbols;
        }
        public ByteEncodingSymbol GenerateCodeSymbol()
        {
            if (this.symbolsCreated < this.maxSymbols)
            {
                this.symbolsCreated++;
                return new ByteEncodingSymbol();
            }
            else
            {
                return null;
            }
        }
    }
}
