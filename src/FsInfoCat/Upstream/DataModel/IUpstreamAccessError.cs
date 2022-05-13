using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
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
