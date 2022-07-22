using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Log of crawl job results.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="ICrawlSettings" />
    /// <seealso cref="Local.Model.ILocalCrawlJobLog" />
    /// <seealso cref="Upstream.Model.IUpstreamCrawlJobLog" />
    /// <seealso cref="ICrawlConfiguration.Logs" />
    /// <seealso cref="IDbContext.CrawlJobLogs" />
    public interface ICrawlJobLog : ICrawlJobLogRow, IEquatable<ICrawlJobLog>
    {
        /// <summary>
        /// Gets the configuration source for the file system crawl.
        /// </summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.Configuration), ResourceType = typeof(Properties.Resources))]
        ICrawlConfiguration Configuration { get; }
    }
}
