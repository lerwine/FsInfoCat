using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    /// <seealso cref="Upstream.Model.IUpstreamSubdirectoryListItem" />
    public interface ILocalSubdirectoryListItem : ISubdirectoryListItem, ILocalSubdirectoryRow { }
}
