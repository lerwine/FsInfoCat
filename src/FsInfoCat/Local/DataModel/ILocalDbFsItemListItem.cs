namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="ILocalDbContext.Files"/> or <see cref="ILocalDbContext.Subdirectories"/> table.
    /// </summary>
    /// <seealso cref="IDbFsItemListItem" />
    /// <seealso cref="ILocalDbFsItemRow" />
    public interface ILocalDbFsItemListItem : IDbFsItemListItem, ILocalDbFsItemRow { }
}
