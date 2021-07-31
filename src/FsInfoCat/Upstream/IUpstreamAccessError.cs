using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Base interface for all database entity objects which track the creation and modification dates as well as implementing the
    /// <see cref="IValidatableObject" /> and <see cref="IRevertibleChangeTracking" /> interfaces.
    /// </summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError&lt;IUpstreamDbEntity&gt;" />
    /// <seealso cref="IDbEntity" />
    public interface IUpstreamAccessError : IAccessError<IUpstreamDbEntity>, IDbEntity
    {
        /// <summary>Gets the target entity to which the access error applies.</summary>
        /// <value>The <see cref="IUpstreamDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamDbEntity Target { get; }
    }
}
