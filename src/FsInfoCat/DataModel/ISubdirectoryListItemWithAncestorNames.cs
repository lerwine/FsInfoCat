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
    /// <seealso cref="Local.ILocalSubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="Upstream.IUpstreamSubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="IDbContext.SubdirectoryListingWithAncestorNames" />
    [System.Obsolete("Use FsInfoCat.Model.ISubdirectoryListItemWithAncestorNames")]
    public interface ISubdirectoryListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, ISubdirectoryListItem, ISubdirectoryRow, ISubdirectoryAncestorName,
        IEquatable<ISubdirectoryListItemWithAncestorNames> { }
}
