using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for entities which represent a specific file system type.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.Model.ILocalFileSystem" />
    /// <seealso cref="Upstream.Model.IUpstreamFileSystem" />
    /// <seealso cref="IVolume.FileSystem" />
    /// <seealso cref="ISymbolicName.FileSystem" />
    /// <seealso cref="IDbContext.FileSystems" />
    public interface IFileSystem : IFileSystemRow, IEquatable<IFileSystem>
    {
        /// <summary>
        /// Gets the volumes that share this file system type.
        /// </summary>
        /// <value>The volumes that share this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IVolume> Volumes { get; }

        /// <summary>
        /// Gets the symbolic names for the current file system type.
        /// </summary>
        /// <value>The symbolic names that are used to identify the current file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISymbolicName> SymbolicNames { get; }
    }
}