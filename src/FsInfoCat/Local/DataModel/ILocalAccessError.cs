using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for access error entities that occur on the local host system.
    /// </summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="IDbEntity" />
    public interface ILocalAccessError : IAccessError, IDbEntity
    {
        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ILocalDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalDbEntity Target { get; }
    }
}
