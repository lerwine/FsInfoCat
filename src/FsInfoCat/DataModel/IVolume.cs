using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{

    /// <summary>
    /// Interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.ILocalVolume" />
    /// <seealso cref="Upstream.IUpstreamVolume" />
    public interface IVolume : IVolumeRow, IEquatable<IVolume>
    {
        /// <summary>
        /// Gets the file system type.
        /// </summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        IFileSystem FileSystem { get; }

        /// <summary>
        /// Gets the root directory of this volume.
        /// </summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        ISubdirectory RootDirectory { get; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IVolumeAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current volume.
        /// </summary>
        /// <value>The <see cref="IPersonalVolumeTag"/> entities that associate <see cref="IPersonalTagDefinition"/> entities with the current volume.</value>
        IEnumerable<IPersonalVolumeTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current volume.
        /// </summary>
        /// <value>The <see cref="ISharedVolumeTag"/> entities that associate <see cref="ISharedTagDefinition"/> entities with the current volume.</value>
        IEnumerable<ISharedVolumeTag> SharedTags { get; }

        /// <summary>
        /// Attempts to get the primary key of the associated filesystem.
        /// </summary>
        /// <param name="fileSystemId">The <see cref="IHasSimpleIdentifier.Id"/> of the associated <see cref="IFileSystem"/>.</param>
        /// <returns><see langword="true"/> if <see cref="IVolumeRow.FileSystemId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetFileSystemId(out Guid fileSystemId);
    }
}
