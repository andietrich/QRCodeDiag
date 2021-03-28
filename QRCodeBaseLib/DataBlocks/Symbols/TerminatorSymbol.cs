using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks.Symbols
{
    internal class TerminatorSymbol : CodeSymbol
    {
        public override bool IsComplete => this.bitCoordinates.Count == 4;
        public TerminatorSymbol() : base()
        { }
    }
}
