using QRCodeBaseLib.DataBlocks.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.MetaInfo
{
    public class MessageMode
    {
        public enum EncodingMode
        {
            Terminator = 0,
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            //ECI = 7,    // extended channel interpretation
            Kanji = 8
        }

        public const uint MODEINFOLENGTH = 4u; // the message mode information is stored in the first nibble (4 bits)

        public EncodingMode Mode { get; private set; }
        public uint CharacterCountIndicatorLength { get; private set; }

        private MessageMode(EncodingMode mode, uint characterCountIndicatorLength)
        {
            this.Mode = mode;
            this.CharacterCountIndicatorLength = characterCountIndicatorLength;
        }

        internal static bool TryParseMessageMode(MessageModeSymbol modeSymbol, QRCodeVersion version, out MessageMode mode)
        {
            if (MessageMode.TryParse(modeSymbol.GetSymbolValue(), out var encodingMode))
            {
                mode = new MessageMode(encodingMode, MessageMode.GetCharacterCountIndicatorLength(version, encodingMode));
                return true;
            }
            else
            {
                mode = null;
                return false;
            }
        }
        internal static MessageMode ParseMessageMode(MessageModeSymbol modeSymbol, QRCodeVersion version)
        {
            var mode = MessageMode.Parse(modeSymbol.GetSymbolValue());
            return new MessageMode(mode, MessageMode.GetCharacterCountIndicatorLength(version, mode));
        }

        internal static bool TryParse(int value, out EncodingMode mode)
        {
            if (Enum.IsDefined(typeof(EncodingMode), value))
            {
                mode = (EncodingMode)value;
                return true;
            }
            else
            {
                mode = EncodingMode.Byte;
                return false;
            }
        }

        internal static EncodingMode Parse(int value)
        {
            if (Enum.IsDefined(typeof(MessageMode.EncodingMode), value))
            {
                return (EncodingMode)value;
            }
            else
            {
                throw new ArgumentException($"{value} is not a valid Message Mode.");
            }
        }

        internal static uint GetCharacterCountIndicatorLength(QRCodeVersion version, EncodingMode mode)
        {
            switch (mode)
            {
                case EncodingMode.Byte:
                    return version.VersionNumber < 10u ? 8u : 16u;
                case EncodingMode.Alphanumeric:
                    {
                        if (version.VersionNumber < 10u)
                            return 9u;
                        else if (version.VersionNumber < 27u)
                            return 11u;
                        else
                            return 13u;
                    }
                case EncodingMode.Kanji:
                    {
                        if (version.VersionNumber < 10u)
                            return 8u;
                        else if (version.VersionNumber < 27u)
                            return 10u;
                        else
                            return 12u;
                    }
                case EncodingMode.Numeric:
                    {
                        if (version.VersionNumber < 10u)
                            return 10u;
                        else if (version.VersionNumber < 27u)
                            return 12u;
                        else
                            return 14u;
                    }
                case EncodingMode.Terminator:
                    return 0;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
