using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public interface ILocalCrawlJobLogRow : ICrawlJobLogRow, ILocalDbEntity { }

    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ICrawlJobLog" />
    public interface ILocalCrawlJobLog : ILocalCrawlJobLogRow, ICrawlJobLog
    {
        /// <summary>Gets the configuration source for the file system crawl.</summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Configuration), ResourceType = typeof(Properties.Resources))]
        new ILocalCrawlConfiguration Configuration { get; }
    }

    public interface ILocalCrawlJobListItem : ILocalCrawlJobLogRow, ICrawlJobListItem { }
}
