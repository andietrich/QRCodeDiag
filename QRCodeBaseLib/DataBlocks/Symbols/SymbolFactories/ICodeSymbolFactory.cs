using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories
{
    interface ICodeSymbolFactory<T> where T : IBuildableCodeSymbol
    {
        T GenerateCodeSymbol();
    }
}
