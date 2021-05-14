using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    class PaddingSymbol : CodeSymbol
    {
        private readonly uint maxSymbolLength;
        public override uint MaxSymbolLength => this.maxSymbolLength;
        public override bool IsComplete => this.CurrentSymbolLength == this.MaxSymbolLength;
        public PaddingSymbol(uint setMaxSymbolLength)
        {
            this.maxSymbolLength = setMaxSymbolLength;
        }

        /// <summary>
        /// Gets the byte representation of the PaddingSymbol.
        /// If there are unknown bits they will be replaced by 0 and the function will return false.
        /// If not all bits have been set, the missing bits will be treated as unknown.
        /// </summary>
        /// <param name="value">The byte representation will be written to this parameter.</param>
        /// <param name="unknownBitValue">The value assumed for unknown bits. Defaults to 0.</param>
        /// <returns>True if all bits are known, otherwise false.</returns>
        public bool TryGetAsByte(out byte value, byte unknownBitValue = 0)
        {
            if (unknownBitValue > 1)
                throw new ArgumentException("Invalid bit value", nameof(unknownBitValue));

            bool unknownBits = this.HasUnknownBits();

            if (unknownBits)
                value = Convert.ToByte(this.BitString.Replace("u", unknownBitValue.ToString()), 2);
            else
                value = Convert.ToByte(this.BitString, 2);

            return !unknownBits;
        }

        public override string ToString()
        {
            if (this.TryGetAsByte(out var value))
            {
                if (this.MaxSymbolLength == 4)
                    return $"{value:X1}";
                else if (this.MaxSymbolLength == 8)
                    return $"{value:X2}";
                else
                    return base.ToString();
            }
            else
                return base.ToString();
        }
    }
}
