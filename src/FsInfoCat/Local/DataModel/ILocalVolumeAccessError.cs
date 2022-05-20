using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for volume access error entities.
    /// </summary>
    /// <seealso cref="IEquatable{ILocalVolumeAccessError}" />
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="IVolumeAccessError" />
    /// <seealso cref="ILocalDbContext.VolumeAccessErrors" />
    /// <seealso cref="ILocalVolume.AccessErrors" />
    /// <seealso cref="Upstream.IUpstreamVolumeAccessError" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalVolumeAccessError")]
    public interface ILocalVolumeAccessError : ILocalAccessError, IVolumeAccessError, IEquatable<ILocalVolumeAccessError>
    {
        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ILocalDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalVolume Target { get; }
    }
}
