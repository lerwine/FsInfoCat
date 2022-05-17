namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="Upstream.IUpstreamDbFsItemRow" />
    public interface ILocalDbFsItemRow : ILocalDbEntity, IDbFsItemRow { }
}
