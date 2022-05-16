namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="IUpstreamDbContext.Files"/> or <see cref="IUpstreamDbContext.Subdirectories"/> table
    /// and also contains path names as well as columns from the volume and file system entities.
    /// </summary>
    /// <seealso cref="IDbFsItemListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamDbFsItemListItem" />
    /// <seealso cref="Local.ILocalDbFsItemListItemWithAncestorNames" />
    public interface IUpstreamDbFsItemListItemWithAncestorNames : IDbFsItemListItemWithAncestorNames, IUpstreamDbFsItemListItem { }
}
