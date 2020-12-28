using QRCodeBaseLib.DataBlocks.SymbolCodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    public class DrawableCodeSymbolCode : IDrawableCodeSymbolCode
    {
        public ICodeSymbolCode CodeSymbolCode { get; set; }
        public Color BitIndexColor { get; set; }
        public Color SymbolIndexColor { get; set; }
        public Color SymbolOutlineColor { get; set; }
        public Color SymbolValueColor { get; set;}
        public bool DrawSymbolCode { get; set; }
        public bool DrawSymbolValues { get; set; }
        public bool DrawSymbolIndices { get; set; }
        public bool DrawBitIndices { get; set; }
        public DrawableCodeSymbolCode(ICodeSymbolCode codeSymbolCode, Color bitIndexColor, Color symbolIndexColor, Color symbolOutlineColor, Color symbolValueColor)
        {
            this.CodeSymbolCode = codeSymbolCode;
            this.BitIndexColor = bitIndexColor;
            this.SymbolIndexColor = symbolIndexColor;
            this.SymbolOutlineColor = symbolOutlineColor;
            this.SymbolValueColor = symbolValueColor;

            this.DrawSymbolCode = false;
            this.DrawSymbolValues = false;
            this.DrawSymbolIndices = false;
            this.DrawBitIndices = false;
        }
    }
}
