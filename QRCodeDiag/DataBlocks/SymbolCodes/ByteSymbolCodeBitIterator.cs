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
            this.CurrentPosition = this.startPosition-1;
        }
        public ByteSymbolCodeBitIterator(IByteSymbolCode fc, int startIndex, int length)
        {
            this.byteSymbolCode = fc;
            this.bitString = fc.GetBitString();
            this.startPosition = startIndex;
            this.endPosition = startIndex + length;
            this.CurrentPosition = this.startPosition-1;
        }
        public bool EndReached { get { return this.CurrentPosition == this.bitString.Length; } }

        public char CurrentChar
        {
            get
            {
                if (this.CurrentPosition < this.startPosition)
                {
                    return '\0';
                }
                else if (this.CurrentPosition < this.endPosition)
                {
                    return this.bitString[this.CurrentPosition];
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
                if (this.CurrentPosition < 0 || this.CurrentPosition >= this.bitString.Length)
                {
                    throw new InvalidOperationException("The iterator is not on a valid position.");
                }
                else
                {
                    return this.byteSymbolCode.GetBitPosition(this.CurrentPosition);
                }
            }
        }

        public int CurrentPosition { get => currentPosition; set => currentPosition = value; }

        public char NextBit()
        {
            this.CurrentPosition++;
            return this.CurrentChar;
        }
    }
}
