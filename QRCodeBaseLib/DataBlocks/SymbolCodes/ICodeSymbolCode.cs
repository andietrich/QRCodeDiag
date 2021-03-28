using QRCodeBaseLib.DataBlocks.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks.SymbolCodes
{
    public interface ICodeSymbolCode
    {
        uint BitCount { get; }
        string GetBitString();
        string GetBitString(uint startIndex, uint length);
        string[] GetSymbolBitStrings();
        Vector2D GetBitPosition(uint bitNumber);
        List<ICodeSymbol> GetCodeSymbols();
    }
}
