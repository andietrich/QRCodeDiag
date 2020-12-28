using QRCodeBaseLib.MetaInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib
{
    internal class QRCodeElementWriter
    {
        private readonly char[,] qrCodeBits;
        private readonly QRCodeVersion version;
        public QRCodeElementWriter(char[,] bits)
        {
            if (bits.GetLength(0) != bits.GetLength(1))
            {
                throw new ArgumentException("Bad QR Code size: Not a square", "setBits");
            }

            this.version = QRCodeVersion.GetVersionFromSize((uint)bits.GetLength(0));
            this.qrCodeBits = bits;
        }
        public void PlaceStaticElements()
        {
            this.PlaceFinderPatterns();
            this.PlaceSeparators();
            this.PlaceAlignmentPatterns();
            this.PlaceTimingPattern();
            this.PlaceDarkModule();
            // ToDo: Place format information, alternatively allow user to set it manually - mask information is required for this. Make selecting a mask mandatory/always associate a specific mask with a QRCode instance?
            // ToDo: Place version information where needed
        }
        public void PlaceFormatInformation(FormatInformation formatInfo)    //ToDo create DataBlock/Symbol for Format Info 1 and 2
        {
            var fiBits = formatInfo.GetFormatInfoBits();
            var fiLocations = FormatInformation.GetFormatInformationLocations(this.version, FormatInformation.FormatInfoLocation.SplitBottomLeftTopRight);

            for (int i = 0; i < fiLocations.Count; i++)
            {
                this.qrCodeBits[fiLocations[i].X, fiLocations[i].Y] = fiBits[i];
            }

            fiLocations = FormatInformation.GetFormatInformationLocations(this.version, FormatInformation.FormatInfoLocation.TopLeft);

            for (int i = 0; i < fiLocations.Count; i++)
            {
                this.qrCodeBits[fiLocations[i].X, fiLocations[i].Y] = fiBits[i];
            }
        }

        private void PlaceDarkModule()
        {
            this.qrCodeBits[8, (4 * this.version.VersionNumber) + 9] = 'b';
        }

        private void PlaceTimingPattern()
        {
            int edgeLength = this.qrCodeBits.GetLength(0);

            for (int i = 6; i < edgeLength - 7; i++)
            {
                this.qrCodeBits[6, i] = i % 2 == 0 ? 'b' : 'w';
                this.qrCodeBits[i, 6] = i % 2 == 0 ? 'b' : 'w';
            }
        }
        private void PlaceSeparators()
        {
            int edgeLength = this.qrCodeBits.GetLength(0);

            for (int i = 0; i < 8; i++)
            {
                // Top Left
                this.qrCodeBits[7, i] = 'w';
                this.qrCodeBits[i, 7] = 'w';
                // Top Right
                this.qrCodeBits[edgeLength - 1 - i, 7] = 'w';
                this.qrCodeBits[edgeLength - 8, i] = 'w';
                // Bottom Left
                this.qrCodeBits[7, edgeLength - 1 - i] = 'w';
                this.qrCodeBits[i, edgeLength - 8] = 'w';
            }
        }

        private void PlaceFinderPatterns()
        {
            int edgeLength = this.qrCodeBits.GetLength(0);

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

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    this.qrCodeBits[x, y] = finderPattern[x, y]; // Top Left
                    this.qrCodeBits[edgeLength - 1 - x, y] = finderPattern[x, y]; // Top Right
                    this.qrCodeBits[x, edgeLength - 1 - y] = finderPattern[x, y]; // Bottom Left
                }
            }
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
                    this.qrCodeBits[centerX + x - 2, centerY + y - 2] = alignmentPattern[x, y];
                }
            }
        }

        private void PlaceAlignmentPatterns()
        {
            int num_total = this.version.VersionNumber == 1 ? 0 : (int)((this.version.VersionNumber / 7) + 2); // number of coordinates (coordinates in x- and y-direction are identical)
            int[] coordValues = new int[num_total];

            if (num_total > 1)
            {
                coordValues[0] = 6; // first coordinate is always 6
                coordValues[num_total - 1] = 4 * (int)this.version.VersionNumber + 10; // last coordinate is always 7 codeEls from the right/bottom border of the code

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

            foreach (var x in coordValues)
            {
                foreach (var y in coordValues)
                {
                    if (!(x <= 10 && (y <= 10 || y >= (int)this.version.GetEdgeSizeFromVersion() - 10))
                    && !(y <= 10 && (x <= 10 || x >= (int)this.version.GetEdgeSizeFromVersion() - 10))) // no collision with finder pattern
                    {
                        this.InsertAlignmentPattern(x, y);
                    }
                }
            }
        }
    }
}
