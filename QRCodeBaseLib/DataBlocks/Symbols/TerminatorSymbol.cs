using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    public class TerminatorSymbol : CodeSymbol
    {
        private readonly string terminatorBitString;
        public override string BitString { get { return this.terminatorBitString; } }
        public TerminatorSymbol(string terminatorBits, ICollection<Vector2D> coordinates) : base((uint)terminatorBits.Length)
        {
            if (terminatorBits.Length > 4)
                throw new ArgumentException("Terminator cannot be longer than 4 bits.");
            if (coordinates.Count != terminatorBits.Length)
                throw new ArgumentException("The coordinates count doesn't match the bit count.");
            this.terminatorBitString = terminatorBits;
            this.bitCoordinates = new List<Vector2D>(coordinates);
            this.bitArray = terminatorBits.ToCharArray();
        }

        public override string ToString()
        {
            return $"0b{this.terminatorBitString}";
        }
    }
}
