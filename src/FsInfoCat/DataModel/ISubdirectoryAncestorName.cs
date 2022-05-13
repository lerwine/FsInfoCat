using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for subdirectory entities that contain the name of a file system node and a delimited list of parent subdirectory names.
    /// </summary>
    /// <seealso cref="IDbFsItemAncestorName" />
    public interface ISubdirectoryAncestorName : IDbFsItemAncestorName, IEquatable<ISubdirectoryAncestorName>
    {
        /// <summary>
        /// Gets the primary key of the parent subdirectory.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> value of the parent <see cref="ISubdirectoryRow"/> or <see langword="null"/> if there is no parent <see cref="ISubdirectoryRow"/>.</value>
        Guid? ParentId { get; }
    }
}
