using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Log of crawl job results.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamCrawlJobLog" />
public interface ILocalCrawlJobLog : ILocalCrawlJobLogRow, ICrawlJobLog, IEquatable<ILocalCrawlJobLog>
{
    /// <summary>
    /// Gets the configuration source for the file system crawl.
    /// </summary>
    /// <value>The configuration for the file system crawl.</value>
    [Display(Name = nameof(Properties.Resources.Configuration), ResourceType = typeof(Properties.Resources))]
    new ILocalCrawlConfiguration Configuration { get; }
}
