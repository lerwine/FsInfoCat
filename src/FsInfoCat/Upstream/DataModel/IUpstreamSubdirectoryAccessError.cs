using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for subdirectory access error entities.
    /// </summary>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="ISubdirectoryAccessError" />
    /// <seealso cref="Local.ILocalSubdirectoryAccessError" />
    /// <seealso cref="IUpstreamSubdirectory.AccessErrors" />
    /// <seealso cref="IEquatable{IUpstreamSubdirectoryAccessError}" />
    /// <seealso cref="IUpstreamSubdirectory.AccessErrors" />
    /// <seealso cref="IUpstreamDbContext.SubdirectoryAccessErrors" />
    public interface IUpstreamSubdirectoryAccessError : IUpstreamAccessError, ISubdirectoryAccessError, IEquatable<IUpstreamSubdirectoryAccessError>
    {
        /// <summary>
        /// Gets the target subdirectory to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IUpstreamSubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory Target { get; }
    }
}
