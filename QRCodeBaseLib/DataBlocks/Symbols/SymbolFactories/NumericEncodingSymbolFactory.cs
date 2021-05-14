using QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class NumericEncodingSymbolFactory : ICodeSymbolFactory<NumericEncodingSymbol>
    {
        private uint digitsUsed;
        private readonly uint maxDigits;
        public NumericEncodingSymbolFactory(uint setMaxCharacters)
        {
            this.maxDigits = setMaxCharacters;
            this.digitsUsed = 0;
        }
        public NumericEncodingSymbol GenerateCodeSymbol()
        {
            uint remainingDigits = this.maxDigits - this.digitsUsed;

            switch (remainingDigits)
            {
                case 0:
                    return null;
                case 1:
                    this.digitsUsed++;
                    return new NumericEncodingSymbol(1);
                case 2:
                    this.digitsUsed++;
                    return new NumericEncodingSymbol(2);
                default:
                    this.digitsUsed += 2;
                    return new NumericEncodingSymbol(3);
            }
        }
    }
}
