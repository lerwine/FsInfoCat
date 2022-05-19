using M = FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Interface for entities which represent a specific file system type.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.IFileSystem" />
    /// <seealso cref="IEquatable{IUpstreamFileSystem}" />
    /// <seealso cref="Local.Model.IFileSystem" />
    public interface IUpstreamFileSystem : IUpstreamDbEntity, M.IFileSystem, IEquatable<IUpstreamFileSystem>
    {
        /// <summary>
        /// Gets the volumes that share this file system type.
        /// </summary>
        /// <value>The volumes that share this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamVolume> Volumes { get; }

        /// <summary>
        /// Gets the symbolic names for the current file system type.
        /// </summary>
        /// <value>The symbolic names that are used to identify the current file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamSymbolicName> SymbolicNames { get; }
    }
}
