using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Reprsents a crawl configuration report row.
    /// </summary>
    /// <seealso cref="ICrawlConfigurationListItem" />
    /// <seealso cref="IEquatable{ICrawlConfigReportItem}" />
    /// <seealso cref="Upstream.Model.IUpstreamCrawlConfigReportItem" />
    /// <seealso cref="Local.Model.ILocalCrawlConfigReportItem" />
    /// <seealso cref="IDbContext.CrawlConfigReport" />
    public interface ICrawlConfigReportItem : ICrawlConfigurationListItem, IEquatable<ICrawlConfigReportItem>
    {
        /// <summary>
        /// Gets the number of successful crawls.
        /// </summary>
        /// <value>The number of successful crawls that used this crawl configuration.</value>
        long SucceededCount { get; }

        /// <summary>
        /// Gets the number of crawls that were stopped when the time limit was reached.
        /// </summary>
        /// <value>The number of crawls that used this crawl configuration and were stopped when the time limit was reached.</value>
        long TimedOutCount { get; }

        /// <summary>
        /// Gets the number of crawls that were stopped when item limit was reached.
        /// </summary>
        /// <value>The number of crawls that used this crawl configuration and were stopped when the item limit was reached.</value>
        long ItemLimitReachedCount { get; }

        /// <summary>
        /// Gets the number of crawls that were canceled before completion.
        /// </summary>
        /// <value>The number of crawls that used this crawl configuration and canceled before completion.</value>
        long CanceledCount { get; }

        /// <summary>
        /// Gets the number of crawls that did not complete due to a failure.
        /// </summary>
        /// <value>The number of crawls that used this crawl configuration and did not complete due to an unrecoverable failure.</value>
        long FailedCount { get; }

        /// <summary>
        /// Gets the average duration of all crawls.
        /// </summary>
        /// <value>The average duration of all crawls or <see langword="null"/> if no crawls had been started.</value>
        long? AverageDuration { get; }

        /// <summary>
        /// Gets the maximum duration of all the crawls.
        /// </summary>
        /// <value>The maximum duration out of all the crawls or <see langword="null"/> if no crawls had been started.</value>
        long? MaxDuration { get; }
    }
}
