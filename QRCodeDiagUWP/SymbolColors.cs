using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace QRCodeDiagUWP
{
    internal class SymbolColors
    {
        public Color BitIndex { get; private set; }
        public Color Outline { get; private set; }
        public Color SymbolValue { get; private set; }

        public SymbolColors(Color bitIndex, Color outline, Color symbolValue)
        {
            this.BitIndex = bitIndex;
            this.Outline = outline;
            this.SymbolValue = symbolValue;
        }
    }
}
