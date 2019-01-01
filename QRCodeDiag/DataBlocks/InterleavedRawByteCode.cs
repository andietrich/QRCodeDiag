using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    internal class InterleavedRawByteCode : ByteSymbolCode<RawCodeByte>
    {
        public InterleavedRawByteCode(IBitIterator it) : base(it)
        { }

        private void DeInterleave()
        {

        }

        public IByteSymbolCode GetDataSymbolCode()
        {
            throw new NotImplementedException();
        }

        public ByteSymbolCode<RawCodeByte> GetECCSymbolCode()
        {
            throw new NotImplementedException();
        }
    }
}
