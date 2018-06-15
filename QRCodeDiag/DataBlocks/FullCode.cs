using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    /// <summary>
    /// Contains all code bytes consisting of data and ecc in correct order and the coordinates of each bit.
    /// Set by GenerateRawByteList()
    /// </summary>
    internal class FullCode<T> where T : RawCodeByte, new() //ToDo implement for CodeSymbol instead of RawCodeByte
    {
        /// <summary>
        /// All code bytes consisting of data and ecc in correct order together with the coordinates of each bit.
        /// Set by GenerateRawByteList()
        /// </summary>
        private List<T> rawCodeByteList;
        private string bitString;
        public int SymbolCount { get { return this.rawCodeByteList.Count; } }
        public int BitCount
        {
            get
            {
                if(this.rawCodeByteList.Count > 1)
                {
                    return this.rawCodeByteList.Count * (int)this.rawCodeByteList[0].SymbolLength + this.rawCodeByteList[this.rawCodeByteList.Count - 1].CurrentSymbolLength;
                }
                else if (this.rawCodeByteList.Count == 1)
                {
                    return this.rawCodeByteList[0].CurrentSymbolLength;
                }
                else
                {
                    return 0;
                }
            }
        }
        public FullCode(IBitIterator it)
        {
            this.rawCodeByteList = new List<T>();

            var wd = new T();
            char c = it.NextBit();
            while(c != 'e')
            {
                if (c != '0' && c != '1' && c != 'u')
                    throw new NotImplementedException("Bit value " + c + " was not defined.");
                wd.AddBit(c, it.Position);
                if (wd.IsComplete)
                {
                    rawCodeByteList.Add(wd);
                    wd = new T();
                }
                c = it.NextBit();
            }

            this.InitializeBitString();
        }

        private IBitIterator GetBitIterator()
        {
            return new FullCodeBitIterator<T>(this);
        }
        private IBitIterator GetBitIterator(int startIndex, int length)
        {
            return new FullCodeBitIterator<T>(this, startIndex, length);
        }

        public void InitializeBitString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < this.rawCodeByteList.Count; i++)
            {
                sb.Append(this.rawCodeByteList[i].BitString);
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
            if (this.rawCodeByteList.Count > 0)
            {
                var symbollength = (int)this.rawCodeByteList[0].SymbolLength;
                return this.rawCodeByteList[bitNumber / symbollength].GetBitCoordinate(bitNumber % symbollength);
            }
            else
            {
                throw new InvalidOperationException("The code is empty.");
            }
        }
        
        public byte[] ToByteArray()
        {
            var symbolsAsBytes = new byte[this.rawCodeByteList.Count];
            for (int i = 0; i < this.rawCodeByteList.Count; i++)
            {
                this.rawCodeByteList[i].GetAsByte(out var value);
                symbolsAsBytes[i] = value;
            }
            return symbolsAsBytes;
        }
        public byte[] ToByteArray(byte unknownSymbolByte)
        {
            var symbolsAsBytes = new byte[this.rawCodeByteList.Count];
            for (int i = 0; i < this.rawCodeByteList.Count; i++)
            {
                symbolsAsBytes[i] = this.rawCodeByteList[i].GetAsByte(out var value) ? value : unknownSymbolByte;
            }
            return symbolsAsBytes;
        }
        public FullCode<T2> ToFullCode<T2>(int startIndex, int length) where T2 : RawCodeByte, new()
        {
            return new FullCode<T2>(this.GetBitIterator(startIndex, length));
        }
        public string DecodeSymbols(char unknownSymbol, Encoding encoding) //Encoding.GetEncoding("iso-8859-1"), unknownSymbol = '_'
        {
            var unknownSymbolByte = encoding.GetBytes(new char[] { unknownSymbol })[0];
            return encoding.GetString(this.ToByteArray(unknownSymbolByte));
        }
        public string DecodeSymbols(Encoding encoding) //Encoding.GetEncoding("iso-8859-1")
        {
            return encoding.GetString(this.ToByteArray());
        }

        public void DrawCode(Graphics g, Size size, Color bitColor, Color symbolColor, bool drawBitIndices, bool drawSymbolIndices, int codeVersion)
        {
            var preferredSymbolDrawLocation = 2;

            var codeElWidth = (float)size.Width / QRCode.GetEdgeSizeFromVersion(codeVersion);
            var codeElHeight = (float)size.Height / QRCode.GetEdgeSizeFromVersion(codeVersion);

            var fontFamily = new FontFamily("Lucida Console");
            var largeFont = new Font(fontFamily, codeElHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var symbolIndexBrush = new SolidBrush(symbolColor);


            for (int j = 0; j < this.rawCodeByteList.Count; j++)
            {
                var wd = this.rawCodeByteList[j];
                wd.DrawSymbol(g, size, bitColor, drawBitIndices, codeVersion);
                if (drawSymbolIndices && wd.CurrentSymbolLength > 0)
                {
                    var drawIndexCoord = wd.GetBitCoordinate(Math.Min(preferredSymbolDrawLocation, wd.CurrentSymbolLength));
                    g.DrawString(j.ToString(), largeFont, symbolIndexBrush, new Point((int)(drawIndexCoord.X * codeElWidth), (int)(drawIndexCoord.Y * codeElHeight)));
                }
            }
        }
        //ToDo change RawCodeByte to CodeSymbol
        //public static FullCode<EncodingSymbol> CreateEncodingSpecificSymbols<EncodingSymbol>(FullCode<RawCodeByte> fc) where EncodingSymbol : RawCodeByte, IEncodingSymbol, new()
        //{

        //}
        //ToDo: GetMessageBits, GetECCBits, ToByteArray for change to CodeSymbol
        //ToDo: Apply Reed Solomon repair
    }
}
