using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.MetaInfo
{
    class InvalidFormatInformationException : QRCodeFormatException
    {
        public InvalidFormatInformationException(string message) : base(message)
        { }
        public InvalidFormatInformationException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
