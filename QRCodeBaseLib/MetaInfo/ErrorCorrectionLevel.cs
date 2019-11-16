using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeBaseLib.MetaInfo
{
    public class ErrorCorrectionLevel
    {
        public enum ECCLevel
        {
            Low = 0,
            Medium = 1,
            Quartile = 2,
            High = 3
        }

        private readonly ECCGroupInfo[] eccGroups;

        public uint ECCBytesPerBlock { get; private set; }
        public ECCLevel Level { get; private set; }

        private ErrorCorrectionLevel(ECCLevel level, uint eccBytesPerBlock, ECCGroupInfo[] _eccGroups)
        {
            this.ECCBytesPerBlock = eccBytesPerBlock;
            this.eccGroups = _eccGroups;
            this.Level = level;
        }

        public ECCGroupInfo[] GetECCGroups()
        {
            return this.eccGroups;
        }

        public static ErrorCorrectionLevel GetECCLevel(ECCLevel level, QRCodeVersion version)
        {
            switch(level)
            {
                case ECCLevel.Low:
                    switch(version.VersionNumber)
                    {
                        case  1: return new ErrorCorrectionLevel(level,  7, new ECCGroupInfo[] { new ECCGroupInfo( 19,  1) });
                        case  2: return new ErrorCorrectionLevel(level, 10, new ECCGroupInfo[] { new ECCGroupInfo( 34,  1) });
                        case  3: return new ErrorCorrectionLevel(level, 15, new ECCGroupInfo[] { new ECCGroupInfo( 55,  1) });
                        case  4: return new ErrorCorrectionLevel(level, 20, new ECCGroupInfo[] { new ECCGroupInfo( 80,  1) });
                        case  5: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo( 108, 1) });
                        case  6: return new ErrorCorrectionLevel(level, 18, new ECCGroupInfo[] { new ECCGroupInfo( 68,  2) });
                        case  7: return new ErrorCorrectionLevel(level, 20, new ECCGroupInfo[] { new ECCGroupInfo( 78,  2) });
                        case  8: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo( 97,  2) });
                        case  9: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(116,  2) });
                        case 10: return new ErrorCorrectionLevel(level, 18, new ECCGroupInfo[] { new ECCGroupInfo( 68,  2), new ECCGroupInfo( 69,  2) });
                        case 11: return new ErrorCorrectionLevel(level, 20, new ECCGroupInfo[] { new ECCGroupInfo( 81,  4) });
                        case 12: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo( 92,  2), new ECCGroupInfo( 93,  2) });
                        case 13: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(107,  4) });
                        case 14: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(115,  3), new ECCGroupInfo(116,  1) });
                        case 15: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo( 87,  5), new ECCGroupInfo( 88,  1) });
                        case 16: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo( 98,  5), new ECCGroupInfo( 99,  1) });
                        case 17: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(107,  1), new ECCGroupInfo(108,  5) });
                        case 18: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(120,  5), new ECCGroupInfo(121,  1) });
                        case 19: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(113,  3), new ECCGroupInfo(114,  4) });
                        case 20: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(107,  3), new ECCGroupInfo(108,  5) });
                        case 21: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(116,  4), new ECCGroupInfo(117,  4) });
                        case 22: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(111,  2), new ECCGroupInfo(112,  7) });
                        case 23: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(121,  4), new ECCGroupInfo(122,  5) });
                        case 24: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(117,  6), new ECCGroupInfo(118,  4) });
                        case 25: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(106,  8), new ECCGroupInfo(107,  4) });
                        case 26: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(114, 10), new ECCGroupInfo(115,  2) });
                        case 27: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(122,  8), new ECCGroupInfo(123,  4) });
                        case 28: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(117,  3), new ECCGroupInfo(118, 10) });
                        case 29: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(116,  7), new ECCGroupInfo(117,  7) });
                        case 30: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(115,  5), new ECCGroupInfo(116, 10) });
                        case 31: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(115, 13), new ECCGroupInfo(116,  3) });
                        case 32: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(115, 17) });
                        case 33: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(115, 17), new ECCGroupInfo(116,  1) });
                        case 34: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(115, 13), new ECCGroupInfo(116,  6) });
                        case 35: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(121, 12), new ECCGroupInfo(122,  7) });
                        case 36: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(121,  6), new ECCGroupInfo(122, 14) });
                        case 37: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(122, 17), new ECCGroupInfo(123,  4) });
                        case 38: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(122,  4), new ECCGroupInfo(123, 18) });
                        case 39: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(117, 20), new ECCGroupInfo(118,  4) });
                        case 40: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(118, 19), new ECCGroupInfo(119,  6) });
                        default: break;
                    }
                    break;

                case ECCLevel.Medium:
                    switch (version.VersionNumber)
                    {
                        case  1: return new ErrorCorrectionLevel(level, 10, new ECCGroupInfo[] { new ECCGroupInfo(16,  1) });
                        case  2: return new ErrorCorrectionLevel(level, 16, new ECCGroupInfo[] { new ECCGroupInfo(28,  1) });
                        case  3: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(44,  1) });
                        case  4: return new ErrorCorrectionLevel(level, 18, new ECCGroupInfo[] { new ECCGroupInfo(32,  2) });
                        case  5: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(43,  2) });
                        case  6: return new ErrorCorrectionLevel(level, 16, new ECCGroupInfo[] { new ECCGroupInfo(27,  4) });
                        case  7: return new ErrorCorrectionLevel(level, 18, new ECCGroupInfo[] { new ECCGroupInfo(31,  4) });
                        case  8: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(38,  2), new ECCGroupInfo(39,  2) });
                        case  9: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(36,  3), new ECCGroupInfo(37,  2) });
                        case 10: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(43,  4), new ECCGroupInfo(44,  1) });
                        case 11: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(50,  1), new ECCGroupInfo(51,  4) });
                        case 12: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(36,  6), new ECCGroupInfo(37,  2) });
                        case 13: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(37,  8), new ECCGroupInfo(38,  1) });
                        case 14: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(40,  4), new ECCGroupInfo(41,  5) });
                        case 15: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(41,  5), new ECCGroupInfo(42,  5) });
                        case 16: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(45,  7), new ECCGroupInfo(46,  3) });
                        case 17: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46, 10), new ECCGroupInfo(47,  1) });
                        case 18: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(43,  9), new ECCGroupInfo(44,  4) });
                        case 19: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(44,  3), new ECCGroupInfo(45, 11) });
                        case 20: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(41,  3), new ECCGroupInfo(42, 13) });
                        case 21: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(42, 17) });
                        case 22: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46, 17) });
                        case 23: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(47,  4), new ECCGroupInfo(48, 14) });
                        case 24: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(45,  6), new ECCGroupInfo(46, 14) });
                        case 25: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(47,  8), new ECCGroupInfo(48, 13) });
                        case 26: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46, 19), new ECCGroupInfo(47,  4) });
                        case 27: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(45, 22), new ECCGroupInfo(46,  3) });
                        case 28: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(45,  3), new ECCGroupInfo(46, 23) });
                        case 29: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(45, 21), new ECCGroupInfo(46,  7) });
                        case 30: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(47, 19), new ECCGroupInfo(48, 10) });
                        case 31: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46,  2), new ECCGroupInfo(47, 29) });
                        case 32: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46, 10), new ECCGroupInfo(47, 23) });
                        case 33: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46, 14), new ECCGroupInfo(47, 21) });
                        case 34: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46, 14), new ECCGroupInfo(47, 23) });
                        case 35: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(47, 12), new ECCGroupInfo(48, 26) });
                        case 36: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(47,  6), new ECCGroupInfo(48, 34) });
                        case 37: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46, 29), new ECCGroupInfo(47, 14) });
                        case 38: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(46, 13), new ECCGroupInfo(47, 32) });
                        case 39: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(47, 40), new ECCGroupInfo(48,  7) });
                        case 40: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(47, 18), new ECCGroupInfo(48, 31) });
                        default: break;
                    }
                    break;

                case ECCLevel.Quartile:
                    switch (version.VersionNumber)
                    {
                        case  1: return new ErrorCorrectionLevel(level, 13, new ECCGroupInfo[] { new ECCGroupInfo(13,  1) });
                        case  2: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(22,  1) });
                        case  3: return new ErrorCorrectionLevel(level, 18, new ECCGroupInfo[] { new ECCGroupInfo(17,  2) });
                        case  4: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(24,  2) });
                        case  5: return new ErrorCorrectionLevel(level, 18, new ECCGroupInfo[] { new ECCGroupInfo(15,  2), new ECCGroupInfo(16,  2) });
                        case  6: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(19,  4) });
                        case  7: return new ErrorCorrectionLevel(level, 18, new ECCGroupInfo[] { new ECCGroupInfo(14,  2), new ECCGroupInfo(15,  4) });
                        case  8: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(18,  4), new ECCGroupInfo(19,  2) });
                        case  9: return new ErrorCorrectionLevel(level, 20, new ECCGroupInfo[] { new ECCGroupInfo(16,  4), new ECCGroupInfo(17,  4) });
                        case 10: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(19,  6), new ECCGroupInfo(20,  2) });
                        case 11: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(22,  4), new ECCGroupInfo(23,  4) });
                        case 12: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(20,  4), new ECCGroupInfo(21,  6) });
                        case 13: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(20,  8), new ECCGroupInfo(21,  4) });
                        case 14: return new ErrorCorrectionLevel(level, 20, new ECCGroupInfo[] { new ECCGroupInfo(16, 11), new ECCGroupInfo(17,  5) });
                        case 15: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24,  5), new ECCGroupInfo(25,  7) });
                        case 16: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(19, 15), new ECCGroupInfo(20,  2) });
                        case 17: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(22,  1), new ECCGroupInfo(23, 15) });
                        case 18: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(22, 17), new ECCGroupInfo(23,  1) });
                        case 19: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(21, 17), new ECCGroupInfo(22,  4) });
                        case 20: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 15), new ECCGroupInfo(25,  5) });
                        case 21: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(22, 17), new ECCGroupInfo(23,  6) });
                        case 22: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24,  7), new ECCGroupInfo(25, 16) });
                        case 23: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 11), new ECCGroupInfo(25, 14) });
                        case 24: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 11), new ECCGroupInfo(25, 16) });
                        case 25: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24,  7), new ECCGroupInfo(25, 22) });
                        case 26: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(22, 28), new ECCGroupInfo(23,  6) });
                        case 27: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(23,  8), new ECCGroupInfo(24, 26) });
                        case 28: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24,  4), new ECCGroupInfo(25, 31) });
                        case 29: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(23,  1), new ECCGroupInfo(24, 37) });
                        case 30: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 15), new ECCGroupInfo(25, 25) });
                        case 31: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 42), new ECCGroupInfo(25,  1) });
                        case 32: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 10), new ECCGroupInfo(25, 35) });
                        case 33: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 29), new ECCGroupInfo(25, 19) });
                        case 34: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 44), new ECCGroupInfo(25,  7) });
                        case 35: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 39), new ECCGroupInfo(25, 14) });
                        case 36: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 46), new ECCGroupInfo(25, 10) });
                        case 37: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 49), new ECCGroupInfo(25, 10) });
                        case 38: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 48), new ECCGroupInfo(25, 14) });
                        case 39: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 43), new ECCGroupInfo(25, 22) });
                        case 40: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(24, 34), new ECCGroupInfo(25, 34) });
                        default: break;
                    }
                    break;
                case ECCLevel.High:
                    switch (version.VersionNumber)
                    {
                        case  1: return new ErrorCorrectionLevel(level, 17, new ECCGroupInfo[] { new ECCGroupInfo( 9,  1) });
                        case  2: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(16,  1) });
                        case  3: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(13,  2) });
                        case  4: return new ErrorCorrectionLevel(level, 16, new ECCGroupInfo[] { new ECCGroupInfo( 9,  4) });
                        case  5: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(11,  2), new ECCGroupInfo(12,  2) });
                        case  6: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(15,  4) });
                        case  7: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(13,  4), new ECCGroupInfo(14,  1) });
                        case  8: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(14,  4), new ECCGroupInfo(15,  2) });
                        case  9: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(12,  4), new ECCGroupInfo(13,  4) });
                        case 10: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(15,  6), new ECCGroupInfo(16,  2) });
                        case 11: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(12,  3), new ECCGroupInfo(13,  8) });
                        case 12: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(14,  7), new ECCGroupInfo(15,  4) });
                        case 13: return new ErrorCorrectionLevel(level, 22, new ECCGroupInfo[] { new ECCGroupInfo(11, 12), new ECCGroupInfo(12,  4) });
                        case 14: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(12, 11), new ECCGroupInfo(13,  5) });
                        case 15: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(12, 11), new ECCGroupInfo(13,  7) });
                        case 16: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15,  3), new ECCGroupInfo(16, 13) });
                        case 17: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(14,  2), new ECCGroupInfo(15, 17) });
                        case 18: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(14,  2), new ECCGroupInfo(15, 19) });
                        case 19: return new ErrorCorrectionLevel(level, 26, new ECCGroupInfo[] { new ECCGroupInfo(13,  9), new ECCGroupInfo(14, 16) });
                        case 20: return new ErrorCorrectionLevel(level, 28, new ECCGroupInfo[] { new ECCGroupInfo(15, 15), new ECCGroupInfo(16, 10) });
                        case 21: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(16, 19), new ECCGroupInfo(17,  6) });
                        case 22: return new ErrorCorrectionLevel(level, 24, new ECCGroupInfo[] { new ECCGroupInfo(13, 34) });
                        case 23: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 16), new ECCGroupInfo(16, 14) });
                        case 24: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(16, 30), new ECCGroupInfo(17,  2) });
                        case 25: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 22), new ECCGroupInfo(16, 13) });
                        case 26: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(16, 33), new ECCGroupInfo(17,  4) });
                        case 27: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 12), new ECCGroupInfo(16, 28) });
                        case 28: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 11), new ECCGroupInfo(16, 31) });
                        case 29: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 19), new ECCGroupInfo(16, 26) });
                        case 30: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 23), new ECCGroupInfo(16, 25) });
                        case 31: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 23), new ECCGroupInfo(16, 28) });
                        case 32: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 19), new ECCGroupInfo(16, 35) });
                        case 33: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 11), new ECCGroupInfo(16, 46) });
                        case 34: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(16, 59), new ECCGroupInfo(17,  1) });
                        case 35: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 22), new ECCGroupInfo(16, 41) });
                        case 36: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15,  2), new ECCGroupInfo(16, 64) });
                        case 37: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 24), new ECCGroupInfo(16, 46) });
                        case 38: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 42), new ECCGroupInfo(16, 32) });
                        case 39: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 10), new ECCGroupInfo(16, 67) });
                        case 40: return new ErrorCorrectionLevel(level, 30, new ECCGroupInfo[] { new ECCGroupInfo(15, 20), new ECCGroupInfo(16, 61) });
                        default: break;
                    }
                    break;

                default:
                    break;
            }
            throw new ArgumentException("Invalid ECCLevel or version");
        }
    }
}
