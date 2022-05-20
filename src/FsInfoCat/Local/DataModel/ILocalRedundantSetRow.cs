namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for an entity representing set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IRedundantSetRow" />
    /// <seealso cref="Upstream.IUpstreamRedundantSetRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalRedundantSetRow")]
    public interface ILocalRedundantSetRow : ILocalDbEntity, IRedundantSetRow { }
}
