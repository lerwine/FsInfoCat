namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for file system entities.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IFileSystemRow" />
    /// <seealso cref="Upstream.IUpstreamFileSystemRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalFileSystemRow")]
    public interface ILocalFileSystemRow : ILocalDbEntity, IFileSystemRow { }
}
