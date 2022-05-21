using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a crawl configuration report entity associated with the <see cref="IUpstreamDbContext"/>.
    /// </summary>
    /// <seealso cref="IUpstreamCrawlConfigurationListItem" />
    /// <seealso cref="ICrawlConfigReportItem" />
    /// <seealso cref="Local.Model.ILocalCrawlConfigReportItem" />
    public interface IUpstreamCrawlConfigReportItem : IUpstreamCrawlConfigurationListItem, ICrawlConfigReportItem { }
}
