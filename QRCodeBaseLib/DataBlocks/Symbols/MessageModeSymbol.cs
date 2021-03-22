using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    public class MessageModeSymbol : CodeSymbol
    {
        public const uint MODEINFOLENGTH = 4; // the message mode information is stored in the first nibble (4 bits)
        public MessageModeSymbol() : base()
        { }

        public override bool IsComplete => this.bitCoordinates.Count == MODEINFOLENGTH;

        public MessageMode.EncodingMode GetMessageMode()
        {
            var bitString = this.BitString;

            try
            {
                return MessageMode.Parse(Convert.ToByte(bitString, 2));
            }
            catch (Exception e) when (e is FormatException || e is ArgumentException)
            {
                throw new QRCodeFormatException("Mode indicator nibble could not be decoded: " + bitString, e);
            }
        }

        public override string ToString()
        {
            try
            {
                return this.GetMessageMode().ToString();

            }
            catch(QRCodeFormatException)
            {
                if (this.IsComplete && !this.HasUnknownBits())
                    return Convert.ToByte(this.BitString, 2).ToString("X2");
                else
                    return base.ToString();
            }
        }
    }
}
