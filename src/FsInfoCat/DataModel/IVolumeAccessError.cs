using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for volume access error entities.
    /// </summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError" />
    public interface IVolumeAccessError : IAccessError, IEquatable<IVolumeAccessError>
    {
        /// <summary>
        /// Gets the target volume to which the access error applies.
        /// </summary>
        /// <value>The <typeparamref name="IVolume" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IVolume Target { get; }
    }
}
