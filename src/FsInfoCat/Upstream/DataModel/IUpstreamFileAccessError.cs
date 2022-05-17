using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for file access error entities.
    /// </summary>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="IEquatable{IUpstreamFileAccessError}" />
    /// <seealso cref="Local.ILocalFileAccessError" />
    public interface IUpstreamFileAccessError : IUpstreamAccessError, IFileAccessError, IEquatable<IUpstreamFileAccessError>
    {
        /// <summary>
        /// Gets the target file to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IUpstreamFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile Target { get; }
    }
}
