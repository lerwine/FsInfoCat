using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Describes a host platform.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IHostPlatform : IHostPlatformRow, IEquatable<IHostPlatform>
    {
        /// <summary>
        /// Gets teh default file system type.
        /// </summary>
        /// <value>The default file system type for the current platform or <see langword="null"/> if there is no clear default fie system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_HostPlatform_DefaultFsType), ShortName = nameof(Properties.Resources.DisplayName_DefaultFsType),
            Description = nameof(Properties.Resources.Description_HostPlatform_DefaultFsType), ResourceType = typeof(Properties.Resources))]
        IUpstreamFileSystem DefaultFsType { get; }

        /// <summary>
        /// Gets the host devices for this platform.
        /// </summary>
        /// <value>The host devices for this platform.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_HostDevices), ShortName = nameof(Properties.Resources.DisplayName_HostDevices),
            Description = nameof(Properties.Resources.Description_HostPlatform_HostDevices), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IHostDevice> HostDevices { get; }
    }
}
