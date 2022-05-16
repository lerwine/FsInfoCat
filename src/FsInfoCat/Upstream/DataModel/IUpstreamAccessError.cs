using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for access error entities that from the remote host system database.
    /// </summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.ILocalAccessError" />
    public interface IUpstreamAccessError : IAccessError, IDbEntity
    {
        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IUpstreamDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamDbEntity Target { get; }
    }
}
