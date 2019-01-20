using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeDiag.MetaInfo
{
    internal class ErrorCorrectionLevel
    {
        public enum ECCLevel
        {
            Low = 0,
            Medium = 1,
            Quartile = 2,
            High = 3
        }

        private readonly ECCGroup[] eccGroups;

        public uint ECCBytesPerBlock { get; private set; }

        private ErrorCorrectionLevel(uint eccBytesPerBlock, ECCGroup[] _eccGroups)
        {
            this.ECCBytesPerBlock = eccBytesPerBlock;
            this.eccGroups = _eccGroups;
        }

        public static ErrorCorrectionLevel GetECCLevel(ECCLevel level, uint version)
        {
            switch(level)
            {
                case ECCLevel.Low:
                    switch(version)
                    {
                        case  1: return new ErrorCorrectionLevel( 7, new ECCGroup[] { new ECCGroup( 19,  1) });
                        case  2: return new ErrorCorrectionLevel(10, new ECCGroup[] { new ECCGroup( 34,  1) });
                        case  3: return new ErrorCorrectionLevel(15, new ECCGroup[] { new ECCGroup( 55,  1) });
                        case  4: return new ErrorCorrectionLevel(20, new ECCGroup[] { new ECCGroup( 80,  1) });
                        case  5: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup( 108, 1) });
                        case  6: return new ErrorCorrectionLevel(18, new ECCGroup[] { new ECCGroup( 68,  2) });
                        case  7: return new ErrorCorrectionLevel(20, new ECCGroup[] { new ECCGroup( 78,  2) });
                        case  8: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup( 97,  2) });
                        case  9: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(116,  2) });
                        case 10: return new ErrorCorrectionLevel(18, new ECCGroup[] { new ECCGroup( 68,  2), new ECCGroup( 69,  2) });
                        case 11: return new ErrorCorrectionLevel(20, new ECCGroup[] { new ECCGroup( 81,  4) });
                        case 12: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup( 92,  2), new ECCGroup( 93,  2) });
                        case 13: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(107,  4) });
                        case 14: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(115,  3), new ECCGroup(116,  1) });
                        case 15: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup( 87,  5), new ECCGroup( 88,  1) });
                        case 16: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup( 98,  5), new ECCGroup( 99,  1) });
                        case 17: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(107,  1), new ECCGroup(108,  5) });
                        case 18: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(120,  5), new ECCGroup(121,  1) });
                        case 19: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(113,  3), new ECCGroup(114,  4) });
                        case 20: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(107,  3), new ECCGroup(108,  5) });
                        case 21: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(116,  4), new ECCGroup(117,  4) });
                        case 22: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(111,  2), new ECCGroup(112,  7) });
                        case 23: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(121,  4), new ECCGroup(122,  5) });
                        case 24: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(117,  6), new ECCGroup(118,  4) });
                        case 25: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(106,  8), new ECCGroup(107,  4) });
                        case 26: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(114, 10), new ECCGroup(115,  2) });
                        case 27: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(122,  8), new ECCGroup(123,  4) });
                        case 28: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(117,  3), new ECCGroup(118, 10) });
                        case 29: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(116,  7), new ECCGroup(117,  7) });
                        case 30: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(115,  5), new ECCGroup(116, 10) });
                        case 31: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(115, 13), new ECCGroup(116,  3) });
                        case 32: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(115, 17) });
                        case 33: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(115, 17), new ECCGroup(116,  1) });
                        case 34: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(115, 13), new ECCGroup(116,  6) });
                        case 35: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(121, 12), new ECCGroup(122,  7) });
                        case 36: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(121,  6), new ECCGroup(122, 14) });
                        case 37: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(122, 17), new ECCGroup(123,  4) });
                        case 38: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(122,  4), new ECCGroup(123, 18) });
                        case 39: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(117, 20), new ECCGroup(118,  4) });
                        case 40: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(118, 19), new ECCGroup(119,  6) });
                        default: break;
                    }
                    break;

                case ECCLevel.Medium:
                    switch (version)
                    {
                        case  1: return new ErrorCorrectionLevel(10, new ECCGroup[] { new ECCGroup(16,  1) });
                        case  2: return new ErrorCorrectionLevel(16, new ECCGroup[] { new ECCGroup(28,  1) });
                        case  3: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(44,  1) });
                        case  4: return new ErrorCorrectionLevel(18, new ECCGroup[] { new ECCGroup(32,  2) });
                        case  5: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(43,  2) });
                        case  6: return new ErrorCorrectionLevel(16, new ECCGroup[] { new ECCGroup(27,  4) });
                        case  7: return new ErrorCorrectionLevel(18, new ECCGroup[] { new ECCGroup(31,  4) });
                        case  8: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(38,  2), new ECCGroup(39,  2) });
                        case  9: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(36,  3), new ECCGroup(37,  2) });
                        case 10: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(43,  4), new ECCGroup(44,  1) });
                        case 11: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(50,  1), new ECCGroup(51,  4) });
                        case 12: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(36,  6), new ECCGroup(37,  2) });
                        case 13: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(37,  8), new ECCGroup(38,  1) });
                        case 14: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(40,  4), new ECCGroup(41,  5) });
                        case 15: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(41,  5), new ECCGroup(42,  5) });
                        case 16: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(45,  7), new ECCGroup(46,  3) });
                        case 17: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46, 10), new ECCGroup(47,  1) });
                        case 18: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(43,  9), new ECCGroup(44,  4) });
                        case 19: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(44,  3), new ECCGroup(45, 11) });
                        case 20: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(41,  3), new ECCGroup(42, 13) });
                        case 21: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(42, 17) });
                        case 22: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46, 17) });
                        case 23: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(47,  4), new ECCGroup(48, 14) });
                        case 24: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(45,  6), new ECCGroup(46, 14) });
                        case 25: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(47,  8), new ECCGroup(48, 13) });
                        case 26: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46, 19), new ECCGroup(47,  4) });
                        case 27: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(45, 22), new ECCGroup(46,  3) });
                        case 28: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(45,  3), new ECCGroup(46, 23) });
                        case 29: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(45, 21), new ECCGroup(46,  7) });
                        case 30: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(47, 19), new ECCGroup(48, 10) });
                        case 31: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46,  2), new ECCGroup(47, 29) });
                        case 32: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46, 10), new ECCGroup(47, 23) });
                        case 33: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46, 14), new ECCGroup(47, 21) });
                        case 34: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46, 14), new ECCGroup(47, 23) });
                        case 35: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(47, 12), new ECCGroup(48, 26) });
                        case 36: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(47,  6), new ECCGroup(48, 34) });
                        case 37: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46, 29), new ECCGroup(47, 14) });
                        case 38: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(46, 13), new ECCGroup(47, 32) });
                        case 39: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(47, 40), new ECCGroup(48,  7) });
                        case 40: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(47, 18), new ECCGroup(48, 31) });
                        default: break;
                    }
                    break;

                case ECCLevel.Quartile:
                    switch (version)
                    {
                        case  1: return new ErrorCorrectionLevel(13, new ECCGroup[] { new ECCGroup(13,  1) });
                        case  2: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(22,  1) });
                        case  3: return new ErrorCorrectionLevel(18, new ECCGroup[] { new ECCGroup(17,  2) });
                        case  4: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(24,  2) });
                        case  5: return new ErrorCorrectionLevel(18, new ECCGroup[] { new ECCGroup(15,  2), new ECCGroup(16,  2) });
                        case  6: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(19,  4) });
                        case  7: return new ErrorCorrectionLevel(18, new ECCGroup[] { new ECCGroup(14,  2), new ECCGroup(15,  4) });
                        case  8: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(18,  4), new ECCGroup(19,  2) });
                        case  9: return new ErrorCorrectionLevel(20, new ECCGroup[] { new ECCGroup(16,  4), new ECCGroup(17,  4) });
                        case 10: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(19,  6), new ECCGroup(20,  2) });
                        case 11: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(22,  4), new ECCGroup(23,  4) });
                        case 12: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(20,  4), new ECCGroup(21,  6) });
                        case 13: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(20,  8), new ECCGroup(21,  4) });
                        case 14: return new ErrorCorrectionLevel(20, new ECCGroup[] { new ECCGroup(16, 11), new ECCGroup(17,  5) });
                        case 15: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24,  5), new ECCGroup(25,  7) });
                        case 16: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(19, 15), new ECCGroup(20,  2) });
                        case 17: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(22,  1), new ECCGroup(23, 15) });
                        case 18: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(22, 17), new ECCGroup(23,  1) });
                        case 19: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(21, 17), new ECCGroup(22,  4) });
                        case 20: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 15), new ECCGroup(25,  5) });
                        case 21: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(22, 17), new ECCGroup(23,  6) });
                        case 22: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24,  7), new ECCGroup(25, 16) });
                        case 23: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 11), new ECCGroup(25, 14) });
                        case 24: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 11), new ECCGroup(25, 16) });
                        case 25: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24,  7), new ECCGroup(25, 22) });
                        case 26: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(22, 28), new ECCGroup(23,  6) });
                        case 27: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(23,  8), new ECCGroup(24, 26) });
                        case 28: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24,  4), new ECCGroup(25, 31) });
                        case 29: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(23,  1), new ECCGroup(24, 37) });
                        case 30: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 15), new ECCGroup(25, 25) });
                        case 31: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 42), new ECCGroup(25,  1) });
                        case 32: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 10), new ECCGroup(25, 35) });
                        case 33: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 29), new ECCGroup(25, 19) });
                        case 34: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 44), new ECCGroup(25,  7) });
                        case 35: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 39), new ECCGroup(25, 14) });
                        case 36: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 46), new ECCGroup(25, 10) });
                        case 37: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 49), new ECCGroup(25, 10) });
                        case 38: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 48), new ECCGroup(25, 14) });
                        case 39: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 43), new ECCGroup(25, 22) });
                        case 40: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(24, 34), new ECCGroup(25, 34) });
                        default: break;
                    }
                    break;
                case ECCLevel.High:
                    switch (version)
                    {
                        case  1: return new ErrorCorrectionLevel(17, new ECCGroup[] { new ECCGroup( 9,  1) });
                        case  2: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(16,  1) });
                        case  3: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(13,  2) });
                        case  4: return new ErrorCorrectionLevel(16, new ECCGroup[] { new ECCGroup( 9,  4) });
                        case  5: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(11,  2), new ECCGroup(12,  2) });
                        case  6: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(15,  4) });
                        case  7: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(13,  4), new ECCGroup(14,  1) });
                        case  8: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(14,  4), new ECCGroup(15,  2) });
                        case  9: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(12,  4), new ECCGroup(13,  4) });
                        case 10: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(15,  6), new ECCGroup(16,  2) });
                        case 11: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(12,  3), new ECCGroup(13,  8) });
                        case 12: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(14,  7), new ECCGroup(15,  4) });
                        case 13: return new ErrorCorrectionLevel(22, new ECCGroup[] { new ECCGroup(11, 12), new ECCGroup(12,  4) });
                        case 14: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(12, 11), new ECCGroup(13,  5) });
                        case 15: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(12, 11), new ECCGroup(13,  7) });
                        case 16: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15,  3), new ECCGroup(16, 13) });
                        case 17: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(14,  2), new ECCGroup(15, 17) });
                        case 18: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(14,  2), new ECCGroup(15, 19) });
                        case 19: return new ErrorCorrectionLevel(26, new ECCGroup[] { new ECCGroup(13,  9), new ECCGroup(14, 16) });
                        case 20: return new ErrorCorrectionLevel(28, new ECCGroup[] { new ECCGroup(15, 15), new ECCGroup(16, 10) });
                        case 21: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(16, 19), new ECCGroup(17,  6) });
                        case 22: return new ErrorCorrectionLevel(24, new ECCGroup[] { new ECCGroup(13, 34) });
                        case 23: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 16), new ECCGroup(16, 14) });
                        case 24: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(16, 30), new ECCGroup(17,  2) });
                        case 25: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 22), new ECCGroup(16, 13) });
                        case 26: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(16, 33), new ECCGroup(17,  4) });
                        case 27: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 12), new ECCGroup(16, 28) });
                        case 28: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 11), new ECCGroup(16, 31) });
                        case 29: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 19), new ECCGroup(16, 26) });
                        case 30: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 23), new ECCGroup(16, 25) });
                        case 31: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 23), new ECCGroup(16, 28) });
                        case 32: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 19), new ECCGroup(16, 35) });
                        case 33: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 11), new ECCGroup(16, 46) });
                        case 34: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(16, 59), new ECCGroup(17,  1) });
                        case 35: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 22), new ECCGroup(16, 41) });
                        case 36: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15,  2), new ECCGroup(16, 64) });
                        case 37: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 24), new ECCGroup(16, 46) });
                        case 38: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 42), new ECCGroup(16, 32) });
                        case 39: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 10), new ECCGroup(16, 67) });
                        case 40: return new ErrorCorrectionLevel(30, new ECCGroup[] { new ECCGroup(15, 20), new ECCGroup(16, 61) });
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
