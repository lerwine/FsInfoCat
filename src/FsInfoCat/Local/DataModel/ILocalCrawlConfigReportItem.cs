namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a crawl configuration report entity associated with the <see cref="ILocalDbContext"/>.
    /// </summary>
    /// <seealso cref="ILocalCrawlConfigurationListItem" />
    /// <seealso cref="ICrawlConfigReportItem" />
    /// <seealso cref="Upstream.IUpstreamCrawlConfigReportItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalCrawlConfigReportItem")]
    public interface ILocalCrawlConfigReportItem : ILocalCrawlConfigurationListItem, ICrawlConfigReportItem { }
}
