using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="ILocalDbContext.Files"/> or <see cref="ILocalDbContext.Subdirectories"/> table.
    /// </summary>
    /// <seealso cref="M.IDbFsItemListItem" />
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="Upstream.Model.IDbFsItemListItem" />
    public interface ILocalDbFsItemListItem : M.IDbFsItemListItem, ILocalDbFsItemRow { }
}
