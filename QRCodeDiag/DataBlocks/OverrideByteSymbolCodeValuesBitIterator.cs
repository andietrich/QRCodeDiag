﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    class OverrideByteSymbolCodeValuesBitIterator<T> : IBitIterator where T: ByteSymbol, new()
    {
        private int bitPosition;
        public bool EndReached
        {
            get
            {
                return this.bitPosition == this.newValues.Length - 1;
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
                else if (this.bitPosition < this.newValues.Length)
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
                return this.originalCode.GetBitPosition(this.bitPosition);
            }
        }

        private readonly ByteSymbolCode<T> originalCode;
        private readonly int[] newValues;

        private char GetBitCharAt(int index)
        {
            var bitValue = (((byte)newValues[index / ByteSymbol.BYTESYMBOLLENGTH]) >> (int)(index % ByteSymbol.BYTESYMBOLLENGTH)) & 1;
            if (bitValue == 1)
                return '1';
            else
                return '0';
        }

        public OverrideByteSymbolCodeValuesBitIterator(ByteSymbolCode<T> _originalCode, int[] _newValues)
        {
            this.bitPosition = -1;
            this.originalCode = _originalCode;
            this.newValues = _newValues;
        }

        public char NextBit()
        {
            if(!this.EndReached)
                this.bitPosition++;
            return this.CurrentChar;
        }
    }
}