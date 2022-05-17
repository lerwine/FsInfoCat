using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for subdirectory access error entities.
    /// </summary>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="Local.ILocalSubdirectoryAccessError" />
    /// <seealso cref="Upstream.IUpstreamSubdirectoryAccessError" />
    /// <seealso cref="ISubdirectory.AccessErrors" />
    /// <seealso cref="IEquatable{ISubdirectoryAccessError}" />
    public interface ISubdirectoryAccessError : IAccessError, IEquatable<ISubdirectoryAccessError>
    {
        /// <summary>
        /// Gets the target subdirectory to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ISubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ISubdirectory Target { get; }
    }
}

