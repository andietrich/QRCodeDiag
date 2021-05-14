using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class CharCountIndicatorSymbolFactory : ICodeSymbolFactory<CharCountIndicatorSymbol>
    {
        private int symbolsCreated = 0;
        public MessageMode Mode { get; private set; }
        public CharCountIndicatorSymbolFactory(MessageMode messageMode)
        {
            this.Mode = messageMode;
        }
        public CharCountIndicatorSymbol GenerateCodeSymbol()
        {
            if (this.symbolsCreated == 0)
            {
                this.symbolsCreated++;
                return new CharCountIndicatorSymbol(this.Mode);
            }
            else
            {
                return null;
            }
        }
    }
}
