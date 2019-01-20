using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.MetaInfo
{
    class ECCGroup
    {
        public uint DataBytesPerBlock { get; private set; }
        public uint NumberOfBlocks { get; private set; }
        public ECCGroup(uint dataBytesPerBlock, uint numOfBlocks)
        {
            this.DataBytesPerBlock = dataBytesPerBlock;
            this.NumberOfBlocks = numOfBlocks;
        }
    }
}
