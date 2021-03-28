using QRCodeBaseLib.DataBlocks.Symbols;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    class DrawableCodeSymbol
    {
        public ICodeSymbol CodeSymbol { get; private set; }
        public SymbolColors SymbolColoring { get; private set; }
        public bool DrawSymbol { get; }
        public bool DrawSymbolValue { get; }
        public bool DrawBitIndices { get; }
        public DrawableCodeSymbol(ICodeSymbol codeSymbol, SymbolColors symbolColors)
        {
            this.CodeSymbol = codeSymbol;
            this.SymbolColoring = symbolColors;

            this.DrawSymbol = false;
            this.DrawSymbolValue = false;
            this.DrawBitIndices = false;
        }
    }
}
