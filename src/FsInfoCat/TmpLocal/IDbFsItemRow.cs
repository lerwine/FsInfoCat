using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="M.IDbFsItemRow" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ILocalDbFsItem" />
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="ILocalFileRow" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    /// <seealso cref="Upstream.Model.IDbFsItemRow" />
    public interface ILocalDbFsItemRow : ILocalDbEntity, M.IDbFsItemRow { }
}
