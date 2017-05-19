using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    class QRCodeFormatException : Exception
    {
        public QRCodeFormatException(string message) : base(message)
        { }
        public QRCodeFormatException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
