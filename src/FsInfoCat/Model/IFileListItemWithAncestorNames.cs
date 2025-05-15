using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="IFileListItemWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="IFile" />
    /// <seealso cref="IFileListItemWithBinaryProperties" />
    /// <seealso cref="ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="IDbContext.FileListingWithAncestorNames"/>
    public interface IFileListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, IFileRow, IFileAncestorName, IEquatable<IFileListItemWithAncestorNames> { }
}
