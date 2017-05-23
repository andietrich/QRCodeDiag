﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    class RawCodeByte : CodeSymbol
    {
        public const uint RAWBYTELENGTH = 8;
        public RawCodeByte() : base(RAWBYTELENGTH)
        { }
        public virtual object Clone()
        {
            var ret = new RawCodeByte();
            for(int i = 0; i < this.bitCoordinates.Count; i++)
            {
                ret.AddBit(this.bitArray[i], this.bitCoordinates[i].X, this.bitCoordinates[i].Y);
            }
            return ret;
        }
        /// <summary>
        /// Gets the byte representation of the RawCodeByte.
        /// If there are unknown bits they will be replaced by 0 and the function will return false.
        /// If not all bits have been set, the missing bits will be treated as unknown.
        /// </summary>
        /// <param name="value">The byte representation will be written to this parameter.</param>
        /// <returns>True if all bits are known, otherwise false.</returns>
        public bool GetAsByte(out byte value)
        {
            int bits = 0;
            value = 0;
            for (int i = 0; i < this.bitCoordinates.Count; i++)
            {
                if (this.bitArray[i] == '0')
                {
                    bits++;
                }
                else if (this.bitArray[i] == '1')
                {
                    bits++;
                    value += (byte)(0x80 >> i);
                }
            }
            System.Diagnostics.Debug.Assert(bits != this.SymbolLength || (Convert.ToByte(this.BitString, 2) == value));
            return bits == this.SymbolLength;
        }
    }
}
