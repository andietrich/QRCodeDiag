using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class MessageModeSymbolFactory : ICodeSymbolFactory<MessageModeSymbol>
    {
        public MessageModeSymbolFactory()
        { }

        public MessageModeSymbol GenerateCodeSymbol()
        {
            return new MessageModeSymbol();
        }
    }
}
