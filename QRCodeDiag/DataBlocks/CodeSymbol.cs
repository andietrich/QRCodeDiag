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
        public uint SymbolLength { get; private set; }
        public int CurrentSymbolLength { get { return this.bitCoordinates.Count; } }
        public virtual string BitString { get { return new string(this.bitArray, 0, this.bitCoordinates.Count); } }
        public bool IsComplete { get { return this.bitCoordinates.Count == this.bitArray.Length; } }
        protected CodeSymbol(uint symbolLength)
        {
            this.SymbolLength = symbolLength;
            this.bitArray = new char[symbolLength];
            this.bitCoordinates = new List<Vector2D>((int)symbolLength);
        }
        public void AddBit(char bit, int x, int y)
        {
            this.AddBit(bit, new Vector2D(x, y));
        }
        public void AddBit(char bit, Vector2D bitPosition)
        {
            if (this.bitCoordinates.Count == this.SymbolLength)
                throw new InvalidOperationException("The maximum symbol length " + this.SymbolLength + " has already been reached.");
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

        public virtual void DrawSymbol(Graphics g, Size size, Color color, bool drawBitIndices)
        {
            // Draw symbol edges
            var pixelWidth = (float)size.Width / QRCode.VERSION3SIZE;
            var pixelHeight = (float)size.Height / QRCode.VERSION3SIZE;
            float penWidth = 2;
            var p = new Pen(color, penWidth);
            var fontFamily = new FontFamily("Lucida Console");
            var smallFont = new Font(fontFamily, 0.5F * pixelHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var solidrush = new SolidBrush(color);
            foreach (var edge in this.GetContour())
            {
                var edgeStartX = edge.Start.X * pixelWidth;
                var edgeStartY = edge.Start.Y * pixelHeight;
                var edgeEndX = edge.End.X * pixelWidth;
                var edgeEndY = edge.End.Y * pixelHeight;
                switch (edge.GetDirection()) // Inside is on the right looking in edge direction.
                {
                    case PolygonEdge.Direction.Down:
                        edgeStartX -= (penWidth / 2); // shift left
                        edgeEndX -= (penWidth / 2);   // shift left
                        edgeStartY += (penWidth / 2); // push down
                        edgeEndY  -= (penWidth / 2);  // pull up
                        break;
                    case PolygonEdge.Direction.Up:
                        edgeStartX += (penWidth / 2); // shift right
                        edgeEndX += (penWidth / 2);   // shift right
                        edgeStartY -= (penWidth / 2); // push up
                        edgeEndY += (penWidth / 2);   // pull down
                        break;
                    case PolygonEdge.Direction.Left:
                        edgeStartX -= (penWidth / 2); // shift left
                        edgeEndX += (penWidth / 2);   // shift right
                        edgeStartY -= (penWidth / 2); // pull up
                        edgeEndY -= (penWidth / 2);   // pull up
                        break;
                    case PolygonEdge.Direction.Right:
                        edgeStartX += (penWidth / 2); // shift right
                        edgeEndX -= (penWidth / 2);   // shift left
                        edgeStartY += (penWidth / 2); // push down
                        edgeEndY += (penWidth / 2);   // push down
                        break;
                    default:
                        throw new NotImplementedException("Drawing Direction type " + edge.GetDirection().ToString() + " is not implemented.");
                }
                g.DrawLine(p, edgeStartX, edgeStartY, edgeEndX, edgeEndY);
            }
            // Draw bit index
            if (drawBitIndices)
            {
                for (int i = 0; i < this.CurrentSymbolLength; i++)
                {
                    var pixCoord = this.GetBitCoordinate(i);
                    g.DrawString(i.ToString(), smallFont, solidrush, new Point((int)((pixCoord.X + 0.4F) * pixelWidth), (int)((pixCoord.Y + 0.4F) * pixelHeight)));
                }
            }
        }
    }
}
