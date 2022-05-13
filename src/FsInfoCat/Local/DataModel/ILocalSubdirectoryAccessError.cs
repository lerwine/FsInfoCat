using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for subdirectory access error entities that occurred on the local host machine.
    /// </summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="ISubdirectoryAccessError" />
    public interface ILocalSubdirectoryAccessError : ILocalAccessError, ISubdirectoryAccessError, IEquatable<ILocalSubdirectoryAccessError>
    {
        /// <summary>
        /// Gets the target subdirectory to which the access error applies.
        /// </summary>
        /// <value>The <typeparamref name="ISubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Target { get; }
    }
}
