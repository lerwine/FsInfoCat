using M = FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{

    /// <summary>
    /// Specifies the configuration of a file system crawl.
    /// </summary>
    /// <seealso cref="M.ICrawlConfiguration" />
    /// <seealso cref="IEquatable{ILocalCrawlConfiguration}" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ILocalCrawlJobLog.Configuration" />
    /// <seealso cref="ILocalDbContext.CrawlConfigurations" />
    /// <seealso cref="ILocalSubdirectory.CrawlConfiguration" />
    /// <seealso cref="Upstream.Model.ICrawlConfiguration" />
    public interface ILocalCrawlConfiguration : ILocalCrawlConfigurationRow, M.ICrawlConfiguration, IEquatable<ILocalCrawlConfiguration>
    {
        /// <summary>
        /// Gets the starting subdirectory for the configured subdirectory crawl.
        /// </summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Root { get; }

        /// <summary>
        /// Gets the crawl log entries.
        /// </summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Logs), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalCrawlJobLog> Logs { get; }
    }
}
