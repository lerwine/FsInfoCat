using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.IDbFsItemRow" />
    /// <seealso cref="Local.Model.IDbFsItemRow" />
    public interface IUpstreamDbFsItemRow : IUpstreamDbEntity, M.IDbFsItemRow { }
}
