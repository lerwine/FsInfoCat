using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents a file system crawl job log entity.
    /// </summary>
    /// <seealso cref="ICrawlJobListItem" />
    /// <seealso cref="ICrawlJobLog" />
    /// <seealso cref="ICrawlConfigurationRow" />
    public interface ICrawlJobLogRow : IDbEntity, ICrawlJob, IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets root path of the crawl.
        /// </summary>
        /// <value>The root path of the crawl.</value>
        [Display(Name = nameof(Properties.Resources.RootPath), ResourceType = typeof(Properties.Resources))]
        string RootPath { get; }

        /// <summary>
        /// Gets a value indicating whether the current crawl configuration has been deactivated.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the current crawl configuration has been deactivated; otherwise, <see langword="false" /> to indicate that is
        /// available for use.
        /// </value>
        [Display(Name = nameof(Properties.Resources.StatusCode), ResourceType = typeof(Properties.Resources))]
        CrawlStatus StatusCode { get; }

        /// <summary>
        /// Gets the date and time when the crawl was finshed.
        /// </summary>
        /// <value>The date and time when the crawl was finshed or <see langword="null" /> if the current crawl is still active.</value>
        [Display(Name = nameof(Properties.Resources.CrawlEnd), ResourceType = typeof(Properties.Resources))]
        DateTime? CrawlEnd { get; }
    }
}
