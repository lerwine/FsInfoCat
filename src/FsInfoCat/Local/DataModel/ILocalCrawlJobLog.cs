using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{

    /// <summary>
    /// Log of crawl job results.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ICrawlJobLog" />
    public interface ILocalCrawlJobLog : ILocalCrawlJobLogRow, ICrawlJobLog, IEquatable<ILocalCrawlJobLog>
    {
        /// <summary>
        /// Gets the configuration source for the file system crawl.
        /// </summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Configuration), ResourceType = typeof(Properties.Resources))]
        new ILocalCrawlConfiguration Configuration { get; }
    }
}
