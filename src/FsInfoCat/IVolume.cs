using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{

    /// <summary>Interface for entities which represent a logical file system volume.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IVolume : IVolumeRow, IEquatable<IVolume>
    {
        /// <summary>Gets the file system type.</summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        IFileSystem FileSystem { get; }

        /// <summary>Gets the root directory of this volume.</summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        ISubdirectory RootDirectory { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IVolumeAccessError> AccessErrors { get; }

        IEnumerable<IPersonalVolumeTag> PersonalTags { get; }

        IEnumerable<ISharedVolumeTag> SharedTags { get; }
    }
}
