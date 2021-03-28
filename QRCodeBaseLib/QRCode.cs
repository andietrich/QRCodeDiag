﻿using QRCodeBaseLib.MetaInfo;
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
        public delegate void MessageChangedHandler(string newMessage, bool messageIsValid);
        public delegate void RawCodeChangedHandler(ICodeSymbolCode newRawCode);
        public delegate void RawDataBytesChangedHandler(ICodeSymbolCode newRawDataBytes);
        public delegate void RawECCBytesChangedHandler(ICodeSymbolCode newRawECCBytes);
        public delegate void MessageModeChangedHandler(ICodeSymbolCode newMessageMode);
        public delegate void CharCountIndicatorChangedHandler(ICodeSymbolCode newCharCountIndicator);
        public delegate void PaddingBytesChangedHandler(ICodeSymbolCode newPaddingBytes);
        public delegate void EncodedSymbolsChangedHandler(ICodeSymbolCode newEncodedSymbols);
        public delegate void TerminatorSymbolCodeChangedHandler(ICodeSymbolCode newTerminatorSymbol);
        public event VersionChangedHandler VersionChangedEvent;
        public event ECCLevelChangedHandler ECCLevelChangedEvent;
        public event MessageChangedHandler MessageChangedEvent;
        public event RawCodeChangedHandler RawCodeChangedEvent;
        public event RawDataBytesChangedHandler RawDataBytesChangedEvent;
        public event RawECCBytesChangedHandler RawECCBytesChangedEvent;
        public event MessageModeChangedHandler MessageModeChangedEvent;
        public event CharCountIndicatorChangedHandler CharCountIndicatorChangedEvent;
        public event PaddingBytesChangedHandler PaddingBytesChangedEvent;
        public event EncodedSymbolsChangedHandler EncodedSymbolsChangedEvent;
        public event TerminatorSymbolCodeChangedHandler TerminatorSymbolCodeChangedEvent;

        #region private members

        private readonly char[,] bits; //ToDo consider BitArray class, at least where no unknown values appear

        private MessageMode messageMode; // ToDo: set initial value: byte? Mode.Unknown?
        private string message;

        // parsed/drawable code elements
        private CodeSymbolCode<RawCodeByte> rawCode;
        private CodeSymbolCode<RawCodeByte> rawDataBytes;
        private CodeSymbolCode<RawCodeByte> rawECCBytes;
        private CodeSymbolCode<MessageModeSymbol> messageModeSymbolCode;
        private CodeSymbolCode<CharCountIndicatorSymbol> charCountIndicatorSymbolCode;
        private CodeSymbolCode<RawCodeByte> paddingBits;
        private CodeSymbolCode<ByteEncodingSymbol> encodedSymbols; //ToDo generalize encoding
        private CodeSymbolCode<TerminatorSymbol> terminatorSymbolCode;
        private List<ECCBlock> interleavingBlocks;
        private QRCodeVersion version;
        private XORMask.MaskType appliedXORMaskType;
        private XORMask.MaskType detectedXORMaskType;
        private ErrorCorrectionLevel eccLevel; //ToDo parse correct value or use user-provided/default
                                               //ToDo Use/Set/Check Remainder Bits
                                               //ToDo highlight message mode
                                               //ToDo highlight version/ecc info 1 + 2
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
        internal CodeSymbolCode<CharCountIndicatorSymbol> CharCountIndicatorSymbolCode
        {
            get { return this.charCountIndicatorSymbolCode; }
            private set
            {
                this.charCountIndicatorSymbolCode = value;
                this.CharCountIndicatorChangedEvent?.Invoke(value);
            }
        }
        internal CodeSymbolCode<MessageModeSymbol> MessageModeSymbolCode
        {
            get { return this.messageModeSymbolCode; }
            private set
            {
                this.messageModeSymbolCode = value;
                this.MessageModeChangedEvent?.Invoke(value);
            }
        }
        internal CodeSymbolCode<ByteEncodingSymbol> EncodedSymbols
        {
            get { return this.encodedSymbols; }
            private set
            {
                this.encodedSymbols = value;
                this.EncodedSymbolsChangedEvent?.Invoke(value);
            }
        }
        internal CodeSymbolCode<RawCodeByte> PaddingBits
        {
            get { return this.paddingBits; }
            private set
            {
                this.paddingBits = value;
                this.PaddingBytesChangedEvent?.Invoke(value);
            }
        }
        internal CodeSymbolCode<TerminatorSymbol> TerminatorSymbolCode
        {
            get { return this.terminatorSymbolCode; }
            private set
            {
                this.terminatorSymbolCode = value;
                this.TerminatorSymbolCodeChangedEvent?.Invoke(value);
            }
        }
        #endregion

        #region internal properties
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
        public uint? CharacterCount => this.charCountIndicatorSymbolCode?.GetSymbolAt(0).CurrentSymbolLength;

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
                dataCodeSymbols.Add(deinterleavedBlocks[i].GetPostRepairData());
            }

            return new CodeSymbolCode<RawCodeByte>(dataCodeSymbols);
        }

        private static CodeSymbolCode<RawCodeByte> GetRawECCBytes(List<ECCBlock> deinterleavedBlocks)
        {
            var eccCodeSymbols = new List<CodeSymbolCode<RawCodeByte>>();

            for (int i = 0; i < deinterleavedBlocks.Count; i++)
            {
                eccCodeSymbols.Add(deinterleavedBlocks[i].GetPostRepairECC());
            }

            return new CodeSymbolCode<RawCodeByte>(eccCodeSymbols);
        }

        private string ReadMessage()
        {
            this.ReadFormatInformation();

            this.RawCode = CodeSymbolCode<RawCodeByte>.CreateInstance(this.GetBitIterator(), new RawCodeByteFactory());
            this.interleavingBlocks = DeInterleaver.DeInterleave(this.RawCode, this.eccLevel);
            this.RawDataBytes = QRCode.GetRawDataBytes(this.interleavingBlocks);
            this.RawECCBytes = QRCode.GetRawECCBytes(this.interleavingBlocks);
            this.MessageModeSymbolCode = this.rawDataBytes.ToCodeSymbolCode(0, MessageModeSymbol.MODEINFOLENGTH, new MessageModeSymbolFactory());
            this.messageMode = MessageMode.ParseMessageMode(this.MessageModeSymbolCode.GetSymbolAt(0), this.Version);
            this.CharCountIndicatorSymbolCode = this.rawDataBytes.ToCodeSymbolCode(MessageModeSymbol.MODEINFOLENGTH, messageMode.CharacterCountIndicatorLength, new CharCountIndicatorSymbolFactory(this.messageMode));

            uint characterCount = this.CharCountIndicatorSymbolCode.GetSymbolAt(0).GetCharacterCount();

            var max_capacity = QRCodeCapacities.GetCapacity(this.Version, this.ECCLevel, this.messageMode.Mode);

            if (characterCount > max_capacity)
            {
                this.EncodedSymbols = null;
                this.TerminatorSymbolCode = null;
                this.PaddingBits = null;

                throw new QRCodeFormatException($"Character count {characterCount} exceeds max. capacity of {max_capacity}");
            }

            if (this.messageMode.Mode == MessageMode.EncodingMode.Byte)
            {
                var firstSymbolOffset = MessageModeSymbol.MODEINFOLENGTH + this.messageMode.CharacterCountIndicatorLength;
                var messageLenghtInBits = characterCount * this.messageMode.CharacterLength;
                var messageEndOffset = messageLenghtInBits + firstSymbolOffset;
                var terminatorLength = 4u; // Always 4 for MessageMode.Byte, no incomplete padding bytes for this mode. // TODO generalize as terminatorLength = RAWBYTELENGTH - (messageEndOffset % RAWBYTELENGTH) to fill up the remaining bits in the last used raw code byte?
                var terminatorLocation = new Vector2D[terminatorLength];

                for (uint i = 0u, bitNumber = messageEndOffset; i < terminatorLength; i++, bitNumber++)
                {
                    terminatorLocation[i] = this.RawDataBytes.GetBitPosition(bitNumber);
                }

                this.EncodedSymbols = this.RawDataBytes.ToCodeSymbolCode(firstSymbolOffset, messageLenghtInBits, new ByteEncodingSymbolFactory());
                this.TerminatorSymbolCode = this.RawDataBytes.ToCodeSymbolCode(messageEndOffset, terminatorLength, new TerminatorSymbolFactory());
                this.PaddingBits = this.RawDataBytes.ToCodeSymbolCode(messageEndOffset + terminatorLength, QRCodeCapacities.GetDataBytes(this.Version, this.eccLevel.Level) * 8 - (messageEndOffset + terminatorLength), new RawCodeByteFactory());

                return EncodedSymbols.ToString();
            }
            else
            {
                throw new NotImplementedException("Other encodings are not implemented yet."); //ToDo implement reading other encodings
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
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= this.Version.GetEdgeSizeFromVersion())
                throw new ArgumentOutOfRangeException(nameof(y));

            if (this.IsDataCell((uint)x, (uint)y))
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
