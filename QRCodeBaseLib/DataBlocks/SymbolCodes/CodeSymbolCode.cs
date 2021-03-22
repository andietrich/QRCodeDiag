using QRCodeBaseLib.DataBlocks.Symbols;
using QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.DataBlocks.SymbolCodes
{
    public class CodeSymbolCode<T> : ICodeSymbolCode where T : CodeSymbol
    {
        protected List<CodeSymbol> codeSymbolList;
        public int SymbolCount => this.codeSymbolList.Count;
        public uint BitCount
        {
            get
            {
                uint count = 0;

                foreach(var codeSymbol in this.codeSymbolList)
                {
                    count += codeSymbol.CurrentSymbolLength;
                }

                return count;
            }
        }

        /// <summary>
        /// Iterates through all bits of <see cref="IBitIterator"/> <paramref name="it"/> creating a list of type <typeparamref name="T"/> <see cref="ByteSymbol"/>s.
        /// </summary>
        /// <param name="it"><see cref="IBitIterator"/> to iterate through all bits the <see cref="CodeSymbolCode{T}"/> will be composed of.</param>
        internal CodeSymbolCode(IBitIterator it, ICodeSymbolFactory<T> symbolFactory)
        {
            this.codeSymbolList = new List<CodeSymbol>();

            var wd = symbolFactory.GenerateCodeSymbol();
            char c = it.NextBit();

            while(c != 'e')
            {
                if (c != '0' && c != '1' && c != 'u')
                    throw new NotImplementedException("Bit value " + c + " was not defined.");

                wd.AddBit(c, it.Position);

                if (wd.IsComplete)
                {
                    codeSymbolList.Add(wd);
                    wd = symbolFactory.GenerateCodeSymbol();
                }

                c = it.NextBit();
            }

            //ToDo handle remainder bits or error: if(!wd.IsComplete){ if(wd.CurrentSymbolLength > 0) or: if(wd.CurrentSymbolLength != number of remainder bits) for QRCodeBitIterator
        }

        public CodeSymbolCode(List<T> codeSymbols)
        {
            this.codeSymbolList = new List<CodeSymbol>();

            for (int i = 0; i < codeSymbols.Count; i++)
            {
                this.codeSymbolList.Add(codeSymbols[i]);
            }
        }

        public CodeSymbolCode(List<CodeSymbolCode<T>> codeSymbolCodes)
        {
            this.codeSymbolList = new List<CodeSymbol>();

            for (int i = 0; i < codeSymbolCodes.Count; i++)
            {
                this.codeSymbolList.AddRange(codeSymbolCodes[i].GetCodeSymbols());
            }
        }

        internal IBitIterator GetBitIterator()
        {
            return new CodeSymbolCodeBitIterator(this);
        }
        internal IBitIterator GetBitIterator(uint startIndex, uint length)
        {
            return new CodeSymbolCodeBitIterator(this, startIndex, length);
        }
        public List<CodeSymbol> GetCodeSymbols()
        {
            return this.codeSymbolList;
        }
        public T GetSymbolAt(int index)
        {
            if (index < 0 || index > this.codeSymbolList.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            else
                return this.codeSymbolList[index] as T;
        }
        public string GetBitString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < this.codeSymbolList.Count; i++)
            {
                sb.Append(this.codeSymbolList[i].BitString);
            }
            return sb.ToString();
        }
        public string GetBitString(uint startIndex, uint length)
        {
            return this.GetBitString().Substring((int)startIndex, (int)length);
        }
        public string[] GetSymbolBitStrings()
        {
            var symbolBitStrings = new string[this.SymbolCount];
            for(int i = 0; i < this.SymbolCount; i++)
            {
                symbolBitStrings[i] = this.codeSymbolList[i].BitString;
            }
            return symbolBitStrings;
        }
        public Vector2D GetBitPosition(uint bitNumber)
        {
            uint bitsFound = 0;

            for(int i = 0; i < this.codeSymbolList.Count; i++)
            {
                uint bitsInSymbol = this.codeSymbolList[i].CurrentSymbolLength;

                if (bitNumber < bitsFound + bitsInSymbol)
                    return this.codeSymbolList[i].GetBitCoordinate(bitNumber - bitsFound);
                else
                    bitsFound += bitsInSymbol;
            }

            throw new ArgumentOutOfRangeException();
        }
        internal CodeSymbolCode<T2> ToCodeSymbolCode<T2>(uint startIndex, uint length, ICodeSymbolFactory<T2> codeSymbolFactory) where T2 : CodeSymbol
        {
            return new CodeSymbolCode<T2>(this.GetBitIterator(startIndex, length), codeSymbolFactory);
        }
        public override string ToString()
        {
            return String.Join("", this.codeSymbolList.Select(s => s.ToString()));
        }
    }
}
