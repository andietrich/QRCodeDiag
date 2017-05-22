using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    class FullCodeBitIterator<T> : IBitIterator where T : RawCodeByte, new() //ToDo adapt for CodeSymbol instead of RawCodeByte
    {
        private FullCode<T> fullCode;
        private string bitString;
        private int currentPosition;
        private int endPosition;
        private int startPosition;

        public FullCodeBitIterator(FullCode<T> fc)
        {
            this.fullCode = fc;
            this.bitString = fc.GetBitString();
            this.startPosition = 0;
            this.endPosition = this.bitString.Length;
            this.currentPosition = this.startPosition-1;
        }
        public FullCodeBitIterator(FullCode<T> fc, int startIndex, int length)
        {
            this.fullCode = fc;
            this.bitString = fc.GetBitString();
            this.startPosition = startIndex;
            this.endPosition = startIndex + length;
            this.currentPosition = this.startPosition-1;
        }
        public bool EndReached { get { return this.currentPosition == this.bitString.Length; } }

        public char CurrentChar
        {
            get
            {
                if (this.currentPosition < this.startPosition)
                {
                    return '\0';
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
                if (this.currentPosition < 0 || this.currentPosition >= this.bitString.Length)
                {
                    throw new InvalidOperationException("The iterator is not on a valid position.");
                }
                else
                {
                    return this.fullCode.GetBitPosition(this.currentPosition);
                }
            }
        }

        public char NextBit()
        {
            this.currentPosition++;
            return this.CurrentChar;
        }
    }
}
