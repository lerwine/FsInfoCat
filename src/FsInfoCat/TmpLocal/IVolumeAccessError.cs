using M = FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for volume access error entities.
    /// </summary>
    /// <seealso cref="IEquatable{ILocalVolumeAccessError}" />
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="M.IVolumeAccessError" />
    /// <seealso cref="ILocalDbContext.VolumeAccessErrors" />
    /// <seealso cref="ILocalVolume.AccessErrors" />
    /// <seealso cref="Upstream.Model.IVolumeAccessError" />
    public interface ILocalVolumeAccessError : ILocalAccessError, M.IVolumeAccessError, IEquatable<ILocalVolumeAccessError>
    {
        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ILocalDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalVolume Target { get; }
    }
}
