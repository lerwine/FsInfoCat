using M = FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Identifies a device that hosts one or more <see cref="IUpstreamVolume">volumes</see>.
    /// </summary>
    /// <seealso cref="IHostDeviceRow" />
    /// <seealso cref="IEquatable{IHostDevice}" />
    public interface IHostDevice : IHostDeviceRow, IEquatable<IHostDevice>
    {
        /// <summary>
        /// Gets the host platform type.
        /// </summary>
        /// <value>The <see cref="IHostPlatform"/> that describes the host's platform type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Platform), ShortName = nameof(Properties.Resources.DisplayName_Platform),
            Description = nameof(Properties.Resources.Description_HostDevice_Platform), ResourceType = typeof(Properties.Resources))]
        IHostPlatform Platform { get; }

        /// <summary>
        /// Gets the volumes for the host.
        /// </summary>
        /// <value>The <see cref="IUpstreamVolume">volumes</see> that were crawled on the current host platform.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ShortName = nameof(Properties.Resources.DisplayName_Volumes),
            Description = nameof(Properties.Resources.Description_HostDevice_Volumes), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IUpstreamVolume> Volumes { get; }
    }
}
