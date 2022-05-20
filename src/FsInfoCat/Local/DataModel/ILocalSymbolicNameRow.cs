namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ISymbolicNameRow" />
    /// <seealso cref="Upstream.IUpstreamSymbolicNameRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalSymbolicNameRow")]
    public interface ILocalSymbolicNameRow : ILocalDbEntity, ISymbolicNameRow { }
}
