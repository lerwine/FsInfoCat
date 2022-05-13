namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a crawl configuration report entity associated with the <see cref="ILocalDbContext"/>.
    /// </summary>
    /// <seealso cref="ILocalCrawlConfigurationListItem" />
    /// <seealso cref="ICrawlConfigReportItem" />
    public interface ILocalCrawlConfigReportItem : ILocalCrawlConfigurationListItem, ICrawlConfigReportItem { }
}
