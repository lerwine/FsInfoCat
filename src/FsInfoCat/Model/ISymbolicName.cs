using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.Model.ILocalSymbolicName" />
    /// <seealso cref="Upstream.Model.IUpstreamSymbolicName" />
    /// <seealso cref="IFileSystem.SymbolicNames" />
    /// <seealso cref="IDbContext.SymbolicNames" />
    public interface ISymbolicName : ISymbolicNameRow, IEquatable<ISymbolicName>
    {
        /// <summary>
        /// Gets the file system that this symbolic name refers to.
        /// </summary>
        /// <value>The file system entity that represents the file system type that this symbolic name refers to.</value>
        [Display(Name = nameof(Properties.Resources.FileSystem), ResourceType = typeof(Properties.Resources))]
        IFileSystem FileSystem { get; }

        /// <summary>
        /// Attempts to get the primary key of the associated filesystem.
        /// </summary>
        /// <param name="fileSystemId">The <see cref="IHasSimpleIdentifier.Id"/> of the associated <see cref="IFileSystem"/>.</param>
        /// <returns><see langword="true"/> if <see cref="ISymbolicNameRow.FileSystemId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGetFileSystemId(out Guid fileSystemId);
    }
}
