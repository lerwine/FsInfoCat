using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Specifies the configuration of a file system crawl.
    /// </summary>
    /// <seealso cref="ICrawlConfigurationListItem" />
    /// <seealso cref="IDbContext.CrawlConfigurations"/>
    public interface ICrawlConfiguration : ICrawlConfigurationRow, IEquatable<ICrawlConfiguration>
    {
        /// <summary>
        /// Gets the starting subdirectory for the configured subdirectory crawl.
        /// </summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.RootSubdirectory), ResourceType = typeof(Properties.Resources))]
        ISubdirectory Root { get; }

        /// <summary>
        /// Gets the crawl log entries.
        /// </summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(Properties.Resources.Logs), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ICrawlJobLog> Logs { get; }

        /// <summary>
        /// Gets the unique identifier of the <see cref="Root" /> entity if it has been assigned.
        /// </summary>
        /// <param name="rootId">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier of the <see cref="Root" /> entity has been set; otherwise, <see langword="false" />.</returns>
        bool TryGetRootId(out Guid rootId);
    }
}
