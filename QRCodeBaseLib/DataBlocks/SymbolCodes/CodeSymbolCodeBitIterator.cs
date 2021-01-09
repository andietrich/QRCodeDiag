using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks.SymbolCodes
{
    internal class CodeSymbolCodeBitIterator : IBitIterator
    {
        private readonly ICodeSymbolCode symbolCode;
        private readonly string bitString;
        private readonly uint endPosition;
        private readonly uint startPosition;
        private int currentPosition;

        public CodeSymbolCodeBitIterator(ICodeSymbolCode csc) : this(csc, 0, csc.BitCount)
        {
        }
        public CodeSymbolCodeBitIterator(ICodeSymbolCode csc, uint startIndex, uint length)
        {
            this.symbolCode = csc;
            this.bitString = csc.GetBitString();
            this.startPosition = startIndex;
            this.endPosition = startIndex + length;
            this.currentPosition = (int)this.startPosition-1;
        }
        public bool EndReached { get { return this.currentPosition == this.endPosition; } }

        public char CurrentChar
        {
            get
            {
                if (this.currentPosition < this.startPosition)
                {
                    return this.bitString[(int)this.startPosition];
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
                    return this.symbolCode.GetBitPosition((uint)this.currentPosition);
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
