using QRCodeDiag.DataBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common.ReedSolomon;

namespace QRCodeDiag.ECCDecoding
{
    class ECCBlock
    {
        private readonly ByteSymbolCode<RawCodeByte> preRepairData;
        private readonly ByteSymbolCode<RawCodeByte> preRepairECC;
        private ByteSymbolCode<RawCodeByte> postRepairData;
        private ByteSymbolCode<RawCodeByte> postRepairECC;

        public bool RepairSuccess { get; private set; }

        public ECCBlock(ByteSymbolCode<RawCodeByte> _preRepairData, ByteSymbolCode<RawCodeByte> _preRepairECC)
        {
            this.preRepairData = _preRepairData;
            this.preRepairECC = _preRepairECC;
            this.RepairBlock();
        }

        private void RepairBlock()
        {
            int[] dataWithECC = new int[this.preRepairData.SymbolCount + this.preRepairECC.SymbolCount];
            this.preRepairData.ToIntArray().CopyTo(dataWithECC, 0);
            this.preRepairECC.ToIntArray().CopyTo(dataWithECC, this.preRepairData.SymbolCount);

            var rsDecoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
            this.RepairSuccess = rsDecoder.decode(dataWithECC, this.preRepairECC.SymbolCount);

            if(this.RepairSuccess)
            {
                var dataArr = new int[this.preRepairData.SymbolCount];
                var eccArr = new int[this.preRepairECC.SymbolCount];
                Array.Copy(dataWithECC, 0, dataArr, 0, dataArr.Length);
                Array.Copy(dataWithECC, this.preRepairECC.SymbolCount, eccArr, 0, eccArr.Length);
                var dataIt = new OverrideByteSymbolCodeValuesBitIterator<RawCodeByte>(this.preRepairData, dataArr);
                var eccIt = new OverrideByteSymbolCodeValuesBitIterator<RawCodeByte>(this.preRepairECC, eccArr);
                this.postRepairData = new ByteSymbolCode<RawCodeByte>(dataIt);
                this.postRepairECC = new ByteSymbolCode<RawCodeByte>(eccIt);
            }
            else
            {
                this.postRepairData = this.preRepairData;
                this.postRepairECC = this.preRepairECC;
            }
        }

        private ByteSymbolCode<RawCodeByte> GetPreRepairData()
        {
            return this.preRepairData;
        }
        private ByteSymbolCode<RawCodeByte> GetPreRepairECC()
        {
            return this.preRepairECC;
        }
        private ByteSymbolCode<RawCodeByte> GetPostRepairData()
        {
            return this.postRepairData;
        }
        private ByteSymbolCode<RawCodeByte> GetPostRepairECC()
        {
            return this.postRepairECC;
        }
    }
}
