using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    class ByteSymbolCodeBitIterator : IBitIterator //ToDo adapt for CodeSymbol instead of RawCodeByte
    {
        private readonly IByteSymbolCode byteSymbolCode;
        private readonly string bitString;
        private readonly int endPosition;
        private readonly int startPosition;
        private int currentPosition;

        public ByteSymbolCodeBitIterator(IByteSymbolCode bsc)
        {
            this.byteSymbolCode = bsc;
            this.bitString = bsc.GetBitString();
            this.startPosition = 0;
            this.endPosition = this.bitString.Length;
            this.currentPosition = this.startPosition-1;
        }
        public ByteSymbolCodeBitIterator(IByteSymbolCode fc, int startIndex, int length)
        {
            this.byteSymbolCode = fc;
            this.bitString = fc.GetBitString();
            this.startPosition = startIndex;
            this.endPosition = startIndex + length;
            this.currentPosition = this.startPosition-1;
        }
        public bool EndReached { get { return this.currentPosition == this.endPosition; } }

        public char CurrentChar
        {
            get
            {
                if (this.currentPosition < this.startPosition)
                {
                    return this.bitString[this.startPosition];
                }
                else if (this.currentPosition < this.endPosition)
                {
                    return this.bitString[this.currentPosition];
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
                if (this.currentPosition < 0 || this.currentPosition >= this.endPosition)
                {
                    throw new InvalidOperationException("The iterator is not on a valid position.");
                }
                else
                {
                    return this.byteSymbolCode.GetBitPosition(this.currentPosition);
                }
            }
        }

        public char NextBit()
        {
            if(!this.EndReached)
                this.currentPosition++;
            return this.CurrentChar;
        }
    }
}
