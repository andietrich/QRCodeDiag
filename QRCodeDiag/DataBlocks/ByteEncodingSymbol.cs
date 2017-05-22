﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeDiag.QRCode;

namespace QRCodeDiag.DataBlocks
{
    class ByteEncodingSymbol : RawCodeByte, IEncodingSymbol
    {
        public MessageMode EncodingType { get { return MessageMode.Byte; } }
        public static string DecodeSymbols(IList<ByteEncodingSymbol> symbols, char unknownSymbol = '_')
        {
            return RawCodeByte.DecodeSymbols(symbols, unknownSymbol, Encoding.GetEncoding("iso-8859-1"));
        }
        public override object Clone()
        {
            var ret = new ByteEncodingSymbol();
            for (int i = 0; i < this.bitCoordinates.Count; i++)
            {
                ret.AddBit(this.bitArray[i], this.bitCoordinates[i]);
            }
            return ret;
        }
    }
}
