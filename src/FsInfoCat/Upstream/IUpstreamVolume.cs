using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Interface for entities which represent a logical file system volume.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IVolume" />
    public interface IUpstreamVolume : IUpstreamDbEntity, IVolume
    {
        /// <summary>Gets the device that hosts the current volume.</summary>
        /// <value>The device that hosts the current volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_HostDevice), ResourceType = typeof(Properties.Resources))]
        IHostDevice HostDevice { get; }

        /// <summary>Gets the root directory of this volume.</summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory RootDirectory { get; }

        /// <summary>Gets the file system type.</summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFileSystem FileSystem { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamVolumeAccessError> AccessErrors { get; }
    }
}
