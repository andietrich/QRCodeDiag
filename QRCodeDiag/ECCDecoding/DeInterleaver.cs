﻿using QRCodeDiag.DataBlocks;
using QRCodeDiag.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.ECCDecoding
{
    class DeInterleaver
    {
        private readonly ByteSymbolCode<RawCodeByte> dataByteCode;
        private readonly ByteSymbolCode<RawCodeByte> eccByteCode;
        public DeInterleaver(ByteSymbolCode<RawCodeByte>    interleavedCode,
                             QRCodeVersion                  version,
                             ErrorCorrectionLevel           eccLevel)
        {
            // a block consists of data and ecc codewords for that block
            // blocks are in groups of blocks with the same amount of data codewords (max 2 groups)
            // blocks for all groups have the same amount of ecc codewords
            // the number of data codewords can differ between groups but not between blocks of a group
            var eccGroups = eccLevel.GetECCGroups();
            int numberOfGroups = eccGroups.Length;
            uint numberOfBlocks = 0;
            uint numberOfRawCodeBytes = 0;

            foreach(ECCGroupInfo g in eccGroups)
            {
                numberOfBlocks += g.NumberOfBlocks;
                numberOfRawCodeBytes += g.NumberOfBlocks * (g.DataBytesPerBlock + eccLevel.ECCBytesPerBlock);
            }

            if (interleavedCode.SymbolCount != numberOfRawCodeBytes)
                throw new ArgumentException("Invalid number of symbols in interleaved code.");

            uint largestBlockLength = 0;
            foreach(ECCGroupInfo g in eccGroups)
            {
                largestBlockLength = Math.Max(largestBlockLength, g.DataBytesPerBlock);
            }
            var dataCodewords = new RawCodeByte[numberOfBlocks, largestBlockLength];
            var eccCodewords  = new RawCodeByte[numberOfBlocks, eccLevel.ECCBytesPerBlock];
            int pos = 0; // position in interleavedCode

            // generate data block code
            for (int j = 0; j < largestBlockLength; j++)            // for each symbol in block
            {
                for (int i = 0; i < numberOfBlocks; i++)            // for each block
                {
                    if (i < DeInterleaver.GetBlockLength(i, eccGroups)) // skip block if it is already full
                    {
                        dataCodewords[i, j] = interleavedCode.GetSymbolAt(pos++);
                    }
                }
            }
            // generate ecc block code
            for (int j = 0; j < eccLevel.ECCBytesPerBlock; j++)     // for each symbol in block
            {
                for (int i = 0; i < numberOfBlocks; i++)            // for each block
                {
                    eccCodewords[i, j] = interleavedCode.GetSymbolAt(pos++);
                }
            }

            var dataBytesInOrder = new List<ByteSymbol>();
            var eccBytesInOrder  = new List<ByteSymbol>();
            for(int g = 0; g < eccGroups.Length; g++)                       // group
            {
                for(int b = 0; b < eccGroups[g].NumberOfBlocks; b++)        // block
                {
                    for(int s = 0; s < eccGroups[g].DataBytesPerBlock; s++) // data symbol
                    {
                        dataBytesInOrder.Add(dataCodewords[b, s]);
                    }
                    for(int e = 0; e < eccLevel.ECCBytesPerBlock; e++)      // ecc symbol
                    {
                        dataBytesInOrder.Add(eccCodewords[b, e]);
                    }
                }
            }

            this.dataByteCode = new ByteSymbolCode<RawCodeByte>(dataBytesInOrder);
            this.eccByteCode  = new ByteSymbolCode<RawCodeByte>(eccBytesInOrder);
        }

        private static uint GetBlockLength(int absoluteBlockNumber, ECCGroupInfo[] eccGroups)
        {
            for(int i = 0; i < eccGroups.Length; i++)
            {
                if (absoluteBlockNumber < eccGroups[i].NumberOfBlocks)
                    return eccGroups[i].DataBytesPerBlock;
                else
                    absoluteBlockNumber -= (int) eccGroups[i].NumberOfBlocks;
            }
            throw new ArgumentException("absoluteBlockNumber is larger than the total number of blocks in eccGroups.");
        }

        public ByteSymbolCode<RawCodeByte> GetDataByteCode()
        {
            return this.dataByteCode;
        }
        public ByteSymbolCode<RawCodeByte> GetECCByteCode()
        {
            return this.eccByteCode;
        }
    }
}