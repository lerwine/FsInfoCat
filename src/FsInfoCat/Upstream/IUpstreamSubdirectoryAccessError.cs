﻿using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Generic interface for subdirectory access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="ISubdirectoryAccessError" />
    /// <seealso cref="IAccessError&lt;IUpstreamSubdirectory&gt;" />
    public interface IUpstreamSubdirectoryAccessError : IUpstreamAccessError, ISubdirectoryAccessError, IAccessError<IUpstreamSubdirectory>
    {
        /// <summary>Gets the target subdirectory to which the access error applies.</summary>
        /// <value>The <typeparamref name="IUpstreamSubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory Target { get; }
    }
}