namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a file system crawl job log entity.
    /// </summary>
    /// <seealso cref="ICrawlJobLogRow" />
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="Local.ILocalCrawlJobLogRow" />
    public interface IUpstreamCrawlJobLogRow : ICrawlJobLogRow, IUpstreamDbEntity { }
}
