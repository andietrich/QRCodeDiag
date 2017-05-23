using QRCodeDiag.DataBlocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common.ReedSolomon;

namespace QRCodeDiag
{
    public class QRCode
    {
        internal enum MessageMode
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            Kanji = 8,
            ECI = 7
        }
        public const int VERSIONSIZE = 29; //ToDo adjust for other versions
        public const int DATAWORDS = 55;//ToDo adjust for other versions
        public const int ECCWORDS = 15;//ToDo adjust for other versions

        private char[,] bits; //ToDo consider BitArray class, at least where no unknown values appear
        private string message;
        private bool messageChanged;
        private FullCode<RawCodeByte> rawCode;
        private FullCode<RawCodeByte> paddingBits;
        private FullCode<ByteEncodingSymbol> encodedSymbols; //ToDo generalize encoding
        private TerminatorSymbol terminator;

        public int Version { get; private set; }
        public string Message
        {
            get
            {
                if (this.messageChanged)
                {
                    this.message = this.ReadMessage();
                }
                return this.message;
            }
        }
        public QRCode(char[,] setBits)
        {
            if(setBits.GetLength(0) != VERSIONSIZE ||setBits.GetLength(1) != VERSIONSIZE)
                throw new ArgumentException("Bad QR Code size", "setBits");

            this.bits = setBits;
            this.Version = 3; // ToDo implement other versions
            this.messageChanged = true;
        }

        public QRCode(string path) : this(GenerateBitsFromFile(path))
        { }

        public void SaveToFile(string path)
        {
            var sb = new StringBuilder();
            for (int y = 0; y < this.bits.GetLength(0); y++)
            {
                for (int x = 0; x < this.bits.GetLength(1)-1; x++)
                {
                    sb.Append(this.bits[x, y]);
                    sb.Append("\t");
                }
                sb.Append(this.bits[this.bits.GetLength(1) - 1, y]);
                sb.Append(Environment.NewLine);
            }
            File.WriteAllText(path, sb.ToString());
        }
        private static char[,] GenerateBitsFromFile(string path)
        {
            var bitMask = new char[QRCode.VERSIONSIZE, QRCode.VERSIONSIZE];
            List<string[]> cells = new List<string[]>();
            try
            {
                using (var f = File.OpenText(path))
                {
                    while (!f.EndOfStream)
                    {
                        cells.Add(f.ReadLine().Split(null));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new QRCodeFormatException("Can't read file.", ex);
            }
            bool valid = true;
            if (cells.Count != QRCode.VERSIONSIZE) // column length OK?
            {
                valid = false;
            }
            else
            {
                for (int y = 0; y < QRCode.VERSIONSIZE && valid; y++)
                {
                    if (cells[y].Length != QRCode.VERSIONSIZE) // row length OK?
                    {
                        valid = false;
                    }
                    else
                    {
                        for (int x = 0; x < QRCode.VERSIONSIZE && valid; x++)
                        {
                            if (cells[y][x].Length != 1)  // string in cell = 1 char length?
                            {
                                valid = false;
                            }
                            else
                            {
                                bitMask[x, y] = cells[y][x][0];
                            }
                        }
                    }
                }
            }
            if (!valid)
                throw new QRCodeFormatException("QR Code Size is wrong.");
            else
                return bitMask;
        }

        public char[,] GetBits()
        {
            return (char[,])this.bits.Clone();
        }

        public string GetTerminator()
        {
            return this.terminator.BitString;
        }

        public string[] GetPaddingBits() //ToDo: If message changed read again
        {
            if (this.paddingBits != null)
            {
                var pbs = this.paddingBits.ToByteArray();
                string[] ret = new string[pbs.Length];
                for(int i = 0; i < pbs.Length; i++)
                {
                    ret[i] = Convert.ToString(pbs[i], 2).PadLeft(8, '0');
                }
                return ret;
            }
            else//throw new InvalidOperationException("Padding bits have not been initialized yet.");
            {
                return new string[] { "Padding bits have not been initialized yet." };
            }
        }

        public void ToggleDataCell(int x, int y)
        {
            if (IsDataCell(x, y))
            {
                switch (bits[x, y])
                {
                    case '0':
                        bits[x, y] = '1';
                        break;
                    case '1':
                        bits[x, y] = 'u';
                        break;
                    case 'u':
                        bits[x, y] = '0';
                        break;
                    default:
                        break;
                }
                this.messageChanged = true;
            }
        }

        private QRCodeBitIterator GetBitIterator()
        {
            return new QRCodeBitIterator(this.bits);
        }

        public static bool IsDataCell(int x, int y)
        {
            bool ret = true;
            if (x > VERSIONSIZE || y > VERSIONSIZE || x < 0 || y < 0)
                return false;

            if (x < 9)
            {
                if (y < 9 || y > 20) // Left side position elements
                    ret = false;
            }
            else if (x > 20 && y < 9) // Right side position element
            {
                ret = false;
            }
            if (x > 19 && x < 25 && y > 19 && y < 25) // Alignment
            {
                ret = false;
            }
            if (y == 6 || x == 6) // Timing
            {
                ret = false;
            }

            return ret;
        }
        private static int GetCharacterCountIndicatorLength(int version, MessageMode mode)
        {
            switch (mode)
            {
                case MessageMode.Byte:
                    return version < 10 ? 8 : 16;
                case MessageMode.Alphanumeric:
                    {
                        if (version < 10)
                            return 9;
                        else if (version < 27)
                            return 11;
                        else
                            return 13;
                    }
                case MessageMode.Kanji:
                    {
                        if (version < 10)
                            return 8;
                        else if (version < 27)
                            return 10;
                        else
                            return 12;
                    }
                case MessageMode.Numeric:
                    {
                        if (version < 10)
                            return 10;
                        else if (version < 27)
                            return 12;
                        else
                        return 14;
                    }
                case MessageMode.ECI:
                default:
                    throw new NotImplementedException();
            }
        }
        private static int GetCharacterLength(MessageMode mode)
        {
            switch (mode)
            {
                case MessageMode.Byte:
                    return 8;
                default:
                    throw new NotImplementedException(); //ToDo split combined characters, find out final chunks' character count
            }
        }
        private string RepairMessage(string[] byteStrings, char defaultBit = '0') //ToDo move to FullCode
        {
            var codeAsInts = new int[byteStrings.Length];
            for (int i = 0; i < byteStrings.Length; i++)
            {
                var byteString = byteStrings[i].ToCharArray();
                for (int j = 0; j < byteString.Length; j++)
                {
                    if (byteString[j] != '0' && byteString[j] != '1')
                        byteString[j] = defaultBit;
                }
                codeAsInts[i] = Convert.ToInt32(new String(byteString), 2);
            }
            var rsDecoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
            if (rsDecoder.decode(codeAsInts, ECCWORDS))
            {
                var binarySB = new StringBuilder();
                for (int i = 0; i < DATAWORDS; i++)
                {
                    binarySB.Append(Convert.ToString((byte)codeAsInts[i], 2)); //FIXME ToDo: probably bit order reversed
                }
                return binarySB.ToString();
            }
            else
            {
                return null;
            }
        }
        private string ReadMessage() //ToDo length check of messageBytes, 
        {
            //TODO: Fix this.RepairMessage(binaryBlocks) ?? string.Join("", binaryBlocks, 0, DATAWORDS); // transform RawByteList to byte[] using RawCodeByte.GetAsByte() ?
            this.rawCode = new FullCode<RawCodeByte>(this.GetBitIterator());

            int modeNibble;
            try
            {
                modeNibble = Convert.ToInt32(this.rawCode.GetBitString(0, 4), 2);
            }
            catch (FormatException fe)
            {
                throw new QRCodeFormatException("Mode indicator nibble could not be decoded: " + this.rawCode.GetBitString(0, 4), fe);
            }
            if (Enum.IsDefined(typeof(MessageMode), modeNibble))
            {
                var messageMode = (MessageMode)modeNibble;
                var charIndicatorLength = GetCharacterCountIndicatorLength(this.Version, messageMode);
                int characterCount;
                try
                {
                    characterCount = Convert.ToInt32(this.rawCode.GetBitString(4, charIndicatorLength), 2); //ToDo check if characterCount is in range
                }
                catch (FormatException fe)
                {
                    throw new QRCodeFormatException("Could not parse character count.", fe); //ToDo continue with max possible character count
                }
                if (messageMode == MessageMode.Byte)
                {
                    var encodedCharacterLength = GetCharacterLength(messageMode);
                    var firstSymbolOffset = 4 + charIndicatorLength;
                    var messageLenghtInBits = characterCount * encodedCharacterLength;
                    var messageEndOffset = messageLenghtInBits + firstSymbolOffset;
                    var terminatorLength = 4; // Always 4 for MessageMode.Byte, no incomplete padding bytes for this mode
                    var terminatorLocation = new Vector2D[terminatorLength];
                    for (int i = 0, bitNumber = messageEndOffset; i < terminatorLength; i++, bitNumber++)
                    {
                        terminatorLocation[i] = this.rawCode.GetBitPosition(bitNumber);
                    }
                    this.encodedSymbols = this.rawCode.ToFullCode<ByteEncodingSymbol>(firstSymbolOffset, messageLenghtInBits);
                    this.terminator = new TerminatorSymbol(this.rawCode.GetBitString(messageEndOffset, terminatorLength), terminatorLocation);
                    this.paddingBits = this.rawCode.ToFullCode<RawCodeByte>(messageEndOffset + terminatorLength, DATAWORDS * 8 - (messageEndOffset + terminatorLength));
                    return encodedSymbols.DecodeSymbols('_', Encoding.GetEncoding("iso-8859-1"));
                }
                else
                {
                    throw new NotImplementedException("Other encodings are not implemented yet."); //ToDo implement reading other encodings
                }
            }
            else
            {
                throw new QRCodeFormatException("Mode indicator nibble could not be decoded: " + this.rawCode.GetBitString(0, 4));
            }
        }
        public static QRCode XOR(QRCode lhs, QRCode rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if(rhs == null)
                throw new ArgumentNullException("rhs");

            var xored = new char[VERSIONSIZE, VERSIONSIZE];
            var lhsBits = lhs.GetBits();
            var rhsBits = rhs.GetBits();
            for(int y = 0; y < VERSIONSIZE; y++)
            {
                for (int x = 0; x < VERSIONSIZE; x++)
                {
                    var lhsbit = lhsBits[x, y];
                    var rhsbit = rhsBits[x, y];

                    if (lhsbit != '0' && lhsbit != '1')
                    {
                        xored[x, y] = lhsbit;
                    }
                    else if (rhsbit != '0' && rhsbit != '1')
                    {
                        xored[x, y] = rhsbit;
                    }
                    else
                    {
                        int ones = 0;
                        if (lhsBits[x, y] == '1')
                            ones++;
                        if (rhsBits[x, y] == '1')
                            ones++;
                        xored[x, y] = (ones == 1) ? '1' : '0';
                    }
                }
            }
            return new QRCode(xored);
        }

        /*
         * Mask Number 	If the formula below is true for a given row/column coordinate, switch the bit at that coordinate
         * 0 	(row + column) mod 2 == 0
         * 1 	(row) mod 2 == 0
         * 2 	(column) mod 3 == 0
         * 3 	(row + column) mod 3 == 0
         * 4 	( floor(row / 2) + floor(column / 3) ) mod 2 == 0
         * 5 	((row * column) mod 2) + ((row * column) mod 3) == 0
         * 6 	( ((row * column) mod 2) + ((row * column) mod 3) ) mod 2 == 0
         * 7 	( ((row + column) mod 2) + ((row * column) mod 3) ) mod 2 == 0
         * 
         *  row = y, column = x
         * */
        public static QRCode GetMask100() // ( floor(row / 2) + floor(column / 3) ) mod 2 == 0
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
            {
                for (int x = 0; x < VERSIONSIZE; x++)
                {
                    if (((y / 2 + x / 3) % 2) == 0)
                        mask[x, y] = '1';
                    else
                        mask[x, y] = '0';
                }
            }
            return new QRCode(mask);
        }

        public static QRCode GetMask001() // (row) mod 2 == 0
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y ++)
            {
                for (int x = 0; x < VERSIONSIZE; x++)
                {
                    if((y % 2) == 0)
                        mask[x, y] = '1';
                    else
                        mask[x, y] = '0';
                }
            }
            return new QRCode(mask);
        }

        public static QRCode GetMask111() //TODO ( ((row + column) mod 2) + ((row * column) mod 3) ) mod 2 == 0
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
            {
                for (int x = 0; x < VERSIONSIZE; x++)
                {
                    if ((((y + x) % 2) + ((y * x) % 3)) % 2 == 0)
                        mask[x, y] = '1';
                    else
                        mask[x, y] = '0';
                }
            }
            return new QRCode(mask);
        }
        public void DrawRawByteLocations(Graphics g, Size size, bool drawBitIndices, bool drawByteIndices)
        {
            this.rawCode?.DrawCode(g, size, Color.Orange, Color.Cyan, drawBitIndices, drawByteIndices);
        }
        public void DrawData(Graphics g, Size size, bool drawBitIndices, bool drawSymbolIndices)
        {
            this.encodedSymbols?.DrawCode(g, size, Color.Red, Color.LightBlue, drawBitIndices, drawSymbolIndices);
            this.terminator?.DrawSymbol(g, size, Color.Purple, drawBitIndices);
            this.paddingBits?.DrawCode(g, size, Color.Blue, Color.LightBlue, drawBitIndices, drawSymbolIndices);
        }
        public void DrawCode(Graphics g, Size size)
        {
            float pixelWidth = (float)size.Width / VERSIONSIZE;
            float pixelHeight = (float)size.Height / VERSIONSIZE;

            var blackBrush = new SolidBrush(Color.Black);
            var whiteBrush = new SolidBrush(Color.White);
            var grayBrush = new SolidBrush(Color.Gray);

            for (int y = 0; y < VERSIONSIZE; y++)
            {
                for (int x = 0; x < VERSIONSIZE; x++)
                {
                    SolidBrush b;
                    switch (this.bits[x,y])
                    {
                        case '0':
                        case 'w':
                        case 's':
                            b = whiteBrush;
                            break;
                        case '1':
                        case 'b':
                            b = blackBrush;
                            break;
                        default:
                            b = grayBrush;
                            break;
                            //throw new QRCodeFormatException("Invalid pixel value: " + this.bits[x, y]);
                    }
                    g.FillRectangle(b, x * pixelWidth, y * pixelHeight, pixelWidth, pixelHeight);
                }
            }
        }

        public void PrintBlocks()
        {
            Console.WriteLine(this.Message);
        }

        //public void AnalyzeCode()
        //{
        //    //Determine Size/Version
        //    //GetFormatBits
        //    //ReadFormatBits //Mask, ECC level
        //    //XORWithMask
        //    //ReadModeIndicator
        //    //ReadCharacterCountIndicator

        //    //Byte encoding uses ISO 8859-1
        //}
    }
}
