using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeBaseLib.QRCode;

namespace QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols
{
    internal interface IEncodingSymbol : IBuildableCodeSymbol
    {
        MessageMode.EncodingMode EncodingType { get; }
    }
}
