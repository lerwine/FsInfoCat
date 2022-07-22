using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file access error entities.
    /// </summary>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="Local.Model.ILocalFileAccessError" />
    /// <seealso cref="IEquatable{IUpstreamFileAccessError}" />
    /// <seealso cref="IUpstreamFile.AccessErrors" />
    /// <seealso cref="IUpstreamDbContext.FileAccessErrors" />
    public interface IUpstreamFileAccessError : IUpstreamAccessError, IFileAccessError, IEquatable<IUpstreamFileAccessError>
    {
        /// <summary>
        /// Gets the target file to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IUpstreamFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile Target { get; }
    }
}
