using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an entity representing set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IRedundantSetRow" />
    /// <seealso cref="Upstream.Model.IUpstreamRedundantSetRow" />
    public interface ILocalRedundantSetRow : ILocalDbEntity, IRedundantSetRow { }
}
