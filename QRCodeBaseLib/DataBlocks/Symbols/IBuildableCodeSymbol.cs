using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    internal interface IBuildableCodeSymbol : ICodeSymbol
    {
        uint MaxSymbolLength { get; }
        void AddBit(char bit, Vector2D bitPosition);
    }
}
