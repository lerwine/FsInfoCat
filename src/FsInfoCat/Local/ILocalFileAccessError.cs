using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>Generic interface for file access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="IFileAccessError" />
    public interface ILocalFileAccessError : ILocalAccessError, IFileAccessError
    {
        /// <summary>Gets the target file to which the access error applies.</summary>
        /// <value>The <typeparamref name="ILocalFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Target { get; }
    }
}
