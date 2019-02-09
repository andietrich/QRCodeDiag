using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZXing.QrCode.Internal;

namespace QRCodeDiag.MetaInfo
{
    internal class QRCodeVersion
    {
        private const int BASESIZE = 21; // size for version 1 code. +4 for each higher version
        public uint VersionNumber { get; private set; }

        public QRCodeVersion(uint versionNumber)
        {
            if (versionNumber < 1 || versionNumber > 40)
                throw new ArgumentOutOfRangeException("versionNumber", "Version number must be in range 1 to 40");
            this.VersionNumber = versionNumber;
        }
        public static int GetVersionFromSize(int codeElCount)
        {
            int v = codeElCount - BASESIZE;
            if (v % 4 != 0)
            {
                throw new ArgumentException("Not a valid codeEl count", "codeElCount");
            }
            return 1 + (v / 4);
        }

        public static int GetEdgeSizeFromVersion(int version)
        {
            return BASESIZE + 4 * (version - 1);
        }
    }
}
