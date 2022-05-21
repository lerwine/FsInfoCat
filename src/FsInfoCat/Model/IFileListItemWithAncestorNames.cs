using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="IDbFsItemListItemWithAncestorNames" />
    /// <seealso cref="IFileRow" />
    /// <seealso cref="IFileAncestorName" />
    /// <seealso cref="IEquatable{IFileListItemWithAncestorNames}" />
    /// <seealso cref="Local.Model.ILocalFileListItemWithAncestorNames" />
    /// <seealso cref="Upstream.Model.IUpstreamFileListItemWithAncestorNames" />
    /// <seealso cref="IDbContext.FileListingWithAncestorNames" />
    public interface IFileListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, IFileRow, IFileAncestorName, IEquatable<IFileListItemWithAncestorNames> { }
}
