using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.DataBlocks.Symbols;
using QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols;
using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks
{
    class EncodedMessage
    {
        private CodeSymbolCode<MessageModeSymbol> messageModeSymbol;
        private CodeSymbolCode<CharCountIndicatorSymbol> charCountIndicator;
        private CodeSymbolCode<CodeSymbol> encodedMessage;

        // public EncodedMessage
    }
}
