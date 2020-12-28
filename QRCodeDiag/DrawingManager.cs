using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    internal class DrawingManager
    {
        private readonly List<DrawableCodeSymbol> drawableCodeSymbols;
        private readonly List<DrawableCodeSymbolCode> drawableCodeSymbolCodes;
        private readonly CodeElementDrawer codeElementDrawer;

        public DrawingManager(CodeElementDrawer setCodeElementDrawer)
        {
            this.codeElementDrawer = setCodeElementDrawer;
            this.drawableCodeSymbols = new List<DrawableCodeSymbol>();
            this.drawableCodeSymbolCodes = new List<DrawableCodeSymbolCode>();
        }

        public void RegisterCodeSymbol(DrawableCodeSymbol symbol)
        {
            this.drawableCodeSymbols.Add(symbol);
        }
        public void RegisterCodeSymbolCode(DrawableCodeSymbolCode symbolCode)
        {
            this.drawableCodeSymbolCodes.Add(symbolCode);
        }
        public void UnregisterCodeSymbol(DrawableCodeSymbol symbol)
        {
            this.drawableCodeSymbols.Remove(symbol);
        }
        public void UnregisterCodeSymbolCode(DrawableCodeSymbolCode symbolCode)
        {
            this.drawableCodeSymbolCodes.Remove(symbolCode);
        }
        public void Draw(Graphics g)
        {
            foreach(var symbolCode in this.drawableCodeSymbolCodes)
            {
                this.codeElementDrawer.DrawCodeSymbolCode(symbolCode, g);
            }
            foreach(var symbol in this.drawableCodeSymbols)
            {
                this.codeElementDrawer.DrawCodeSymbol(symbol, g);
            }
        }
    }
}
