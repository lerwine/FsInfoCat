using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public interface ILocalVolumeRow : ILocalDbEntity, IVolumeRow { }

    public interface ILocalVolumeListItem : ILocalVolumeRow, IVolumeListItem { }

    public interface ILocalVolumeListItemWithFileSystem : ILocalVolumeListItem, IVolumeListItemWithFileSystem { }

    /// <summary>Interface for entities which represent a logical file system volume on the local host machine.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IVolume" />
    public interface ILocalVolume : ILocalVolumeRow, IVolume
    {
        /// <summary>Gets the root directory of this volume.</summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory RootDirectory { get; }

        /// <summary>Gets the file system type.</summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new ILocalFileSystem FileSystem { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalVolumeAccessError> AccessErrors { get; }

        new IEnumerable<ILocalPersonalVolumeTag> PersonalTags { get; }

        new IEnumerable<ILocalSharedVolumeTag> SharedTags { get; }
    }
}
