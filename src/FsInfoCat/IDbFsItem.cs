using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a structural instance of filesystem entity.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface IDbFsItem : IDbEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the current file system item.
        /// </summary>
        /// <value>The database primary key for the current file system item.</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the current file system item.
        /// </summary>
        /// <value>The name of the current file system item.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the date and time last accessed.
        /// </summary>
        /// <value>The last accessed for the purposes of this application.</value>
        DateTime LastAccessed { get; set; }

        /// <summary>
        /// Gets or sets custom notes to be associated with the current file system item.
        /// </summary>
        /// <value>The notes.</value>
        string Notes { get; set; }

        /// <summary>
        /// Gets or sets the file's creation time.
        /// </summary>
        /// <value>The creation time as reported by the host file system.</value>
        DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time the file system item was last written nto.
        /// </summary>
        /// <value>The last write time as reported by the host file system.</value>
        DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets or sets the parent subdirectory of the current file system item.
        /// </summary>
        /// <value>The parent <see cref="ISubdirectory"/> of the current file system item.</value>
        ISubdirectory Parent { get; set; }

        /// <summary>
        /// Gets the access errors that occurred while attempting to access teh current file system item.
        /// </summary>
        /// <value>The access errors that occurred while attempting to access teh current file system item.</value>
        IEnumerable<IAccessError> AccessErrors { get; }
    }
}
