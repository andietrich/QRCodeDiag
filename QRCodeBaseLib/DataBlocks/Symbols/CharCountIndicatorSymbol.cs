using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    class CharCountIndicatorSymbol : CodeSymbol
    {
        private string charCountIndicatorBitString;
        public override string BitString { get { return this.charCountIndicatorBitString; } }
        public CharCountIndicatorSymbol(string charCountIndicatorBits, ICollection<Vector2D> coordinates) : base((uint)charCountIndicatorBits.Length)
        {
            if (coordinates.Count != charCountIndicatorBits.Length)
                throw new ArgumentException("The coordinates count doesn't match the bit count.");

            this.charCountIndicatorBitString = charCountIndicatorBits;
            this.bitCoordinates = new List<Vector2D>(coordinates);
            this.bitArray = charCountIndicatorBits.ToCharArray();
        }

        public bool TryGetAsUInt(out uint value, byte unknownBitValue = 0)
        {
            if (unknownBitValue > 1)
                throw new ArgumentException($"Invalid bit value: {unknownBitValue}", "unknownBitValue");

            bool unknownBits = false;

            foreach(var bit in this.BitString)
            {
                if ((bit != '0') && (bit != '1'))
                    unknownBits = true;
            }

            if (unknownBits)
                value = Convert.ToUInt32(this.BitString.Replace("u", unknownBitValue.ToString()), 2);
            else
                value = Convert.ToUInt32(this.BitString, 2);

            return !unknownBits;
        }

        public override string ToString()
        {
            if (this.TryGetAsUInt(out var value))
                return value.ToString();
            else
                return $"-{this.BitString}-";
        }
    }
}
