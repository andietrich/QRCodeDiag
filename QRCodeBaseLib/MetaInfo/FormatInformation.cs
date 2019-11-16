﻿using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeBaseLib.MetaInfo
{
    internal class FormatInformation
    {
        public ErrorCorrectionLevel.ECCLevel ECCLevel;
        public XORMask.MaskType Mask;

        public FormatInformation(string formatInfoString)
        {
            switch(formatInfoString)
            {
                case "111011111000100":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Low;
                    this.Mask = XORMask.MaskType.Mask000;
                    break;

                case "111001011110011":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Low;
                    this.Mask = XORMask.MaskType.Mask001;
                    break;

                case "111110110101010":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Low;
                    this.Mask = XORMask.MaskType.Mask010;
                    break;

                case "111100010011101":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Low;
                    this.Mask = XORMask.MaskType.Mask011;
                    break;

                case "110011000101111":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Low;
                    this.Mask = XORMask.MaskType.Mask100;
                    break;

                case "110001100011000":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Low;
                    this.Mask = XORMask.MaskType.Mask101;
                    break;

                case "110110001000001":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Low;
                    this.Mask = XORMask.MaskType.Mask110;
                    break;

                case "110100101110110":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Low;
                    this.Mask = XORMask.MaskType.Mask111;
                    break;

                case "101010000010010":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Medium;
                    this.Mask = XORMask.MaskType.Mask000;
                    break;

                case "101000100100101":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Medium;
                    this.Mask = XORMask.MaskType.Mask001;
                    break;

                case "101111001111100":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Medium;
                    this.Mask = XORMask.MaskType.Mask010;
                    break;

                case "101101101001011":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Medium;
                    this.Mask = XORMask.MaskType.Mask011;
                    break;

                case "100010111111001":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Medium;
                    this.Mask = XORMask.MaskType.Mask100;
                    break;

                case "100000011001110":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Medium;
                    this.Mask = XORMask.MaskType.Mask101;
                    break;

                case "100111110010111":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Medium;
                    this.Mask = XORMask.MaskType.Mask110;
                    break;

                case "100101010100000":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Medium;
                    this.Mask = XORMask.MaskType.Mask111;
                    break;

                case "011010101011111":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Quartile;
                    this.Mask = XORMask.MaskType.Mask000;
                    break;

                case "011000001101000":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Quartile;
                    this.Mask = XORMask.MaskType.Mask001;
                    break;

                case "011111100110001":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Quartile;
                    this.Mask = XORMask.MaskType.Mask010;
                    break;

                case "011101000000110":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Quartile;
                    this.Mask = XORMask.MaskType.Mask011;
                    break;

                case "010010010110100":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Quartile;
                    this.Mask = XORMask.MaskType.Mask100;
                    break;

                case "010000110000011":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Quartile;
                    this.Mask = XORMask.MaskType.Mask101;
                    break;

                case "010111011011010":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Quartile;
                    this.Mask = XORMask.MaskType.Mask110;
                    break;

                case "010101111101101":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.Quartile;
                    this.Mask = XORMask.MaskType.Mask111;
                    break;

                case "001011010001001":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.High;
                    this.Mask = XORMask.MaskType.Mask000;
                    break;

                case "001001110111110":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.High;
                    this.Mask = XORMask.MaskType.Mask001;
                    break;

                case "001110011100111":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.High;
                    this.Mask = XORMask.MaskType.Mask010;
                    break;

                case "001100111010000":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.High;
                    this.Mask = XORMask.MaskType.Mask011;
                    break;

                case "000011101100010":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.High;
                    this.Mask = XORMask.MaskType.Mask100;
                    break;

                case "000001001010101":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.High;
                    this.Mask = XORMask.MaskType.Mask101;
                    break;

                case "000110100001100":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.High;
                    this.Mask = XORMask.MaskType.Mask110;
                    break;

                case "000100000111011":
                    this.ECCLevel = ErrorCorrectionLevel.ECCLevel.High;
                    this.Mask = XORMask.MaskType.Mask111;
                    break;

                default:
                    throw new ArgumentException($"{0} is not a valid format information string.");
            }
        }
    }
}
