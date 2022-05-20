namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="IUpstreamDbContext.Files"/> or <see cref="IUpstreamDbContext.Subdirectories"/> table.
    /// </summary>
    /// <seealso cref="IDbFsItemListItem" />
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="Local.ILocalDbFsItemListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamDbFsItemListItem")]
    public interface IUpstreamDbFsItemListItem : IDbFsItemListItem, IUpstreamDbFsItemRow { }
}
