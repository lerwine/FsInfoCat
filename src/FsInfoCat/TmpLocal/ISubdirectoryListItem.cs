using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="M.ISubdirectoryListItem" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    /// <seealso cref="Upstream.Model.ISubdirectoryListItem" />
    public interface ILocalSubdirectoryListItem : M.ISubdirectoryListItem, ILocalSubdirectoryRow { }
}
