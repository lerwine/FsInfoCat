using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Generic interface for file access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError" />
    public interface IFileAccessError : IAccessError, IEquatable<IFileAccessError>
    {
        /// <summary>Gets the target file to which the access error applies.</summary>
        /// <value>The <typeparamref name="IFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IFile Target { get; }
    }
}
