using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class TerminatorSymbolFactory : ICodeSymbolFactory<TerminatorSymbol>
    {
        public TerminatorSymbolFactory()
        { }

        public TerminatorSymbol GenerateCodeSymbol()
        {
            return new TerminatorSymbol();
        }
    }
}
