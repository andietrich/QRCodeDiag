using QRCodeBaseLib.DataBlocks.SymbolCodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    interface IDrawableCodeSymbolCode
    {
        ICodeSymbolCode CodeSymbolCode { get; }
        Color BitIndexColor { get; }
        Color SymbolIndexColor { get; }
        Color SymbolOutlineColor { get; }
        Color SymbolValueColor { get; }
        bool DrawSymbolCode { get; }
        bool DrawSymbolValues { get; }
        bool DrawSymbolIndices { get; }
        bool DrawBitIndices { get; }
    }
}
