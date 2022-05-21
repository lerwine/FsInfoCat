using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for volume access error entities.
    /// </summary>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="IVolumeAccessError" />
    /// <seealso cref="Local.Model.IVolumeAccessError" />
    /// <seealso cref="IEquatable{IUpstreamVolumeAccessError}" />
    /// <seealso cref="IUpstreamVolume.AccessErrors" />
    /// <seealso cref="IUpstreamDbContext.VolumeAccessErrors" />
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
