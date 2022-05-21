using FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Interface for entities which represent a logical file system volume on the local host machine.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IVolume" />
    /// <seealso cref="Upstream.Model.IUpstreamVolume" />
    /// <seealso cref="IEquatable{ILocalVolume}" />
    public interface ILocalVolume : ILocalVolumeRow, IVolume, IEquatable<ILocalVolume>
    {
        /// <summary>
        /// Gets the root directory of this volume.
        /// </summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory RootDirectory { get; }

        /// <summary>
        /// Gets the file system type.
        /// </summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new ILocalFileSystem FileSystem { get; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalVolumeAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current volume.
        /// </summary>
        /// <value>The <see cref="ILocalPersonalVolumeTag"/> entities that associate <see cref="ILocalPersonalTagDefinition"/> entities with the current volume.</value>
        new IEnumerable<ILocalPersonalVolumeTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current volume.
        /// </summary>
        /// <value>The <see cref="ILocalSharedVolumeTag"/> entities that associate <see cref="ILocalSharedTagDefinition"/> entities with the current volume.</value>
        new IEnumerable<ILocalSharedVolumeTag> SharedTags { get; }
    }
}
