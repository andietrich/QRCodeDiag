using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    /*
     * ToDo:
     * Map x-y-coordinates to bit-position: for clicking a pixel and setting (toggling) the value
     * Map bit-position to x-y-coordinate: for highlighting a bit position
     * Draw borders around bytes/words
     * Get word size from format information or user input
     */
    class QRCodeBitIterator : IBitIterator
    {
        private char[,] bits;
        private int xPos, yPos;
        private bool directionUp;
        private bool rightCell;
        public int XPos
        {
            get { return this.xPos; }
            private set
            {
                if (value < 0)
                    this.EndReached = true;
                else
                    this.xPos = value;
            }
        }
        public int YPos
        {
            get { return this.yPos; }
            private set
            {
                if (value < 0)
                    this.EndReached = true;
                else
                    this.yPos = value;
            }
        }
        public Vector2D Position { get { return new Vector2D(this.xPos, this.yPos); } }

        public bool EndReached { get; private set; }
        public char CurrentChar { get { return this.bits[this.XPos, this.YPos]; } }

        public QRCodeBitIterator(char[,] setBits)
        {
            if (setBits.GetLength(0) != setBits.GetLength(1) || setBits.GetLength(0) != QRCode.SIZE)
                throw new ArgumentException("Must be 29x29 elements.", "setbits");
            this.bits = setBits;
            this.XPos = QRCode.SIZE - 1;
            this.YPos = QRCode.SIZE - 1;
            this.directionUp = true;
            this.rightCell = true;
            this.EndReached = false;
        }
        
        public char NextBit()
        {
            bool nextFound = false;
            while (!this.EndReached && !nextFound)
            {
                if (this.YPos == 0 && this.directionUp && !this.rightCell)
                {
                    // go to left neighbor lane, right top cell
                    this.directionUp = false;
                    this.rightCell = true;
                    this.XPos--;
                    if (this.XPos == 6)
                        this.XPos--;
                }
                else if (this.YPos == QRCode.SIZE -1 && !this.directionUp && !this.rightCell)
                {
                    // go to left neighbor lane, right bottom cell
                    this.directionUp = true;
                    this.rightCell = true;
                    this.XPos--;
                    if (this.XPos == 6)
                        this.XPos--;
                }
                else
                {
                    if (this.rightCell)
                    {
                        this.XPos--;
                        this.rightCell = false;
                    }
                    else
                    {
                        this.XPos++;
                        this.rightCell = true;
                        if (this.directionUp)
                        {
                            this.YPos--;
                        }
                        else
                        {
                            this.YPos++;
                        }
                    }
                }
                //var str = string.Format("({0}, {1})", this.XPos, this.YPos);
                //System.Diagnostics.Debug.Write(str);
                if (QRCode.IsDataCell(this.XPos, this.YPos))
                {
                    nextFound = true;
                    //System.Diagnostics.Debug.Write(" " + this.bits[this.XPos, this.YPos] + Environment.NewLine);
                }
                //else
                //    System.Diagnostics.Debug.Write(" invalid" + Environment.NewLine);
            }
            if (nextFound)
            {
                return this.bits[this.XPos, this.YPos];
            }
            else
                return 'e';
        }
    }
}
