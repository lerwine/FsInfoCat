using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlJobLog" />
    public interface IUpstreamCrawlJobLog : IUpstreamDbEntity, ICrawlJobLog
    {
        /// <summary>Gets the configuration source for the file system crawl.</summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Configuration), ResourceType = typeof(Properties.Resources))]
        new IUpstreamCrawlConfiguration Configuration { get; }
    }
}
