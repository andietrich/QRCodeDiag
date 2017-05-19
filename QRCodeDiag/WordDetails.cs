using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    class WordDetails : ICloneable
    {
        private List<Vector2D> pixelCoordinates;
        public string DataWord { get; private set; }
        public uint MaxLength { get; private set; }
        public WordDetails(uint maxLength)
        {
            this.MaxLength = maxLength;
            this.DataWord = string.Empty;
            this.pixelCoordinates = new List<Vector2D>();
        }
        public void AddBit(char bit, int x, int y)
        {
            if (this.pixelCoordinates.Count == this.MaxLength)
                throw new InvalidOperationException("The maximum word length " + this.MaxLength + " has already been reached.");
            this.DataWord = this.DataWord + bit;
            this.pixelCoordinates.Add(new Vector2D(x, y));
        }
        public int GetWordLength()
        {
            return this.pixelCoordinates.Count;
        }
        public Vector2D GetPixelCoordinate(int bitNumber)
        {
            if (bitNumber < 0 || bitNumber >= this.pixelCoordinates.Count)
            {
                throw new ArgumentOutOfRangeException(
                      "bitNumber",
                      String.Format("Pixel {0} does not exist. Word length is {1}",
                        bitNumber,
                        this.pixelCoordinates.Count));
            }
            return this.pixelCoordinates[bitNumber];
        }

        public bool IsComplete()
        {
            return this.pixelCoordinates.Count == this.MaxLength;
        }

        //ToDo
        //Contour must contain all outside borders or word-pixels.
        //Every contour-line must know where the "inside" is, to avoid overlapping with other words
        //Define: Square at x, y has corner points at x, y, x+1, y+1
        public List<PolygonEdge> GetContour()
        {
            var edges = new HashSet<PolygonEdge>();
            foreach(var cell in pixelCoordinates)
            {
                var p0 = new Vector2D(cell.X, cell.Y);
                var p1 = new Vector2D(cell.X + 1, cell.Y);
                var p2 = new Vector2D(cell.X + 1, cell.Y+1);
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

        public object Clone()
        {
            var ret = new WordDetails(this.MaxLength);
            for(int i = 0; i < this.pixelCoordinates.Count; i++)
            {
                ret.AddBit(this.DataWord[i], this.pixelCoordinates[i].X, this.pixelCoordinates[i].Y);
            }
            return ret;
        }
    }
}
