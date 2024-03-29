using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Log of crawl job results.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IUpstreamCrawlJobLog" />
    /// <seealso cref="IEquatable{IUpstreamCrawlJobLog}" />
    /// <seealso cref="Local.Model.ILocalCrawlJobLog" />
    public interface IUpstreamCrawlJobLog : IUpstreamDbEntity, ICrawlJobLog, IEquatable<IUpstreamCrawlJobLog>
    {
        /// <summary>
        /// Gets the configuration source for the file system crawl.
        /// </summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.Configuration), ResourceType = typeof(Properties.Resources))]
        new IUpstreamCrawlConfiguration Configuration { get; }
    }
}
