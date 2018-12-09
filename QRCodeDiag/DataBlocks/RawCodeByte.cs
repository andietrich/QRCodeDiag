using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
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
            byte numericValue;
            base.GetAsByte(out numericValue);
            return "0x" + BitConverter.ToString(new byte[] { numericValue });
        }
    }
}
