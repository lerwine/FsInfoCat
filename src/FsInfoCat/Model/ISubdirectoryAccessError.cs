using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for subdirectory access error entities.
    /// </summary>
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="IVolumeAccessError" />
    /// <seealso cref="IDbContext.SubdirectoryAccessErrors"/>
    public interface ISubdirectoryAccessError : IAccessError, IEquatable<ISubdirectoryAccessError>
    {
        /// <summary>
        /// Gets the target subdirectory to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ISubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.Target), ResourceType = typeof(Properties.Resources))]
        new ISubdirectory Target { get; }
    }
}

