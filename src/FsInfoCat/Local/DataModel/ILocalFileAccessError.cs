using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for file access error entities.
    /// </summary>
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="Upstream.IUpstreamFileAccessError" />
    /// <seealso cref="ILocalFile.AccessErrors" />
    /// <seealso cref="IEquatable{ILocalFileAccessError}" />
    public interface ILocalFileAccessError : ILocalAccessError, IFileAccessError, IEquatable<ILocalFileAccessError>
    {
        /// <summary>
        /// Gets the target file to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ILocalFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Target { get; }
    }
}
