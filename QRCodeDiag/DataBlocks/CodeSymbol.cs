using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    internal abstract class CodeSymbol
    {
        protected List<Vector2D> bitCoordinates;
        protected char[] bitArray;
        public abstract uint SymbolLength { get; }
        public int CurrentSymbolLength { get { return this.bitCoordinates.Count; } }
        public string BitString { get { return new string(this.bitArray, 0, this.bitCoordinates.Count); } }
        public int MaxBitCount { get { return this.bitArray.Length; } }
        public bool IsComplete { get { return this.bitCoordinates.Count == this.bitArray.Length; } }
        protected CodeSymbol()
        {
            this.bitArray = new char[this.SymbolLength];
            this.bitCoordinates = new List<Vector2D>((int)this.SymbolLength);
        }
        public void AddBit(char bit, int x, int y)
        {
            this.AddBit(bit, new Vector2D(x, y));
        }
        public void AddBit(char bit, Vector2D bitPosition)
        {
            if (this.bitCoordinates.Count == this.MaxBitCount)
                throw new InvalidOperationException("The maximum symbol length " + this.MaxBitCount + " has already been reached.");
            this.bitArray[bitCoordinates.Count] = bit;
            this.bitCoordinates.Add(bitPosition);
        }
        public Vector2D GetBitCoordinate(int bitNumber)
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

        public virtual void DrawSymbol(Graphics g, bool drawBitIndices)
        {
            // Draw symbol edges
            var pixelWidth = g.VisibleClipBounds.Size.Width / QRCode.SIZE;
            var pixelHeight = g.VisibleClipBounds.Size.Height / QRCode.SIZE;
            var p = new Pen(Color.Green, 2); //ToDo new color every word (or the correct one of 4 different ones)
            var fontFamily = new FontFamily("Lucida Console");
            var smallFont = new Font(fontFamily, 0.5F * pixelHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var redBrush = new SolidBrush(Color.Red);            
            foreach (var edge in this.GetContour())
            {
                g.DrawLine(p, edge.Start.X * pixelWidth, edge.Start.Y * pixelHeight, edge.End.X * pixelWidth, edge.End.Y * pixelHeight);
            }
            // Draw bit index
            if (drawBitIndices)
            {
                for (int i = 0; i < this.CurrentSymbolLength; i++)
                {
                    var pixCoord = this.GetBitCoordinate(i);
                    g.DrawString(i.ToString(), smallFont, redBrush, new Point((int)((pixCoord.X + 0.4F) * pixelWidth), (int)((pixCoord.Y + 0.4F) * pixelHeight)));
                }
            }
        }
    }
}
