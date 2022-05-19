using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a crawl configuration report entity associated with the <see cref="ILocalDbContext"/>.
    /// </summary>
    /// <seealso cref="ILocalCrawlConfigurationListItem" />
    /// <seealso cref="M.ICrawlConfigReportItem" />
    /// <seealso cref="Upstream.Model.ICrawlConfigReportItem" />
    public interface ILocalCrawlConfigReportItem : ILocalCrawlConfigurationListItem, M.ICrawlConfigReportItem { }
}
