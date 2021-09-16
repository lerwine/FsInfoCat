using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public interface ILocalCrawlConfigurationRow : ILocalDbEntity, ICrawlConfigurationRow { }

    public interface ILocalCrawlConfigurationListItem : ILocalDbEntity, ICrawlConfigurationListItem { }

    /// <summary>Specifies the configuration of a file system crawl.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ICrawlConfiguration" />
    public interface ILocalCrawlConfiguration : ILocalDbEntity, ICrawlConfiguration
    {
        /// <summary>Gets the starting subdirectory for the configured subdirectory crawl.</summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Root { get; }

        /// <summary>Gets the crawl log entries.</summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Logs), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalCrawlJobLog> Logs { get; }
    }

    public interface ILocalCrawlConfigReportItem : ILocalCrawlConfigurationListItem, ICrawlConfigReportItem { }
}
