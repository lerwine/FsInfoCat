using FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Specifies the configuration of a file system crawl.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlConfiguration" />
    /// <seealso cref="IEquatable{IUpstreamCrawlConfiguration}" />
    /// <seealso cref="Local.Model.ILocalCrawlConfiguration" />
    public interface IUpstreamCrawlConfiguration : IUpstreamDbEntity, ICrawlConfiguration, IEquatable<IUpstreamCrawlConfiguration>
    {
        /// <summary>
        /// Gets the starting subdirectory for the configured subdirectory crawl.
        /// </summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory Root { get; }

        /// <summary>
        /// Gets the crawl log entries.
        /// </summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Logs), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamCrawlJobLog> Logs { get; }
    }
}
