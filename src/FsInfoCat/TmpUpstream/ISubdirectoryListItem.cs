using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="M.ISubdirectoryListItem" />
    /// <seealso cref="IUpstreamSubdirectoryRow" />
    /// <seealso cref="Local.Model.ISubdirectoryListItem" />
    public interface IUpstreamSubdirectoryListItem : M.ISubdirectoryListItem, IUpstreamSubdirectoryRow { }
}
