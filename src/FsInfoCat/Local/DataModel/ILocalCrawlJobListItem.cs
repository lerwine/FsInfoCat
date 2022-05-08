namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a file system crawl job log list item entity.
    /// </summary>
    /// <seealso cref="ILocalCrawlJobLogRow" />
    /// <seealso cref="ICrawlJobListItem" />
    public interface ILocalCrawlJobListItem : ILocalCrawlJobLogRow, ICrawlJobListItem { }
}
