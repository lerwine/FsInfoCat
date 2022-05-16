using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory and contains the names of the ancestor subdirectories.
    /// </summary>
    /// <seealso cref="IDbFsItemListItemWithAncestorNames" />
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="ISubdirectoryRow" />
    /// <seealso cref="ISubdirectoryAncestorName" />
    /// <seealso cref="IEquatable{ISubdirectoryListItemWithAncestorNames}" />
    public interface ISubdirectoryListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, ISubdirectoryListItem, ISubdirectoryRow, ISubdirectoryAncestorName,
        IEquatable<ISubdirectoryListItemWithAncestorNames> { }
}
