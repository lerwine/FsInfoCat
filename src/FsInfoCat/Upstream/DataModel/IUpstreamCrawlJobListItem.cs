namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a file system crawl job log list item entity.
    /// </summary>
    /// <seealso cref="IUpstreamCrawlJobLogRow" />
    /// <seealso cref="ICrawlJobListItem" />
    /// <seealso cref="Local.ILocalCrawlJobListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamCrawlJobListItem")]
    public interface IUpstreamCrawlJobListItem : IUpstreamCrawlJobLogRow, ICrawlJobListItem { }
}
