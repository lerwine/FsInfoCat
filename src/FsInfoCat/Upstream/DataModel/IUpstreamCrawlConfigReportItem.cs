namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a crawl configuration report entity associated with the <see cref="IUpstreamDbContext"/>.
    /// </summary>
    /// <seealso cref="IUpstreamCrawlConfigurationListItem" />
    /// <seealso cref="ICrawlConfigReportItem" />
    /// <seealso cref="Local.ILocalCrawlConfigReportItem" />
    public interface IUpstreamCrawlConfigReportItem : IUpstreamCrawlConfigurationListItem, ICrawlConfigReportItem { }
}
