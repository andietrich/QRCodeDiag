using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.MetaInfo
{
    internal class MessageMode
    {
        public enum Mode
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            ECI = 7,    // extended channel interpretation
            Kanji = 8
        }

        public static bool TryParse(int value, out Mode mode)
        {
            if (Enum.IsDefined(typeof(MessageMode.Mode), value))
            {
                mode = (Mode)value;
                return true;
            }
            else
            {
                mode = Mode.Byte;
                return false;
            }
        }

        public static int GetCharacterCountIndicatorLength(QRCodeVersion version, Mode mode)
        {
            switch (mode)
            {
                case Mode.Byte:
                    return version.VersionNumber < 10 ? 8 : 16;
                case Mode.Alphanumeric:
                    {
                        if (version.VersionNumber < 10)
                            return 9;
                        else if (version.VersionNumber < 27)
                            return 11;
                        else
                            return 13;
                    }
                case Mode.Kanji:
                    {
                        if (version.VersionNumber < 10)
                            return 8;
                        else if (version.VersionNumber < 27)
                            return 10;
                        else
                            return 12;
                    }
                case Mode.Numeric:
                    {
                        if (version.VersionNumber < 10)
                            return 10;
                        else if (version.VersionNumber < 27)
                            return 12;
                        else
                            return 14;
                    }
                case Mode.ECI:
                default:
                    throw new NotImplementedException();
            }
        }


        public static int GetCharacterLength(Mode mode)
        {
            switch (mode)
            {
                case Mode.Byte:
                    return 8;
                default:
                    throw new NotImplementedException(); //ToDo split combined characters, find out final chunks' character count
            }
        }
    }
}
