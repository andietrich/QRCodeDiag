using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib
{
    public class XORMask
    {
        public enum MaskType
        {
            Mask000 = 0,
            Mask001 = 1,
            Mask010 = 2,
            Mask011 = 3,
            Mask100 = 4,
            Mask101 = 5,
            Mask110 = 6,
            Mask111 = 7,
            None
        }

        public static char ApplyXOR(MaskType maskType, char codeBit, uint x, uint y)
        {
            if (codeBit != '0' && codeBit != '1')
                return codeBit;

            bool codeBitBool = codeBit == '1';

            if (codeBitBool ^ GetMaskValue(maskType, x, y))
                return '1';
            else
                return '0';
        }

        /*
         * Mask Number 	If the formula below is true for a given row/column coordinate, switch the bit at that coordinate
         * 0 	(row + column) mod 2 == 0
         * 1 	(row) mod 2 == 0
         * 2 	(column) mod 3 == 0
         * 3 	(row + column) mod 3 == 0
         * 4 	( floor(row / 2) + floor(column / 3) ) mod 2 == 0
         * 5 	((row * column) mod 2) + ((row * column) mod 3) == 0
         * 6 	( ((row * column) mod 2) + ((row * column) mod 3) ) mod 2 == 0
         * 7 	( ((row + column) mod 2) + ((row * column) mod 3) ) mod 2 == 0
         * 
         *  row = y, column = x
         * */

        private static bool GetMaskValue(MaskType maskType, uint x, uint y)
        {
            switch (maskType)
            {
                case MaskType.Mask000:
                    return GetMask000Value(x, y);
                case MaskType.Mask001:
                    return GetMask001Value(y);
                case MaskType.Mask010:
                    return GetMask010Value(x);
                case MaskType.Mask011:
                    return GetMask011Value(x, y);
                case MaskType.Mask100:
                    return GetMask100Value(x, y);
                case MaskType.Mask101:
                    return GetMask101Value(x, y);
                case MaskType.Mask110:
                    return GetMask110Value(x, y);
                case MaskType.Mask111:
                    return GetMask111Value(x, y);
                default:
                    return false;
            }
        }
        private static bool GetMask000Value(uint x, uint y)
        {
            return (x + y) % 2 == 0;
        }
        private static bool GetMask001Value(uint y)
        {
            return y % 2 == 0;
        }
        private static bool GetMask010Value(uint x)
        {
            return x % 3 == 0;
        }
        private static bool GetMask011Value(uint x, uint y)
        {
            return (x + y) % 3 == 0;
        }
        private static bool GetMask100Value(uint x, uint y)
        {
            return ((y / 2 + x / 3) % 2) == 0;
        }
        private static bool GetMask101Value(uint x, uint y)
        {
            return (((y * x) % 2) + ((y * x) % 3)) == 0;
        }
        private static bool GetMask110Value(uint x, uint y)
        {
            return (((y * x) % 2) + ((y * x) % 3)) % 2 == 0;
        }
        private static bool GetMask111Value(uint x, uint y)
        {
            return (((y + x) % 2) + ((y * x) % 3)) % 2 == 0;
        }
    }
}
