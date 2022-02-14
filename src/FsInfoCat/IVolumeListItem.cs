using System;

namespace FsInfoCat
{
    public interface IVolumeListItem : IVolumeRow, IEquatable<IVolumeListItem>
    {
        string RootPath { get; }

        long RootSubdirectoryCount { get; }

        long RootFileCount { get; }

        long AccessErrorCount { get; }

        long SharedTagCount { get; }

        long PersonalTagCount { get; }
    }
}
