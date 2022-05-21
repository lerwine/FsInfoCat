using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="ILocalDbContext.Files"/> or <see cref="ILocalDbContext.Subdirectories"/> table.
    /// </summary>
    /// <seealso cref="IDbFsItemListItem" />
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="Upstream.Model.IUpstreamDbFsItemListItem" />
    public interface ILocalDbFsItemListItem : IDbFsItemListItem, ILocalDbFsItemRow { }
}
