namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="ILocalDbContext.Files"/> or <see cref="ILocalDbContext.Subdirectories"/> table.
    /// </summary>
    /// <seealso cref="IDbFsItemListItem" />
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="Upstream.IUpstreamDbFsItemListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalDbFsItemListItem")]
    public interface ILocalDbFsItemListItem : IDbFsItemListItem, ILocalDbFsItemRow { }
}
