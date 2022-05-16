namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="Local.ILocalDbFsItemRow" />
    public interface IUpstreamDbFsItemRow : IUpstreamDbEntity, IDbFsItemRow { }
}
