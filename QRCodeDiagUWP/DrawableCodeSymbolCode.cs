using QRCodeBaseLib.DataBlocks.SymbolCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace QRCodeDiagUWP
{
    internal class DrawableCodeSymbolCode : IDrawableCodeSymbolCode
    {
        public ICodeSymbolCode CodeSymbolCode { get; set; }
        public Color SymbolIndexColor => this.DrawingProperties.SymbolIndexColor;
        public SymbolColors SymbolColoring => this.DrawingProperties.SymbolColoring;
        public string DisplayName => this.DrawingProperties.DisplayName;
        CodeSymbolCodeDrawingProperties DrawingProperties { get; set; }
        public bool DrawSymbolCode { get; set; }
        public bool DrawSymbolValues { get; set; }
        public bool DrawSymbolIndices { get; set; }
        public bool DrawBitIndices { get; set; }
        public DrawableCodeSymbolCode(ICodeSymbolCode codeSymbolCode, CodeSymbolCodeDrawingProperties drawingProperties)
        {
            this.CodeSymbolCode = codeSymbolCode;
            this.DrawingProperties = drawingProperties;
            this.DrawSymbolCode = false;
            this.DrawSymbolValues = false;
            this.DrawSymbolIndices = false;
            this.DrawBitIndices = false;
        }
    }
}
