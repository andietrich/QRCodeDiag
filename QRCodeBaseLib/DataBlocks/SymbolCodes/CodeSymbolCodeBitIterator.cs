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
        private uint currentPosition;
        public uint BitsConsumed => this.currentPosition;

        public CodeSymbolCodeBitIterator(ICodeSymbolCode csc) : this(csc, 0, csc.BitCount)
        {
        }
        public CodeSymbolCodeBitIterator(ICodeSymbolCode csc, uint startIndex, uint length)
        {
            this.symbolCode = csc;
            this.bitString = csc.GetBitString();
            this.startPosition = startIndex;
            this.endPosition = startIndex + length;
            this.currentPosition = this.startPosition;
        }
        public bool EndReached => this.currentPosition == this.endPosition;
        public char CurrentChar => this.EndReached ? 'e' : this.bitString[(int)this.currentPosition];

        public Vector2D Position
        {
            get
            {
                if (this.EndReached)
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
