using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    /// <summary>
    /// Abstract super class for any type of symbol that has byte length (8 bits)
    /// </summary>
    internal abstract class ByteSymbol : CodeSymbol
    {
        public const uint BYTESYMBOLLENGTH = 8;
        public override bool IsComplete => this.bitCoordinates.Count == BYTESYMBOLLENGTH;
        public abstract object Clone();
        /// <summary>
        /// Gets the byte representation of the ByteSymbol.
        /// If there are unknown bits they will be replaced by 0 and the function will return false.
        /// If not all bits have been set, the missing bits will be treated as unknown.
        /// </summary>
        /// <param name="value">The byte representation will be written to this parameter.</param>
        /// <param name="unknownBitValue">The value assumed for unknown bits. Defaults to 0.</param>
        /// <returns>True if all bits are known, otherwise false.</returns>
        public bool TryGetAsByte(out byte value, byte unknownBitValue = 0)
        {
            if (unknownBitValue > 1)
                throw new ArgumentException("Invalid bit value", "unknownBitValue");

            bool unknownBits = this.HasUnknownBits();

            if (unknownBits)
                value = Convert.ToByte(this.BitString.Replace("u", unknownBitValue.ToString()), 2);
            else
                value = Convert.ToByte(this.BitString, 2);

            return !unknownBits;
        }

        public override string ToString()
        {
            if (this.TryGetAsByte(out var value))
                return $"{value:X2}";
            else
                return base.ToString();
        }
    }
}
