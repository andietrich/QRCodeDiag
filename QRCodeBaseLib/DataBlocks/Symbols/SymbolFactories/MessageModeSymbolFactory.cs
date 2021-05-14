using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class MessageModeSymbolFactory : ICodeSymbolFactory<MessageModeSymbol>
    {
        private int symbolsCreated = 0;
        public MessageModeSymbolFactory()
        { }

        public MessageModeSymbol GenerateCodeSymbol()
        {
            if (this.symbolsCreated == 0)
            {
                this.symbolsCreated++;
                return new MessageModeSymbol();
            }
            else
            {
                return null;
            }
        }
    }
}
