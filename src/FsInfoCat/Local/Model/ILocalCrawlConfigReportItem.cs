using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a crawl configuration report entity associated with the <see cref="ILocalDbContext"/>.
    /// </summary>
    /// <seealso cref="ILocalCrawlConfigurationListItem" />
    /// <seealso cref="ICrawlConfigReportItem" />
    /// <seealso cref="Upstream.Model.IUpstreamCrawlConfigReportItem" />
    public interface ILocalCrawlConfigReportItem : ILocalCrawlConfigurationListItem, ICrawlConfigReportItem { }
}
