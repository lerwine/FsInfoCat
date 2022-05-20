namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="Local.ILocalDbFsItemRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamDbFsItemRow")]
    public interface IUpstreamDbFsItemRow : IUpstreamDbEntity, IDbFsItemRow { }
}
