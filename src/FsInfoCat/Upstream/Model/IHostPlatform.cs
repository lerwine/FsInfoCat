using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Describes a host platform.
    /// </summary>
    /// <seealso cref="IHostPlatformRow" />
    /// <seealso cref="IEquatable{IHostPlatform}" />
    public interface IHostPlatform : IHostPlatformRow, IEquatable<IHostPlatform>
    {
        /// <summary>
        /// Gets teh default file system type.
        /// </summary>
        /// <value>The default file system type for the current platform or <see langword="null"/> if there is no clear default fie system type.</value>
        [Display(Name = nameof(Properties.Resources.DefaultFileSystemType), ShortName = nameof(Properties.Resources.DefaultFsType),
            Description = nameof(Properties.Resources.Description_DefaultFileSystemType), ResourceType = typeof(Properties.Resources))]
        IUpstreamFileSystem DefaultFsType { get; }

        /// <summary>
        /// Gets the host devices for this platform.
        /// </summary>
        /// <value>The host devices for this platform.</value>
        [Display(Name = nameof(Properties.Resources.HostDevices), ShortName = nameof(Properties.Resources.HostDevices),
            Description = nameof(Properties.Resources.Description_PlatformHostDevices), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IHostDevice> HostDevices { get; }
    }
}
