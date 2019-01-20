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
        private readonly ErrorCorrectionLevel ecLevel;
        public uint VersionNumber { get; private set; }

        public QRCodeVersion(uint versionNumber, ErrorCorrectionLevel.ECCLevel eccLevel)
        {
            if (versionNumber < 1 || versionNumber > 40)
                throw new ArgumentOutOfRangeException("versionNumber", "Version number must be in range 1 to 40");
            this.VersionNumber = versionNumber;

            ErrorCorrectionLevel.GetECCLevel(eccLevel, versionNumber)

            //switch(eccLevel)
            //{
            //    case ECCLevel.
            //}


        }

        public static QRCodeVersion GetVersionAndECCFromBitString()
        {
            throw new NotImplementedException();    //ToDo implement
        }
    }
}
