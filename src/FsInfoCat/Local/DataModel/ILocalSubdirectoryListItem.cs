namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    /// <seealso cref="Upstream.IUpstreamSubdirectoryListItem" />
    public interface ILocalSubdirectoryListItem : ISubdirectoryListItem, ILocalSubdirectoryRow { }
}
