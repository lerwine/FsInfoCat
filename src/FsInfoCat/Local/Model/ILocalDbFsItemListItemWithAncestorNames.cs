using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="ILocalDbContext.Files"/> or <see cref="ILocalDbContext.Subdirectories"/> table
    /// and also contains path names as well as columns from the volume and file system entities.
    /// </summary>
    /// <seealso cref="IDbFsItemListItemWithAncestorNames" />
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamDbFsItemListItemWithAncestorNames" />
    public interface ILocalDbFsItemListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, ILocalDbFsItemListItem { }
}
