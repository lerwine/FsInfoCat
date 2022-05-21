using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for a list item entity representing a filesystem symbolic name.
    /// </summary>
    /// <seealso cref="ISymbolicNameRow" />
    /// <seealso cref="IEquatable{ISymbolicNameListItem}" />
    /// <seealso cref="Local.Model.ILocalSymbolicNameListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamSymbolicNameListItem" />
    /// <seealso cref="IDbContext.SymbolicNameListing" />
    public interface ISymbolicNameListItem : ISymbolicNameRow, IEquatable<ISymbolicNameListItem>
    {
        /// <summary>
        /// Gets the display name of the associated filesystem.
        /// </summary>
        /// <value>The <see cref="IFileSystemProperties.DisplayName"/> of the associated <see cref="IFileSystemRow"/>.</value>
        string FileSystemDisplayName { get; }
    }
}
