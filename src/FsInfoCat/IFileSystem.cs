using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IFileSystemRow : IFileSystemProperties, IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>Gets the custom notes for this file system type.</summary>
        /// <value>The custom notes to associate with this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets a value indicating whether this file system type is inactive.</summary>
        /// <value><see langword="true" /> if this file system type is inactive; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

    }
    /// <summary>Interface for entities which represent a specific file system type.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IFileSystem : IFileSystemRow
    {
        /// <summary>Gets the volumes that share this file system type.</summary>
        /// <value>The volumes that share this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IVolume> Volumes { get; }

        /// <summary>Gets the symbolic names for the current file system type.</summary>
        /// <value>The symbolic names that are used to identify the current file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISymbolicName> SymbolicNames { get; }
    }
    public interface IFileSystemListItem : IFileSystemRow
    {
        Guid? PrimarySymbolicNameId { get; }

        string PrimarySymbolicName { get; }

        long SymbolicNameCount { get; }

        long VolumeCount { get; }
    }

}

