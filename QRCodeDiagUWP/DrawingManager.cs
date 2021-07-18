using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiagUWP
{
    internal class DrawingManager
    {
        private readonly List<DrawableCodeSymbolCode> drawableCodeSymbolCodes;

        public DrawingManager()
        {
            this.drawableCodeSymbolCodes = new List<DrawableCodeSymbolCode>();
        }

        public void RegisterCodeSymbolCode(DrawableCodeSymbolCode symbolCode)
        {
            this.drawableCodeSymbolCodes.Add(symbolCode);
        }
        public void UnregisterCodeSymbolCode(DrawableCodeSymbolCode symbolCode)
        {
            this.drawableCodeSymbolCodes.Remove(symbolCode);
        }
        public void Draw(CanvasDrawingSession canvasDrawingSession, int codeElHeight)
        {
            foreach (var symbolCode in this.drawableCodeSymbolCodes)
            {
                CodeElementDrawer.DrawCodeSymbolCode(symbolCode, canvasDrawingSession, codeElHeight);
            }
        }
    }
}
