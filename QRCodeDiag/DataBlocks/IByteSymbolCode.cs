using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.DataBlocks
{
    interface IByteSymbolCode
    {
        string GetBitString();
        string GetBitString(int startIndex, int length);
        Vector2D GetBitPosition(int bitNumber);
    }
}
