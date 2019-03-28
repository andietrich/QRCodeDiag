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
    abstract class ByteSymbol : CodeSymbol
    {
        public const uint BYTESYMBOLLENGTH = 8;
        public ByteSymbol() : base(BYTESYMBOLLENGTH)
        { }
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

            bool unknownBits = false;
            value = 0;

            for (int i = 0; i < this.bitCoordinates.Count; i++)
            {
                if (this.bitArray[i] == '1')
                {
                    value |= (byte)(0x80 >> i);
                }
                else if(this.bitArray[i] != '0')
                {
                    value |= (byte)(unknownBitValue << (byte)(8u - i));
                    unknownBits = true;
                }
            }
            System.Diagnostics.Debug.Assert(Convert.ToByte(this.BitString, 2) == value);
            return unknownBits;
        }
    }
}
