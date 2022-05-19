using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for file access error entities.
    /// </summary>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="IEquatable{IFileAccessError}" />
    /// <seealso cref="Local.ILocalFileAccessError" />
    /// <seealso cref="Upstream.IUpstreamFileAccessError" />
    /// <seealso cref="IDbContext.FileAccessErrors" />
    /// <seealso cref="IFile.AccessErrors" />
    public interface IFileAccessError : IAccessError, IEquatable<IFileAccessError>
    {
        /// <summary>
        /// Gets the target file to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IFile Target { get; }
    }
}
