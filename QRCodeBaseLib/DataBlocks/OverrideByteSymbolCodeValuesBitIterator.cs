using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.DataBlocks.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks
{
    /// <summary>
    /// Used to create a new ByteSymbolCode with new bit values from an array
    /// using the bit positions of a source ByteSymbolCode
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class OverrideByteSymbolCodeValuesBitIterator<T> : IBitIterator where T: ByteSymbol, new()
    {
        private uint bitPosition;
        private uint bitCount;
        private readonly CodeSymbolCode<T> originalCode;
        private readonly int[] newValues;
        public bool EndReached => this.bitPosition >= this.bitCount;
        public char CurrentChar => this.EndReached ? 'e' : this.GetBitCharAt(this.bitPosition);
        public Vector2D Position => this.originalCode.GetBitPosition(this.bitPosition);
        public uint BitsConsumed => this.bitPosition;

        public OverrideByteSymbolCodeValuesBitIterator(CodeSymbolCode<T> setOriginalCode, int[] setNewValues)
        {
            this.bitPosition = 0;
            this.bitCount = setOriginalCode.BitCount;
            this.originalCode = setOriginalCode;
            this.newValues = setNewValues;
        }

        private char GetBitCharAt(uint index)
        {
            var bytePos = index / 8;
            var bitPos = (int)((8 - 1) - (index % 8));  // iterate from msbit to lsbit
            var bitValue = (((byte)newValues[bytePos]) >> bitPos) & 1;
            
            return (bitValue == 1) ? '1' : '0';
        }

        public char NextBit()
        {
            if(!this.EndReached)
                this.bitPosition++;
            return this.CurrentChar;
        }
    }
}
