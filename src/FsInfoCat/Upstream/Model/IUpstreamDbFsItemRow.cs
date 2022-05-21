using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="Local.Model.ILocalDbFsItemRow" />
    public interface IUpstreamDbFsItemRow : IUpstreamDbEntity, IDbFsItemRow { }
}
