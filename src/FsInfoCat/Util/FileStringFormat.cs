using System;

namespace FsInfoCat
{
    [Flags]
    public enum FileStringFormat
    {
        RelativeLocalPath           = 0b000_0000,
        WellFormedRelativeLocalPath = 0b000_0001,
        RelativeAltPath             = 0b000_0010,
        WellFormedRelativeAltPath   = 0b000_0011,
        AbsoluteLocalPath           = 0b000_0100,
        WellFormedAbsoluteLocalPath = 0b000_0101,
        AbsoluteAltPath             = 0b000_0110,
        WellFormedAbsoluteAltPath   = 0b000_0111,
        RelativeLocalUri            = 0b000_1000,
        WellformedRelativeLocalUri  = 0b000_1001,
        RelativeAltUri              = 0b000_1010,
        WellformedRelativeAltUri    = 0b000_1011,
        AbsoluteLocalUrl            = 0b000_1100,
        WellformedAbsoluteLocalUrl  = 0b000_1101,
        AbsoluteAltUrl              = 0b000_1110,
        WellformedAbsoluteAltUrl    = 0b000_1111,
        LocalUrn                    = 0b001_0100,
        WellFormedLocalUrn          = 0b001_0101,
        AltUrn                      = 0b001_0110,
        WellFormedAltUrn            = 0b001_0111,
        NonFileUrl                  = 0b010_0000,
        WellFormedNonFileUrl        = 0b010_0001,
        Invalid                     = 0b100_0000,
        WellFormed                  = 0b000_0001,
        Alt                         = 0b000_0010,
        Absolute                    = 0b000_0100,
        FileUrl                     = 0b000_1000,
        URN                         = 0b001_0000
    }
}
