using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    internal abstract class CodeSymbol
    {
        protected List<Vector2D> bitCoordinates;
        protected char[] bitArray;
        public string BitString { get { return new string(this.bitArray, 0, this.bitCoordinates.Count); } }
        public int MaxBitCount { get { return this.bitArray.Length; } }
        public bool IsComplete { get { return this.bitCoordinates.Count == this.bitArray.Length; } }
        protected CodeSymbol(uint symbolLength)
        {
            this.bitArray = new char[symbolLength];
            this.bitCoordinates = new List<Vector2D>((int)symbolLength);
        }
        public void AddBit(char bit, int x, int y)
        {
            if (this.bitCoordinates.Count == this.MaxBitCount)
                throw new InvalidOperationException("The maximum symbol length " + this.MaxBitCount + " has already been reached.");
            this.bitArray[bitCoordinates.Count] = bit;
            this.bitCoordinates.Add(new Vector2D(x, y));
        }
        public int GetCurrentWordLength()
        {
            return this.bitCoordinates.Count;
        }
        public Vector2D GetPixelCoordinate(int bitNumber)
        {
            if (bitNumber < 0 || bitNumber >= this.bitCoordinates.Count)
            {
                throw new ArgumentOutOfRangeException(
                    "bitNumber",
                    String.Format("Bit number {0} does not exist. Current symbol length is {1}.",
                    bitNumber,
                    this.bitCoordinates.Count));
            }
            return this.bitCoordinates[bitNumber];
        }
        //ToDo when drawing contours make sure they don't overlap with neighboring polygons
        //ToDo generate point array and use drawPolygon method
        //Contour must contain all outside borders or word-pixels.
        //Every contour-line must know where the "inside" is, to avoid overlapping with other words
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
        public abstract char[] GetDecodedSymbols();
    }
}
