using QRCodeBaseLib.DataBlocks.SymbolCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace QRCodeDiagUWP
{
    internal interface IDrawableCodeSymbolCode
    {
        ICodeSymbolCode CodeSymbolCode { get; }
        SymbolColors SymbolColoring { get; }
        Color SymbolIndexColor { get; }
        bool DrawSymbolCode { get; }
        bool DrawSymbolValues { get; }
        bool DrawSymbolIndices { get; }
        bool DrawBitIndices { get; }
    }
}
