using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    class RawCodeByte : CodeSymbol
    {
        public const uint RAWBYTELENGTH = 8;
        public override uint SymbolLength => RAWBYTELENGTH;
        public object Clone()
        {
            var ret = new RawCodeByte();
            for(int i = 0; i < this.bitCoordinates.Count; i++)
            {
                ret.AddBit(this.bitArray[i], this.bitCoordinates[i].X, this.bitCoordinates[i].Y);
            }
            return ret;
        }
        public bool GetAsByte(out byte value)
        {
            int bits = 0;
            value = 0;
            if (this.IsComplete)
            {
                for (int i = 0; i < this.bitArray.Length; i++)
                {
                    if (this.bitArray[i] == '0')
                    {
                        bits++;
                    }
                    else if (this.bitArray[i] == '1')
                    {
                        bits++;
                        value += (byte)(0x80 >> i);
                    }
                }
                System.Diagnostics.Debug.Assert(bits != this.MaxBitCount || (Convert.ToByte(this.BitString, 2) == value));
                return bits == this.MaxBitCount;
            }
            else
            {
                return false;
            }
        }
        public static string DecodeSymbols<T>(IList<T> symbols, char unknownSymbol, Encoding encoding) where T : RawCodeByte
        {
            var unknownSymbolByte = encoding.GetBytes(new char[] { unknownSymbol })[0];
            var symbolsAsBytes = new byte[symbols.Count];
            for (int i = 0; i < symbols.Count; i++)
            {
                symbolsAsBytes[i] = symbols[i].GetAsByte(out var value) ? value : unknownSymbolByte;
            }
            return encoding.GetString(symbolsAsBytes);
        }
    }
}
