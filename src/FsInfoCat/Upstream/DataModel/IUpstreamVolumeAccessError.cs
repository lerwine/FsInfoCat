using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for volume access error entities.
    /// </summary>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="IVolumeAccessError" />
    /// <seealso cref="IEquatable{IUpstreamVolumeAccessError}" />
    /// <seealso cref="Local.ILocalVolumeAccessError" />
    public interface IUpstreamVolumeAccessError : IUpstreamAccessError, IVolumeAccessError, IEquatable<IUpstreamVolumeAccessError>
    {
        /// <summary>
        /// Gets the target volume to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IUpstreamVolume" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamVolume Target { get; }
    }
}
