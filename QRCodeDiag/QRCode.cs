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
    internal class QRCode
    {
        internal enum MessageMode
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            Kanji = 8,
            ECI = 7
        }
        internal enum MaskType
        {
            None,
            Mask000 = 0,
            Mask001 = 1,
            Mask010 = 2,
            Mask011 = 3,
            Mask100 = 4,
            Mask101 = 5,
            Mask110 = 6,
            Mask111 = 7
        }

        public const int VERSIONSIZE = 29; //ToDo adjust for other versions
        public const int VERSION = 3;
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

        /// <summary>
        /// Generates an empty QRCode of the specified <paramref name="version"/>
        /// </summary>
        /// <param name="version">The version defines the size of the QR Code. Valid versions are 1-40</param>
        public QRCode(int version)
        {
            if(version == 3)
            {
                this.bits = new char[VERSIONSIZE, VERSIONSIZE];
                for(int x=0; x < this.bits.GetLength(0); x++)
                {
                    for(int y = 0; y < this.bits.GetLength(0); y++)
                    {
                        this.bits[x, y] = 'u';
                    }
                }
                this.PlaceStaticElements();
            }
            else
            {
                throw new NotImplementedException("Only version 3 is implemented so far."); //ToDo implement other versions
            }
        }

        public QRCode(string path) : this(GenerateBitsFromFile(path))
        { }

        private void PlaceStaticElements()
        {
            char[,] finderPattern = new char[,] 
            {
                { '1', '1', '1', '1', '1', '1', '1'},
                { '1', '0', '0', '0', '0', '0', '1'},
                { '1', '0', '1', '1', '1', '0', '1'},
                { '1', '0', '1', '1', '1', '0', '1'},
                { '1', '0', '1', '1', '1', '0', '1'},
                { '1', '0', '0', '0', '0', '0', '1'},
                { '1', '1', '1', '1', '1', '1', '1'}
            };
            // Place Finder Patterns
            for(int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    this.bits[x, y] = finderPattern[x, y]; // Top Left
                    this.bits[VERSIONSIZE - 1 - x, y] = finderPattern[x, y]; // Top Right
                    this.bits[x, VERSIONSIZE - 1 - y] = finderPattern[x, y]; // Bottom Left
                }
            }
            // Place separators
            for(int i = 0; i < 8; i++)
            {
                // Top Left
                this.bits[7, i] = '0';
                this.bits[i, 7] = '0';
                // Top Right
                this.bits[VERSIONSIZE - 1 - i, 7] = '0';
                this.bits[VERSIONSIZE - 8, i] = '0';
                // Bottom Left
                this.bits[7, VERSIONSIZE - 1 - i] = '0';
                this.bits[i, VERSIONSIZE - 8] = '0';
            }
            // Place Alignment Patterns
            char[,] alignmentPattern = new char[,]
            {
                { '1', '1', '1', '1', '1', },
                { '1', '0', '0', '0', '1', },
                { '1', '0', '1', '0', '1', },
                { '1', '0', '0', '0', '1', },
                { '1', '1', '1', '1', '1', }
            };
            // Version 3 Center Module: 22, 22 // ToDo: Implement for all versions
            for(int x = 0; x < alignmentPattern.GetLength(0); x++)
            {
                for (int y = 0; y < alignmentPattern.GetLength(1); y++)
                {
                    this.bits[x + 20, y + 20] = alignmentPattern[x, y];
                }
            }
            // Place timing
            for(int i = 6; i < VERSIONSIZE-7; i++)
            {
                this.bits[6, i] = i % 2 == 0 ? '1' : '0';
                this.bits[i, 6] = i % 2 == 0 ? '1' : '0';
            }
            // Place dark module
            this.bits[8, (4 * VERSION) + 9] = '1';
            // ToDo: Place version information where needed
        }

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
                        cells.Add(f.ReadLine().Trim().Split(null));
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
            return this.terminator?.BitString ?? "No terminator found.";
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
        public string RepairMessage() //ToDo move to FullCode
        {
            var fullCode = new FullCode<RawCodeByte>(this.GetBitIterator());
            var codeBytes = fullCode.ToByteArray();
            var codeAsInts = new int[codeBytes.Length];
            for (int i = 0; i < codeBytes.Length; i++)
            {
                codeAsInts[i] = codeBytes[i] & 0xFF; //ToDo remaining zeros in int ignored by decoder? - using as in zxing example app
            }
            var rsDecoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
            if (rsDecoder.decode(codeAsInts, ECCWORDS))
            {
                var binarySB = new StringBuilder();
                for (int i = 0; i < codeAsInts.Length; i++) // also repair the ecc bytes, the goal is to completely restore the broken qr code
                {
                    binarySB.Append(Convert.ToString((byte)codeAsInts[i], 2).PadLeft(8, '0'));
                }
                return fullCode.GetBitString() + Environment.NewLine + binarySB.ToString();
            }
            else
            {
                return "Could not repair";
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

        public static QRCode GetMask(MaskType mtype)
        {
            switch (mtype)
            {
                case MaskType.Mask000:
                    return GetMask000();
                case MaskType.Mask001:
                    return GetMask001();
                case MaskType.Mask010:
                    return GetMask010();
                case MaskType.Mask011:
                    return GetMask011();
                case MaskType.Mask100:
                    return GetMask100();
                case MaskType.Mask101:
                    return GetMask101();
                case MaskType.Mask110:
                    return GetMask110();
                case MaskType.Mask111:
                    return GetMask111();
                default:
                    throw new ArgumentException("Cannot get null mask.");
            }
        }
        public static QRCode GetMask000()
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
                for (int x = 0; x < VERSIONSIZE; x++)
                    mask[x, y] = ((x + y) % 2 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        public static QRCode GetMask001() // (row) mod 2 == 0
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
                for (int x = 0; x < VERSIONSIZE; x++)
                    mask[x, y] = (y % 2 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        public static QRCode GetMask010()
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
                for (int x = 0; x < VERSIONSIZE; x++)
                    mask[x, y] = (x % 3 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        public static QRCode GetMask011()
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
                for (int x = 0; x < VERSIONSIZE; x++)
                    mask[x, y] = ((x + y) % 3 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        public static QRCode GetMask100() // ( floor(row / 2) + floor(column / 3) ) mod 2 == 0
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
                for (int x = 0; x < VERSIONSIZE; x++)
                    mask[x, y] = (((y / 2 + x / 3) % 2) == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        public static QRCode GetMask101()
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
                for (int x = 0; x < VERSIONSIZE; x++)
                    mask[x, y] = ((((y * x) % 2) + ((y * x) % 3)) == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        public static QRCode GetMask110()
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
                for (int x = 0; x < VERSIONSIZE; x++)
                    mask[x, y] = ((((y * x) % 2) + ((y * x) % 3)) % 2 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        public static QRCode GetMask111() //( ((row + column) mod 2) + ((row * column) mod 3) ) mod 2 == 0
        {
            var mask = new char[VERSIONSIZE, VERSIONSIZE];
            for (int y = 0; y < VERSIONSIZE; y++)
                for (int x = 0; x < VERSIONSIZE; x++)
                    mask[x, y] = ((((y + x) % 2) + ((y * x) % 3)) % 2 == 0) ? '1' : '0';
            return new QRCode(mask);
        }
        public void DrawRawByteLocations(Graphics g, Size size, bool drawBitIndices, bool drawByteIndices)
        {
            this.rawCode?.DrawCode(g, size, Color.Orange, Color.Cyan, drawBitIndices, drawByteIndices);
        }
        public void DrawEncodedData(Graphics g, Size size, bool drawBitIndices, bool drawSymbolIndices)
        {
            this.encodedSymbols?.DrawCode(g, size, Color.Red, Color.LightBlue, drawBitIndices, drawSymbolIndices);
        }
        public void DrawPadding(Graphics g, Size size, bool drawBitIndices, bool drawSymbolIndices)
        {   
            this.paddingBits?.DrawCode(g, size, Color.Blue, Color.LightBlue, drawBitIndices, drawSymbolIndices);
        }
        public void DrawTerminator(Graphics g, Size size, bool drawBitIndices)
        {
            this.terminator?.DrawSymbol(g, size, Color.Purple, drawBitIndices);
        }
        public void DrawCode(Graphics g, Size size, bool transparent = false)
        {
            byte alpha = transparent ? (byte)128 : (byte)255;

            float pixelWidth = (float)size.Width / VERSIONSIZE;
            float pixelHeight = (float)size.Height / VERSIONSIZE;

            var blackBrush = new SolidBrush(Color.FromArgb(alpha, Color.Black.R, Color.Black.G, Color.Black.B));
            var whiteBrush = new SolidBrush(Color.FromArgb(alpha, Color.White.R, Color.White.G, Color.White.B));
            var grayBrush = new SolidBrush(Color.FromArgb(alpha, Color.Gray.R, Color.Gray.G, Color.Gray.B));

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
