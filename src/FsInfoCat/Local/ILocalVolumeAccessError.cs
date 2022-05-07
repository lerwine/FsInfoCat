using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for volume access error entities.
    /// </summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="IVolumeAccessError" />
    public interface ILocalVolumeAccessError : ILocalAccessError, IVolumeAccessError
    {
        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ILocalDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalVolume Target { get; }
    }
}
