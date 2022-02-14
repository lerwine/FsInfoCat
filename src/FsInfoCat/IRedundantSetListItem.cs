using System;

namespace FsInfoCat
{
    public interface IRedundantSetListItem : IRedundantSetRow, IEquatable<IRedundantSetListItem>
    {
        long Length { get; }

        MD5Hash? Hash { get; }

        long RedundancyCount { get; }
    }
}
