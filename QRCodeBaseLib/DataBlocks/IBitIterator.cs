using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks
{
    internal interface IBitIterator //ToDo implement iterators for RawCode and IEncodingSymbol
    {
        /// <summary>
        /// Advances one bit and returns the new bit. If the end has already been reached 'e' is returned
        /// </summary>
        /// <returns></returns>
        char NextBit();
        bool EndReached { get; }
        /// <summary>
        /// The char at the current iterator position, 'e' if the end has already been reached '\0' initial
        /// </summary>
        char CurrentChar { get; }
        Vector2D Position { get; }
        uint BitsConsumed { get; }
    }
}
