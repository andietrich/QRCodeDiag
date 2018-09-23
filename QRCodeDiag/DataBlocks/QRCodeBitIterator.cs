using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    class QRCodeBitIterator : IBitIterator
    {
        private QRCode qrCode;
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
        public bool IsInitial { get; private set; }
        public char CurrentChar
        {
            get
            {
                return this.qrCode.GetBit(this.XPos, this.YPos);
            }
        }

        public QRCodeBitIterator(QRCode code)
        {
            this.qrCode = code;
            this.XPos = this.qrCode.GetEdgeLength() - 1;
            this.YPos = this.qrCode.GetEdgeLength() - 1;
            this.directionUp = true;
            this.rightCell = true;
            this.EndReached = false;
            this.IsInitial = true;
        }
        
        public char NextBit()
        {
            if(this.IsInitial)
            {
                this.IsInitial = false;
                return this.CurrentChar;
            }
            bool nextFound = false;
            while (!this.EndReached && !nextFound)
            {
                if (this.YPos == 0 && this.directionUp && !this.rightCell) // Top end of a line reached
                {
                    // go to left neighbor lane, right top cell
                    this.directionUp = false;
                    this.rightCell = true;
                    this.XPos--;
                    if (this.XPos == 6)
                        this.XPos--;
                }
                else if (this.YPos == this.qrCode.GetEdgeLength() - 1 && !this.directionUp && !this.rightCell) // Bottom end of a line reached
                {
                    // go to left neighbor lane, right bottom cell
                    this.directionUp = true;
                    this.rightCell = true;
                    this.XPos--;
                    if (this.XPos == 6)
                        this.XPos--;
                }
                else // Somewhere in the line
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
                if (this.qrCode.IsDataCell(this.XPos, this.YPos))
                {
                    nextFound = true;
                }
            }
            if (nextFound)
            {
                return this.qrCode.GetBit(this.XPos, this.YPos);
            }
            else
            {
                return 'e';
            }
        }
    }
}
