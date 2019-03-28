using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    /// <summary>
    /// Represents a byte of the interleaved block code (data or ecc)
    /// </summary>
    internal class RawCodeByte : ByteSymbol
    {
        public override object Clone()
        {
            var ret = new RawCodeByte();
            for (int i = 0; i < base.bitCoordinates.Count; i++)
            {
                ret.AddBit(base.bitArray[i], base.bitCoordinates[i]);
            }
            return ret;
        }

        public override string ToString()
        {
            base.TryGetAsByte(out byte numericValue);
            return "0x" + BitConverter.ToString(new byte[] { numericValue });
        }
    }
}
