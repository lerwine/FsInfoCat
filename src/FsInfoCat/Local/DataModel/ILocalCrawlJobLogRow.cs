namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a file system crawl job log entity.
    /// </summary>
    /// <seealso cref="ICrawlJobLogRow" />
    /// <seealso cref="ILocalDbEntity" />
    public interface ILocalCrawlJobLogRow : ICrawlJobLogRow, ILocalDbEntity { }
}
