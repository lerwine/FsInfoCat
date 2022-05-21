using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="IUpstreamSubdirectoryRow" />
    /// <seealso cref="Local.Model.ILocalSubdirectoryListItem" />
    public interface IUpstreamSubdirectoryListItem : ISubdirectoryListItem, IUpstreamSubdirectoryRow { }
}
