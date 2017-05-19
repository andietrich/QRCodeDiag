using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag
{
    public class QRCode
    {
        private enum MessageMode
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            Kanji = 8,
            ECI = 7
        }
        public const int SIZE = 29; //ToDo adjust for other versions
        private char[,] bits;
        public int Version { get; private set; }

        public QRCode(char[,] setBits)
        {
            if(setBits.GetLength(0) != SIZE ||setBits.GetLength(1) != SIZE)
                throw new ArgumentException("Bad QR Code size", "setBits");

            this.bits = setBits;
            this.Version = 3; // ToDo implement other versions
        }

        public QRCode(string path) : this(GenerateBitsFromFile(path))
        { }

        private static char[,] GenerateBitsFromFile(string path)
        {
            var bitMask = new char[QRCode.SIZE, QRCode.SIZE];
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
            if (cells.Count != QRCode.SIZE) // column length OK?
            {
                valid = false;
            }
            else
            {
                for (int y = 0; y < QRCode.SIZE && valid; y++)
                {
                    if (cells[y].Length != QRCode.SIZE) // row length OK?
                    {
                        valid = false;
                    }
                    else
                    {
                        for (int x = 0; x < QRCode.SIZE && valid; x++)
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
            return this.bits;
        }

        private BitIterator GetBitIterator()
        {
            return new BitIterator(this.bits);
        }

        public static bool IsDataCell(int x, int y)
        {
            bool ret = true;
            if (x > SIZE || y > SIZE || x < 0 || y < 0)
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

        /// <summary>
        /// Reads blocks of 8 bits in placement order. For larger QR-Codes the bytes have to be reordered to obtain the message and ECC bytes.
        /// After obtaining the bytes apply ECC checking/correction. After ECC read mode indicator, char count indicator, chars.
        /// </summary>
        /// <returns></returns>
        private List<WordDetails> GenerateWordList()
        {
            uint wordLength = 8; // Always read 8 bit blocks
            var wordList = new List<WordDetails>();
            var it = this.GetBitIterator();
            var wd = new WordDetails(wordLength);
            var c = it.CurrentChar;
            wd.AddBit(c, it.XPos, it.YPos);

            while (c != 'e')
            {
                c = it.NextBit();
                if (c == '0' || c == '1' || c == 'u')
                {
                    wd.AddBit(c, it.XPos, it.YPos);
                    DebugDrawingForm.DebugHighlightCell(this, wd);
                    if (wd.IsComplete())
                    {
                        wordList.Add(wd);
                        wd = new WordDetails(wordLength);
                    }
                }
            }
            return wordList;
        }

        private string[][] GenerateBlocks() //ToDo dynamically find ECC and DATA block count and location for all versions
        {
            var dataWords = 55; //For V3-L
            var eccWords = 15; //For V3-L
            var orderedDataAndECC = new string[2][];
            orderedDataAndECC[0] = new string[dataWords]; //Data
            orderedDataAndECC[1] = new string[eccWords]; //ECC
            var byteList = this.GenerateWordList();
            for(int i = 0; i < dataWords; i++)
            {
                orderedDataAndECC[0][i] = byteList[i].DataWord;
            }
            for(int i = 0; i < eccWords; i++)
            {
                orderedDataAndECC[1][i] = byteList[dataWords + i].DataWord;
            }
            return orderedDataAndECC;
            //for (int i = 0; i < 13; i++)
            //{
            //    this.OrderedDataAndECC[0][i] = byteList[2 * i].DataWord;
            //    this.OrderedDataAndECC[0][i + 13] = byteList[2 * i + 1].DataWord;
            //}
            //for (int i = 0; i < 22; i++)
            //{
            //    this.OrderedDataAndECC[1][i] = byteList[2 * i + 26].DataWord;
            //    this.OrderedDataAndECC[1][i + 22] = byteList[2 * i + 1 + 26].DataWord;
            //}
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
                    throw new NotImplementedException(); //ToDo split combined characters, find out final chunks character count
            }
        }
        private static string GetMessageFromCharacters(List<string> characters, MessageMode mode)
        {
            var sb = new StringBuilder(characters.Count);
            var unknownSymbol = '_';
            switch (mode)
            {
                case MessageMode.Byte:
                    {
                        var encoding = Encoding.GetEncoding("iso-8859-1");
                        var byteArr = new byte[characters.Count];
                        for (int i = 0; i < characters.Count; i++)
                        {
                            try
                            {
                                var symbol = Convert.ToByte(characters[i], 2);
                                sb.Append(encoding.GetString(new byte[] { symbol }));
                            }
                            catch(Exception e) when (e is ArgumentException || e is FormatException)
                            {
                                sb.Append(unknownSymbol);
                            }
                        }
                        break;
                    }
                default:
                    throw new NotImplementedException(); //ToDo
            }
            return sb.ToString();
        }
        private string ReadMessage() //ToDo length check of messageBytes, 
        {
            var binaryBlocks = this.GenerateBlocks(); //ToDo: use ECC bytes
            var messageBlob = string.Join("", binaryBlocks[0]);
            int modeNibble;
            try
            {
                modeNibble = Convert.ToInt32(messageBlob.Substring(0, 4), 2);
            }
            catch(FormatException fe)
            {
                throw new QRCodeFormatException("Mode indicator nibble could not be decoded: " + messageBlob.Substring(0, 4), fe);
            }
            if (Enum.IsDefined(typeof(MessageMode), modeNibble))
            {
                var messageMode = (MessageMode)modeNibble;
                var charIndicatorLength = GetCharacterCountIndicatorLength(this.Version, messageMode);
                int characterCount;
                try
                {
                    characterCount = Convert.ToInt32(messageBlob.Substring(4, charIndicatorLength), 2); //ToDo check if characterCount is a valid value
                }
                catch (FormatException fe)
                {
                    throw new QRCodeFormatException("Could not parse character count.", fe);
                }
                var characterList = new List<string>(characterCount);
                var characterLength = GetCharacterLength(messageMode);
                for (int i = 4; i < characterCount*characterLength; i+= characterLength)
                {
                    characterList.Add(messageBlob.Substring(i, characterLength));
                }
                return GetMessageFromCharacters(characterList, messageMode);
            }
            else
            {
                throw new QRCodeFormatException("Mode indicator nibble could not be decoded: " + messageBlob.Substring(0, 4));
            }
        }
        public static QRCode XOR(QRCode lhs, QRCode rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if(rhs == null)
                throw new ArgumentNullException("rhs");

            var xored = new char[SIZE, SIZE];
            var lhsBits = lhs.GetBits();
            var rhsBits = rhs.GetBits();
            for(int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
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
            var mask = new char[SIZE, SIZE];
            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
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
            var mask = new char[SIZE, SIZE];
            for (int y = 0; y < SIZE; y ++)
            {
                for (int x = 0; x < SIZE; x++)
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
            var mask = new char[SIZE, SIZE];
            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    if ((((y + x) % 2) + ((y * x) % 3)) % 2 == 0)
                        mask[x, y] = '1';
                    else
                        mask[x, y] = '0';
                }
            }
            return new QRCode(mask);
        }

        public void DrawCode(Graphics g)
        {
            var width = g.VisibleClipBounds.Size.Width;
            var height = g.VisibleClipBounds.Size.Height;
            var pixelWidth = width / SIZE;
            var pixelHeight = height / SIZE;

            var blackBrush = new SolidBrush(Color.Black);
            var whiteBrush = new SolidBrush(Color.White);
            var grayBrush = new SolidBrush(Color.Gray);

            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
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
            Console.WriteLine(this.ReadMessage());
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
