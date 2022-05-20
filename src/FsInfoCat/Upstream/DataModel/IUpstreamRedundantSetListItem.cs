namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a listing item for set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="IRedundantSetListItem" />
    /// <seealso cref="IUpstreamRedundantSetRow" />
    /// <seealso cref="Local.ILocalRedundantSetListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamRedundantSetListItem")]
    public interface IUpstreamRedundantSetListItem : IRedundantSetListItem, IUpstreamRedundantSetRow { }
}
