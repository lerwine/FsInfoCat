using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an entity representing set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="M.IRedundantSetRow" />
    /// <seealso cref="Upstream.Model.IRedundantSetRow" />
    public interface ILocalRedundantSetRow : ILocalDbEntity, M.IRedundantSetRow { }
}
