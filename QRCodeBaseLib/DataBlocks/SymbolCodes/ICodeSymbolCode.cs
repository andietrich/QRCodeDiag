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
        int BitCount { get; }
        string GetBitString();
        string GetBitString(int startIndex, int length);
        string[] GetSymbolBitStrings();
        Vector2D GetBitPosition(int bitNumber);
        List<CodeSymbol> GetCodeSymbols();
    }
}
