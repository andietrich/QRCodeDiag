using QRCodeBaseLib.MetaInfo;
using QRCodeBaseLib.DataBlocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QRCodeBaseLib.ECCDecoding;
using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.DataBlocks.Symbols;
using QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols;

namespace QRCodeBaseLib
{
    public class QRCode
    {
        public delegate void VersionChangedHandler(QRCodeVersion newVersion);
        public delegate void ECCLevelChangedHandler(ErrorCorrectionLevel.ECCLevel newECCLevel);
        public delegate void MessageChangedHandler(string newMessage, bool messageIsValid);
        public event VersionChangedHandler VersionChangedEvent;
        public event ECCLevelChangedHandler ECCLevelChangedEvent;
        public event MessageChangedHandler MessageChangedEvent;

        #region private members

        private const int MODEINFOLENGTH = 4; // the message mode information is stored in the first nibble (4 bits)

        private readonly char[,] bits; //ToDo consider BitArray class, at least where no unknown values appear

        private MessageMode.Mode messageMode; // ToDo: set initial value: byte? Mode.Unknown?
        private string message;

        // parsed/drawable code elements
        private CodeSymbolCode<RawCodeByte> rawCode;
        private CodeSymbolCode<RawCodeByte> rawDataBytes;
        private CodeSymbolCode<RawCodeByte> rawECCBytes;
        private CodeSymbolCode<RawCodeByte> paddingBits;
        private CodeSymbolCode<ByteEncodingSymbol> encodedSymbols; //ToDo generalize encoding
        private List<ECCBlock> interleavingBlocks;
        private TerminatorSymbol terminator;
        private QRCodeVersion version;
        private ErrorCorrectionLevel eccLevel; //ToDo parse correct value or use user-provided/default
                                               //ToDo Use/Set/Check Remainder Bits
                                               //ToDo highlight message mode
                                               //ToDo highlight version/ecc info 1 + 2

        #endregion
        #region public properties

        public QRCodeVersion Version
        {
            get
            {
                return this.version;
            }
            private set //ToDo should be settable from outside
            {
                this.version = value;
                //this.eccLevel = ErrorCorrectionLevel.GetECCLevel(ErrorCorrectionLevel.ECCLevel.Low, this.version); //ToDo parse correct value before reading the code
                this.VersionChangedEvent?.Invoke(this.version);
            }
        }
        public ErrorCorrectionLevel.ECCLevel ECCLevel
        {
            get
            {
                return this.eccLevel.Level;
            }
            private set //ToDo should be settable from outside
            {
                this.eccLevel = ErrorCorrectionLevel.GetECCLevel(value, this.version); //ToDo write the correct bits into the info locations
                this.ECCLevelChangedEvent?.Invoke(value);
            }
        }
        public string Message
        {
            get
            {
                return this.message;
            }
            private set     //ToDo message should be settable from outside and then written into the correct location
            {
                this.message = value;
                this.MessageChangedEvent?.Invoke(this.message, this.ReadMessageSuccess);
            }
        }
        public bool ReadMessageSuccess { get; private set; }

        #endregion
        #region constructors

        public QRCode(char[,] setBits, ErrorCorrectionLevel.ECCLevel eccLevel)
        {
            try
            {
                this.version = QRCodeVersion.GetVersionFromSize((uint) setBits.GetLength(0));
                this.ECCLevel = eccLevel;
            }
            catch(ArgumentException ae)
            {
                throw new ArgumentException("Bad QR Code size: Not a valid version number", "setBits", ae);
            }
            if (setBits.GetLength(0) != setBits.GetLength(1))
            {
                throw new ArgumentException("Bad QR Code size: Not a square", "setBits");
            }
            this.bits = setBits;
            this.UpdateMessage();
        }

        /// <summary>
        /// Generates a QRCode of the specified <paramref name="version"/> with static elements and format information
        /// </summary>
        /// <param name="_version">The version defines the size of the QR Code. Valid versions are 1-40</param>
        /// <param name="eccLevel">Error correction code level</param>
        /// <param name="maskType">XOR Mask-Type</param>
        public QRCode(uint _version, ErrorCorrectionLevel.ECCLevel eccLevel, XORMask.MaskType maskType)
        {
            this.version = new QRCodeVersion(_version);
            this.ECCLevel = eccLevel;

            var size = version.GetEdgeSizeFromVersion();
            this.bits = new char[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    this.bits[x, y] = 'u';
                }
            }

            this.PlaceStaticElements();
            this.PlaceFormatInformation(new FormatInformation(eccLevel, maskType));
            this.UpdateMessage();
        }

        public QRCode(string path)
        {
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
            try
            {
                this.version = QRCodeVersion.GetVersionFromSize((uint)cells.Count);
                
                this.bits = new char[cells.Count, cells.Count];
                for (int y = 0; y < cells.Count; y++)
                {
                    if (cells[y].Length != cells.Count) // compare row length to column length
                    {
                        throw new QRCodeFormatException("QR Code row length is wrong in row " + y);
                    }
                    for (int x = 0; x < cells.Count; x++)
                    {
                        if (cells[y][x].Length != 1)  // more than one "bit" char in cell?
                        {
                            throw new QRCodeFormatException("Invalid QR Code data");
                        }
                        else
                        {
                            this.bits[x, y] = cells[y][x][0];
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException ae)
            {
                throw new QRCodeFormatException("QR Code column count is wrong.", ae);
            }
            this.PlaceStaticElements(); // make sure static elements have correct value
            this.UpdateMessage();
        }

        #endregion
        #region private methods

        private void PlaceFormatInformation(FormatInformation formatInfo)
        {
            var fiBits = formatInfo.GetFormatInfoBits();
            var fiLocations = FormatInformation.GetFormatInformationLocations(this.Version, FormatInformation.FormatInfoLocation.SplitBottomLeftTopRight);

            for (int i = 0; i < fiLocations.Count; i++)
            {
                this.bits[fiLocations[i].X, fiLocations[i].Y] = fiBits[i];
            }

            fiLocations = FormatInformation.GetFormatInformationLocations(this.Version, FormatInformation.FormatInfoLocation.TopLeft);

            for (int i = 0; i < fiLocations.Count; i++)
            {
                this.bits[fiLocations[i].X, fiLocations[i].Y] = fiBits[i];
            }
        }

        private void PlaceStaticElements()
        {
            int edgeLength = this.bits.GetLength(0);

            char[,] finderPattern = new char[,] 
            {
                { 'b', 'b', 'b', 'b', 'b', 'b', 'b'},
                { 'b', 'w', 'w', 'w', 'w', 'w', 'b'},
                { 'b', 'w', 'b', 'b', 'b', 'w', 'b'},
                { 'b', 'w', 'b', 'b', 'b', 'w', 'b'},
                { 'b', 'w', 'b', 'b', 'b', 'w', 'b'},
                { 'b', 'w', 'w', 'w', 'w', 'w', 'b'},
                { 'b', 'b', 'b', 'b', 'b', 'b', 'b'}
            };
            // Place Finder Patterns
            for(int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    this.bits[x, y] = finderPattern[x, y]; // Top Left
                    this.bits[edgeLength - 1 - x, y] = finderPattern[x, y]; // Top Right
                    this.bits[x, edgeLength - 1 - y] = finderPattern[x, y]; // Bottom Left
                }
            }
            // Place separators
            for(int i = 0; i < 8; i++)
            {
                // Top Left
                this.bits[7, i] = 'w';
                this.bits[i, 7] = 'w';
                // Top Right
                this.bits[edgeLength - 1 - i, 7] = 'w';
                this.bits[edgeLength - 8, i] = 'w';
                // Bottom Left
                this.bits[7, edgeLength - 1 - i] = 'w';
                this.bits[i, edgeLength - 8] = 'w';
            }
            // Place Alignment Patterns
            this.PlaceAlignmentPatterns();

            // Place timing
            for(int i = 6; i < edgeLength - 7; i++)
            {
                this.bits[6, i] = i % 2 == 0 ? 'b' : 'w';
                this.bits[i, 6] = i % 2 == 0 ? 'b' : 'w';
            }
            // Place dark module
            this.bits[8, (4 * this.Version.VersionNumber) + 9] = 'b';
            // ToDo: Place format information, alternatively allow user to set it manually - mask information is required for this. Make selecting a mask mandatory/always associate a specific mask with a QRCode instance?
            // ToDo: Place version information where needed
        }

        private void InsertAlignmentPattern(int centerX, int centerY)
        {
            char[,] alignmentPattern = new char[,]
            {
                { 'b', 'b', 'b', 'b', 'b', },
                { 'b', 'w', 'w', 'w', 'b', },
                { 'b', 'w', 'b', 'w', 'b', },
                { 'b', 'w', 'w', 'w', 'b', },
                { 'b', 'b', 'b', 'b', 'b', }
            };

            for (int x = 0; x < alignmentPattern.GetLength(0); x++)
            {
                for (int y = 0; y < alignmentPattern.GetLength(1); y++)
                {
                    this.bits[centerX + x - 2, centerY + y - 2] = alignmentPattern[x, y];
                }
            }
        }

        private void PlaceAlignmentPatterns()
        {
            int num_total = this.Version.VersionNumber == 1 ? 0 : (int)((this.Version.VersionNumber / 7) + 2); // number of coordinates (coordinates in x- and y-direction are identical)

            int[] coordValues = new int[num_total];
            
            if (num_total > 1)
            {
                coordValues[0] = 6; // first coordinate is always 6

                coordValues[num_total - 1] = 4 * (int)this.Version.VersionNumber + 10; // last coordinate is always 7 codeEls from the right/bottom border of the code

                if (num_total > 2)
                {
                    coordValues[num_total - 2] = 2 * ((coordValues[0] + coordValues[num_total - 1] * (num_total - 2)) / ((num_total - 1) * 2));

                    if (num_total > 3)
                    {
                        int step = coordValues[num_total - 1] - coordValues[num_total - 2];

                        for (int i = num_total - 3; i > 0; i--)
                        {
                            coordValues[i] = coordValues[i + 1] - step;
                        }
                    }
                }
            }

            foreach(var x in coordValues)
            {
                foreach(var y in coordValues)
                {
                    if(!(x <= 10 && (y <= 10 || y >= (int)this.Version.GetEdgeSizeFromVersion() - 10))
                    && !(y <= 10 && (x <= 10 || x >= (int)this.Version.GetEdgeSizeFromVersion() - 10))) // no collision with finder pattern
                    {
                        this.InsertAlignmentPattern(x, y);
                    }
                }
            }
        }

        private void UpdateMessage()
        {
            try
            {
                var msg = this.ReadMessage();
                this.ReadMessageSuccess = true; // only set if ReadMessage threw no exception, but before setting the property (invokes event)
                this.Message = msg;
            }
            catch(QRCodeFormatException qfe)
            {
                this.ReadMessageSuccess = false;
                this.Message = qfe.Message + Environment.NewLine;

                if (qfe.InnerException != null)
                    this.Message += qfe.InnerException.Message;
            }
        }

        private void SetFormatInfo(char[] fInfo)    //ToDo create DataBlock/Symbol for Format Info 1 and 2
        {
            if (fInfo.Length != 15)
                throw new ArgumentException("Wrong length", "fInfo");

            var edgeLen = this.GetEdgeLength();
            var index = 0;

            for (int x = 0; x < 8; x++) // left to right
            {
                if (x != 6)
                {
                    this.bits[x, 8] = fInfo[index++];
                }
            }
            for (int y = 7; y >= 0; y--) // towards top
            {
                if (y != 6)
                {
                    this.bits[8, y] = fInfo[index++];
                }
            }
            index = 0;
            for (int y = edgeLen - 1; y >= edgeLen - 7; y--) // from bottom up
            {
                this.bits[8, y] = fInfo[index++];
            }
            for (int x = edgeLen - 9; x < edgeLen; x++) // towards right edge
            {
                this.bits[x, 8] = fInfo[index++];
            }
        }

        private char[] GetFormatInfo(FormatInformation.FormatInfoLocation loc)
        {
            var fInfo = new List<char>();
            var fInfoLocs = FormatInformation.GetFormatInformationLocations(this.version, loc);

            for(int i = 0; i < fInfoLocs.Count; i++)
            {
                fInfo.Add(this.bits[fInfoLocs[i].X, fInfoLocs[i].Y]);
            }

            return fInfo.ToArray();
        }

        private void ReadFormatInformation()
        {
            var splitInfo = new string(this.GetFormatInfo(FormatInformation.FormatInfoLocation.SplitBottomLeftTopRight));
            var topLeftInfo = new string(this.GetFormatInfo(FormatInformation.FormatInfoLocation.TopLeft));
            // TODO use correction mechanism of format info

            if (splitInfo == topLeftInfo)
            {
                var fi = new FormatInformation(splitInfo);
                this.ECCLevel = fi.ECCLevel;
                // TODO do something with Mask information
            }
            else
            {
                //TODO display error message
            }
        }

        private string ReadMessage()
        {
            try
            {
                this.ReadFormatInformation();
            }
            catch(ArgumentException ae)
            {
                throw new QRCodeFormatException("Could not parse Format information: ", ae);
            }

            this.rawCode = new CodeSymbolCode<RawCodeByte>(this.GetBitIterator());
            this.interleavingBlocks = DeInterleaver.DeInterleave(this.rawCode, this.eccLevel);

            var dataCodeSymbols = new List<CodeSymbolCode<RawCodeByte>>();
            var eccCodeSymbols = new List<CodeSymbolCode<RawCodeByte>>();

            for (int i = 0; i < this.interleavingBlocks.Count; i++)
            {
                dataCodeSymbols.Add(this.interleavingBlocks[i].GetPostRepairData());
                eccCodeSymbols.Add(this.interleavingBlocks[i].GetPostRepairECC());
            }

            this.rawDataBytes = new CodeSymbolCode<RawCodeByte>(dataCodeSymbols);
            this.rawECCBytes = new CodeSymbolCode<RawCodeByte>(eccCodeSymbols);

            int modeNibble;
            try
            {
                modeNibble = Convert.ToInt32(this.rawDataBytes.GetBitString(0, MODEINFOLENGTH), 2);
            }
            catch (FormatException fe)
            {
                throw new QRCodeFormatException("Mode indicator nibble could not be decoded: " + this.rawDataBytes.GetBitString(0, MODEINFOLENGTH), fe);
            }

            if (MessageMode.TryParse(modeNibble, out this.messageMode))
            {
                var charIndicatorLength = MessageMode.GetCharacterCountIndicatorLength(this.Version, this.messageMode);
                int characterCount;
                try
                {
                    characterCount = Convert.ToInt32(this.rawDataBytes.GetBitString(MODEINFOLENGTH, charIndicatorLength), 2);
                }
                catch (FormatException fe)
                {
                    throw new QRCodeFormatException("Could not parse character count.", fe); //ToDo continue with max possible character count, but inform the user
                }

                var max_capacity = QRCodeCapacities.GetCapacity(this.Version, this.ECCLevel, this.messageMode);
                if (characterCount > max_capacity)
                {
                    throw new QRCodeFormatException("Character count " + characterCount + " exceeds max. capacity of " + max_capacity);
                }

                if (this.messageMode == MessageMode.Mode.Byte)
                {
                    var encodedCharacterLength = MessageMode.GetCharacterLength(this.messageMode);
                    var firstSymbolOffset = MODEINFOLENGTH + charIndicatorLength;
                    var messageLenghtInBits = characterCount * encodedCharacterLength;
                    var messageEndOffset = messageLenghtInBits + firstSymbolOffset;
                    var terminatorLength = 4; // Always 4 for MessageMode.Byte, no incomplete padding bytes for this mode. // TODO generalize as terminatorLength = RAWBYTELENGTH - (messageEndOffset % RAWBYTELENGTH) to fill up the remaining bits in the last used raw code byte?
                    var terminatorLocation = new Vector2D[terminatorLength];

                    for (int i = 0, bitNumber = messageEndOffset; i < terminatorLength; i++, bitNumber++)
                    {
                        terminatorLocation[i] = this.rawDataBytes.GetBitPosition(bitNumber);
                    }

                    this.encodedSymbols = this.rawDataBytes.ToCodeSymbolCode<ByteEncodingSymbol>(firstSymbolOffset, messageLenghtInBits);
                    this.terminator = new TerminatorSymbol(this.rawDataBytes.GetBitString(messageEndOffset, terminatorLength), terminatorLocation);
                    this.paddingBits = this.rawDataBytes.ToCodeSymbolCode<RawCodeByte>(messageEndOffset + terminatorLength, QRCodeCapacities.GetDataBytes(this.Version, this.eccLevel.Level) * 8 - (messageEndOffset + terminatorLength));

                    return encodedSymbols.DecodeSymbols('_');
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

        private QRCodeBitIterator GetBitIterator()
        {
            return new QRCodeBitIterator(this);
        }

        #endregion
        #region public methods

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

        public char GetBit(int x, int y)
        {
            return this.bits[x, y];
        }

        public char[,] GetBits()
        {
            return (char[,])this.bits.Clone();
        }

        public void ToggleDataCell(int x, int y)
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
            this.UpdateMessage();
        }

        public void SetDataCell(int x, int y, char cellValue)   //ToDo set as expected in un-masked view
        {
            if (x < 0 || x >= this.Version.GetEdgeSizeFromVersion())
                throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y >= this.Version.GetEdgeSizeFromVersion())
                throw new ArgumentOutOfRangeException("y");

            if (this.IsDataCell(x, y))
            {
                switch (cellValue)
                {
                    case '0':
                    case '1':
                    case 'u':
                        bits[x, y] = cellValue;
                        break;
                    default:
                        throw new ArgumentException("Invalid cellValue: " + cellValue);
                }
                this.UpdateMessage();
            }
        }

        public bool IsDataCell(int x, int y)
        {
            if ((y == 8 && (x < 9 || x > this.GetEdgeLength() - 9))
            ||  (x == 8 && (y < 9 || y > this.GetEdgeLength() - 8))) // Format Information //ToDo make editable
            {
                return false;
            }
            else if(    this.Version.VersionNumber > 6  // version 7 or larger has additional version information
                    && (    (x < 6 && y < (this.GetEdgeLength() - 8) && y > (this.GetEdgeLength() - 12))
                         || (y < 6 && x < (this.GetEdgeLength() - 8) && x > (this.GetEdgeLength() - 12))))
            {
                return false;
            }
            else
            {
                return (this.bits[x, y] == '1') || (this.bits[x, y] == '0') || (this.bits[x, y] == 'u');
            }
        }

        //public static bool IsDataCell(int x, int y, int version) //ToDo extend for other versions
        //{
        //    var versionSize = QRCode.GetEdgeSizeFromVersion(version);
        //    const int finderPatternWidth = 7;
        //    const int formatInfoWidth = 1; // width of the version information area
        //    const int separatorWidth = 1; // width of the separator

        //    bool ret = true;
        //    if (x > versionSize || y > versionSize || x < 0 || y < 0)
        //        return false;

        //    if (x < 9) // left side
        //    {
        //        if (y < finderPatternWidth + separatorWidth + formatInfoWidth) // top left finder pattern
        //            ret = false;
        //        else if (y >= versionSize - finderPatternWidth - separatorWidth) // bottom left finder pattern
        //            ret = false;
        //    }
        //    else if ((x >= versionSize - finderPatternWidth - separatorWidth) && (y < finderPatternWidth + separatorWidth + formatInfoWidth)) // Top right finder pattern
        //    {
        //        ret = false;
        //    }
        //    if (x > 19 && x < 25 && y > 19 && y < 25) // Alignment TODO for all versions
        //    {
        //        ret = false;
        //    }
        //    if (x == 6 || y == 6) // Timing
        //    {
        //        ret = false;
        //    }

        //    return ret;
        //}

        public string GetRepairMessageStatusLine()
        {
            if (this.interleavingBlocks == null)
            {
                return "Nothing to repair.";
            }
            else
            {
                var sb = new StringBuilder();

                for (int i = 0; i < this.interleavingBlocks.Count; i++)
                {
                    sb.AppendLine(String.Format("Block {0} {1} repaired", i, this.interleavingBlocks[i].RepairSuccess ? "successfully" : "not"));
                }
                return sb.ToString();
            }
        }

        public void PrintBlocks()
        {
            Console.WriteLine(this.Message);
        }

        public int GetEdgeLength()
        {
            return this.bits.GetLength(0);
        }

        public CodeSymbolCode<RawCodeByte> GetRawCode()
        {
            return this.rawCode;
        }

        public CodeSymbolCode<RawCodeByte> GetPaddingBits()
        {
            return this.paddingBits;
        }

        public CodeSymbolCode<ByteEncodingSymbol> GetEncodedSymbols()
        {
            return this.encodedSymbols;
        }

        //public List<ECCBlock> GetInterleavingBlocks()
        //{
        //    return this.interleavingBlocks;
        //}
        public TerminatorSymbol GetTerminator()
        {
            return this.terminator;
        }

        public CodeSymbolCode<RawCodeByte> GetRawDataBytes()
        {
            return this.rawDataBytes;
        }
        public CodeSymbolCode<RawCodeByte> GetRawECCBytes()
        {
            return this.rawECCBytes;
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

        #endregion
    }
}
