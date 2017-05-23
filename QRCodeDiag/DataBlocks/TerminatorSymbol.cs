using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    class TerminatorSymbol : CodeSymbol
    {
        private string terminatorBitString;
        public override string BitString { get { return this.terminatorBitString; } }
        public TerminatorSymbol(string terminatorBits, ICollection<Vector2D> coordinates) : base((uint)terminatorBits.Length)
        {
            if (terminatorBits.Length > 4)
                throw new ArgumentException("Terminator can be no longer than 4 bits.");
            if (coordinates.Count != terminatorBits.Length)
                throw new ArgumentException("The coordinates count doesn't match the bit count.");
            this.terminatorBitString = terminatorBits;
            this.bitCoordinates = new List<Vector2D>(coordinates);
            this.bitArray = terminatorBits.ToCharArray();
        }
    }
}
