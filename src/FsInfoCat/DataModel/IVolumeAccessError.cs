using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for volume access error entities.
    /// </summary>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="Local.ILocalVolumeAccessError" />
    /// <seealso cref="Upstream.IUpstreamVolumeAccessError" />
    /// <seealso cref="IVolume.AccessErrors" />
    /// <seealso cref="IEquatable{IVolumeAccessError}" />
    /// <seealso cref="Local.ILocalVolumeAccessError" />
    /// <seealso cref="Upstream.IUpstreamVolumeAccessError" />
    public interface IVolumeAccessError : IAccessError, IEquatable<IVolumeAccessError>
    {
        /// <summary>
        /// Gets the target volume to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IVolume" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IVolume Target { get; }
    }
}
