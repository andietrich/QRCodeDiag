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
    public class CodeSymbolCode<T> : ICodeSymbolCode where T : ICodeSymbol
    {
        protected List<ICodeSymbol> codeSymbolList;
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


        #region Internal Functions
        internal static CodeSymbolCode<T2> CreateInstance<T2>(IBitIterator it, ICodeSymbolFactory<T2> symbolFactory) where T2 : IBuildableCodeSymbol
        {
            var codeSymbolList = new List<T2>();
            var sym = symbolFactory.GenerateCodeSymbol();
            char c = it.NextBit();

            while (c != 'e')
            {
                if (c != '0' && c != '1' && c != 'u')
                    throw new NotImplementedException("Bit value " + c + " was not defined.");

                sym.AddBit(c, it.Position);

                if (sym.IsComplete)
                {
                    codeSymbolList.Add(sym);
                    sym = symbolFactory.GenerateCodeSymbol();
                }

                c = it.NextBit();
            }

            //ToDo handle remainder bits or error: if(!wd.IsComplete){ if(wd.CurrentSymbolLength > 0) or: if(wd.CurrentSymbolLength != number of remainder bits) for QRCodeBitIterator

            return new CodeSymbolCode<T2>(codeSymbolList);
        }

        internal CodeSymbolCode(IReadOnlyList<T> codeSymbols)
        {
            this.codeSymbolList = new List<ICodeSymbol>();

            for (int i = 0; i < codeSymbols.Count; i++)
            {
                this.codeSymbolList.Add(codeSymbols[i]);
            }
        }

        internal CodeSymbolCode(IReadOnlyList<CodeSymbolCode<T>> codeSymbolCodes)
        {
            this.codeSymbolList = new List<ICodeSymbol>();

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
        internal CodeSymbolCode<T2> ToCodeSymbolCode<T2>(uint startIndex, uint length, ICodeSymbolFactory<T2> codeSymbolFactory) where T2 : IBuildableCodeSymbol
        {
            return CreateInstance(this.GetBitIterator(startIndex, length), codeSymbolFactory);
        }
        #endregion
        #region Public Functions
        public List<ICodeSymbol> GetCodeSymbols()
        {
            return this.codeSymbolList;
        }
        public T GetSymbolAt(int index)
        {
            if (index < 0 || index > this.codeSymbolList.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            else
                return (T)this.codeSymbolList[index];
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
        public override string ToString()
        {
            return String.Join("", this.codeSymbolList.Select(s => s.ToString()));
        }
        #endregion
    }
}
