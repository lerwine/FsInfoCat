using System;

namespace FsInfoCat
{
    [Flags]
    public enum FileCrawlOptions : byte
    {
        None = 0b0000_0000,
        DoNotCompare = 0b0000_0001,
        DoNotShow = 0b0000_0010,
        FlaggedForDeletion = 0b0000_0100,
        FlaggedForRescan = 0b0000_1000
    }
}
