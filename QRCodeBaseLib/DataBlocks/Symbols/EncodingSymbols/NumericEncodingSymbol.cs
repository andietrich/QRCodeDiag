using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols
{
    internal class NumericEncodingSymbol : CodeSymbol, IEncodingSymbol
    {
        private readonly uint maxBits;
        public MessageMode.EncodingMode EncodingType { get { return MessageMode.EncodingMode.Numeric; } }

        public override bool IsComplete => this.CurrentSymbolLength == 10u || this.CurrentSymbolLength == 7u || this.CurrentSymbolLength == 4u;
        public override uint MaxSymbolLength => this.maxBits;

        public NumericEncodingSymbol(uint setMaxDigits) : base()
        {
            if (setMaxDigits == 0 || setMaxDigits > 3)
                throw new ArgumentException("Must be between 1 and 3", nameof(setMaxDigits));

            this.maxBits = setMaxDigits * 3 + 1;
        }

        public override string ToString()
        {
            if (this.IsComplete && !this.HasUnknownBits())
            {
                var val = Convert.ToUInt32(this.BitString, 2);

                switch (this.CurrentSymbolLength)
                {
                    case 4:
                        return $"{val:1}";
                    case 7:
                        return $"{val:2}";
                    case 10:
                        return $"{val:3}";
                    default:
                        throw new ApplicationException($"Invalid number of bits for a completed NumericEncodingSymbol");
                }
            }
            else
            {
                return base.ToString();
            }
        }
    }
}
