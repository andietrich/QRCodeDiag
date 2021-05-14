using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    internal class CharCountIndicatorSymbol : CodeSymbol
    {
        private readonly uint size;
        public override bool IsComplete => this.bitCoordinates.Count == this.size;
        public override uint MaxSymbolLength => this.size;

        public CharCountIndicatorSymbol(MessageMode messageMode) : base()
        {
            this.size = messageMode.CharacterCountIndicatorLength;
        }

        public bool TryGetAsUInt(out uint value, byte unknownBitValue = 0)
        {
            if (unknownBitValue > 1)
                throw new ArgumentException($"Invalid bit value: {unknownBitValue}", "unknownBitValue");

            bool unknownBits = false;

            foreach(var bit in this.BitString)
            {
                if ((bit != '0') && (bit != '1'))
                    unknownBits = true;
            }

            if (unknownBits)
                value = Convert.ToUInt32(this.BitString.Replace("u", unknownBitValue.ToString()), 2);
            else
                value = Convert.ToUInt32(this.BitString, 2);

            return !unknownBits;
        }

        public override string ToString()
        {
            if (this.TryGetAsUInt(out var value))
                return value.ToString();
            else
                return $"-{this.BitString}-";
        }

        public uint GetCharacterCount()
        {
            try
            {
                return Convert.ToUInt32(this.BitString, 2);
            }
            catch (FormatException fe)
            {
                throw new QRCodeFormatException("Could not parse character count.", fe);
            }
        }
    }
}
