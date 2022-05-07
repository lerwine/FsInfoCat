using System;

namespace FsInfoCat
{
    public interface ISubdirectoryListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, ISubdirectoryListItem, ISubdirectoryRow, ISubdirectoryAncestorName, IEquatable<ISubdirectoryListItemWithAncestorNames> { }
}
