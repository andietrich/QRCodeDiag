using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeDiag.QRCode;

namespace QRCodeDiag
{
    class XORMask
    {
        public static QRCode XOR(QRCode code, MaskType maskType)
        {
            return XORMask.XOR(code, XORMask.GetMask(maskType, code.Version));
        }

        public static QRCode XOR(QRCode lhs, QRCode rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Version != rhs.Version)
                throw new ArgumentException("Argument version mismatch");

            var lhsBits = lhs.GetBits();
            var rhsBits = rhs.GetBits();
            var edgeLength = lhsBits.GetLength(0);
            var xored = new char[edgeLength, edgeLength];
            for (int y = 0; y < edgeLength; y++)
            {
                for (int x = 0; x < edgeLength; x++)
                {
                    var lhsbit = lhsBits[x, y];
                    var rhsbit = rhsBits[x, y];

                    if (lhsbit != '0' && lhsbit != '1')
                    {
                        xored[x, y] = lhsbit;
                    }
                    else if (rhsbit != '0' && rhsbit != '1')
                    {
                        xored[x, y] = rhsbit;
                    }
                    else
                    {
                        int ones = 0;
                        if (lhsBits[x, y] == '1')
                            ones++;
                        if (rhsBits[x, y] == '1')
                            ones++;
                        xored[x, y] = (ones == 1) ? '1' : '0';
                    }
                }
            }
            return new QRCode(xored);
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

        private static QRCode GetMask(MaskType mtype, int version)
        {
            if (version < 1 || version > 40)
                throw new ArgumentOutOfRangeException("version");

            var versionSize = QRCode.GetEdgeSizeFromVersion(version);
            switch (mtype)
            {
                case MaskType.Mask000:
                    return GetMask000(versionSize);
                case MaskType.Mask001:
                    return GetMask001(versionSize);
                case MaskType.Mask010:
                    return GetMask010(versionSize);
                case MaskType.Mask011:
                    return GetMask011(versionSize);
                case MaskType.Mask100:
                    return GetMask100(versionSize);
                case MaskType.Mask101:
                    return GetMask101(versionSize);
                case MaskType.Mask110:
                    return GetMask110(versionSize);
                case MaskType.Mask111:
                    return GetMask111(versionSize);
                default:
                    return GetEmptyMask(versionSize);
            }
        }
        private static QRCode GetEmptyMask(int versionSize)
        {
            var mask = new char[versionSize, versionSize];
            for (int i = 0; i < mask.Length; i++)
            {
                mask.SetValue('0', i);
            }
            return new QRCode(mask);
        }
        private static QRCode GetMask000(int versionSize)
        {
            var mask = new char[versionSize, versionSize];
            for (int y = 0; y < versionSize; y++)
                for (int x = 0; x < versionSize; x++)
                    mask[x, y] = ((x + y) % 2 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        private static QRCode GetMask001(int versionSize) // (row) mod 2 == 0
        {
            var mask = new char[versionSize, versionSize];
            for (int y = 0; y < versionSize; y++)
                for (int x = 0; x < versionSize; x++)
                    mask[x, y] = (y % 2 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        private static QRCode GetMask010(int versionSize)
        {
            var mask = new char[versionSize, versionSize];
            for (int y = 0; y < versionSize; y++)
                for (int x = 0; x < versionSize; x++)
                    mask[x, y] = (x % 3 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        private static QRCode GetMask011(int versionSize)
        {
            var mask = new char[versionSize, versionSize];
            for (int y = 0; y < versionSize; y++)
                for (int x = 0; x < versionSize; x++)
                    mask[x, y] = ((x + y) % 3 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        private static QRCode GetMask100(int versionSize) // ( floor(row / 2) + floor(column / 3) ) mod 2 == 0
        {
            var mask = new char[versionSize, versionSize];
            for (int y = 0; y < versionSize; y++)
                for (int x = 0; x < versionSize; x++)
                    mask[x, y] = (((y / 2 + x / 3) % 2) == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        private static QRCode GetMask101(int versionSize)
        {
            var mask = new char[versionSize, versionSize];
            for (int y = 0; y < versionSize; y++)
                for (int x = 0; x < versionSize; x++)
                    mask[x, y] = ((((y * x) % 2) + ((y * x) % 3)) == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        private static QRCode GetMask110(int versionSize)
        {
            var mask = new char[versionSize, versionSize];
            for (int y = 0; y < versionSize; y++)
                for (int x = 0; x < versionSize; x++)
                    mask[x, y] = ((((y * x) % 2) + ((y * x) % 3)) % 2 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        private static QRCode GetMask111(int versionSize) //( ((row + column) mod 2) + ((row * column) mod 3) ) mod 2 == 0
        {
            var mask = new char[versionSize, versionSize];
            for (int y = 0; y < versionSize; y++)
                for (int x = 0; x < versionSize; x++)
                    mask[x, y] = ((((y + x) % 2) + ((y * x) % 3)) % 2 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
    }
}
