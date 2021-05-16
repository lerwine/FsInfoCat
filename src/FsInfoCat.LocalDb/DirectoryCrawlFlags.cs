using System;

namespace FsInfoCat.LocalDb
{
    [Flags]
    public enum DirectoryCrawlFlags : byte
    {
        None = 0b0000_0000,
        SkipSubdirectories = 0b0000_0001,
        Skip = 0b0000_0010,
        DoNotCompareFiles = 0b0000_0100,
        DoNotShow = 0b0000_1000,
        FlaggedForDeletion = 0b0001_0000,
        FlaggedForRescan = 0b0010_0000
    }
}
