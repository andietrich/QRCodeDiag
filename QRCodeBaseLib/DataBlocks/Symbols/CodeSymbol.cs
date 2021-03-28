using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    /// <summary>
    /// Abstract super class for all other symbol classes.
    /// Provides functions accumulatively store bit values with coordinates, checking completeness
    /// and drawing a contour of all bits in the code including each bit with a bit number indicator.
    /// </summary>
    internal abstract class CodeSymbol : IBuildableCodeSymbol
    {
        protected List<Vector2D> bitCoordinates;
        protected List<char> bitValues;
        public uint CurrentSymbolLength { get { return (uint)this.bitCoordinates.Count; } }
        public virtual string BitString { get { return new string(this.bitValues.ToArray(), 0, this.bitCoordinates.Count); } } // TODO this should not be a property, but a get method, because it creates a new object every call
        public abstract bool IsComplete { get; }
        protected CodeSymbol()
        {
            this.bitValues = new List<char>();
            this.bitCoordinates = new List<Vector2D>();
        }

        /* Filling from most significant bit to least significant bit */
        public void AddBit(char bit, Vector2D bitPosition)
        {
            if (this.IsComplete)
                throw new InvalidOperationException($"The maximum symbol length {this.CurrentSymbolLength} has already been reached.");

            this.bitValues.Add(bit);
            this.bitCoordinates.Add(bitPosition);
        }

        //ToDo generate point array and use drawPolygon method
        //Define: Square at x, y has corner points at x, y, x+1, y+1
        public List<PolygonEdge> GetContour()
        {
            var edges = new HashSet<PolygonEdge>();
            foreach (var cell in bitCoordinates)
            {
                var p0 = new Vector2D(cell.X, cell.Y);
                var p1 = new Vector2D(cell.X + 1, cell.Y);
                var p2 = new Vector2D(cell.X + 1, cell.Y + 1);
                var p3 = new Vector2D(cell.X, cell.Y + 1);
                var top = new PolygonEdge(p0, p1);
                var right = new PolygonEdge(p1, p2);
                var bottom = new PolygonEdge(p2, p3);
                var left = new PolygonEdge(p3, p0);

                if (!edges.Remove(top))
                    edges.Add(top);
                if (!edges.Remove(right))
                    edges.Add(right);
                if (!edges.Remove(bottom))
                    edges.Add(bottom);
                if (!edges.Remove(left))
                    edges.Add(left);
            }
            return edges.ToList();
        }

        public Vector2D GetBitCoordinate(uint bitNumber)
        {
            if (bitNumber >= this.bitCoordinates.Count)
            {
                throw new ArgumentOutOfRangeException(
                    "bitNumber",
                    String.Format("Bit number {0} does not exist. Current symbol length is {1}.",
                    bitNumber,
                    this.bitCoordinates.Count));
            }
            return this.bitCoordinates[(int)bitNumber];
        }

        public override string ToString()
        {
            return $"-0b{this.BitString}-";
        }

        public bool HasUnknownBits()
        {
            bool unknownBits = false;

            foreach (var bit in this.BitString)
            {
                if ((bit != '0') && (bit != '1'))
                    unknownBits = true;
            }

            return unknownBits;
        }
    }
}
