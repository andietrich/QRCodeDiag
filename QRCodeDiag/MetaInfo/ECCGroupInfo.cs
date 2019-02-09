using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.MetaInfo
{
    class ECCGroupInfo
    {
        public uint DataBytesPerBlock { get; private set; }
        public uint NumberOfBlocks { get; private set; }
        public ECCGroupInfo(uint dataBytesPerBlock, uint numOfBlocks)
        {
            this.DataBytesPerBlock = dataBytesPerBlock;
            this.NumberOfBlocks = numOfBlocks;
        }
    }
}
