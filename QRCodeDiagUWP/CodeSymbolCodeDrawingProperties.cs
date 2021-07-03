using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace QRCodeDiagUWP
{
    internal class CodeSymbolCodeDrawingProperties
    {
        public SymbolColors SymbolColoring { get; set; }
        public Color SymbolIndexColor { get; set; }
        public string DisplayName { get; set; }

        public CodeSymbolCodeDrawingProperties(SymbolColors symbolColoring, Color symbolIndexColor, string displayName)
        {
            this.SymbolColoring = symbolColoring;
            this.SymbolIndexColor = symbolIndexColor;
            this.DisplayName = displayName;
        }
    }
}
