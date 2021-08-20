using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Generic interface for subdirectory access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError" />
    public interface ISubdirectoryAccessError : IAccessError
    {
        /// <summary>Gets the target subdirectory to which the access error applies.</summary>
        /// <value>The <typeparamref name="ISubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ISubdirectory Target { get; }
    }
}

