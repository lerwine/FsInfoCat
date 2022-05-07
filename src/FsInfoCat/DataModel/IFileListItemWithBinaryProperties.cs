using System;

namespace FsInfoCat
{
    public interface IFileListItemWithBinaryProperties : IDbFsItemListItem, IFileRow, IEquatable<IFileListItemWithBinaryProperties>
    {
        long Length { get; }

        MD5Hash? Hash { get; }

        long RedundancyCount { get; }

        long ComparisonCount { get; }
    }
}
