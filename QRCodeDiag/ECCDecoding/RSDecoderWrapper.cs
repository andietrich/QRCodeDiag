using QRCodeDiag.DataBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common.ReedSolomon;

namespace QRCodeDiag.ECCDecoding
{
    class RSDecoderWrapper
    {
        private static void DecodeAllBlocks(ByteSymbolCode<RawCodeByte> dataBytes, ByteSymbolCode<RawCodeByte> eccBytes)
        {

        }
        //private static void DecodeBlock(int[] dataBytes, int[] eccBytes)
        //{
        //    int[] dataWithECC = new int[dataBytes.Length + eccBytes.Length];
        //    dataBytes.CopyTo(dataWithECC, 0);
        //    eccBytes.CopyTo(dataWithECC, dataBytes.Length);
        //    var rsDecoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
        //    if (rsDecoder.decode(dataWithECC, eccBytes.Length))
        //    {
        //        var binarySB = new StringBuilder();
        //        for (int i = 0; i < codeAsInts.Length; i++) // also repair the ecc bytes, the goal is to completely restore the broken qr code
        //        {
        //            binarySB.Append(Convert.ToString((byte)codeAsInts[i], 2).PadLeft(8, '0'));
        //        }
        //        return fullCode.GetBitString() + Environment.NewLine + binarySB.ToString();
        //    }
        //    else
        //    {
        //        return "Could not repair";
        //    }
        //}
    }
}
