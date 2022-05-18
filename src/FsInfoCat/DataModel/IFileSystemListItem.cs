using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for file system list item entities.
    /// </summary>
    /// <seealso cref="IFileSystemRow" />
    /// <seealso cref="IEquatable{IFileSystemListItem}" />
    /// <seealso cref="Local.ILocalFileSystemListItem" />
    /// <seealso cref="Upstream.IUpstreamFileSystemListItem" />
    /// <seealso cref="IDbContext.FileSystemListing" />
    public interface IFileSystemListItem : IFileSystemRow, IEquatable<IFileSystemListItem>
    {
        /// <summary>
        /// Gets the primary symbolic name identifier.
        /// </summary>
        /// <value>The primary symbolic name identifier.</value>
        Guid? PrimarySymbolicNameId { get; }

        /// <summary>
        /// Gets the name of the primary symbolic.
        /// </summary>
        /// <value>The name of the primary symbolic.</value>
        string PrimarySymbolicName { get; }

        /// <summary>
        /// Gets the symbolic name count.
        /// </summary>
        /// <value>The symbolic name count.</value>
        long SymbolicNameCount { get; }

        /// <summary>
        /// Gets the volume count.
        /// </summary>
        /// <value>The volume count.</value>
        long VolumeCount { get; }
    }
}
