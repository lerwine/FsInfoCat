using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for subdirectory access error entities.
    /// </summary>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="Local.Model.ILocalSubdirectoryAccessError" />
    /// <seealso cref="Upstream.Model.IUpstreamSubdirectoryAccessError" />
    /// <seealso cref="ISubdirectory.AccessErrors" />
    /// <seealso cref="IEquatable{ISubdirectoryAccessError}" />
    /// <seealso cref="ISubdirectory.AccessErrors" />
    /// <seealso cref="IDbContext.SubdirectoryAccessErrors" />
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

