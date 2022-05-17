using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a file system crawl job log entity.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="ICrawlJob" />
    /// <seealso cref="IHasSimpleIdentifier" />
    /// <seealso cref="Local.ILocalCrawlJobLogRow" />
    /// <seealso cref="Upstream.IUpstreamCrawlJobLogRow" />
    public interface ICrawlJobLogRow : IDbEntity, ICrawlJob, IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets root path of the crawl.
        /// </summary>
        /// <value>The root path of the crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootPath), ResourceType = typeof(Properties.Resources))]
        string RootPath { get; }

        /// <summary>
        /// Gets a value indicating whether the current crawl configuration has been deactivated.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the current crawl configuration has been deactivated; otherwise, <see langword="false" /> to indicate that is
        /// available for use.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_StatusCode), ResourceType = typeof(Properties.Resources))]
        CrawlStatus StatusCode { get; }

        /// <summary>
        /// Gets the date and time when the crawl was finshed.
        /// </summary>
        /// <value>The date and time when the crawl was finshed or <see langword="null" /> if the current crawl is still active.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlEnd), ResourceType = typeof(Properties.Resources))]
        DateTime? CrawlEnd { get; }
    }
}
