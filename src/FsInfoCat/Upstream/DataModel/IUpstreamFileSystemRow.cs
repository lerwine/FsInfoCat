namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for file system entities.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IFileSystemRow" />
    /// <seealso cref="Local.ILocalFileSystemRow" />
    public interface IUpstreamFileSystemRow : IUpstreamDbEntity, IFileSystemRow { }
}
