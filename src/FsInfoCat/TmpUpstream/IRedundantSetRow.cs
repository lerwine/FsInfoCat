using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an entity representing set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.IRedundantSetRow" />
    /// <seealso cref="Local.Model.IRedundantSetRow" />
    public interface IUpstreamRedundantSetRow : IUpstreamDbEntity, M.IRedundantSetRow { }
}
