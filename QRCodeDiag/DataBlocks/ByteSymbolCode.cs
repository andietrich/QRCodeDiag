using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    internal class ByteSymbolCode<T> : DrawableCode, IByteSymbolCode where T : ByteSymbol, new() //ToDo implement for CodeSymbol instead of ByteSymbol
    {
        private List<ByteSymbol> byteSymbolList;
        private string bitString;
        public int SymbolCount { get { return this.byteSymbolList.Count; } }
        public int BitCount
        {
            get
            {
                if(this.byteSymbolList.Count > 1)
                {
                    return this.byteSymbolList.Count * (int)this.byteSymbolList[0].SymbolLength + this.byteSymbolList[this.byteSymbolList.Count - 1].CurrentSymbolLength;
                }
                else if (this.byteSymbolList.Count == 1)
                {
                    return this.byteSymbolList[0].CurrentSymbolLength;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Iterates through all bits of <see cref="IBitIterator"/> <paramref name="it"/> creating a list of type <typeparamref name="T"/> <see cref="ByteSymbol"/>s.
        /// </summary>
        /// <param name="it"><see cref="IBitIterator"/> to iterate through all bits the <see cref="ByteSymbolCode{T}"/> will be composed of.</param>
        public ByteSymbolCode(IBitIterator it)
        {
            this.byteSymbolList = new List<ByteSymbol>();

            var wd = new T();
            char c = it.NextBit();
            while(c != 'e')
            {
                if (c != '0' && c != '1' && c != 'u')
                    throw new NotImplementedException("Bit value " + c + " was not defined.");
                wd.AddBit(c, it.Position);
                if (wd.IsComplete)
                {
                    byteSymbolList.Add(wd);
                    wd = new T();
                }
                c = it.NextBit();
            }

            //ToDo handle remainder bits or error: if(!wd.IsComplete){ if(wd.CurrentSymbolLength > 0) or: if(wd.CurrentSymbolLength != number of remainder bits) for QRCodeBitIterator

            this.InitializeBitString();
        }

        private IBitIterator GetBitIterator()
        {
            return new ByteSymbolCodeBitIterator(this);
        }
        private IBitIterator GetBitIterator(int startIndex, int length)
        {
            return new ByteSymbolCodeBitIterator(this, startIndex, length);
        }

        protected override List<ByteSymbol> GetByteSymbols()
        {
            return this.byteSymbolList;
        }

        public void InitializeBitString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < this.byteSymbolList.Count; i++)
            {
                sb.Append(this.byteSymbolList[i].BitString);
            }
            this.bitString = sb.ToString();
        }
        public string GetBitString()
        {
            return this.bitString;
        }
        public string GetBitString(int startIndex, int length)
        {
            return this.bitString.Substring(startIndex, length);
        }
        public Vector2D GetBitPosition(int bitNumber)
        {
            if (this.byteSymbolList.Count > 0)
            {
                var symbollength = (int)this.byteSymbolList[0].SymbolLength;
                return this.byteSymbolList[bitNumber / symbollength].GetBitCoordinate(bitNumber % symbollength);
            }
            else
            {
                throw new InvalidOperationException("The code is empty.");
            }
        }
        
        public byte[] ToByteArray() //ToDo: adapt for CodeSymbol
        {
            var symbolsAsBytes = new byte[this.byteSymbolList.Count];
            for (int i = 0; i < this.byteSymbolList.Count; i++)
            {
                this.byteSymbolList[i].GetAsByte(out var value);
                symbolsAsBytes[i] = value;
            }
            return symbolsAsBytes;
        }
        public byte[] ToByteArray(byte unknownSymbolByte) //ToDo: adapt for CodeSymbol
        {
            var symbolsAsBytes = new byte[this.byteSymbolList.Count];
            for (int i = 0; i < this.byteSymbolList.Count; i++)
            {
                symbolsAsBytes[i] = this.byteSymbolList[i].GetAsByte(out var value) ? value : unknownSymbolByte;
            }
            return symbolsAsBytes;
        }
        public ByteSymbolCode<T2> ToByteSymbolCode<T2>(int startIndex, int length) where T2 : ByteSymbol, new()
        {
            return new ByteSymbolCode<T2>(this.GetBitIterator(startIndex, length));
        }
        public string DecodeSymbols(char unknownSymbol, Encoding encoding) //Encoding.GetEncoding("iso-8859-1"), unknownSymbol = '_'
        {
            var unknownSymbolByte = encoding.GetBytes(new char[] { unknownSymbol })[0];
            return encoding.GetString(this.ToByteArray(unknownSymbolByte));
        }
    }
}
