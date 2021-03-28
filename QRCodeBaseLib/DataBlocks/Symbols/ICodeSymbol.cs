using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    public interface ICodeSymbol
    {
        uint CurrentSymbolLength { get; }
        string BitString { get; } // TODO this should not be a property, but a get method, because it creates a new object every call
        bool IsComplete { get; }

        List<PolygonEdge> GetContour();
        Vector2D GetBitCoordinate(uint bitNumber);
        string ToString();
        bool HasUnknownBits();
    }
}
