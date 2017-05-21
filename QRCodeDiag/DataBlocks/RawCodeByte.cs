using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    class RawCodeByte : CodeSymbol
    {
        private const uint RAWBYTELENGTH = 8;
        public RawCodeByte() : base(RAWBYTELENGTH)
        { }

        public object Clone()
        {
            var ret = new RawCodeByte();
            for(int i = 0; i < this.bitCoordinates.Count; i++)
            {
                ret.AddBit(this.bitArray[i], this.bitCoordinates[i].X, this.bitCoordinates[i].Y);
            }
            return ret;
        }
        public override char[] GetDecodedSymbols()
        {
            throw new NotImplementedException();
        }
        public bool GetAsByte(out byte value)
        {
            int bits = 0;
            value = 0;
            if (this.IsComplete)
            {
                for (int i = 0; i < this.bitArray.Length; i++)
                {
                    if (this.bitArray[i] == '0')
                    {
                        bits++;
                    }
                    else if (this.bitArray[i] != '1')
                    {
                        bits++;
                        value += (byte)(1 << i);
                    }
                }
                System.Diagnostics.Debug.Assert(bits != this.MaxBitCount || (Convert.ToByte(this.BitString, 2) == value));
                return bits == this.MaxBitCount;
            }
            else
            {
                return false;
            }
        }
    }
}
