namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="IUpstreamSubdirectoryRow" />
    /// <seealso cref="Local.ILocalSubdirectoryListItem" />
    public interface IUpstreamSubdirectoryListItem : ISubdirectoryListItem, IUpstreamSubdirectoryRow { }
}
