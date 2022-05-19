using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for a list item entity representing a filesystem symbolic name.
    /// </summary>
    /// <seealso cref="ISymbolicNameRow" />
    /// <seealso cref="IEquatable{ISymbolicNameListItem}" />
    /// <seealso cref="Local.ILocalSymbolicNameListItem" />
    /// <seealso cref="Upstream.IUpstreamSymbolicNameListItem" />
    /// <seealso cref="IDbContext.SymbolicNameListing" />
    [System.Obsolete("Use FsInfoCat.Model.ISymbolicNameListItem")]
    public interface ISymbolicNameListItem : ISymbolicNameRow, IEquatable<ISymbolicNameListItem>
    {
        /// <summary>
        /// Gets the display name of the associated filesystem.
        /// </summary>
        /// <value>The <see cref="IFileSystemProperties.DisplayName"/> of the associated <see cref="IFileSystemRow"/>.</value>
        string FileSystemDisplayName { get; }
    }
}
