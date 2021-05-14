using QRCodeBaseLib.DataBlocks;
using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.DataBlocks.Symbols;
using QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common.ReedSolomon;

namespace QRCodeBaseLib.ECCDecoding
{
    internal class ECCBlock
    {
        private readonly CodeSymbolCode<RawCodeByte> preRepairData;
        private readonly CodeSymbolCode<RawCodeByte> preRepairECC;
        private CodeSymbolCode<RawCodeByte> postRepairData;
        private CodeSymbolCode<RawCodeByte> postRepairECC;

        public bool RepairSuccess { get; private set; }

        public ECCBlock(CodeSymbolCode<RawCodeByte> _preRepairData, CodeSymbolCode<RawCodeByte> _preRepairECC)
        {
            this.preRepairData = _preRepairData;
            this.preRepairECC = _preRepairECC;
            this.RepairBlock();
        }

        private void RepairBlock()
        {
            int[] dataWithECC = new int[this.preRepairData.SymbolCount + this.preRepairECC.SymbolCount];
            ECCBlock.ConvertToIntArray(preRepairData).CopyTo(dataWithECC, 0);
            ECCBlock.ConvertToIntArray(preRepairECC).CopyTo(dataWithECC, this.preRepairData.SymbolCount);

            var rsDecoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
            this.RepairSuccess = rsDecoder.decode(dataWithECC, this.preRepairECC.SymbolCount);


            if (dataWithECC.All(x => x == 0))   // TODO determine better way to detect zeroing out of block
                this.RepairSuccess = false;


            if(this.RepairSuccess)
            {
                var dataArr = new int[this.preRepairData.SymbolCount];
                var eccArr = new int[this.preRepairECC.SymbolCount];
                Array.Copy(dataWithECC, 0, dataArr, 0, dataArr.Length);
                Array.Copy(dataWithECC, dataArr.Length, eccArr, 0, eccArr.Length);
                var dataIt = new OverrideByteSymbolCodeValuesBitIterator<RawCodeByte>(this.preRepairData, dataArr);
                var eccIt = new OverrideByteSymbolCodeValuesBitIterator<RawCodeByte>(this.preRepairECC, eccArr);
                var factory = new RawCodeByteFactory();
                this.postRepairData = CodeSymbolCode<RawCodeByte>.BuildInstance(dataIt, factory);
                this.postRepairECC = CodeSymbolCode<RawCodeByte>.BuildInstance(eccIt, factory);
            }
            else
            {
                this.postRepairData = this.preRepairData;
                this.postRepairECC = this.preRepairECC;
            }
        }

        private static int[] ConvertToIntArray(CodeSymbolCode<RawCodeByte> code)
        {
            var symbolsAsInts = new int[code.SymbolCount];
            for (int i = 0; i < code.SymbolCount; i++)
            {
                code.GetSymbolAt(i).TryGetAsByte(out var symbolAsByte);
                symbolsAsInts[i] = symbolAsByte;
            }
            return symbolsAsInts;
        }

        public CodeSymbolCode<RawCodeByte> GetPreRepairData()
        {
            return this.preRepairData;
        }
        public CodeSymbolCode<RawCodeByte> GetPreRepairECC()
        {
            return this.preRepairECC;
        }
        public CodeSymbolCode<RawCodeByte> GetPostRepairData()
        {
            return this.postRepairData;
        }
        public CodeSymbolCode<RawCodeByte> GetPostRepairECC()
        {
            return this.postRepairECC;
        }
    }
}
