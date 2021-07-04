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
using QRCodeBaseLib.DataBlocks.Symbols.SymbolFactories;

namespace QRCodeBaseLib
{
    public class QRCode
    {
        public delegate void VersionChangedHandler(QRCodeVersion newVersion);
        public delegate void ECCLevelChangedHandler(ErrorCorrectionLevel.ECCLevel newECCLevel);
        public delegate void RawCodeChangedHandler(ICodeSymbolCode newRawCode);
        public delegate void RawDataBytesChangedHandler(ICodeSymbolCode newRawDataBytes);
        public delegate void RawECCBytesChangedHandler(ICodeSymbolCode newRawECCBytes);
        public delegate void EncodedMessagesChangedHandler(IEnumerable<EncodedMessage> encodedMessages);
        public delegate void InterleavingBlocksChangedHandler(IEnumerable<ECCBlock> newInterleavingBlocks);
        public delegate void TerminatorSymbolCodeChangedHandler(ICodeSymbolCode newTerminatorSymbol);
        public delegate void PaddingBytesChangedHandler(ICodeSymbolCode newPaddingBytes);
        public event VersionChangedHandler VersionChangedEvent;
        public event ECCLevelChangedHandler ECCLevelChangedEvent;
        public event RawCodeChangedHandler RawCodeChangedEvent;
        public event RawDataBytesChangedHandler RawDataBytesChangedEvent;
        public event RawECCBytesChangedHandler RawECCBytesChangedEvent;
        public event EncodedMessagesChangedHandler EncodedMessagesChangedEvent;
        public event InterleavingBlocksChangedHandler InterleavingBlocksChangedEvent;
        public event TerminatorSymbolCodeChangedHandler TerminatorSymbolCodeChangedEvent;
        public event PaddingBytesChangedHandler PaddingBytesChangedEvent;

        #region private members

        private readonly char[,] bits; //ToDo consider BitArray class, at least where no unknown values appear

        // parsed/drawable code elements
        private CodeSymbolCode<RawCodeByte> rawCode;
        private CodeSymbolCode<RawCodeByte> rawDataBytes;
        private CodeSymbolCode<RawCodeByte> rawECCBytes;
        private CodeSymbolCode<PaddingSymbol> paddingBits;
        private CodeSymbolCode<MessageModeSymbol> terminatorSymbolCode;
        private IEnumerable<EncodedMessage> encodedMessages;
        private List<ECCBlock> interleavingBlocks;
        private QRCodeVersion version;
        private XORMask.MaskType appliedXORMaskType;
        private XORMask.MaskType detectedXORMaskType;
        private ErrorCorrectionLevel eccLevel; //ToDo parse correct value or use user-provided/default
                                               //ToDo Use/Set/Check Remainder Bits
                                               //ToDo highlight message mode
                                               //ToDo highlight version/ecc info 1 + 2
        #endregion

        #region private properties
        private List<ECCBlock> InterleavingBlocks
        {
            get => this.interleavingBlocks;
            set
            {
                this.interleavingBlocks = value;
                this.InterleavingBlocksChangedEvent?.Invoke(value);
            }
        }
        #endregion
        #region internal properties
        internal CodeSymbolCode<RawCodeByte> RawCode
        {
            get { return this.rawCode; }
            private set
            {
                this.rawCode = value;
                this.RawCodeChangedEvent?.Invoke(value);
            }
        }
        internal CodeSymbolCode<RawCodeByte> RawDataBytes
        {
            get { return this.rawDataBytes; }
            private set
            {
                this.rawDataBytes = value;
                this.RawDataBytesChangedEvent?.Invoke(value);
            }
        }
        internal CodeSymbolCode<RawCodeByte> RawECCBytes
        {
            get { return this.rawECCBytes; }
            private set
            {
                this.rawECCBytes = value;
                this.RawECCBytesChangedEvent?.Invoke(value);
            }
        }
        internal IEnumerable<EncodedMessage> EncodedMessages
        {
            get { return this.encodedMessages; }
            private set
            {
                this.encodedMessages = value;
                this.EncodedMessagesChangedEvent?.Invoke(value);
            }
        }
        internal CodeSymbolCode<MessageModeSymbol> TerminatorSymbolCode
        {
            get { return this.terminatorSymbolCode; }
            private set
            {
                this.terminatorSymbolCode = value;
                this.TerminatorSymbolCodeChangedEvent?.Invoke(value);
            }
        }
        internal CodeSymbolCode<PaddingSymbol> PaddingBits
        {
            get { return this.paddingBits; }
            private set
            {
                this.paddingBits = value;
                this.PaddingBytesChangedEvent?.Invoke(value);
            }
        }
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
        public XORMask.MaskType AppliedXORMaskType
        {
            get
            {
                return this.appliedXORMaskType;
            }
            set
            {
                this.appliedXORMaskType = value;
                this.UpdateMessage();
            }
        }
        public XORMask.MaskType DetectedXORMaskType
        {
            get { return this.detectedXORMaskType; }
            set { this.detectedXORMaskType = value; }
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

        public bool ReadMessageSuccess { get; private set; }

        #endregion
        #region constructors

        public QRCode(char[,] setBits, ErrorCorrectionLevel.ECCLevel eccLevel)
        {
            if (setBits.GetLength(0) != setBits.GetLength(1))
            {
                throw new ArgumentException("Bad QR Code size: Not a square", "setBits");
            }
            this.version = QRCodeVersion.GetVersionFromSize((uint) setBits.GetLength(0));
            this.ECCLevel = eccLevel;
            this.appliedXORMaskType = XORMask.MaskType.None;
            this.bits = setBits;
            this.UpdateMessage();
        }

        /// <summary>
        /// Generates a QRCode of the specified <paramref name="version"/> with static elements and format information
        /// </summary>
        /// <param name="setVersion">The version defines the size of the QR Code. Valid versions are 1-40</param>
        /// <param name="eccLevel">Error correction code level</param>
        /// <param name="maskType">XOR Mask-Type</param>
        public QRCode(QRCodeVersion setVersion, ErrorCorrectionLevel.ECCLevel eccLevel, XORMask.MaskType maskType)
        {
            this.version = setVersion;
            this.ECCLevel = eccLevel;
            this.appliedXORMaskType = maskType;

            var size = version.GetEdgeSizeFromVersion();
            this.bits = new char[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    this.bits[x, y] = 'u';
                }
            }

            var elemWriter = new QRCodeElementWriter(this.bits);
            elemWriter.PlaceStaticElements();
            elemWriter.PlaceFormatInformation(new FormatInformation(eccLevel, maskType));

            this.UpdateMessage();
        }

        public QRCode(IEnumerable<string> fileContent)
        {
            List<string[]> cells = new List<string[]>();

            foreach(var line in fileContent)
            {
                cells.Add(line.Trim().Split(null));
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
            catch (ArgumentException ae)
            {
                throw new QRCodeFormatException("QR Code size is wrong.", ae);
            }

            var elemWriter = new QRCodeElementWriter(this.bits);
            elemWriter.PlaceStaticElements(); // make sure static elements have correct value

            this.appliedXORMaskType = XORMask.MaskType.None;

            this.UpdateMessage();
        }

        #endregion
        #region private methods

        private void UpdateMessage()
        {
            try
            {
                this.ReadMessage();
                this.ReadMessageSuccess = true; // only set if ReadMessage threw no exception, but before setting the property (invokes event)
            }
            catch(QRCodeFormatException)
            {
                this.ReadMessageSuccess = false;
            }
        }

        private char[] GetFormatInfoBits(FormatInformation.FormatInfoLocation loc)
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
            var splitInfo = new string(this.GetFormatInfoBits(FormatInformation.FormatInfoLocation.SplitBottomLeftTopRight));
            var topLeftInfo = new string(this.GetFormatInfoBits(FormatInformation.FormatInfoLocation.TopLeft));
            // TODO use error correction mechanisms of format info

            if (splitInfo == topLeftInfo)
            {
                var fi = new FormatInformation(splitInfo);
                this.ECCLevel = fi.ECCLevel;
                this.DetectedXORMaskType = fi.Mask; // TODO do something with Mask information
            }
            else
            {
                //TODO display error message
            }
        }

        private static CodeSymbolCode<RawCodeByte> GetRawDataBytes(List<ECCBlock> deinterleavedBlocks)
        {
            var dataCodeSymbols = new List<CodeSymbolCode<RawCodeByte>>();

            for (int i = 0; i < deinterleavedBlocks.Count; i++)
            {
                dataCodeSymbols.Add(deinterleavedBlocks[i].GetRawPostRepairData());
            }

            return new CodeSymbolCode<RawCodeByte>(dataCodeSymbols);
        }

        private static CodeSymbolCode<RawCodeByte> GetRawECCBytes(List<ECCBlock> deinterleavedBlocks)
        {
            var eccCodeSymbols = new List<CodeSymbolCode<RawCodeByte>>();

            for (int i = 0; i < deinterleavedBlocks.Count; i++)
            {
                eccCodeSymbols.Add(deinterleavedBlocks[i].GetRawPostRepairECC());
            }

            return new CodeSymbolCode<RawCodeByte>(eccCodeSymbols);
        }

        private void ReadMessage()
        {
            this.ReadFormatInformation();

            this.RawCode = CodeSymbolCode<RawCodeByte>.BuildInstance(this.GetBitIterator(), new RawCodeByteFactory());
            this.InterleavingBlocks = DeInterleaver.DeInterleave(this.RawCode, this.eccLevel);
            this.RawDataBytes = QRCode.GetRawDataBytes(this.InterleavingBlocks);
            this.RawECCBytes = QRCode.GetRawECCBytes(this.InterleavingBlocks);
            
            var it = this.rawDataBytes.GetBitIterator();
            var encodedMsgs = new List<EncodedMessage>();
            CodeSymbolCode<MessageModeSymbol> newTerminator = null;
            CodeSymbolCode<PaddingSymbol> newPadding = null;

            while(!it.EndReached)
            {
                var modeSymbolCode = CodeSymbolCode<MessageModeSymbol>.BuildInstance(it, new MessageModeSymbolFactory());
                
                if(!MessageMode.TryParseMessageMode(modeSymbolCode.GetSymbolAt(0), this.Version, out var mode))
                {
                    break;
                }

                if (mode.Mode == MessageMode.EncodingMode.Terminator)
                {
                    newTerminator = modeSymbolCode;
                    var zeroPadLen = (8 - (it.BitsConsumed % 8)) % 8;
                    newPadding = CodeSymbolCode<PaddingSymbol>.BuildInstance(it, new PaddingSymbolFactory(zeroPadLen));
                    newPadding.CutIncompleteEnd();
                    break;
                }
                else
                {
                    encodedMsgs.Add(new EncodedMessage(it, mode, modeSymbolCode));
                }
            }

            this.EncodedMessages = encodedMsgs;
            this.TerminatorSymbolCode = newTerminator;
            this.PaddingBits = newPadding;
        }

        private IBitIterator GetBitIterator()
        {
            return new QRCodeBitIterator(this);
        }

        #endregion
        #region public methods

        public string GetSaveFileContent()
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
            return sb.ToString();
        }

        /// <summary>
        /// Gets the bit at the specified index.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xored">
        /// If true, the xor mask is applied so that the actual data bit values are returned.
        /// Otherwise the value corresponding to the visible data cell is returned.</param>
        /// <returns></returns>
        public char GetBit(uint x, uint y, bool xored)
        {
            if (xored && this.IsDataCell(x, y))
                return XORMask.ApplyXOR(this.AppliedXORMaskType, this.bits[x, y], x, y);
            else
                return this.bits[x, y];
        }

        public char[,] GetBits(bool xored)
        {
            if (xored)
            {
                var edgeLen = this.GetEdgeLength();
                char[,] retval = new char[edgeLen, edgeLen];

                for (uint y = 0; y < edgeLen; y++)
                    for (uint x = 0; x < edgeLen; x++)
                        retval[x, y] = this.GetBit(x, y, xored);

                return retval;
            }
            else
            {
                return (char[,])this.bits.Clone();
            }
        }

        public void ToggleDataCell(int x, int y)
        {
            switch (this.bits[x, y])
            {
                case '0':
                    this.bits[x, y] = '1';
                    break;
                case '1':
                    this.bits[x, y] = 'u';
                    break;
                case 'u':
                    this.bits[x, y] = '0';
                    break;
                default:
                    break;
            }
            this.UpdateMessage();
        }

        public void SetDataCell(int x, int y, char cellValue)   //ToDo set as expected in un-masked view
        {
            if (x < 0 || x >= this.Version.GetEdgeSizeFromVersion())
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= this.Version.GetEdgeSizeFromVersion())
                throw new ArgumentOutOfRangeException(nameof(y));

            if (this.IsDataCell((uint)x, (uint)y) && (this.bits[x, y] != cellValue))
            {
                switch (cellValue)
                {
                    case '0':
                    case '1':
                    case 'u':
                        this.bits[x, y] = cellValue;
                        break;
                    default:
                        throw new ArgumentException("Invalid cellValue: " + cellValue);
                }
                this.UpdateMessage();
            }
        }

        public bool IsDataCell(uint x, uint y)
        {
            if ((y == 8 && (x < 9 || x > this.GetEdgeLength() - 9))
            ||  (x == 8 && (y < 9 || y > this.GetEdgeLength() - 8))) // Format Information //ToDo make editable - but don't xor. Get conditions from FormatInformation class?
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

        public string GetRepairMessageStatusLine()
        {
            if (this.InterleavingBlocks == null)
            {
                return "Nothing to repair.";
            }
            else
            {
                var sb = new StringBuilder();

                for (int i = 0; i < this.InterleavingBlocks.Count; i++)
                {
                    sb.AppendLine(String.Format("Block {0} {1} repaired", i, this.InterleavingBlocks[i].RepairSuccess ? "successfully" : "not"));
                }
                return sb.ToString();
            }
        }

        public int GetEdgeLength()
        {
            return this.bits.GetLength(0);
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
