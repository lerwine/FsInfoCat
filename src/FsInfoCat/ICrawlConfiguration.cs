using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Specifies the configuration of a file system crawl.</summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="ICrawlSettings" />
    public interface ICrawlConfiguration : ICrawlConfigurationRow, IEquatable<ICrawlConfiguration>
    {
        /// <summary>
        /// Gets the starting subdirectory for the configured subdirectory crawl.
        /// </summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        ISubdirectory Root { get; }

        /// <summary>Gets the crawl log entries.</summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Logs), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ICrawlJobLog> Logs { get; }
    }
}
