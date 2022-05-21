using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a listing item for set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="IRedundantSetListItem" />
    /// <seealso cref="IUpstreamRedundantSetRow" />
    /// <seealso cref="Local.Model.IRedundantSetListItem" />
    public interface IUpstreamRedundantSetListItem : IRedundantSetListItem, IUpstreamRedundantSetRow { }
}
