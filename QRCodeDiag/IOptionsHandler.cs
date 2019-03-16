using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRCodeDiag
{
    public interface IOptionsHandler
    {
        /// <summary>
        /// Adds an IOptionsItem to the IOptionsItem collection
        /// </summary>
        void AddOptionsItem(IOptionsItem item);
    }
}