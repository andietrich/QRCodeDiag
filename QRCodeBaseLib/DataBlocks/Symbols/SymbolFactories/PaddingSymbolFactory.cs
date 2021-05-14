using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class PaddingSymbolFactory : ICodeSymbolFactory<PaddingSymbol>
    {
        private readonly uint zeroPadLength;
        private bool zeroPadCreated;
        public PaddingSymbolFactory(uint setZeroPadLength)
        {
            if (setZeroPadLength >= 8)
                throw new ArgumentOutOfRangeException("Must be less than 8", nameof(setZeroPadLength));
            
            this.zeroPadLength = setZeroPadLength;
            this.zeroPadCreated = setZeroPadLength == 0;
        }
        public PaddingSymbol GenerateCodeSymbol()
        {
            if(!zeroPadCreated)
            {
                zeroPadCreated = true;

                return new PaddingSymbol(zeroPadLength);
            }
            else
            {
                return new PaddingSymbol(8);
            }
        }
    }
}
