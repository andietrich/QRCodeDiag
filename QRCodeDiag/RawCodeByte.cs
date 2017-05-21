using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
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
    }
}
