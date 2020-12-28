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
        public CodeSymbol CodeSymbol { get; private set; }
        public Color BitIndexColor { get; private set; }
        public Color OutlineColor { get; private set; }
        public Color SymbolValueColor { get; private set; }
        public bool DrawSymbol { get; }
        public bool DrawSymbolValue { get; }
        public bool DrawBitIndices { get; }
        public DrawableCodeSymbol(CodeSymbol codeSymbol, Color bitIndexColor, Color outlineColor, Color symbolValueColor)
        {
            this.CodeSymbol = codeSymbol;
            this.BitIndexColor = bitIndexColor;
            this.OutlineColor = outlineColor;
            this.SymbolValueColor = symbolValueColor;

            this.DrawSymbol = false;
            this.DrawSymbolValue = false;
            this.DrawBitIndices = false;
        }
    }
}
