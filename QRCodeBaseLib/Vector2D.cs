using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib
{
    public class Vector2D //ToDo use Drawing Point to avoid all the overloads (needs solution for polygonedge hash value then)
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Vector2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public static bool operator <(Vector2D lhs, Vector2D rhs)
        {
            return (lhs.X < rhs.X || (lhs.X == rhs.X && lhs.Y < rhs.Y));
        }
        public static bool operator >(Vector2D lhs, Vector2D rhs)
        {
            return (lhs.X > rhs.X || (lhs.X == rhs.X && lhs.Y > rhs.Y));
        }
        public override bool Equals(object obj)
        {
            if(obj is Vector2D)
            {
                return ((Vector2D)obj).X == this.X && ((Vector2D)obj).Y == this.Y;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode() //ToDo Make sure upper bound for coordinates is always correct or improve hash code when implementing other versions
        {
            return this.X * 64 + this.Y; //Upper bound is currently QRCode.SIZE = 29 < 64
        }
        public static bool operator ==(Vector2D lhs, Vector2D rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(Vector2D lhs, Vector2D rhs)
        {
            return !(lhs.Equals(rhs));
        }
        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }
    }
}
