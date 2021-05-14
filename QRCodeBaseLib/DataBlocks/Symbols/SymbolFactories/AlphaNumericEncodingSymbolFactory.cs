using QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class AlphaNumericEncodingSymbolFactory : ICodeSymbolFactory<AlphaNumericEncodingSymbol>
    {
        private uint charsUsed;
        private readonly uint maxChars;
        public AlphaNumericEncodingSymbolFactory(uint setMaxCharacters)
        {
            this.maxChars = setMaxCharacters;
            this.charsUsed = 0u;
        }
        public AlphaNumericEncodingSymbol GenerateCodeSymbol()
        {
            uint remainingChars = this.maxChars - this.charsUsed;

            switch (remainingChars)
            {
                case 0:
                    return null;
                case 1:
                    this.charsUsed++;
                    return new AlphaNumericEncodingSymbol(1);
                default:
                    this.charsUsed+=2;
                    return new AlphaNumericEncodingSymbol(2);
            }
        }
    }
}
