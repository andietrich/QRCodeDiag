using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols
{
    internal class AlphaNumericEncodingSymbol : CodeSymbol, IEncodingSymbol
    {
        private readonly Dictionary<uint, char> SymbolTable = new Dictionary<uint, char>()
        {
            { 0, '0'},
            { 1, '1'},
            { 2, '2'},
            { 3, '3'},
            { 4, '4'},
            { 5, '5'},
            { 6, '6'},
            { 7, '7'},
            { 8, '8'},
            { 9, '9'},
            {10, 'A'},
            {11, 'B'},
            {12, 'C'},
            {13, 'D'},
            {14, 'E'},
            {15, 'F'},
            {16, 'G'},
            {17, 'H'},
            {18, 'I'},
            {19, 'J'},
            {20, 'K'},
            {21, 'L'},
            {22, 'M'},
            {23, 'N'},
            {24, 'O'},
            {25, 'P'},
            {26, 'Q'},
            {27, 'R'},
            {28, 'S'},
            {29, 'T'},
            {30, 'U'},
            {31, 'V'},
            {32, 'W'},
            {33, 'X'},
            {34, 'Y'},
            {35, 'Z'},
            {36, ' '},
            {37, '$'},
            {38, '%'},
            {39, '*'},
            {40, '+'},
            {41, '-'},
            {42, '.'},
            {43, '/'},
            {44, ':'}
        };
        private readonly uint maxBits;
        public MessageMode.EncodingMode EncodingType { get { return MessageMode.EncodingMode.Alphanumeric; } }
        public override bool IsComplete => this.CurrentSymbolLength == 11 || this.CurrentSymbolLength == 6;
        public override uint MaxSymbolLength => this.maxBits;
        
        public AlphaNumericEncodingSymbol(uint setMaxCharacters) : base()
        {
            if (setMaxCharacters == 0 || setMaxCharacters > 2)
                throw new ArgumentException("Must be either 1 or 2", nameof(setMaxCharacters));

            this.maxBits = setMaxCharacters * 5 + 1;
        }

        public override string ToString()
        {
            if (this.IsComplete && !this.HasUnknownBits())
            {
                uint numericValue = Convert.ToUInt32(this.BitString, 2);
                switch (this.CurrentSymbolLength)
                {
                    case 6:
                        return this.SymbolTable[numericValue].ToString();
                    case 11:
                        return $"{this.SymbolTable[numericValue / 45]}{this.SymbolTable[numericValue % 45]}";
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
