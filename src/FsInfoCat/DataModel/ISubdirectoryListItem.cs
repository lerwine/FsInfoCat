using System;

namespace FsInfoCat
{
    public interface ISubdirectoryListItem : IDbFsItemListItem, ISubdirectoryRow, IEquatable<ISubdirectoryListItem>
    {
        long SubdirectoryCount { get; }

        long FileCount { get; }

        string CrawlConfigDisplayName { get; }
    }
}
