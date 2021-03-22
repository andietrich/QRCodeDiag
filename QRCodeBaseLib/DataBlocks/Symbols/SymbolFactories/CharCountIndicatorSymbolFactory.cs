using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    class CharCountIndicatorSymbolFactory : ICodeSymbolFactory<CharCountIndicatorSymbol>
    {
        public MessageMode Mode { get; private set; }
        public CharCountIndicatorSymbolFactory(MessageMode messageMode)
        {
            this.Mode = messageMode;
        }
        public CharCountIndicatorSymbol GenerateCodeSymbol()
        {
            return new CharCountIndicatorSymbol(this.Mode);
        }
    }
}
