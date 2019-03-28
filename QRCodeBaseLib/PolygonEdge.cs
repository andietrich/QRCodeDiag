using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib
{
    public class PolygonEdge
    {
        public enum Direction
        {
            /// <summary>
            /// Decreasing Y-Direction! (y=0 is top)
            /// </summary>
            Up,
            /// <summary>
            /// Increasing Y-Direction! (y=0 is top)
            /// </summary>
            Down,
            /// <summary>
            /// Decreasing X-Direction
            /// </summary>
            Left,
            /// <summary>
            /// Increasing X-Direction
            /// </summary>
            Right
        }
        public Vector2D Start { get; private set; }
        public Vector2D End { get; private set; }
        /// <summary>
        /// Direction of edge determines inside/outside.
        /// Inside is on the right looking in edge direction. Assuming x grows from left to right and y grows from top to bottom.
        /// Inside is relevant for avoiding overlapping polygon borders when drawing bordering polygons.
        /// </summary>
        /// <param name="start">Start point of the edge</param>
        /// <param name="end">End point of the edge</param>
        public PolygonEdge(Vector2D start, Vector2D end) //ToDo check to avoid identity of points, make sure edges are horizontal or vertical
        {
            this.Start = start;
            this.End = end;
        }
        public Direction GetDirection() // Possibilities: X1 < X2 && Y1 == Y2; X1 > X2 && Y1 == Y2; X1 == X2 && ...
        {
            if(this.Start.X < this.End.X)
            {
                // Y's must be equal because of rectangular angles
                return Direction.Right;
            }
            else if(this.Start.X > this.End.X)
            {
                // Y's equal
                return Direction.Left;
            }
            else // X's equal
            {
                if (this.Start.Y < this.End.Y)
                    return Direction.Down;
                else // X's and Y's can't both be equal
                    return Direction.Up;
            }
        }
        public override int GetHashCode()//ToDo Make sure upper bound for coordinates is always correct or improve hash code
        {
            if(this.Start < this.End)   // Make sure hash code is the same for both directions
                return this.Start.GetHashCode() * 128 + this.End.GetHashCode();
            else
                return this.End.GetHashCode() * 128 + this.Start.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var other = obj as PolygonEdge;
            if(other == null)
            {
                return false;
            }
            else
            {
                return (((this.Start == other.Start) && (this.End == other.End)) || ((this.Start == other.End) && (this.End == other.Start)));
            }
        }
    }
}
