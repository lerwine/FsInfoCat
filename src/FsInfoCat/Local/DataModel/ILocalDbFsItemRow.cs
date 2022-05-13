namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IDbFsItemRow" />
    public interface ILocalDbFsItemRow : ILocalDbEntity, IDbFsItemRow { }
}
