using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file access error entities.
    /// </summary>
    /// <seealso cref="IEquatable{ILocalFileAccessError}" />
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="ILocalDbContext.FileAccessErrors" />
    /// <seealso cref="ILocalFile.AccessErrors" />
    /// <seealso cref="Upstream.Model.IUpstreamFileAccessError" />
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
