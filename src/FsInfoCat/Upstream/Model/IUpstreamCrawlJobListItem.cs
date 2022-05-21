using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a file system crawl job log list item entity.
    /// </summary>
    /// <seealso cref="IUpstreamCrawlJobLogRow" />
    /// <seealso cref="ICrawlJobListItem" />
    /// <seealso cref="Local.Model.ILocalCrawlJobListItem" />
    public interface IUpstreamCrawlJobListItem : IUpstreamCrawlJobLogRow, ICrawlJobListItem { }
}
