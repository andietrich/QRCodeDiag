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
    public class RawCodeByte : ByteSymbol
    {
        public override object Clone()
        {
            var ret = new RawCodeByte();
            for (int i = 0; i < base.bitCoordinates.Count; i++)
            {
                ret.AddBit(base.bitValues[i], base.bitCoordinates[i]);
            }
            return ret;
        }
    }
}
