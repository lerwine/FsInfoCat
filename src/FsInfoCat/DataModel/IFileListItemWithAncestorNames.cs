using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="IDbFsItemListItemWithAncestorNames" />
    /// <seealso cref="IFileRow" />
    /// <seealso cref="IFileAncestorName" />
    /// <seealso cref="IEquatable{IFileListItemWithAncestorNames}" />
    public interface IFileListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, IFileRow, IFileAncestorName, IEquatable<IFileListItemWithAncestorNames> { }
}
