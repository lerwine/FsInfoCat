using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for file entities that contain the name of a file system node and a delimited list of parent subdirectory names.
    /// </summary>
    /// <seealso cref="IDbFsItemAncestorName" />
    [System.Obsolete("Use FsInfoCat.Model.IFileAncestorName")]
    public interface IFileAncestorName : IDbFsItemAncestorName
    {
        /// <summary>
        /// Gets the primary key of the parent subdirectory.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> value of the parent <see cref="ISubdirectoryRow"/>.</value>
        Guid ParentId { get; }
    }
}
