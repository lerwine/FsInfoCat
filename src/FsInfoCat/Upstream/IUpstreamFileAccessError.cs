using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Generic interface for file access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="IFileAccessError" />
    public interface IUpstreamFileAccessError : IUpstreamAccessError, IFileAccessError
    {
        /// <summary>Gets the target file to which the access error applies.</summary>
        /// <value>The <typeparamref name="IUpstreamFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile Target { get; }
    }
}
