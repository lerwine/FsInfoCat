using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a file system crawl job log list item entity.
    /// </summary>
    /// <seealso cref="ILocalCrawlJobLogRow" />
    /// <seealso cref="ICrawlJobListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamCrawlJobListItem" />
    public interface ILocalCrawlJobListItem : ILocalCrawlJobLogRow, ICrawlJobListItem { }
}
