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
        private int bitPosition;
        private uint bitCount;
        private readonly CodeSymbolCode<T> originalCode;
        private readonly int[] newValues;
        public bool EndReached
        {
            get
            {
                return this.bitPosition >= this.bitCount;
            }
        }

        public char CurrentChar
        {
            get
            {
                if (this.bitPosition < 0)
                {
                    return this.GetBitCharAt(0);
                }
                else if (!this.EndReached)
                {
                    return this.GetBitCharAt(this.bitPosition);
                }
                else
                {
                    return 'e';
                }
            }
        }

        public Vector2D Position
        {
            get
            {
                var pos = this.bitPosition < 0 ? 0u : (uint)this.bitPosition;
                return this.originalCode.GetBitPosition(pos);
            }
        }

        public OverrideByteSymbolCodeValuesBitIterator(CodeSymbolCode<T> _originalCode, int[] _newValues)
        {
            this.bitPosition = -1;
            this.bitCount = _originalCode.BitCount;
            this.originalCode = _originalCode;
            this.newValues = _newValues;
        }

        private char GetBitCharAt(int index)
        {
            var bytePos = index / ByteSymbol.BYTESYMBOLLENGTH;
            var bitPos = (int)((ByteSymbol.BYTESYMBOLLENGTH - 1) - (index % ByteSymbol.BYTESYMBOLLENGTH));  // iterate from msbit to lsbit
            var bitValue = (((byte)newValues[bytePos]) >> bitPos) & 1;
            if (bitValue == 1)
                return '1';
            else
                return '0';
        }

        public char NextBit()
        {
            if(!this.EndReached)
                this.bitPosition++;
            return this.CurrentChar;
        }
    }
}
