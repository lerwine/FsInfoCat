namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for  that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ISymbolicNameRow" />
    /// <seealso cref="Local.ILocalSymbolicNameRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamSymbolicNameRow")]
    public interface IUpstreamSymbolicNameRow : IUpstreamDbEntity, ISymbolicNameRow { }
}
