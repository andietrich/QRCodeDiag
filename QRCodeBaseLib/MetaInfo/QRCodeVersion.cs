using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZXing.QrCode.Internal;

namespace QRCodeBaseLib.MetaInfo
{
    public class QRCodeVersion
    {
        private const uint BASESIZE = 21; // size for version 1 code. +4 for each higher version
        public uint VersionNumber { get; private set; }

        public QRCodeVersion(uint versionNumber)
        {
            if (versionNumber < 1 || versionNumber > 40)
                throw new ArgumentOutOfRangeException("versionNumber", "Version number must be in range 1 to 40");
            this.VersionNumber = versionNumber;
        }

        public uint GetEdgeSizeFromVersion()
        {
            return BASESIZE + 4u * (this.VersionNumber - 1u);
        }

        public static QRCodeVersion GetVersionFromSize(uint codeElCount)
        {
            uint v = codeElCount - BASESIZE;
            if ((v % 4 != 0) || (codeElCount < BASESIZE))
            {
                throw new ArgumentException("Not a valid codeEl count", "codeElCount");
            }
            return new QRCodeVersion(1u + (v / 4u));
        }
    }
}
