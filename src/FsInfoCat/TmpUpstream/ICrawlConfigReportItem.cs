using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a crawl configuration report entity associated with the <see cref="IUpstreamDbContext"/>.
    /// </summary>
    /// <seealso cref="IUpstreamCrawlConfigurationListItem" />
    /// <seealso cref="M.ICrawlConfigReportItem" />
    /// <seealso cref="Local.Model.ICrawlConfigReportItem" />
    public interface IUpstreamCrawlConfigReportItem : IUpstreamCrawlConfigurationListItem, M.ICrawlConfigReportItem { }
}
