using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Base interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IFile" />
    /// <seealso cref="ISubdirectory" />
    /// <seealso cref="Local.ILocalDbFsItem" />
    /// <seealso cref="Upstream.IUpstreamDbFsItem" />
    public interface IDbFsItem : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
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
