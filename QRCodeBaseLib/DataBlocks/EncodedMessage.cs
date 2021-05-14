using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.DataBlocks.Symbols;
using QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols;
using QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories;
using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace QRCodeBaseLib.DataBlocks
{
    public class EncodedMessage
    {
        private CodeSymbolCode<CharCountIndicatorSymbol> charCountIndicatorSymbolCode;
        private uint characterCount;
        public ICodeSymbolCode Message { get; private set; }
        public ICodeSymbolCode CharCountIndicatorSymbolCode => this.charCountIndicatorSymbolCode;
        public ICodeSymbolCode MessageModeSymbolCode { get; private set; }
        private readonly MessageMode mode;
        internal EncodedMessage(IBitIterator it, MessageMode setMode, CodeSymbolCode<MessageModeSymbol> setMessageModeSymbolCode)
        {
            this.MessageModeSymbolCode = setMessageModeSymbolCode;
            this.mode = setMode;
            this.ParseEncodingMessage(it, mode);
        }

        private void ParseEncodingMessage(IBitIterator it, MessageMode mode)
        {
            if (!it.EndReached)
            {
                this.charCountIndicatorSymbolCode = CodeSymbolCode<CharCountIndicatorSymbol>.BuildInstance(it, new CharCountIndicatorSymbolFactory(mode));
                this.characterCount = charCountIndicatorSymbolCode.GetSymbolAt(0).GetCharacterCount();

                if (!it.EndReached)
                {
                    switch (mode.Mode)
                    {
                        case MessageMode.EncodingMode.Byte:
                            this.Message = CodeSymbolCode<ByteEncodingSymbol>.BuildInstance(it, new ByteEncodingSymbolFactory(characterCount));
                            break;

                        case MessageMode.EncodingMode.Alphanumeric:
                            this.Message = CodeSymbolCode<AlphaNumericEncodingSymbol>.BuildInstance(it, new AlphaNumericEncodingSymbolFactory(characterCount));
                            break;

                        case MessageMode.EncodingMode.Numeric:
                            this.Message = CodeSymbolCode<NumericEncodingSymbol>.BuildInstance(it, new NumericEncodingSymbolFactory(characterCount));
                            break;

                        default:
                            throw new NotImplementedException($"{mode.Mode} decoding not implemented");
                    }
                }
            }
        }

        public string GetEncodedString()
        {
            return this.Message?.ToString() ?? "Error: No message detected";
        }
    }
}
