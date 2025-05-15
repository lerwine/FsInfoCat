using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for volume access error entities.
    /// </summary>
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="ISubdirectoryAccessError" />
    /// <seealso cref="IDbContext.VolumeAccessErrors"/>
    public interface IVolumeAccessError : IAccessError, IEquatable<IVolumeAccessError>
    {
        /// <summary>
        /// Gets the target volume to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IVolume" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.Target), ResourceType = typeof(Properties.Resources))]
        new IVolume Target { get; }
    }
}
