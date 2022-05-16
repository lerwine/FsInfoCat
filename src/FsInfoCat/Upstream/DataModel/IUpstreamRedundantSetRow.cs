namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an entity representing set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IRedundantSetRow" />
    /// <seealso cref="Local.ILocalRedundantSetRow" />
    public interface IUpstreamRedundantSetRow : IUpstreamDbEntity, IRedundantSetRow { }
}
