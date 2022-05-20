namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a file system crawl job log list item entity.
    /// </summary>
    /// <seealso cref="ILocalCrawlJobLogRow" />
    /// <seealso cref="ICrawlJobListItem" />
    /// <seealso cref="Upstream.IUpstreamCrawlJobListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalCrawlJobListItem")]
    public interface ILocalCrawlJobListItem : ILocalCrawlJobLogRow, ICrawlJobListItem { }
}
